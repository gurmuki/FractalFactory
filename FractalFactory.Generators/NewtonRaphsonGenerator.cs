using FractalFactory.Math;

namespace FractalFactory.Generators
{
    public class NewtonRaphsonGenerator : FractalGenerator
    {
        private PolyTerms fx = new PolyTerms();
        private PolyTerms dfx = new PolyTerms();

        public NewtonRaphsonGenerator()
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
        // when Factory.Math is built. Turning it off yields some interesting results!!!
        public override int SolveAt(double xn, double yn)
        {
            int iter = 0;

            if (xn == 0 && yn == 0)
            {
                iter = 0;
            }
            else
            {
                Complex numerator = new Complex(1, 0);
                Complex denominator = new Complex(1, 0);
                double modulusSqrd = double.MaxValue;
                double goal = 1e-8;
                double a = 1;

                int hitInfinity = 0;

                Complex zn = new Complex(xn, yn);
                while ((iter < IterationLimit) && (modulusSqrd > goal))
                {
                    if (double.IsInfinity(modulusSqrd))
                        ++hitInfinity;

                    numerator = PolyEval(fx, zn);
                    denominator = PolyEval(dfx, zn);

                    try { zn = zn - a * (numerator / denominator); }
                    catch { return IterationLimit; }

                    if (hitInfinity > 1)
                    {
                        if (!LimitIterations)
                            iter = IterationLimit;
                        break;
                    }

                    ++iter;

                    modulusSqrd = numerator.GetModulusSquared();
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
