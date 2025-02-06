using FractalFactory.Common;
using FractalFactory.Graphics;
using FractalFactory.Math;
using System;
using System.Drawing;
using System.Threading;
using System.Threading.Tasks;

namespace FractalFactory.Generators
{
    public abstract class FractalGenerator
    {
        const int MAX_ITERATIONS = 100;

        private ColorTable colorTable = new ColorTable(MAX_ITERATIONS);

        /// <summary>Only relevant to NewtonRaphson generators.</summary>
        public virtual void NumeratorPoly(PolyTerms numerTerms) { }

        /// <summary>Only relevant to NewtonRaphson generators.</summary>
        public virtual void DenominatorPoly(PolyTerms denomTerms) { }

        /// <summary>Uses IterationLimit how?</summary>
        public bool LimitIterations { set; get; }

        /// <summary>The upper bound in iterations executed by SolveAt().</summary>
        public int IterationLimit { get; set; } = MAX_ITERATIONS;

        /// <summary>A larger DomainStep should be used for previewing mode.</summary>
        public int DomainStep { get; set; } = 1;

        public bool UseParallelGeneration { get; set; } = false;
        public bool UseParallelColorization { get; set; } = false;

        public TimeSpan ExecutionTime { get; private set; } = TimeSpan.Zero;

        /// <summary>Returns the number of iterations required to reach a solution at the given location.</summary>
        public abstract int SolveAt(double xn, double yn);

        public void BitmapGenerate(CancellationToken cancelToken, Domain domain, Texture texture)
        {
            ExecutionTime = TimeSpan.Zero;

            try
            {
                // The number of iterations per pixel is tracked in the hope
                // the data may be used to help blend colors across the bitmap
                // where you otherwise see discrete bands of color.
                TDArray<int> pixelIterations = new TDArray<int>(texture.Rows, texture.Cols);

                DateTime start = DateTime.Now;

                try
                {
                    if (UseParallelGeneration)
                        ParallelExecution(cancelToken, domain, pixelIterations);
                    else
                        SerialExecution(cancelToken, domain, pixelIterations);
                }
                catch (OperationCanceledException)
                {
                    if (cancelToken.IsCancellationRequested)
                        return;
                }

                // The cost of spinning up the thread for smaller images
                // actually causes worse performance.
                if (UseParallelColorization)
                    ParallelColorize(cancelToken, pixelIterations, texture);
                else
                    SerialColorize(cancelToken, pixelIterations, texture);

                ExecutionTime = (DateTime.Now - start);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private void SerialExecution(CancellationToken cancelToken, Domain domain, TDArray<int> pixelIterations)
        {
            double x0 = domain.Xmin;
            double y0 = domain.Ymin;
            double dx = domain.Dx() / (double)pixelIterations.Cols;
            double dy = domain.Dy() / (double)pixelIterations.Rows;

            // iterations[] tracks the number of times a solution is used, in the
            // hope the data may be useful in generating a color map of soln vs color.
#if DESIRED
            int[] iterations = IterationArray();
#endif
            // Generate the raw data.
            for (int row = 0; row < pixelIterations.Rows; row++)
            {
                double y = y0 + (row * dy);
                for (int col = 0; col < pixelIterations.Cols; col++)
                {
                    if (cancelToken.IsCancellationRequested)
                        return;

                    double x = x0 + (col * dx);
                    int iteration = SolveAt(x, y);

#if DESIRED
                    ++iterations[iteration];
#endif

                    pixelIterations.Values[row, col] = iteration;
                }
            }
        }

        private void ParallelExecution(CancellationToken cancelToken, Domain domain, TDArray<int> pixelIterations)
        {
            double x0 = domain.Xmin;
            double y0 = domain.Ymin;
            double dx = domain.Dx() / (double)pixelIterations.Cols;
            double dy = domain.Dy() / (double)pixelIterations.Rows;

            ParallelOptions options = new ParallelOptions();
            options.MaxDegreeOfParallelism = -1; // -1 is for unlimited. 1 is for sequential.

            //=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=

            // iterations[] tracks the number of times a solution is used, in the
            // hope the data may be useful in generating a color map of soln vs color.
#if DESIRED
            int[] iterations = IterationArray();
#endif
            // Generate the raw data.
            for (int row = 0; row < pixelIterations.Rows; row += DomainStep)
            {
                double y = y0 + (row * dy);
                Parallel.For(0, pixelIterations.Cols, options,
                    (col) =>
                    {
                        if (cancelToken.IsCancellationRequested)
                            return;

                        double x = x0 + (col * dx);
                        int iteration = MAX_ITERATIONS;
                        if ((DomainStep == 1) || ((DomainStep > 1) && ((col % DomainStep) == 0)))
                            iteration = SolveAt(x, y);
#if DESIRED
                        ++iterations[iteration];
#endif

                        pixelIterations.Values[row, col] = iteration;
                    });
            }
        }

        private void SerialColorize(CancellationToken cancelToken, TDArray<int> pixelIterations, Texture texture)
        {
            for (int row = 0; row < pixelIterations.Rows; row++)
            {
                for (int col = 0; col < pixelIterations.Cols; col++)
                {
                    if (cancelToken.IsCancellationRequested)
                        return;

                    Color color = colorTable.At(pixelIterations.Values[row, col]);
                    ColorIt(row, col, color, texture);
                }
            }
        }

        private void ParallelColorize(CancellationToken cancelToken, TDArray<int> pixelIterations, Texture texture)
        {
            ParallelOptions options = new ParallelOptions();
            options.MaxDegreeOfParallelism = -1; // -1 is for unlimited. 1 is for sequential.

            for (int row = 0; row < texture.Rows; row += DomainStep)
            {
                Parallel.For(0, texture.Cols, options,
                    (col) =>
                    {
                        if (cancelToken.IsCancellationRequested)
                            return;

                        Color color = colorTable.At(pixelIterations.Values[row, col]);
                        ColorIt(row, col, color, texture);
                    });
            }
        }

        private void ColorIt(int row, int col, Color color, Texture texture)
        {
            int indx = ((texture.Cols * row) + col) * 4;
            texture.Values[indx + 0] = color.R;
            texture.Values[indx + 1] = color.G;
            texture.Values[indx + 2] = color.B;
            texture.Values[indx + 3] = 255;
        }

#if DESIRED
        private int[] IterationArray()
        {
            // The number of times a solution is used is tracked in the hope
            // the data may be useful in generating a color map of soln vs color.
            //   SortedDictionary<int, int> iterations = new SortedDictionary<int, int>();
            int[] iterations = new int[MAX_ITERATIONS + 1];
            for (int indx = 0; indx < iterations.Length; ++indx)
            {
                iterations[indx] = 0;
            }

            return iterations;
        }
#endif
    }
}
