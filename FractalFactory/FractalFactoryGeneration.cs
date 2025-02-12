using FractalFactory.Database;
using FractalFactory.Generators;
using FractalFactory.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FractalFactory
{
    public partial class FractalFactory : Form
    {
        private void RunEnable()
        {
            if (initializingControlPanel || singleFrame.Checked)
                return;

            isTaskRunning = false;
            Debug.WriteLine("Overall excution time: {0}\n", DateTime.Now - overallStart);

            if (workspaceSettings.method == OptionsDialog.NEWTON1)
                run.Enabled = (IsDomainDefined() && NonEmpty(numerPoly));
            else if (workspaceSettings.method == OptionsDialog.NEWTON2)
                run.Enabled = (IsDomainDefined() && NonEmpty(numerPoly) && NonEmpty(denomPoly));

            stop.Enabled = false;
        }

        private void StopEnable()
        {
            run.Enabled = false;
            run.Update();

            stop.Enabled = true;
            stop.Update();
        }

        private void MovieBitmapsSave(CancellationToken cancellationToken, int indxA, int indxB, bool reverse)
        {
            string folder = workspaceSettings.movieFolder;

            int step = (reverse ? -1 : 1);
            if (reverse)
            {
                // For example, assuming we're processing the entire grid
                //   indxA = 0, indxB = count
                // So, in the forward direction we need to process [0..count-1]
                // In the reverse direction, we need to process [count-1..0]
                int tmp = indxA;
                indxA = indxB - 1;
                indxB = tmp;
            }

            int count = 0;
            while (true)
            {
                if (reverse && (indxA < indxB))
                    break;
                else if (!reverse && (indxA >= indxB))
                    break;

                Invoke((Action)(() => { grid.ActiveRow = indxA; }));

                if (cancelToken.IsCancellationRequested)
                    cancelToken.ThrowIfCancellationRequested();

                if (fractalDb.WorkspaceImageAt(indxA, theBitmap))
                {
                    string filename = string.Format("img{0:000}.png", ++count);
                    string filepath = Path.Combine(folder, filename);
                    Invoke((Action)(() =>
                    {
                        texture.Update();
                        Render();

                        ImageSave(filepath, texture);
                    }));
                }

                indxA += step;
            }
        }

        private FractalGenerator GeneratorCreate()
        {
            FractalGenerator? generator = null;

            // In order of likelyhood.
            if ((workspaceSettings.method == OptionsDialog.NEWTON2)
                || (workspaceSettings.method == OptionsDialog.NEWTON1))
            {
                generator = new NewtonRaphsonGenerator();
            }
            else if (workspaceSettings.method == OptionsDialog.SECANT)
            {
                generator = new SecantGenerator();
            }
            else if (workspaceSettings.method == OptionsDialog.MANDELBROT)
            {
                generator = new MandelbrotGenerator();
            }

            if (generator == null)
                throw new Exception("GeneratorCreate() -- generator is null");

            generator.DomainStep = (workspaceSettings.preview ? 2 : 1);
            generator.UseParallelGeneration = workspaceSettings.parallel;
            generator.UseParallelColorization = !workspaceSettings.reduced;
            generator.LimitIterations = workspaceSettings.limit;

            return generator;
        }

        private void SingleBitmapGenerate()
        {
            cameraMode = CameraMode.Static;

            TimesUpdate(TimeSpan.MaxValue);

            isTaskRunning = true;
            WaitCursorStart(false);

            //=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=
            // Establish new base viewing parameters.
            //=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=

            // Why use DomainFromInputs() and not ViewDataPeek()?
            //    Because the text of the domain controls may have been edited.
            theDomain = DomainFromInputs();
            theCamera = new Camera(baseCamera);

            ViewDataClear();
            ViewDataPush();

            //=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=

            ImageClear();

            update.Enabled = false;

            FractalGenerator generator = GeneratorCreate();

            generator.NumeratorPoly(Poly.TermsGet(numerPoly.Text, PolyFunction.VERBATIM));
            if (workspaceSettings.method == OptionsDialog.NEWTON2)
                generator.DenominatorPoly(Poly.TermsGet(denomPoly.Text, PolyFunction.VERBATIM));

            cancelTokenSource = new CancellationTokenSource();
            cancelToken = cancelTokenSource.Token;
            generator.BitmapGenerate(cancelToken, theDomain, texture);

            TimesUpdate(generator.ExecutionTime);

            texture.Update();

            WaitCursorStop(false);
            isTaskRunning = false;

            update.Enabled = (grid.RowCount > 0);

            Render();
        }

        private int GeneratedImageCount { get; set; } = 0;

        /// <returns>The count of newly generated images.</returns>
        private void ProjectBitmapsGenerate(CancellationToken cancellationToken)
        {
            GeneratedImageCount = 0;

            FractalGenerator generator = GeneratorCreate();

            List<int> selectedIndices = grid.SelectedIndices(true);
            int initialRow = selectedIndices[0];

            TimeSpan saveTime = TimeSpan.Zero;

            IterationFrame frame;
            while (true)
            {
                int indx = dbWalker.NextFrame(out frame);
                if (indx < 0)
                    break;

                if (indx < initialRow)
                    continue;

                if (cancelToken.IsCancellationRequested)
                    cancelToken.ThrowIfCancellationRequested();

                Invoke((Action)(() => { grid.ActiveRow = indx; }));

                bool delay = true;

                if (!fractalDb.WorkspaceImageAt(frame.RowNumber, theBitmap))
                {
                    AdjustDomainToWindow(frame.Domain);
                    DomainValuesReflect(frame.Domain);

                    generator.NumeratorPoly(Poly.TermsGet(frame.FX, PolyFunction.VERBATIM));
                    generator.DenominatorPoly(Poly.TermsGet(frame.DFX, PolyFunction.VERBATIM));

                    generator.BitmapGenerate(cancellationToken, frame.Domain, texture);

                    if (cancelToken.IsCancellationRequested)
                        cancelToken.ThrowIfCancellationRequested();

                    TimesUpdate(generator.ExecutionTime);

                    DateTime start = DateTime.Now;
                    fractalDb.WorkspaceImageUpdate(frame.RowNumber, theBitmap);
                    saveTime += DateTime.Now - start;

                    ++GeneratedImageCount;
                    delay = false;
                }

                Invoke((Action)(() =>
                {
                    texture.Update();
                    Render();
                }));

                if (delay)
                    Thread.Sleep(125);
            }

            Debug.WriteLine("ImageSave time: {0}\n", saveTime);
        }

        private async void MultiExecution()
        {
            TimesUpdate(TimeSpan.MaxValue);

            dbWalker = new FractalDBWalker(fractalDb);

            ControlsEnable(false);
            WaitCursorStart();

            cancelTokenSource = new CancellationTokenSource();
            cancelToken = cancelTokenSource.Token;
            var task = Task.Run(() =>
            {
                ProjectBitmapsGenerate(cancelToken);
            }, cancelToken);

            StopEnable();

            try
            {
                await task;

                WaitCursorStop();
                ControlsEnable(true);
                RunEnable();

                ProjectMenuItemsEnable();
            }
            catch (OperationCanceledException)
            {
                Debug.WriteLine($"MultiExecution: {nameof(OperationCanceledException)} thrown\n");
                WaitCursorStop();
                ControlsEnable(true);
                RunEnable();
            }

            if (GeneratedImageCount > 0)
            {
                // This will give the user an opportunity to save any newly
                // generated images. Without this, it is possible to have
                // spent a significant amount of time generating images,
                // only to lose them when exiting the application.
                fractalDb.IsDirty = true;
                AppTitleUpdate();
            }
        }

        private void ControlsEnable(bool enable)
        {
            frames.Enabled = enable;
            domainSettings.Enabled = enable;
            polysGroupBox.Enabled = enable;
            recordingGroupBox.Enabled = enable;
            this.Update();
        }
    }
}
