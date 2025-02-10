using FractalFactory.Common;
using FractalFactory.Database;
using FractalFactory.Graphics;
using FractalFactory.Math;
using FractalFactory.Statements;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
// using System.Windows;
using System.Windows.Forms;

namespace FractalFactory
{
    public partial class FractalFactory : Form
    {
        //=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=
        // Project-related methods.
        //=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=

        private void MethodControlsUpdate(string newMethod)
        {
            workspaceSettings.method = newMethod;

            numerLabel.Enabled = false;
            numerPoly.Enabled = false;

            denomLabel.Enabled = false;
            denomPoly.Enabled = false;

            if ((workspaceSettings.method == OptionsDialog.NEWTON1))
            {
                PolyTerms numerTerms = Poly.TermsGet(numerPoly.Text, PolyFunction.VERBATIM);
                if (numerTerms.Count > 0)
                {
                    PolyTerms denomTerms = Poly.TermsGet(numerPoly.Text, PolyFunction.DERIVATIVE);
                    if (numerTerms.Count > 0)
                        denomPoly.Text = statementFormatter.FunctionStatementCreate(denomTerms);
                }
                else
                {
                    denomPoly.Text = string.Empty;
                }

                numerLabel.Enabled = true;
                numerPoly.Enabled = true;
                denomPoly.Enabled = false;
            }
            else if (workspaceSettings.method == OptionsDialog.NEWTON2)
            {
                numerLabel.Enabled = true;
                numerPoly.Enabled = true;

                denomLabel.Enabled = true;
                denomPoly.Enabled = true;
            }
            else if (workspaceSettings.method == OptionsDialog.SECANT)
            {
                numerLabel.Enabled = true;
                numerPoly.Enabled = true;
            }
            else if (workspaceSettings.method == OptionsDialog.MANDELBROT)
            {
                // xmin:-2.5, ymin:-1.25, xmax:1.38, ymax:1.25
                xmin.Text = "-2.5";
                xmax.Text = "1.38";
                ymin.Text = "-1.25";
                ymax.Text = "1.25";
            }
        }

        private async void MovieSave()
        {
            ProjectMovieDialog dialog = new ProjectMovieDialog(projectMenu, 20);
            dialog.Settings = workspaceSettings;
            dialog.MovieName = (Stringy.IsEmpty(workspaceSettings.movieName)
                ? $"{projectName}.mp4" : workspaceSettings.movieName);

            if (dialog.ShowDialog() == DialogResult.Cancel)
                return;

            workspaceSettings = dialog.Settings;

            if (!fractalDb.IsWorkspaceID(projectID))
            {
                string json = workspaceSettings.Serialize();
                fractalDb.SettingsSave(projectID, json);
            }

            string[] imageFiles = Directory.GetFiles(workspaceSettings.movieFolder, "img*.png");
            string[] movieFile = Directory.GetFiles(workspaceSettings.movieFolder, workspaceSettings.movieName);
            List<int> selectedIndices = grid.SelectedIndices(true);

            ProjectMovieDirectives PMDialog = new ProjectMovieDirectives(projectMenu, 20);
            PMDialog.ImageFilesOverwrite = (imageFiles.Length > 0);
            PMDialog.MoveFileOverwrite = (movieFile.Length > 0);
            PMDialog.UseSelectedRange = (selectedIndices.Count > 1);

            if (PMDialog.ShowDialog() == DialogResult.Cancel)
                return;

            if (PMDialog.ImageFilesOverwrite)
            {
                foreach (string file in imageFiles)
                { File.Delete(file); }
            }

            if (PMDialog.MoveFileOverwrite)
            {
                foreach (string file in movieFile)
                { File.Delete(file); }
            }

            int indxA = 0;
            int indxB = grid.RowCount;
            if (PMDialog.UseSelectedRange)
            {
                indxA = selectedIndices[0];
                indxB = selectedIndices[1] + 1;
            }

            WaitCursorStart();

            // It is a rare occasion to save movie images in reverse order.
            // Restoring the option will require additional UI changes.
            //    MovieBitmapsSave(indxA, indxB, reverseSave.Checked);
            cancelTokenSource = new CancellationTokenSource();
            cancelToken = cancelTokenSource.Token;
            var task = Task.Run(() =>
            {
                MovieBitmapsSave(cancelToken, indxA, indxB, reverse: false);
            }, cancelToken);

            StopEnable();

            bool stitch = false;
            try
            {
                await task;
                WaitCursorStop();
                stitch = true;
            }
            catch (OperationCanceledException)
            {
                Debug.WriteLine($"MovieSave: {nameof(OperationCanceledException)} thrown\n");
                WaitCursorStop();
            }

            if (stitch)
                MovieStitch(dialog.ProcessShow);

            RunEnable();

            string moviePath = Path.Combine(workspaceSettings.movieFolder, workspaceSettings.movieName);
            MovieOpen(moviePath);
        }

