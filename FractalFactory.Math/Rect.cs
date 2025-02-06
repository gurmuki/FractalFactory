using OpenTK.Mathematics;
using System.Collections.Generic;

namespace FractalFactory.Math
{
    public class Rect
    {
        private int count;

        public double[] ords = { 0, 0, 0, 0, 0, 0, 0, 0 };

        public Rect() { Clear(); }

        public bool AllowDraw { get { return (count > 1); } }

        public void Clear() { count = 0; }

        public double Width { get { return System.Math.Abs(ords[4] - ords[0]); } }

        public double Height { get { return System.Math.Abs(ords[5] - ords[1]); } }

        // Returns: p[0] = BL, p[1] = TR
        public List<PointD> Points(int orientation)
        {
            //                  Xmin            Ymin            Xmax            Ymax
            double[] minmax = { double.MaxValue, double.MaxValue, double.MinValue, double.MinValue };

            if (orientation != 0)
            {
                Vector2 xvec, yvec;
                if (orientation == 90)
                {
                    xvec = new Vector2(0, -1);
                    yvec = new Vector2(1, 0);
                }
                else
                {
                    xvec = new Vector2(0, 1);
                    yvec = new Vector2(-1, 0);
                }

                Matrix2 rot = new Matrix2(xvec, yvec);
                rot.Invert();

                int j = 0;
                while (j < ords.Length)
                {
                    Vector2 vec = new Vector2((float)ords[j], (float)ords[j + 1]);
                    vec = rot * vec;

                    ords[j] = vec.X;
                    ords[j + 1] = vec.Y;

                    j += 2;
                }
            }

            int i = -1;
            while (true)
            {
                ++i;
                if (i >= ords.Length)
                    break;

                if (ords[i] < minmax[0])
                    minmax[0] = ords[i];
                else if (ords[i] > minmax[2])
                    minmax[2] = ords[i];

                ++i;
                if (ords[i] < minmax[1])
                    minmax[1] = ords[i];
                else if (ords[i] > minmax[3])
                    minmax[3] = ords[i];
            }

            List<PointD> pts = new List<PointD>();
            pts.Add(new PointD(minmax[0], minmax[1]));
            pts.Add(new PointD(minmax[2], minmax[3]));
            return pts;
        }

        public void FirstPt(PointD pt)
        {
            Clear();

            ords[0] = pt.X;
            ords[1] = pt.Y;

            ords[2] = pt.X;
            ords[3] = pt.Y;

            ords[4] = pt.X;
            ords[5] = pt.Y;

            ords[6] = pt.X;
            ords[7] = pt.Y;

            count = 1;
        }

        public void SecondPt(PointD pt)
        {
            if (count < 1)
                return;

            ords[2] = pt.X;
            ords[3] = ords[1];

            ords[4] = pt.X;
            ords[5] = pt.Y;

            ords[6] = ords[0];
            ords[7] = pt.Y;

            bool hasArea = ((ords[4] != ords[0]) && (ords[5] != ords[1]));
            count = (hasArea ? 2 : 1);
        }
    }
}
