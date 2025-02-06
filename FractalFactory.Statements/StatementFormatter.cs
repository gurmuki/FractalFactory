using FractalFactory.Common;
using FractalFactory.Math;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FractalFactory.Statements
{
    public class StatementFormatter
    {
        private int precisionInt;
        private double precisionDouble;

        public StatementFormatter(int precision)
        {
            precisionInt = precision;

            string fmt = "0." + new string('0', precisionInt - 1) + "1";
            precisionDouble = Double.Parse(fmt);
        }

        public string FxStatementCreate(string numerPoly)
        {
            return PolyFormatter.Format(Stringy.NUMERC, numerPoly);
        }

        public string DFxStatementCreate(string denomPoly)
        {
            return PolyFormatter.Format(Stringy.DENOMC, denomPoly);
        }

        public string FunctionStatementCreate(PolyTerms terms)
        {
            return FunctionStatementCreate(string.Empty, terms);
        }

        public string FunctionStatementCreate(string key, PolyTerms terms)
        {
            string statement = ((key == string.Empty) ? string.Empty : string.Format("{0}: ", key));

            List<double> exponents = terms.Keys.ToList();
            for (int indx = exponents.Count - 1; indx >= 0; --indx)
            {
                double exp = exponents[indx];
                double coeff = terms[exp];

                if (System.Math.Abs(coeff) < precisionDouble)
                    continue;  // essentially a coefficient of zero

                if (indx < (exponents.Count - 1))
                    statement += ((coeff < 0) ? " - " : " + ");

                if (System.Math.Abs(exp) >= precisionDouble)
                {
                    // We've encountered a non-zero exponent.

                    if (System.Math.Abs(coeff) > precisionDouble)
                        statement += string.Format("{0}*", Stringy.DoubleFormat(System.Math.Abs(coeff), precisionInt));

                    statement += string.Format("x^{0}", Stringy.DoubleFormat(exp, precisionInt));
                }
                else
                {
                    if (System.Math.Abs(coeff) > precisionDouble)
                        statement += string.Format("{0}", Stringy.DoubleFormat(System.Math.Abs(coeff), precisionInt));
                }
            }

            return statement;
        }

        public string DomainStatementCreate(Domain domain)
        {
            return DomainStatementCreate(domain.Xmin, domain.Ymin, domain.Xmax, domain.Ymax);
        }

        public string DomainStatementCreate(double xmin, double ymin, double xmax, double ymax)
        {
            string sxmin = Stringy.DoubleFormat(xmin, precisionInt);
            string symin = Stringy.DoubleFormat(ymin, precisionInt);
            string sxmax = Stringy.DoubleFormat(xmax, precisionInt);
            string symax = Stringy.DoubleFormat(ymax, precisionInt);

            return $"xmin:{sxmin}, ymin:{symin}, xmax:{sxmax}, ymax:{symax}";
        }
    }
}