        /// <summary>Stiches together the .png files into a .mp4</summary>
        private void MovieStitch(bool processShow)
        {
            int showCmdWindow = (processShow ? 1 : 0);  // set to zero to hide the command window

            Process process = new Process();
            process.StartInfo.Verb = "runas";

            process.StartInfo.Arguments = string.Format("{0} {1} {2} {3} {4}",
                        workspaceSettings.movieFolder,
                        workspaceSettings.movieName,
                        workspaceSettings.movieWater,
                        workspaceSettings.movieRate,
                        showCmdWindow
                    );

            process.StartInfo.FileName = workspaceSettings.movieGenerator;

            if (showCmdWindow == 0)
                process.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;

            process.Start();
            Debug.WriteLine($"MovieStitch({process.Id})");
            process.WaitForExit();
            Debug.WriteLine("MovieStitch(exited)");
        }

#if false
        // Open for display using the default associated application.
        //   See also: https://csharpforums.net/threads/process-start-more-image.5557/
        private void MovieOpen(string path)
        {
            ProcessStartInfo Process_Info = new ProcessStartInfo(path, @"%SystemRoot%\System32\rundll32.exe % ProgramFiles %\Windows Photo Viewer\PhotoViewer.dll, ImageView_Fullscreen %1")
            {
                UseShellExecute = true,
                WorkingDirectory = Path.GetDirectoryName(path),
                FileName = path,
                Verb = "OPEN"
            };
            Process.Start(Process_Info);
        }
#else
        private void MovieOpen(string path)
        {
            int showCmdWindow = 0;  // set to zero to hide the command window

            Process process = new Process();
            process.StartInfo.Verb = "runas";

            process.StartInfo.Arguments = $"\"{path}\" {showCmdWindow}";
            process.StartInfo.FileName = workspaceSettings.movieViewer;

            if (showCmdWindow == 0)
                process.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;

            process.Start();
            Debug.WriteLine($"MovieOpen({process.Id})");
            process.WaitForExit();
            Debug.WriteLine("MovieOpen(exited)");
        }
#endif

        //=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=
        // Domain-related methods.
        //=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=

