using FractalFactory.Common;
using FractalFactory.Math;
using System;

namespace FractalFactory.Database
{
    public class IterationFrame
    {
        public IterationFrame()
        {
            FX = string.Empty;
            DFX = string.Empty;
            Domain = new Domain();
            RowNumber = -1;
        }

        public string FX { set; get; }
        public string DFX { set; get; }
        public Domain Domain { set; get; }
        public int RowNumber { set; get; }
    }

    public class InvalidFractalDBWalker : FractalDBWalker
    {
        public InvalidFractalDBWalker() : base() { }
    }

    public class FractalDBWalker
    {
        private FractalDB? db = null;

        private int recordCount;
        private int currentRowNumber;

        private string fx = string.Empty;
        private string dfx = string.Empty;
        private Domain domain = new Domain(0, 0, 0, 0);

        // Used only by InvalidFractalDBWalker ctor.
        protected FractalDBWalker() { }

        public FractalDBWalker(FractalDB fractalDb)
        {
            if (fractalDb == null)
                throw new ArgumentNullException(nameof(fractalDb));

            db = fractalDb;

            recordCount = db.WorkspaceRecordCount();
            currentRowNumber = 0;
        }

        public int NextFrame(out IterationFrame frame, bool allowPartial = false)
        {
            char[] DELIMITERS = { ':', ',' };

            frame = new IterationFrame();
            frame.RowNumber = currentRowNumber;

            int changed = 0;
            for (/*contentIndex*/ ; currentRowNumber < recordCount; ++currentRowNumber)
            {
                if ((changed > 0) && ParametersSatisfied(false))
                    break;

                string tmp = db!.WorkspaceStatementFetch(currentRowNumber);
                // TODO: Use StatementProcessor to do this work(?)
                string statement = Stringy.WhitespaceRemove(tmp);
                string[] pieces = statement.Split(DELIMITERS, StringSplitOptions.None);
                for (int pndx = 0; pndx < pieces.Length; ++pndx)
                {
                    string paramName = pieces[pndx];
                    if (paramName == Stringy.NUMER)
                    {
                        ++pndx;
                        fx = pieces[pndx];
                        ++changed;
                    }
                    else if (paramName == Stringy.DENOM)
                    {
                        ++pndx;
                        dfx = pieces[pndx];
                        ++changed;
                    }
                    else if (paramName == "xmin")
                    {
                        ++pndx;
                        domain.Xmin = Double.Parse(pieces[pndx]);
                        ++changed;
                    }
                    else if (paramName == "ymin")
                    {
                        ++pndx;
                        domain.Ymin = Double.Parse(pieces[pndx]);
                        ++changed;
                    }
                    else if (paramName == "xmax")
                    {
                        ++pndx;
                        domain.Xmax = Double.Parse(pieces[pndx]);
                        ++changed;
                    }
                    else if (paramName == "ymax")
                    {
                        ++pndx;
                        domain.Ymax = Double.Parse(pieces[pndx]);
                        ++changed;
                    }
                }
            }

            frame.FX = fx;
            frame.DFX = dfx;
            frame.Domain = domain;

            return (((changed > 0) && ParametersSatisfied(allowPartial)) ? (currentRowNumber - 1) : -1);
        }

        private bool ParametersSatisfied(bool allowPartial)
        {
            if (allowPartial)
                return ((fx != string.Empty) || (dfx != string.Empty) || ((domain.Xmin < domain.Xmax) && (domain.Ymin < domain.Ymax)));
            else
                return ((fx != string.Empty) && (dfx != string.Empty) && (domain.Xmin < domain.Xmax) && (domain.Ymin < domain.Ymax));
        }
    }
}
