using System;

namespace FractalFactory.Common
{
    public class Stringy
    {
        public const string NUMER = "numer";
        public const string NUMERC = "numer:";

        public const string DENOM = "denom";
        public const string DENOMC = "denom:";

        public static string DoubleFormat(double dval, int precision)
        {
            string fmt = "{0:0." + new string('0', precision) + "}";
            string tmp = string.Format(fmt, dval);

            int indx = tmp.Length - 1;
            while ((indx > 0) && (tmp[indx] == '0'))
            { --indx; }

            if ((indx > 0) && (tmp[indx] == '.'))
                --indx;

            if (indx < (tmp.Length - 1))
                tmp = tmp.Substring(0, indx + 1);

            return tmp;
        }

        public static string Trim(string str)
        {
            char[] spaceChars = { ' ', '\t' };
            return str.Trim(spaceChars);
        }

        // From https://stackoverflow.com/questions/6219454/efficient-way-to-remove-all-whitespace-from-string
        public static string WhitespaceRemove(string str)
        {
            return string.Join("", str.Split(default(string[]), StringSplitOptions.RemoveEmptyEntries));
        }

        public static bool IsEmpty(string str)
        {
            return ((str == null) || (str.Length == 0));
        }

    }
}
