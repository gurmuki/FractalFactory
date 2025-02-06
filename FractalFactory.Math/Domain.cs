
namespace FractalFactory.Math
{
    public class Domain
    {
        public Domain()
        {
            Xmin = double.MaxValue;
            Ymin = double.MaxValue;
            Xmax = double.MinValue;
            Ymax = double.MinValue;
        }

        public Domain(Domain rhs)
        {
            Xmin = rhs.Xmin;
            Ymin = rhs.Ymin;
            Xmax = rhs.Xmax;
            Ymax = rhs.Ymax;
        }

        public Domain(double xmin, double ymin, double xmax, double ymax)
        {
            Xmin = xmin;
            Ymin = ymin;
            Xmax = xmax;
            Ymax = ymax;
        }

        public bool IsValid() { return ((Xmin < Xmax) && (Ymin < Ymax)); }

        public double Xmin { set; get; }
        public double Ymin { set; get; }
        public double Xmax { set; get; }
        public double Ymax { set; get; }

        public double Dx() { return (Xmax - Xmin); }
        public double Dy() { return (Ymax - Ymin); }

        public double Xc() { return (0.5f * (Xmax + Xmin)); }
        public double Yc() { return (0.5f * (Ymax + Ymin)); }

        public bool EqualTo(Domain rhs, double tol)
        {
            int count = 0;

            if (System.Math.Abs(Xmin - rhs.Xmin) < tol)
                ++count;

            if (System.Math.Abs(Ymin - rhs.Ymin) < tol)
                ++count;

            if (System.Math.Abs(Xmax - rhs.Xmax) < tol)
                ++count;

            if (System.Math.Abs(Ymax - rhs.Ymax) < tol)
                ++count;

            return (count == 4);
        }

        public void Translate(double dx, double dy)
        {
            Xmin += dx;
            Ymin += dy;
            Xmax += dx;
            Ymax += dy;
        }

        public void ScaleAbout(double xp, double yp, double factor)
        {
            // Translate to the origin.
            Translate(-xp, -yp);

            // Scale.
            Xmin *= factor;
            Ymin *= factor;
            Xmax *= factor;
            Ymax *= factor;

            // Translate back.
            Translate(xp, yp);
        }
    }
}
