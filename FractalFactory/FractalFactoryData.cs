using FractalFactory.Common;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace FractalFactory
{
    // Grid and database editing functionality.
    public partial class FractalFactory : Form
    {
        private bool SelectedImagesClear(string message)
        {
            List<int> selectedIndices = grid.SelectedIndices();
            int count = selectedIndices.Count;
            if (count <= 0)
                return false;

            // ASSUMPTION: The Clear button was enabled because some selected rows have images.
            DialogResult status = MessageDialog.Show(grid, 20, "Warning!", message, MessageBoxButtons.YesNo);
            if (status == DialogResult.No)
                return false;

            foreach (int rowNumber in selectedIndices)
            {
                fractalDb.WorkspaceImageClear(rowNumber);
            }

            fractalDb.IsDirty = true;
            ImageClear();

            ProjectStateReflect(false);

            return true;
        }

        private void ExtendExecute()
        {
            ExtendSetupDialog dialog = new ExtendSetupDialog(grid, 20);

            List<int> selectedIndices = grid.SelectedIndices(true);
            int referenceRowNumber = selectedIndices[0];

            dialog.PolynomialsInit(grid, referenceRowNumber);

            dialog.Count = 1;

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                ControlsEnable(false);

                int additions = 0;
                if (dialog.ApplyBelow)
                    additions += Extend(dialog.BelowClassification, dialog.BelowDirectives, dialog.Count, true);

                if (dialog.ApplyAbove)
                {
                    grid.ClearSelection();
                    grid.Rows[referenceRowNumber].Selected = true;
                    additions += Extend(dialog.AboveClassification, dialog.AboveDirectives, dialog.Count, false);
                }

                ControlsEnable(true);
            }
        }

        private void SmoothExecute()
        {
            int divisions = 0;
            Int32.TryParse(steps.Text, out divisions);

            List<int> gridIndices = new List<int>(new int[2]);

            List<int> selectedIndices = grid.SelectedIndices(false);
            int indx = selectedIndices.Count - 2;
            if (indx < 0)
                return;

            ControlsEnable(false);

            // To avoid adversely affecting the selected indices
            // we must iterate through them in reverse order.
            int additions = 0;
            while (indx >= 0)
            {
                gridIndices[0] = selectedIndices[indx];
                gridIndices[1] = selectedIndices[indx + 1];

                if (InterpolationAllowed(gridIndices))
                {
                    grid.ClearSelection();
                    grid.Rows[gridIndices[0]].Selected = true;
                    grid.Rows[gridIndices[1]].Selected = true;
                    if (Interpolate(false, divisions))
                        ++additions;
                }

                --indx;
            }

            ControlsEnable(true);

            smooth.Enabled = false;
            multiFrame.Checked = (additions > 0);
        }

        private void InterpolateExecute(object sender, EventArgs e)
        {
            int divisions = 0;
            Int32.TryParse(divs.Text, out divisions);

            if (Interpolate(true, divisions))
            {
                interpolate.Enabled = false;

                if (divisions == 2)
                {
                    // By the time we arrive here grid.ActiveRow is the inserted row.
                    SetParameters(true);
                    generate_Click(sender, e);
                }
            }
        }

        private string StatementGenerate(bool updating)
        {
            StringBuilder sb = new StringBuilder();

            string cs = (updating ? grid.StatementAt(grid.ActiveRow) : string.Empty);

            SortedDictionary<string, string> state;
            CurrentState(grid.ActiveRow, out state);
            if (state.Count == 3)
            {
                string fxAwithSpaces = statementFormatter.FxStatementCreate(state[Stringy.NUMERC]);
                string fxBwithSpaces = statementFormatter.FxStatementCreate(numerPoly.Text);

                string dfxAwithSpaces = statementFormatter.DFxStatementCreate(state[Stringy.DENOMC]);
                string dfxBwithSpaces = statementFormatter.DFxStatementCreate(denomPoly.Text);

                string domAwithSpaces = state["domain:"];
                string domBwithSpaces = statementFormatter.DomainStatementCreate(RawDomain());

                // We must be careful to compare, ignoring whitespace.
                string fxAnoSpaces = Stringy.WhitespaceRemove(fxAwithSpaces);
                string fxBnoSpaces = Stringy.WhitespaceRemove(fxBwithSpaces);

                string dfxAnoSpaces = Stringy.WhitespaceRemove(dfxAwithSpaces);
                string dfxBnoSpaces = Stringy.WhitespaceRemove(dfxBwithSpaces);

                string domAnoSpaces = Stringy.WhitespaceRemove(domAwithSpaces);
                string domBnoSpaces = Stringy.WhitespaceRemove(domBwithSpaces);

                if (updating)
                {
                    if (fxBnoSpaces != fxAnoSpaces)
                        sb.Append(fxBwithSpaces);
                    else if (cs.Contains(Stringy.NUMERC))
                        sb.Append(fxAwithSpaces);

                    if (dfxBnoSpaces != dfxAnoSpaces)
                    {
                        if (sb.Length > 0)
                            sb.Append(", ");

                        sb.Append(dfxBwithSpaces);
                    }
                    else if (cs.Contains(Stringy.DENOMC))
                    {
                        if (sb.Length > 0)
                            sb.Append(", ");

                        sb.Append(dfxAwithSpaces);
                    }

                    if (domBnoSpaces != domAnoSpaces)
                    {
                        if (sb.Length > 0)
                            sb.Append(", ");

                        sb.Append(domBwithSpaces);
                    }
                    else if (cs.Contains("xmin:"))
                    {
                        if (sb.Length > 0)
                            sb.Append(", ");

                        sb.Append(domAwithSpaces);
                    }
                }
                else
                {
                    if (fxBnoSpaces != fxAnoSpaces)
                        sb.Append(fxBwithSpaces);

                    if (dfxBnoSpaces != dfxAnoSpaces)
                    {
                        if (sb.Length > 0)
                            sb.Append(", ");

                        sb.Append(dfxBwithSpaces);
                    }

                    if (domBnoSpaces != domAnoSpaces)
                    {
                        if (sb.Length > 0)
                            sb.Append(", ");

                        sb.Append(domBwithSpaces);
                    }
                }
            }

            return ((sb.Length > 0) ? sb.ToString() : string.Empty);
        }
    }
}
