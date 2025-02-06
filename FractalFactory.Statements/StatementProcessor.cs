using FractalFactory.Common;
using FractalFactory.Math;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace FractalFactory.Statements
{
    public enum ParseStatus
    {
        PARSE_FAILED = 0,
        ONE_FX = 1,
        TWO_FX = 2,
        ONE_DFX = 4,
        TWO_DFX = 8,
        ONE_DOMAIN = 16,
        TWO_DOMAIN = 32,
        MULTI_STATEMENT = 64
    }

    public class StatementProcessor
    {
        private double precision;

        public StatementProcessor(int precisionDigitCount)
        {
            Classification = ParseStatus.PARSE_FAILED;
            FX = new List<string>();
            DFX = new List<string>();
            Domains = new List<Domain>();

            string fmt = "0." + new string('0', precisionDigitCount - 1) + "1";
            precision = Double.Parse(fmt);
        }

        public ParseStatus Classification { private set; get; }
        public List<string> FX { private set; get; }
        public List<string> DFX { private set; get; }
        public List<Domain> Domains { private set; get; }

        public bool IsMaskSatisfied(ParseStatus status, ParseStatus mask)
        {
            return (((int)status & (int)mask) == (int)mask);
        }

        public ParseStatus Parse(string text)
        {
            Classification = ParseStatus.PARSE_FAILED;  // Assume failure

            FX = new List<string>();
            DFX = new List<string>();
            Domains = new List<Domain>();

            if (ParametersParse(text))
                Classification = (ParseStatus)ParsingResultsClassify();

            return Classification;
        }

        public ParseStatus Parse(List<string> text)
        {
            Classification = ParseStatus.PARSE_FAILED;  // Assume failure

            FX = new List<string>();
            DFX = new List<string>();
            Domains = new List<Domain>();

            bool validStatementCount = (text.Count > 0) && (text.Count < 3);
            Debug.Assert(validStatementCount, "Assertion: StatementProcessor.Parse() -- invalid text count");

            if (!validStatementCount)
                return Classification;

            int status = 0;
            if (ParametersParse(text[0]))
            {
                if (text.Count == 2)
                {
                    if (ParametersParse(text[1]))
                    {
                        int status2 = ParsingResultsClassify();
                        if (status2 != 0)
                        {
                            status += status2;
                            status += (int)ParseStatus.MULTI_STATEMENT;
                        }
                    }
                    else
                    {
                        status += ParsingResultsClassify();
                    }
                }
            }

            Classification = (ParseStatus)status;

            return Classification;
        }

        // TODO: Fix this!!!
        public bool InterpolationAllowed(List<string> selectedText)
        {
            bool allowInterpolation = false;

            ParseStatus status = Parse(selectedText);
            if ((status != ParseStatus.PARSE_FAILED) && IsMaskSatisfied(status, ParseStatus.MULTI_STATEMENT))
            {
                if (IsMaskSatisfied(status, ParseStatus.TWO_DOMAIN))
                {
                    Domain i = Domains[0];
                    Domain f = Domains[1];
                    allowInterpolation = !i.EqualTo(f, precision);
                }

                if (!allowInterpolation && IsMaskSatisfied(status, ParseStatus.TWO_FX))
                {
                    bool identicalStatements = (FX[0] == FX[1]);
                    allowInterpolation = !identicalStatements;
                }

                if (!allowInterpolation && IsMaskSatisfied(status, ParseStatus.TWO_DFX))
                {
                    bool identicalStatements = (DFX[0] == DFX[1]);
                    allowInterpolation = !identicalStatements;
                }
            }

            return allowInterpolation;
        }

        //=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=

        private int ParsingResultsClassify()
        {
            int status = (int)ParseStatus.PARSE_FAILED;

            if (FX.Count == 1)
                status += (int)ParseStatus.ONE_FX;
            else if (FX.Count == 2)
                status += (int)ParseStatus.TWO_FX;

            if (DFX.Count == 1)
                status += (int)ParseStatus.ONE_DFX;
            else if (DFX.Count == 2)
                status += (int)ParseStatus.TWO_DFX;

            if (Domains.Count == 1)
                status += (int)ParseStatus.ONE_DOMAIN;
            else if (Domains.Count == 2)
                status += (int)ParseStatus.TWO_DOMAIN;

            return status;
        }

        private bool ParametersParse(string statement)
        {
            bool success = false;

            StatementParser sp = new StatementParser();
            sp.Parse(statement);

            if (sp.HasA(Stringy.NUMER))
            {
                FX.Add(sp.Value(Stringy.NUMER));  // TODO: Validate the polynomial
                success = true;
            }

            if (sp.HasA(Stringy.DENOM))
            {
                DFX.Add(sp.Value(Stringy.DENOM));  // TODO: Validate the polynomial
                success = true;
            }

            if (sp.HasA("xmin") && sp.HasA("ymin") && sp.HasA("xmax") && sp.HasA("ymax"))
            {
                if (!Double.TryParse(sp.Value("xmin"), out double xmin))
                    return false;

                if (!Double.TryParse(sp.Value("ymin"), out double ymin))
                    return false;

                if (!Double.TryParse(sp.Value("xmax"), out double xmax))
                    return false;

                if (!Double.TryParse(sp.Value("ymax"), out double ymax))
                    return false;

                Domains.Add(new Domain(xmin, ymin, xmax, ymax));
                success = true;
            }

            return success;
        }
    }
}
