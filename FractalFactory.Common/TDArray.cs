using System;

namespace FractalFactory.Common
{
    /// <summary>A two dimensional array.</summary>
    public class TDArray<T>
    {
        public TDArray(int rows, int cols)
        {
            try
            {
                Values = new T[rows, cols];
                Rows = rows;
                Cols = cols;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public int Rows { get; private set; }
        public int Cols { get; private set; }

        public T[,] Values { get; private set; } = null!;
    }
}