        private void PictureSizeAdjust(bool useReducedSize)
        {
            // NOTE: The location of the glControl is relative to its container (picturePanel). So, ....
            if (useReducedSize)
            {
                double REDUCTION = 0.5;
                int reducedWidth = (int)(REDUCTION * picturePanel.Width);
                int reducedHeight = (int)(REDUCTION * picturePanel.Height);
                Size centerPt = new Size(picturePanel.Size.Width / 2, picturePanel.Size.Height / 2);
                glControl.Location = new Point(centerPt.Width - (reducedWidth / 2), centerPt.Height - (reducedHeight / 2));
                glControl.Size = new Size(reducedWidth, reducedHeight);
            }
            else
            {
                glControl.Location = new Point(0, 0);
                glControl.Size = picturePanel.Size;
            }

            // CRITICAL: The database must know the size of retrieved bitmaps;
            fractalDb.BitmapSize = glControl.Width * glControl.Height * 4;  // rgba

            theBitmap = new byte[fractalDb.BitmapSize];
            if (calibrating)
                texture = Texture.LoadFromFile(@"C:\_sandbox\FractalFactory\FractalFactory\calibrate.png");
            else
                texture = Texture.LoadFromMemory(theBitmap, glControl.Width, glControl.Height);

            texture.Orientation = workspaceSettings.orientation;

            center.X = (double)(glControl.Width / 2);
            center.Y = (double)(glControl.Height / 2);

            if (calibrating)
            {
                ViewDataClear();
                ViewBase();
                Render();
            }
            else
            {
                ImageClear();
            }
        }

        /// <summary>Gets values from the domain controls, normalizes them and then creates a domain using the adusted values.</summary>
        /// <returns>a domain based on the values of the domain control</returns>
        private Domain DomainFromInputs()
        {
            Domain domain = RawDomain();
            AdjustDomainToWindow(domain);
            DomainValuesReflect(domain);

            return domain;
        }

        private Domain RawDomain()
        {
            bool success = false;

            double x0 = 0;
            success = Double.TryParse(xmin.Text, out x0);

            double y0 = 0;
            success = Double.TryParse(ymin.Text, out y0);

            double x1 = 0;
            success = Double.TryParse(xmax.Text, out x1);

            double y1 = 0;
            success = Double.TryParse(ymax.Text, out y1);

            return new Domain(x0, y0, x1, y1);
        }

        private void DomainValuesReflect(Domain domain)
        {
            Invoke((Action)(() =>
            {
                int prec = PrecisionAsInt();
                AdjustDomainToWindow(domain);

                DomainValueReflect(xmin, domain.Xmin, prec);
                DomainValueReflect(ymin, domain.Ymin, prec);
                DomainValueReflect(xmax, domain.Xmax, prec);
                DomainValueReflect(ymax, domain.Ymax, prec);
            }));
        }

        private void DomainValueReflect(TextBox tb, double dval, int precision)
        {
            tb.Text = Stringy.DoubleFormat(dval, precision);
            tb.Update();
        }

        private bool IsDomainDefined()
        {
            return (NonEmpty(xmin) && NonEmpty(ymin) && NonEmpty(xmax) && NonEmpty(ymax));
        }

        private void AdjustDomainToWindow(Domain domain)
        {
            double xc = domain.Xc();
            double yc = domain.Yc();
            double dx = domain.Dx();
            double dy = domain.Dy();

            double ratio;
            if (workspaceSettings.orientation == 0)
            {
                if (dx > dy)
                {
                    ratio = (double)glControl.Height / (double)glControl.Width;
                    dy = 0.5f * (dx * ratio);
                    domain.Ymin = yc - dy;
                    domain.Ymax = yc + dy;
                }
                else
                {
                    ratio = (double)glControl.Width / (double)glControl.Height;
                    dx = 0.5f * (dy * ratio);
                    domain.Xmin = xc - dx;
                    domain.Xmax = xc + dx;
                }
            }
            else
            {
                if (dx > dy)
                {
                    ratio = (double)glControl.Width / (double)glControl.Height;
                    dy = 0.5f * (dx * ratio);
                    domain.Ymin = yc - dy;
                    domain.Ymax = yc + dy;
                }
                else
                {
                    ratio = (double)glControl.Height / (double)glControl.Width;
                    dx = 0.5f * (dy * ratio);
                    domain.Xmin = xc - dx;
                    domain.Xmax = xc + dx;
                }
            }

            int prec = PrecisionAsInt();
            domain.Xmin = (double)System.Decimal.Round((decimal)domain.Xmin, prec, MidpointRounding.ToZero);
            domain.Ymin = (double)System.Decimal.Round((decimal)domain.Ymin, prec, MidpointRounding.ToZero);
            domain.Xmax = (double)System.Decimal.Round((decimal)domain.Xmax, prec, MidpointRounding.ToZero);
            domain.Ymax = (double)System.Decimal.Round((decimal)domain.Ymax, prec, MidpointRounding.ToZero);
        }


