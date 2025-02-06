using System;
using System.Collections.Generic;
using System.Linq;

namespace FractalFactory.Math
{
    public class DirectedRange
    {
        public double start { set; get; }
        public double end { set; get; }
        public double step { set; get; }

        public void StepCalculate(int divisions) { step = (end - start) / divisions; }
    }

    public class InterpolationDirective
    {
        public DirectedRange cRange;  // coefficient range
        public DirectedRange eRange;  // coefficient range

        public InterpolationDirective()
        {
            cRange = new DirectedRange();
            eRange = new DirectedRange();
        }
    }

    public class PolyInterpolator
    {
        private List<InterpolationDirective>? Directives = null;

        public PolyInterpolator(List<InterpolationDirective> directives, int divisions)
        {
            Directives = directives;

            for (int indx = 0; indx < Directives.Count; ++indx)
            {
                Directives[indx].cRange.StepCalculate(divisions);
                Directives[indx].eRange.StepCalculate(divisions);
            }
        }

        // TODO: Ensure interpolationDirectives are properly ordered(?)
        public PolyTerms Interpolate(int div)
        {
            PolyTerms polyTerms = new PolyTerms();

            for (int indx = 0; indx < Directives!.Count; ++indx)
            {
                InterpolationDirective directive = Directives[indx];

                double coeff = directive.cRange.start + (div * directive.cRange.step);
                double exp = directive.eRange.start + (div * directive.eRange.step);

                polyTerms[exp] = coeff;
            }

            return polyTerms;
        }

        static public bool InterpolationDirectivesInit(PolyTerms? numer, PolyTerms? denom, List<InterpolationDirective> directives)
        {
            directives.Clear();
            if ((numer == null) || (denom == null))
                return false;

            if (numer.Count == denom.Count)
            {
                // Implicit interpolation.
                // This branch was introduced to address interpolation between polynomials like
                //   e.g.
                //      16*x^8 + 64*x^4
                //      16*x^8 + 32*x^5
                // Interpolation is done term for term; nothing intelligent about it.
            }
            else
            {
                // Ensure we have the same number of terms
                // in both polynomials before proceeding.
                foreach (var exp in numer.Keys)
                {
                    if (!denom.ContainsKey(exp))
                        denom[exp] = 0;
                }

                foreach (var exp in denom.Keys)
                {
                    if (!numer.ContainsKey(exp))
                        numer[exp] = 0;
                }
            }

            // To simplify processing ....
            double[] numerTerms = numer.ToArray();
            double[] denomTerms = denom.ToArray();

            for (int indx = 0; indx < numerTerms.Count(); indx += 2)
            {
                if ((Convert.ToDouble(numerTerms[indx]) == 0) && (Convert.ToDouble(denomTerms[indx]) == 0))
                    continue;

                InterpolationDirective directive = new InterpolationDirective();
                directive.cRange.start = Convert.ToDouble(numerTerms[indx]);
                directive.cRange.end = Convert.ToDouble(denomTerms[indx]);
                directive.eRange.start = Convert.ToDouble(numerTerms[indx + 1]);
                directive.eRange.end = Convert.ToDouble(denomTerms[indx + 1]);

                directives.Add(directive);
            }

            return false;
        }
    }
}
