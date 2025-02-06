using FractalFactory.Math;

namespace FractalFactory.Generators
{
    public class SecantGenerator : FractalGenerator
    {
        private PolyTerms fx = new PolyTerms();
        private PolyTerms dfx = new PolyTerms();

        public SecantGenerator()
            : base()
        {

        }

        public override void NumeratorPoly(PolyTerms numerTerms)
        {
            fx = numerTerms ^ 0;
            dfx = numerTerms ^ -1;
        }

        public override void DenominatorPoly(PolyTerms denomTerms)
        {
            dfx = denomTerms ^ 0;
        }

        /// <summary>Returns the number of iterations required to reach a solution at the given location.</summary>
        //
        // BEWARE: The results will differ between debug/release if Optimization is turned on
        // when FractalMath is built. Turning it off yields some interesting results!!!
        public override int SolveAt(double xn, double yn)
        {
            int iter = 0;

            if (xn == 0 && yn == 0)
            {
                iter = 0;
            }
            else
            {
                Complex p1 = new Complex(1, 0);
                Complex p2 = new Complex(1, 0);
                Complex p = new Complex(1, 0);
                Complex diff = new Complex(0, 0);
                double goal = 1e-8;

                Complex p0 = new Complex(xn, yn);
                while (iter < IterationLimit)
                {
                    p1 = PolyEval(fx, p0);
                    p2 = PolyEval(fx, p1);

                    try
                    {
                        diff = p1 - p0;
                        Complex numerator = p0 - (diff * diff);
                        Complex denominator = p2 - (2 * p1) + p0;
                        p = numerator / denominator;
                    }
                    catch { }

                    if ((System.Math.Abs(p.Re) <= goal) && (System.Math.Abs(p.Im) <= goal))
                        break;

                    diff = p - p0;
                    if (double.IsNaN(diff.Re) || double.IsNaN(diff.Im))
                        break;

                    if ((System.Math.Abs(diff.Re) < goal) && (System.Math.Abs(diff.Im) < goal))
                        break;

                    p0 = p;
                    ++iter;
                }
            }

            return iter;
        }

        private Complex PolyEval(PolyTerms terms, Complex xn)
        {
            Complex xi = new Complex(0, 0);
            foreach (var term in terms)
            {
                double exponent = term.Key;
                double coefficient = term.Value;

                xi = xi + (coefficient * (xn ^ exponent));
            }
            return xi;
        }
    }
}