        //=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=
        // Miscellaneous (and view-related) methods.
        //=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=

        private Point TopLeft(Rectangle rect)
        {
            return (new Point(rect.Left, rect.Top));
        }

        private Point BottomRight(Rectangle rect)
        {
            return (new Point(rect.Right, rect.Bottom));
        }

        private bool HaveSelectedImages()
        {
            List<int> selectedIndices = grid.SelectedIndices();
            foreach (int rowNumber in selectedIndices)
            {
                if (fractalDb.WorkspaceHasImageAt(rowNumber))
                    return true;
            }

            return false;
        }

        /// <summary>Insert a statement at or below the current statement.</summary>
        private void StatementRecord(bool insertBelow, string statement)
        {
            int rowNumber = grid.StatementInsert(insertBelow, statement);
            Debug.Assert(rowNumber >= 0);

            fractalDb.WorkspaceRecordNew(statement, rowNumber);
            if (imagePending)
            {
                // The Generate button was used to generate a new image.
                fractalDb.WorkspaceImageUpdate(rowNumber, theBitmap);
                imagePending = false;
            }
            else
            {
                ImageClear();
            }
        }

        /// <summary>Updates the indicated grid statement and its associated database image.</summary>
        private void StatementUpdate(int rowNumber, string statement)
        {
            grid.StatementUpdate(rowNumber, statement);

            if (fractalDb.WorkspaceStatementUpdate(rowNumber, statement))
            {
                if (imagePending)
                {
                    // The Generate button was used to generate a new image.
                    fractalDb.WorkspaceImageUpdate(rowNumber, theBitmap);
                    imagePending = false;
                }
            }
        }

        private bool ConditionalInterpolationEnable()
        {
            // This is simply a convenient location for this call
            //   ... which has nothing to do with interpolation.
            clear.Enabled = HaveSelectedImages();

            // Assume failure.
            interpolate.Enabled = false;
            divs.Enabled = false;

            smooth.Enabled = false;
            steps.Enabled = false;

            int count = grid.SelectedRows.Count;
            if (count < 2)
                return false;

            if (count > 2)
            {
                smooth.Enabled = true;
                steps.Enabled = true;
            }
            else
            {
                List<int> selectedIndices = grid.SelectedIndices(true);
                interpolate.Enabled = InterpolationAllowed(selectedIndices);
                divs.Enabled = interpolate.Enabled;
            }

            return (interpolate.Enabled || smooth.Enabled);
        }

        private bool InterpolationAllowed(List<int> selectedIndices)
        {
            bool allowInterpolation = false;

            if (AdjacentSelectedIndices(selectedIndices))
            {
                List<string> selectedText = new List<string>();
                selectedText.Add(grid.StatementAt(selectedIndices[0]));
                selectedText.Add(grid.StatementAt(selectedIndices[1]));

                allowInterpolation = statementProcessor.InterpolationAllowed(selectedText);
            }

            return allowInterpolation;
        }

        private bool AdjacentSelectedIndices(List<int> selectedIndices)
        {
            return ((selectedIndices.Count == 2) && (selectedIndices[1] == selectedIndices[0] + 1));
        }

        private void BitmapShowByRowNumber(int rowNumber)
        {
            // ASSUMPTION: The Generate and Record buttons were clicked, so there is an
            // image pending commit to the database. However, inserting a new statement
            // causes the grid active row to change. Sans the following intervention the
            // pending image will be lost and and the wrong image (if any) will be saved
            // with the newly recorded statement.
            if (imagePending)
                return;

            if (fractalDb.WorkspaceImageAt(rowNumber, theBitmap))
            {
                texture.Update();
                Render();
            }
            else
            {
                ImageClear();
            }
        }

