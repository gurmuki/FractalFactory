using FractalFactory.Math;
using System.Runtime.InteropServices;

namespace FractalFactory
{
    public enum PolyFunction { DERIVATIVE, VERBATIM, INTEGRAL }

    public class Poly
    {
        [DllImport("ParserCore.dll", CallingConvention = CallingConvention.Cdecl)]
        static extern int ExpParse(string expression);

        [DllImport("ParserCore.dll", CallingConvention = CallingConvention.Cdecl)]
        static extern int ExpParseCountOfTerms();

        [DllImport("ParserCore.dll", CallingConvention = CallingConvention.Cdecl)]
        static extern void ExpParseTermsGet(int count, double[] coeff, double[] expon);


        static public PolyTerms TermsGet(string polynomialExpression, PolyFunction func)
        {
            PolyTerms terms = new PolyTerms();

            if (ExpParse(polynomialExpression) == 0)
            {
                int count = ExpParseCountOfTerms();
                double[] coeff = new double[count];
                double[] expon = new double[count];
                ExpParseTermsGet(count, coeff, expon);

                for (int indx = 0; indx < count; ++indx)
                {
                    if (func == PolyFunction.DERIVATIVE)
                        terms[expon[indx] - 1] = (expon[indx] * coeff[indx]);
                    else if (func == PolyFunction.INTEGRAL)
                        terms[expon[indx] + 1] = (coeff[indx] / (expon[indx] + 1));
                    else
                        terms[expon[indx]] = coeff[indx];
                }
            }

            return terms;
        }
    }
}
