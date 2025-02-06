using System;
using System.Collections.Generic;
using System.Linq;

namespace FractalFactory.Math
{
    /// <summary>
    /// Polynomial terms (coefficients and exponents), typically
    /// extracted from a string representation of a polynomial.
    /// </summary>
    public class PolyTerms : SortedDictionary<double, double>
    {
        public PolyTerms() { }

        public static PolyTerms operator ^(PolyTerms poly, int deg)
        {
            PolyTerms result = new PolyTerms();
            foreach (var term in poly)
            {
                double exponent = term.Key;
                double coefficient = term.Value;

                if (deg < 0)
                {
                    // 1st derivate
                    if ((exponent - 1) < 0)
                        continue;

                    result[exponent - 1] = exponent * coefficient;
                }
                else if (deg > 0)
                {
                    // integrate
                    ++exponent;
                    result[exponent] = coefficient / exponent;
                }
                else
                {
                    // copy
                    result[exponent] = coefficient;
                }
            }
            return result;
        }

        public PolyTerms(PolyTerms rhs)
        {
            foreach (var item in rhs)
            {
                this[item.Key] = item.Value;
            }
        }

        /// <summary>Expands to an array of [coeff, exp, ....]</summary>
        /// <returns>double[]</returns>
        public double[] ToArray()
        {
            double[] result = new double[this.Count * 2];

            int indx = 0;
            foreach (var item in this.Reverse())
            {
                result[indx++] = item.Value;
                result[indx++] = item.Key;
            }

            return result;
        }

        public bool EqualTo(PolyTerms rhs, double tol)
        {
            if (this.Count != rhs.Count)
                return false;

            foreach (var item in this)
            {
                if (!rhs.ContainsKey(item.Key))
                    return false;

                if (System.Math.Abs(item.Value - rhs[item.Key]) > tol)
                    return false;
            }

            return true;
        }

        static public void Balance(PolyTerms lhs, PolyTerms rhs)
        {
            if (lhs.Count == rhs.Count)
            {
                // Given the inputs:
                //   2nd: 64*x^3.0
                //   2nd: 64*x^4.0
                // The grid (wrongly) displayed:
                //   coeff exp coeff exp
                //   0     4   64    3
                //   64    4   0     3
                // when it should have displayed
                //   coeff exp
                //   64    3
                //   64    4
                return;
            }

            foreach (var term in lhs)
            {
                if (!rhs.ContainsKey(term.Key))
                    rhs[term.Key] = 0;
            }

            foreach (var term in rhs)
            {
                if (!lhs.ContainsKey(term.Key))
                    lhs[term.Key] = 0;
            }
        }
    }
}
