using FractalFactory.CustomControls;
using FractalFactory.Math;
using FractalFactory.Statements;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace FractalFactory
{
    public partial class ExtendSetupDialog : Form
    {
        private const string NO_ACTION = "No Action";
        private const string APPLY_ABOVE = "Apply Above";
        private const string APPLY_BELOW = "Apply Below";
        private const string APPLY_BOTH = "Apply Both";

        private List<string> polyList;


        public int Count { set; get; }

        public bool ApplyAbove { private set; get; }
        public bool ApplyBelow { private set; get; }

        public ParseStatus AboveClassification { private set; get; }
        public ParseStatus BelowClassification { private set; get; }

        public List<InterpolationDirective> AboveDirectives { private set; get; }
        public List<InterpolationDirective> BelowDirectives { private set; get; }

        public ExtendSetupDialog(Control control, int offset)
        {
            InitializeComponent();

            FormLocator.Locate(this, control, offset);

            polyList = new List<string>();
            Count = 0;
            AboveDirectives = new List<InterpolationDirective>();
            BelowDirectives = new List<InterpolationDirective>();
        }

        public void PolynomialsInit(CustomDbGridCtrl grid, int rowNumber)
        {
            polyList.Add(grid.StatementAt(rowNumber - 1));
            polyList.Add(grid.StatementAt(rowNumber));
            polyList.Add(grid.StatementAt(rowNumber + 1));
        }

        private void ExtendSetupDialog_Load(object sender, EventArgs e)
        {
            StatementProcessor sp = new StatementProcessor(8);  // TODO: Not hardcode to 8?

            GridStylingInit(polyGrid);
            GridStylingInit(deltaGrid);

            List<string> selectedText = new List<string>();
            selectedText.Add(polyList[0]);
            selectedText.Add(polyList[1]);

            deltaGrid.AllowUserToAddRows = true;

            if (sp.InterpolationAllowed(selectedText))
            {
                if (sp.IsMaskSatisfied(sp.Classification, ParseStatus.TWO_FX))
                {
                    AboveClassification = ParseStatus.TWO_FX;

                    PolyTerms Fx0 = Poly.TermsGet(sp.FX[0], PolyFunction.VERBATIM);
                    EquationRowAdd(Fx0, true);

                    PolyTerms Fx1 = Poly.TermsGet(sp.FX[1], PolyFunction.VERBATIM);
                    EquationRowAdd(Fx1, true);

                    DeltaRowAdd(Fx1);

                    action.Items.Add(APPLY_ABOVE);
                }
                else if (sp.IsMaskSatisfied(sp.Classification, ParseStatus.TWO_DFX))
                {
                    AboveClassification = ParseStatus.TWO_DFX;

                    PolyTerms DFx0 = Poly.TermsGet(sp.DFX[0], PolyFunction.VERBATIM);
                    EquationRowAdd(DFx0, true);

                    PolyTerms DFx1 = Poly.TermsGet(sp.DFX[1], PolyFunction.VERBATIM);
                    EquationRowAdd(DFx1, true);

                    DeltaRowAdd(DFx1);

                    action.Items.Add(APPLY_ABOVE);
                }
            }

            selectedText.Clear();
            selectedText.Add(polyList[1]);
            selectedText.Add(polyList[2]);

            if (sp.InterpolationAllowed(selectedText))
            {
                if (sp.IsMaskSatisfied(sp.Classification, ParseStatus.TWO_FX))
                {
                    BelowClassification = ParseStatus.TWO_FX;

                    if (polyGrid.RowCount == 0)
                    {
                        PolyTerms Fx0 = Poly.TermsGet(sp.FX[0], PolyFunction.VERBATIM);
                        EquationRowAdd(Fx0, true);
                    }

                    PolyTerms Fx1 = Poly.TermsGet(sp.FX[1], PolyFunction.VERBATIM);
                    EquationRowAdd(Fx1, true);

                    DeltaRowAdd(Fx1);

                    action.Items.Add(APPLY_BELOW);
                }
                else if (sp.IsMaskSatisfied(sp.Classification, ParseStatus.TWO_DFX))
                {
                    BelowClassification = ParseStatus.TWO_DFX;

                    if (polyGrid.RowCount == 0)
                    {
                        PolyTerms DFx0 = Poly.TermsGet(sp.DFX[0], PolyFunction.VERBATIM);
                        EquationRowAdd(DFx0, true);
                    }

                    PolyTerms DFx1 = Poly.TermsGet(sp.DFX[1], PolyFunction.VERBATIM);
                    EquationRowAdd(DFx1, true);

                    DeltaRowAdd(DFx1);

                    action.Items.Add(APPLY_BELOW);
                }
            }

            if (action.Items.Count == 0)
            {
                action.Items.Add(NO_ACTION);
                action.SelectedItem = NO_ACTION;
            }
            else if (action.Items.Count == 2)
            {
                action.Items.Add(APPLY_BOTH);
                action.SelectedItem = APPLY_BOTH;
            }
            else
            {
                action.SelectedIndex = 0;
            }

            count.Text = Count.ToString();
        }

        private void ok_Click(object sender, EventArgs e)
        {
            string actionStr = action.Text;

            int itmp = 0;
            if (Int32.TryParse(count.Text, out itmp) && (itmp > 0))
                Count = itmp;

            AboveDirectives.Clear();
            BelowDirectives.Clear();

            ApplyAbove = false;
            ApplyBelow = false;

            if (actionStr == APPLY_ABOVE)
                ApplyAbove = true;
            else if (actionStr == APPLY_BELOW)
                ApplyBelow = true;
            else if (actionStr == APPLY_BOTH)
                ApplyAbove = ApplyBelow = true;

            if (ApplyAbove)
                InterpolationDirectivesInit(false, AboveDirectives, Count);

            if (ApplyBelow)
                InterpolationDirectivesInit(true, BelowDirectives, Count);
        }

        private void action_SelectedIndexChanged(object sender, EventArgs e)
        {
            OkButtonEnable();
        }

        private void delta_TextChanged(object sender, EventArgs e)
        {
            OkButtonEnable();
        }

        private void count_TextChanged(object sender, EventArgs e)
        {
            OkButtonEnable();
        }

        private void OkButtonEnable()
        {
            bool enable = false;

            int itmp = 0;
            if (Int32.TryParse(count.Text, out itmp) && (itmp > 0))
                enable = (action.Text != NO_ACTION);

            ok.Enabled = enable;
        }

        private void EquationRowAdd(PolyTerms eq, bool disable)
        {
            if (polyGrid.Rows.Count == 0)
                GridColumnsAdd(polyGrid, eq);

            int row = polyGrid.Rows.Add();
            int col = 0;

            if (disable)
                polyGrid.Rows[row].DefaultCellStyle.BackColor = SystemColors.ControlLight;

            foreach (var value in eq.ToArray())
            {
                polyGrid[col, row].Value = value;
                ++col;
            }

            if (disable)
                polyGrid.Rows[row].ReadOnly = true;
        }

        private void DeltaRowAdd(PolyTerms eq)
        {
            if (deltaGrid.Rows.Count == 0)
            {
                GridColumnsAdd(deltaGrid, eq);

                int row = 0;
                int col = 0;

                foreach (var value in eq.ToArray())
                {
                    deltaGrid[col, row].Value = 0;
                    ++col;
                }
            }
        }

        private void GridColumnsAdd(DataGridView grid, PolyTerms eq)
        {
            int count = 0;
            for (int indx = 0; indx < eq.Count; ++indx)
            {
                grid.Columns.Add(string.Format("{0}", count++), "coeff");
                grid.Columns.Add(string.Format("{0}", count++), "exp");
            }

            foreach (DataGridViewColumn? column in grid.Columns)
            {
                column!.SortMode = DataGridViewColumnSortMode.NotSortable;
                column!.DefaultCellStyle.Format = "F8";
            }
        }

        private void GridStylingInit(DataGridView grid)
        {
            //=-=-= https://stackoverflow.com/questions/1247800/how-to-change-the-color-of-winform-datagridview-header
            grid.ColumnHeadersDefaultCellStyle.BackColor = SystemColors.ControlDark;
            grid.EnableHeadersVisualStyles = false;
            //=-=-=
            grid.SelectionMode = DataGridViewSelectionMode.CellSelect;
            grid.RowHeadersVisible = false;
            polyGrid.ScrollBars = ScrollBars.Horizontal;
        }

        // https://stackoverflow.com/questions/11330147/how-to-disable-the-ability-to-select-in-a-datagridview
        private void polyGrid_SelectionChanged(object sender, EventArgs e)
        {
            polyGrid.ClearSelection();
        }

        private bool InterpolationDirectivesInit(bool below, List<InterpolationDirective> directives, int count)
        {
            double crStart, crDelta;
            double exStart, exDelta;
            double delta;

            if (below)
            {
                for (int indx = 0; indx < polyGrid.ColumnCount; indx += 2)
                {
                    if ((Convert.ToDouble(polyGrid[indx, 1].Value) == 0) && (Convert.ToDouble(polyGrid[indx, 2].Value) == 0))
                        continue;

                    crDelta = 0;
                    crStart = Convert.ToDouble(polyGrid[indx, 1].Value);
                    delta = System.Math.Abs(Convert.ToDouble(deltaGrid[indx, 0].Value));
                    if (delta >= 1e-8)
                    {
                        crDelta = delta * count;

                        double crEnd = Convert.ToDouble(polyGrid[indx, 2].Value);
                        if ((crEnd - crStart) < 0)
                            crDelta = -crDelta;
                    }

                    exDelta = 0;
                    exStart = Convert.ToDouble(polyGrid[indx + 1, 1].Value);
                    delta = System.Math.Abs(Convert.ToDouble(deltaGrid[indx + 1, 0].Value));
                    if (delta >= 1e-8)
                    {
                        exDelta = delta * count;

                        double exEnd = Convert.ToDouble(polyGrid[indx + 1, 2].Value);
                        if ((exEnd - exStart) < 0)
                            exDelta = -exDelta;
                    }

                    InterpolationDirective directive = new InterpolationDirective();
                    directive.cRange.start = crStart;
                    directive.cRange.end = crStart + crDelta;
                    directive.eRange.start = exStart;
                    directive.eRange.end = exStart + exDelta;

                    directives.Add(directive);
                }
            }
            else
            {
                for (int indx = 0; indx < polyGrid.ColumnCount; indx += 2)
                {
                    if ((Convert.ToDouble(polyGrid[indx, 0].Value) == 0) && (Convert.ToDouble(polyGrid[indx, 1].Value) == 0))
                        continue;

                    crDelta = 0;
                    crStart = Convert.ToDouble(polyGrid[indx, 1].Value);
                    delta = System.Math.Abs(Convert.ToDouble(deltaGrid[indx, 0].Value));
                    if (delta >= 1e-8)
                    {
                        crDelta = delta * Count;

                        double crEnd = Convert.ToDouble(polyGrid[indx, 0].Value);
                        if ((crEnd - crStart) < 0)
                            crDelta = -crDelta;
                    }

                    exDelta = 0;
                    exStart = Convert.ToDouble(polyGrid[indx + 1, 1].Value);
                    delta = System.Math.Abs(Convert.ToDouble(deltaGrid[indx + 1, 0].Value));
                    if (delta >= 1e-8)
                    {
                        exDelta = delta * Count;

                        double exEnd = Convert.ToDouble(polyGrid[indx + 1, 0].Value);
                        if ((exEnd - exStart) < 0)
                            exDelta = -exDelta;
                    }

                    InterpolationDirective directive = new InterpolationDirective();
                    directive.cRange.start = crStart + crDelta;
                    directive.cRange.end = crStart;
                    directive.eRange.start = exStart + exDelta;
                    directive.eRange.end = exStart;

                    directives.Add(directive);
                }
            }

            return false;
        }
#if false
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            deltaGrid.AllowUserToAddRows = false;

            return true;
        }
#endif
    }
}
