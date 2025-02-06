using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace FractalFactory.CustomControls
{
    public partial class CustomDbGridCtrl : DataGridView
    {
        private const int WM_KEYDOWN = 0x100;
        private const int WM_DOWNARROW = 0x028;
        private const int VK_ESCAPE = 0x01B;

        enum EditingState
        {
            EDITING_IDLE,
            EDITING_ACTIVE,
            EDITING_CANCELED
        }

        // Flag indicating whether a cell is being edited.
        private EditingState editing { set; get; }

        public CustomDbGridCtrl()
        {
            InitializeComponent();

            // For safe keeping (because Visual Studi keeps messing with things).
            this.RowTemplate.Height = 14;
            this.RowTemplate.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;

            editing = EditingState.EDITING_IDLE;
        }

        public bool EditingCanceled { get { return editing == EditingState.EDITING_CANCELED; } }

        public void GridInitialize(List<string> visibleColumnNames)
        {
            AllowUserToAddRows = false;

            try
            {
                this.Columns.Clear();

                int count = visibleColumnNames.Count;
                for (int indx = 0; indx < count; ++indx)
                {
                    this.Columns.Add(visibleColumnNames[indx], string.Empty);
                    this.Columns[indx].Visible = true;
                }

                Columns[visibleColumnNames[count - 1]].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            }
            catch (Exception ex)
            {
                MessageBox.Show("GridInitialize(): " + ex.Message);
            }
        }

        public void Clear()
        {
            this.Rows.Clear();
        }

        public List<int> SelectedIndices(bool onlySelectedExtremes = false)
        {
            List<int> selectedIndices = new List<int>();

            // Similar to the comment in ClipboardPaste(), we do this to
            // avoid encountering an "access to null object" error.
            AllowUserToAddRows = false;

            // Whatever the reason, SelectedRows is not in my prefered order so from
            //   https://stackoverflow.com/questions/4371514/get-selected-datagridviewrows-in-currently-displayed-order
            List<DataGridViewRow> rows =
                (from DataGridViewRow row in SelectedRows
                 where !row.IsNewRow
                 orderby row.Index
                 select row).ToList<DataGridViewRow>();

            if (rows.Count > 0)
            {
                if (onlySelectedExtremes)
                {
                    selectedIndices.Add(rows[0].Index);
                    if (rows.Count > 1)
                        selectedIndices.Add(rows[rows.Count - 1].Index);
                }
                else
                {
                    foreach (DataGridViewRow row in rows)
                    { selectedIndices.Add(row.Index); }
                }
            }

            return selectedIndices;
        }

        public int ActiveRow
        {
            set
            {
                int currCol = ((CurrentCell == null) ? 0 : CurrentCell.ColumnIndex);
                if (value < RowCount)
                {
                    CurrentCell = this[currCol, value];
                    Update();
                }
            }

            get
            {
                return ((CurrentCell == null) ? 0 : CurrentCell.RowIndex);
            }
        }

        public int MaxVisibleRows { get { return (this.Height / this.ColumnHeadersHeight); } }

        public void StatementSelect(int rowNumber, bool select)
        {
            if (Rows.Count > 0)
                Rows[rowNumber].Selected = select;
        }

        public string StatementAt(int rowNumber)
        {
            string text = string.Empty;
            if ((rowNumber >= 0) && (rowNumber < RowCount))
            {
                object obj = this[0, rowNumber].Value;
                if (obj != null)
                {
                    try { text = obj.ToString(); }
                    catch { }
                }
            }

            return text;
        }

        /// <summary>Insert a statement at or below the current statement.</summary>
        /// <returns>(-1) failed / (otherwise) index of inserted row</returns>
        public int StatementInsert(bool insertBelow, string text)
        {
            int rowNumber = -1;

            List<int> selectedIndices = SelectedIndices();
            int selectionCount = 0;

            if (RowCount == 0)
            {
                Rows.Add(text);
                rowNumber = RowCount - 1;
            }
            else
            {
                selectionCount = selectedIndices.Count;
                if (selectionCount > 0)
                {
                    if (insertBelow)
                        rowNumber = selectedIndices[selectionCount - 1] + 1;
                    else
                        rowNumber = selectedIndices[selectionCount - 1];

                    Rows.Insert(rowNumber, text);
                }
            }

            if (rowNumber >= 0)
            {
                ActiveRow = rowNumber;

                if (!insertBelow && (selectionCount > 0))
                {
                    ClearSelection();
                    Rows[selectedIndices[0]].Selected = true;
                    Rows[selectedIndices[selectionCount - 1] + 1].Selected = true;
                }
            }

            return rowNumber;
        }

        /// <summary>Inserts a statement at the specified position.</summary>
        /// <returns>(true) success / (false) failure</returns>
        public bool StatementInsert(int rowNumber, string text)
        {
            bool success = ((rowNumber >= 0) && (rowNumber <= RowCount));

            if (success)
            {
                ActiveRow = rowNumber;
                Rows.Insert(rowNumber, text);
            }

            return success;
        }

        /// <summary>Updates the indicated grid statement.</summary>
        /// <returns>{true) success / (false) failure</returns>
        public bool StatementUpdate(int rowNumber, string text)
        {
            bool success = false;

            if ((RowCount > 0) && (rowNumber >= 0))
            {
                this["Text", rowNumber].Value = text;

                success = true;
            }

            return success;
        }

        public void StatementDelete(int rowNumber)
        {
            if ((rowNumber < 0) || (rowNumber >= RowCount))
                return;

            Rows.RemoveAt(rowNumber);
        }

        protected override void OnCellMouseEnter(System.Windows.Forms.DataGridViewCellEventArgs e)
        {
            this.ShowCellToolTips = false;
            this.Cursor = Cursors.Default;
        }

        protected override void OnCellMouseUp(DataGridViewCellMouseEventArgs e)
        {
            int currRow = CurrentCell.RowIndex;
        }

        // This (intercepting the down-arrow key when the terminal row is active)
        // was a bitch to figure out! Without this bit of code, the down-arrow
        // key is ignored.
        protected override bool ProcessKeyPreview(ref Message m)
        {
            if ((m.Msg == WM_KEYDOWN) && ((int)m.WParam == VK_ESCAPE))
            {
                editing = EditingState.EDITING_CANCELED;

                EndEdit();
            }

            return base.ProcessKeyPreview(ref m);
        }

        protected override void OnCellBeginEdit(DataGridViewCellCancelEventArgs e)
        {
            Object obj = this["Text", e.RowIndex].Value;
            if (obj == null)
                return;

            editing = EditingState.EDITING_ACTIVE;
        }

#if false
        // DataGridView.ProcessDialogKey(Keys) Method appeared on web search for "C# DataGridView escape key"
        // https://learn.microsoft.com/en-us/dotnet/api/system.windows.forms.datagridview.processdialogkey?view=windowsdesktop-9.0
        protected override bool ProcessDialogKey(Keys keyData)
        {
            // Extract the key code from the key value. 
            Keys key = (keyData & Keys.Escape);

            // Handle the ENTER key as if it were a RIGHT ARROW key. 
            if (key == Keys.Escape)
            {
                // return this.ProcessRightKey(e.KeyData);
                int breakpoint = 0;
            }
            return base.ProcessDialogKey(keyData);
        }

        // DataGridView.ProcessDialogKey(Keys) Method appeared on web search for "C# DataGridView escape key"
        // https://learn.microsoft.com/en-us/dotnet/api/system.windows.forms.datagridview.processdialogkey?view=windowsdesktop-9.0
        protected override bool ProcessDataGridViewKey(KeyEventArgs e)
        {
            // Handle the ENTER key as if it were a RIGHT ARROW key. 
            if (e.KeyCode == Keys.Escape)
            {
                // return this.ProcessRightKey(e.KeyData);
                int breakpoint = 0;
            }
            return base.ProcessDataGridViewKey(e);
        }
#endif
    }
}
