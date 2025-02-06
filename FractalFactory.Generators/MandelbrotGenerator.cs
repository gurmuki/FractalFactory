using FractalFactory.Math;

namespace FractalFactory.Generators
{
    public class MandelbrotGenerator : FractalGenerator
    {
        public MandelbrotGenerator()
            : base()
        {

        }

        public override int SolveAt(double xn, double yn)
        {
            int iter = 0;
            Complex zk, c;

#if false
            if (Method == eMethod.JuliaSet)
            {
                zk = new Complex(x0, y0);
                c = Constant;
            }
            else
#endif
            {
                zk = new Complex(0, 0);
                c = new Complex(xn, yn);
            }

            double modulusSquared;
            do
            {
                // zk = Complex.Pow(zk, 2) + c;  // this soln is 2-3 time slower
                zk = (zk * zk) + c;
                modulusSquared = zk.GetModulusSquared();
                ++iter;
            } while ((modulusSquared <= 4.0) && (iter < IterationLimit));

            return iter;
        }
    }
}