        private void CurrentState(int startingRowNumber, out SortedDictionary<string, string> state)
        {
            state = new SortedDictionary<string, string>();

            int rowNumber = startingRowNumber;
            while (rowNumber >= 0)
            {
                string statement = grid.StatementAt(rowNumber);
                ParseStatus status = statementProcessor.Parse(statement);

                if (statementProcessor.IsMaskSatisfied(status, ParseStatus.ONE_DOMAIN) && !state.ContainsKey("domain:"))
                    state["domain:"] = DomainExtract(statement);

                if (statementProcessor.IsMaskSatisfied(status, ParseStatus.ONE_FX) && !state.ContainsKey(Stringy.NUMERC))
                    state[Stringy.NUMERC] = statementProcessor.FX[0];

                if (statementProcessor.IsMaskSatisfied(status, ParseStatus.ONE_DFX) && !state.ContainsKey(Stringy.DENOMC))
                    state[Stringy.DENOMC] = statementProcessor.DFX[0];

                if (state.Count == 3)
                    break;

                --rowNumber;
            }

            if (!state.ContainsKey("domain:"))
                state["domain:"] = string.Empty;

            if (!state.ContainsKey(Stringy.NUMERC))
                state[Stringy.NUMERC] = string.Empty;

            if (!state.ContainsKey(Stringy.DENOMC))
                state[Stringy.DENOMC] = string.Empty;
        }

        private string DomainExtract(string statement)
        {
            int indx = statement.IndexOf("xmin:");
            if (indx < 0)
                return string.Empty;

            statementProcessor.Parse(statement);

            Domain domain = statementProcessor.Domains[0];
            AdjustDomainToWindow(domain);

            return statementFormatter.DomainStatementCreate(domain);
        }

        private void SetParameters(bool includeDomain)
        {
            FractalDBWalker localWalker = new FractalDBWalker(fractalDb);

            int initialRow = grid.ActiveRow;

            IterationFrame frame;
            while (true)
            {
                // Allow partial results.
                int indx = localWalker.NextFrame(out frame, true);
                if (indx < 0)
                    return;  // Didn't find any useable data.

                if (indx == initialRow)
                    break;  // Enough data was found up to this point.

                if (indx > initialRow)
                    break;  // Had to read beyond the current grid row to get enough data,.
            }

            if (frame.FX != string.Empty)
            {
                numerPoly.Text = PolyFormatter.Format(frame.FX);

                if (workspaceSettings.method == OptionsDialog.NEWTON1)
                {
                    PolyTerms terms = Poly.TermsGet(numerPoly.Text, PolyFunction.DERIVATIVE);
                    denomPoly.Text = statementFormatter.FunctionStatementCreate(terms);  // TODO: Use PolyFormatter instead?
                }
            }

            if ((workspaceSettings.method == OptionsDialog.NEWTON2) && (frame.DFX != string.Empty))
                denomPoly.Text = PolyFormatter.Format(frame.DFX);

            if (includeDomain && frame.Domain.IsValid())
                DomainValuesReflect(frame.Domain);

            ViewBase();

            singleFrame.Checked = true;

            // ASSUMPTION: The input controls were actually set.
            generate.Enabled = true;

            // Anticipating the control inputs may be modified modified and/or
            // an image generated, this allows updating data associated with
            // the currently selected grid statement.
            update.Enabled = true;
        }

