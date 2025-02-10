using FractalFactory.Math;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace FractalFactory
{
    public partial class InterpolationSetupDialog : Form
    {
        private PolyTerms? Numer0 { set; get; } = null;
        private PolyTerms? NumerN { set; get; } = null;
        private PolyTerms? Denom0 { set; get; } = null;
        private PolyTerms? DenomN { set; get; } = null;
        private double Precision { set; get; } = 0;

        public InterpolationSetupDialog(Control control, int offset)
        {
            InitializeComponent();

            FormLocator.Locate(this, control, offset);
        }

        public List<InterpolationDirective> NumerDirectives { get; private set; } = new List<InterpolationDirective>();
        public List<InterpolationDirective> DenomDirectives { get; private set; } = new List<InterpolationDirective>();

        public bool PolynomialsInit(PolyTerms? numer0, PolyTerms? numerN, PolyTerms? denom0, PolyTerms? denomN, double precision)
        {
            Numer0 = numer0!;
            NumerN = numerN!;
            Denom0 = denom0!;
            DenomN = denomN!;
            Precision = precision;

            if ((numer0 != null) && (numerN == null))
                return false;

            if ((numerN != null) && (numer0 == null))
                return false;

            if ((denom0 != null) && (denomN == null))
                return false;

            if ((denomN != null) && (denom0 == null))
                return false;

            return true;
        }

        private void InterpolationSetup_Load(object sender, EventArgs e)
        {
            GridStylingInit(numerGrid);
            GridStylingInit(denomGrid);

            numerGrid.Visible = ((Numer0 != null) && (NumerN != null) && !Numer0.EqualTo(NumerN, Precision));
            denomGrid.Visible = ((Denom0 != null) && (DenomN != null) && !Denom0.EqualTo(DenomN, Precision));

            numerLabel.Visible = numerGrid.Visible;
            denomLabel.Visible = denomGrid.Visible;

            int DELTAY = 32;
            int top = DELTAY;
            if (numerGrid.Visible)
            {
                GridRowAdd(numerGrid, Numer0!, true);
                GridRowAdd(numerGrid, NumerN!, true);
                GridRowAdd(numerGrid, Numer0!, false);
                GridRowAdd(numerGrid, NumerN!, false);

                if (!denomGrid.Visible)
                    denomGrid.Location = numerGrid.Location;
            }

            if (denomGrid.Visible)
            {
                GridRowAdd(denomGrid, Denom0!, true);
                GridRowAdd(denomGrid, DenomN!, true);
                GridRowAdd(denomGrid, Denom0!, false);
                GridRowAdd(denomGrid, DenomN!, false);

                if (!numerGrid.Visible)
                    denomGrid.Location = numerGrid.Location;

                denomLabel.Location = denomGrid.Location - new Size(0, denomLabel.Size.Height);
            }

            top = denomGrid.Bottom + DELTAY;
            cancel.Top = top;
            ok.Top = top;

            this.Size = new Size(this.Width, top + (int)(2.5 * DELTAY));
        }

        private void ok_Click(object sender, EventArgs e)
        {
            NumerDirectives.Clear();
            DenomDirectives.Clear();

            if (numerGrid.Visible)
                InterpolationDirectivesInit(numerGrid, NumerDirectives);

            if (denomGrid.Visible)
                InterpolationDirectivesInit(denomGrid, DenomDirectives);
        }

        private void GridRowAdd(DataGridView grid, PolyTerms eq, bool disable)
        {
            if (grid.Rows.Count == 0)
                GridColumnsAdd(grid, eq);

            int row = grid.Rows.Add();
            int col = 0;

            if (disable)
                grid.Rows[row].DefaultCellStyle.BackColor = SystemColors.ControlLight;

            foreach (var value in eq.ToArray())
            {
                grid[col, row].Value = value;
                ++col;
            }

            if (disable)
                grid.Rows[row].ReadOnly = true;
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
            }
        }

        // https://stackoverflow.com/questions/11330147/how-to-disable-the-ability-to-select-in-a-datagridview
        private void fxGrid_SelectionChanged(object sender, EventArgs e)
        {
            if (numerGrid.CurrentRow.Index < 2)
                numerGrid.ClearSelection();
        }

        private void dfxGrid_SelectionChanged(object sender, EventArgs e)
        {
            if (denomGrid.CurrentRow.Index < 2)
                denomGrid.ClearSelection();
        }

        private bool InterpolationDirectivesInit(DataGridView grid, List<InterpolationDirective> directives)
        {
            for (int indx = 0; indx < grid.ColumnCount; indx += 2)
            {
                if ((Convert.ToDouble(grid[indx, 2].Value) == 0) && (Convert.ToDouble(grid[indx, 3].Value) == 0))
                    continue;

                InterpolationDirective directive = new InterpolationDirective();
                directive.cRange.start = Convert.ToDouble(grid[indx, 2].Value);
                directive.cRange.end = Convert.ToDouble(grid[indx, 3].Value);
                directive.eRange.start = Convert.ToDouble(grid[indx + 1, 2].Value);
                directive.eRange.end = Convert.ToDouble(grid[indx + 1, 3].Value);

                directives.Add(directive);
            }

            return false;
        }

        private void GridStylingInit(DataGridView grid)
        {
            //=-=-= https://stackoverflow.com/questions/1247800/how-to-change-the-color-of-winform-datagridview-header
            grid.ColumnHeadersDefaultCellStyle.BackColor = SystemColors.ControlDark;
            grid.EnableHeadersVisualStyles = false;
            //=-=-=
            grid.SelectionMode = DataGridViewSelectionMode.CellSelect;
            grid.RowHeadersVisible = false;
            numerGrid.ScrollBars = ScrollBars.Horizontal;
        }
    }
}
