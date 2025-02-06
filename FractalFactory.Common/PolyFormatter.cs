using System.Text;

namespace FractalFactory.Common
{
    public class PolyFormatter
    {
        public static string Format(string polynomialExpression)
        {
            return Format(string.Empty, polynomialExpression);
        }

        public static string Format(string prefix, string polynomialExpression)
        {
            StringBuilder sb = new StringBuilder();
            if (prefix != string.Empty)
            {
                sb.Append(prefix);
                sb.Append(' ');
            }

            foreach (var ch in polynomialExpression)
            {
                if (ch == ' ')
                    continue;

                if (ch == '+')
                    sb.Append(" + ");
                else if (ch == '-')
                    sb.Append(" - ");
                else
                    sb.Append(ch);
            }

            return sb.ToString();
        }
    }
}