        // Returns the count of fields set.
        private int TextProcess(string statement)
        {
            int count = 0;

            if (initializingControlPanel)
                return count;

            StatementParser sp = new StatementParser();
            if (!sp.Parse(statement))
                return count;

            // TODO: Error checking (ie. polynomials and floating point values should be syntactically correct).

            if (sp.HasA(Stringy.NUMER))
            {
                numerPoly.Text = PolyFormatter.Format(sp.Value(Stringy.NUMER));
                ++count;

                if (workspaceSettings.method == OptionsDialog.NEWTON1)
                {
                    PolyTerms terms = Poly.TermsGet(numerPoly.Text, PolyFunction.DERIVATIVE);
                    denomPoly.Text = statementFormatter.FunctionStatementCreate(terms);  // TODO: Use PolyFormatter instead?
                }
            }

            if (sp.HasA(Stringy.DENOM) && (workspaceSettings.method == OptionsDialog.NEWTON2))
            {
                denomPoly.Text = PolyFormatter.Format(sp.Value(Stringy.DENOM));
                ++count;
            }

            if (sp.HasA("xmin"))
            {
                if (Double.TryParse(sp.Value("xmin"), out double value))
                {
                    xmin.Text = sp.Value("xmin");
                    ++count;
                }
            }

            if (sp.HasA("ymin"))
            {
                if (Double.TryParse(sp.Value("ymin"), out double value))
                {
                    ymin.Text = sp.Value("ymin");
                    ++count;
                }
            }

            if (sp.HasA("xmax"))
            {
                if (Double.TryParse(sp.Value("xmax"), out double value))
                {
                    xmax.Text = sp.Value("xmax");
                    ++count;
                }
            }

            if (sp.HasA("ymax"))
            {
                if (Double.TryParse(sp.Value("ymax"), out double value))
                {
                    ymax.Text = sp.Value("ymax");
                    ++count;
                }
            }

            return count;
        }


        private bool NonEmpty(TextBox tb)
        {
            return (tb.Text.Length > 0);
        }

        private string MsgFormat(string template, string projectName)
        {
            string fmt = template + "\"{0}\"";
            return string.Format(fmt, projectName);
        }

