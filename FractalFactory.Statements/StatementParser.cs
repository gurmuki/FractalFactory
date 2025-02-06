using FractalFactory.Common;
using System;
using System.Collections.Generic;

namespace FractalFactory.Statements
{
    public class StatementParser
    {
        private char[] COMMA = { ',' };
        private char[] COLON = { ':' };

        private SortedDictionary<string, string> namedValues = new SortedDictionary<string, string>();

        public StatementParser() { }

        // An statement is a string comma separated named values. For example:
        //    numer: x^10 + 8*x^2 - 16, denom: 32*x^8 + 80*x^4,  xmin:-60.84288115, ymin:-48.42084034, xmax:84.87140457, ymax:48.7220168
        public bool Parse(string statement)
        {
            string[] candidates = statement.Split(COMMA, StringSplitOptions.None);
            if (candidates.Length > 0)
            {
                for (int indx = 0; indx < candidates.Length; ++indx)
                {
                    string[] tokens = candidates[indx].Split(COLON, StringSplitOptions.None);
                    if ((tokens.Length % 2) == 0)
                    {
                        string name = Stringy.Trim(tokens[0]);
                        string val = Stringy.Trim(tokens[1]);
                        namedValues[name] = val;
                    }
                }
            }

            return (namedValues.Count > 0);
        }

        public bool HasA(string name)
        { return namedValues.ContainsKey(name); }

        public string Value(string name)
        { return namedValues[name]; }
    }
}