        private bool Interpolate(bool interactive, int divisions)
        {
            PolyInterpolator? numerInterpolator = null;
            PolyInterpolator? denomInterpolator = null;
            DomainInterpolator? domainInterpolator = null;

            bool haveFX = statementProcessor.IsMaskSatisfied(statementProcessor.Classification, ParseStatus.TWO_FX);
            bool haveDFX = statementProcessor.IsMaskSatisfied(statementProcessor.Classification, ParseStatus.TWO_DFX);
            bool haveDomain = statementProcessor.IsMaskSatisfied(statementProcessor.Classification, ParseStatus.TWO_DOMAIN);

            PolyTerms? Fx0 = null;
            PolyTerms? FxN = null;
            if (haveFX)
            {
                Fx0 = Poly.TermsGet(statementProcessor.FX[0], PolyFunction.VERBATIM);
                FxN = Poly.TermsGet(statementProcessor.FX[1], PolyFunction.VERBATIM);
                PolyTerms.Balance(Fx0, FxN);
            }

            PolyTerms? DFx0 = null;
            PolyTerms? DFxN = null;
            if (haveDFX)
            {
                DFx0 = Poly.TermsGet(statementProcessor.DFX[0], PolyFunction.VERBATIM);
                DFxN = Poly.TermsGet(statementProcessor.DFX[1], PolyFunction.VERBATIM);
                PolyTerms.Balance(DFx0, DFxN);
            }

            double prec = PrecisionAsDouble();

            if (haveDomain)
            {
                Domain i = statementProcessor.Domains[0];
                Domain f = statementProcessor.Domains[1];

                if (!i.EqualTo(f, prec))
                    domainInterpolator = new DomainInterpolator(i, f, divisions);
            }

            if (CanProcess(Fx0, FxN, prec) || CanProcess(DFx0, DFxN, prec))
            {
                if (interactive)
                {
                    InterpolationSetupDialog dialog = new InterpolationSetupDialog(grid, 20);
                    dialog.Precision = PrecisionAsInt();
                    dialog.PolynomialsInit(Fx0, FxN, DFx0, DFxN);

                    if (dialog.ShowDialog() == DialogResult.Cancel)
                        return false;

                    numerInterpolator = ((dialog.NumerDirectives.Count > 0)
                        ? new PolyInterpolator(dialog.NumerDirectives, divisions) : null);

                    denomInterpolator = ((dialog.DenomDirectives.Count > 0)
                        ? new PolyInterpolator(dialog.DenomDirectives, divisions) : null);
                }
                else
                {
                    List<InterpolationDirective> numerDirectives = new List<InterpolationDirective>();
                    List<InterpolationDirective> denomDirectives = new List<InterpolationDirective>();

                    PolyInterpolator.InterpolationDirectivesInit(Fx0, FxN, numerDirectives);
                    PolyInterpolator.InterpolationDirectivesInit(DFx0, DFxN, denomDirectives);

                    numerInterpolator = ((numerDirectives.Count > 0)
                        ? new PolyInterpolator(numerDirectives, divisions) : null);
                    denomInterpolator = ((denomDirectives.Count > 0)
                        ? new PolyInterpolator(denomDirectives, divisions) : null);
                }
            }

            List<string> cache = new List<string>();
            if ((numerInterpolator != null) || (denomInterpolator != null) || (domainInterpolator != null))
            {
                StringBuilder sb = new StringBuilder();
                for (int indx = 1; indx < divisions; ++indx)
                {
                    sb.Clear();

                    if (numerInterpolator != null)
                    {
                        PolyTerms terms = numerInterpolator.Interpolate(indx);
                        sb.Append(statementFormatter.FunctionStatementCreate(Stringy.NUMER, terms));
                    }

                    if (denomInterpolator != null)
                    {
                        if (sb.Length > 0)
                            sb.Append(", ");

                        PolyTerms terms = denomInterpolator.Interpolate(indx);
                        sb.Append(statementFormatter.FunctionStatementCreate(Stringy.DENOM, terms));
                    }

                    if (domainInterpolator != null)
                    {
                        if (sb.Length > 0)
                            sb.Append(", ");

                        Domain domain = domainInterpolator.Interpolate(indx);
                        sb.Append(statementFormatter.DomainStatementCreate(domain));
                    }

                    if (sb.Length > 0)
                        cache.Add(sb.ToString());
                }
            }

            if (cache.Count > 0)
            {
                List<int> indices = grid.SelectedIndices(true);

                int rowNumber = indices[0];       // Because all rows beneath this will be renumbered
                int rowMax = grid.RowCount - 1;  // Because row indices a zero-based.

                if (rowNumber < rowMax)
                    fractalDb.DisplayOrderUpdate(rowNumber, cache.Count);

                foreach (string statement in cache)
                {
                    StatementRecord(false, statement);
                }

                ProjectMenuItemsEnable();
            }

            return (cache.Count > 0);
        }

        private int Extend(ParseStatus classification, List<InterpolationDirective> directives, int count, bool below)
        {
            if (directives == null)
                return 0;

            PolyInterpolator? interpolator = ((directives.Count > 0)
                ? new PolyInterpolator(directives, count) : null);

            string prefix = ((classification == ParseStatus.TWO_FX) ? Stringy.NUMER : Stringy.DENOM);

            List<string> cache = new List<string>();
            if (interpolator != null)
            {
                int lb = (below ? 1 : 0);
                int ub = (below ? (count + 1) : count);

                StringBuilder sb = new StringBuilder();
                for (int indx = lb; indx < ub; ++indx)
                {
                    sb.Clear();

                    PolyTerms terms = interpolator.Interpolate(indx);
                    sb.Append(" ");
                    sb.Append(statementFormatter.FunctionStatementCreate(prefix, terms));

                    if (sb.Length > 0)
                        cache.Add(sb.ToString());
                }

                if (cache.Count > 0)
                {
                    int rowNumber = grid.ActiveRow;
                    int rowMax = grid.RowCount - 1;  // Because row indices a zero-based.

                    if (rowNumber < rowMax)
                        fractalDb.DisplayOrderUpdate(rowNumber, cache.Count);

                    foreach (string statement in cache)
                    {
                        StatementRecord(below, statement);
                    }

                    ProjectMenuItemsEnable();
                }
            }

            return cache.Count;
        }

        private bool CanProcess(PolyTerms? numer, PolyTerms? denom, double precision)
        {
            if ((numer == null) || (denom == null))
                return false;

            return (!numer.EqualTo(denom, precision));
        }

        // Set the texture to solid white.
        private void ImageClear()
        {
            if (!calibrating)
            {
                byte[] data = texture.Values;
                for (int i = 0; i < data.Length; ++i)
                { data[i] = 255; }

                texture.Update();
            }

            Render();
        }

        /// <summary>Copies the selected statements into the system Clipboard.</summary>
        public void ClipboardCopy()
        {
            List<int> selectedIndices = grid.SelectedIndices();
            if (selectedIndices.Count < 1)
                return;

            StringBuilder sb = new StringBuilder();
            foreach (int indx in selectedIndices)
            {
                if (sb.Length > 0)
                    sb.Append("\n");

                sb.Append(grid.StatementAt(indx));
            }

            Clipboard.SetText(sb.ToString());
        }

        private void ClipboardPaste()
        {
            char[] EOL = { '\n' };
            string text = Clipboard.GetText().Replace("\r", "");

            string[] rawInput = text.Split(EOL, StringSplitOptions.RemoveEmptyEntries);
            if (rawInput.Length < 1)
                return;  // nothing to paste

            List<string> statements = new List<string>();
            foreach (string str in rawInput)
            {
                string statement = fractalDb.Preprocess(str);
                if (statement.Length < 1)
                    continue;

                statements.Add(statement);
            }

            List<int> selectedIndices = grid.SelectedIndices(true);
            if (selectedIndices.Count > 1)
                return;

            int rowNumber;
            bool pasteBelow = true;
            if (selectedIndices.Count == 0)
            {
                // ASSUMPTION: A zero selection count indicates we have an empty grid.
                rowNumber = 0;
            }
            else
            {
                rowNumber = selectedIndices[0];

                PasteDialog dialog = new PasteDialog(grid, 20);
                if (dialog.ShowDialog() == DialogResult.Cancel)
                    return;

                pasteBelow = dialog.PasteBelow;
            }

            fractalDb.TransactionBegin();

            int rowMax = grid.RowCount - 1;  // Because row indices a zero-based.
            if (rowMax > 0)
            {
                if ((rowNumber < rowMax) || ((rowNumber == rowMax) && !pasteBelow))
                {
                    if (!pasteBelow)
                        --rowNumber;

                    fractalDb.DisplayOrderUpdate(rowNumber, statements.Count);
                }
            }

            foreach (string statement in statements)
            {
                StatementRecord(pasteBelow, statement);
            }

            fractalDb.TransactionEnd();

            ProjectMenuItemsEnable();
        }

        /// <summary>Deletes the selects rows.</summary>
        private bool SelectedRowsDelete(bool usingCtrlX)
        {
            List<int> selectedIndices = grid.SelectedIndices();
            if (selectedIndices.Count < 1)
                return false;

            // The row index beneath which we must update the display order.
            int baseIndex = selectedIndices[0] - 1;

            if (usingCtrlX)
            {
                // Whereas the Delete key is handled by the grid control, <ctrl>X is not, so ....
                foreach (int rowNumber in selectedIndices)
                {
                    grid.StatementDelete(rowNumber);
                }
            }

            fractalDb.TransactionBegin();

            foreach (int rowNumber in selectedIndices)
            {
                fractalDb.WorkspaceRecordDelete(rowNumber);
            }

            fractalDb.DisplayOrderUpdate(baseIndex);

            fractalDb.TransactionEnd();

            ProjectMenuItemsEnable();

            return true;
        }

    }
}
