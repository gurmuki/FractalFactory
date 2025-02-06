namespace FractalFactory
{
    partial class ExtendSetupDialog
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            action = new System.Windows.Forms.ComboBox();
            ok = new System.Windows.Forms.Button();
            cancel = new System.Windows.Forms.Button();
            deltaGrid = new System.Windows.Forms.DataGridView();
            polyGrid = new System.Windows.Forms.DataGridView();
            label3 = new System.Windows.Forms.Label();
            label4 = new System.Windows.Forms.Label();
            count = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)deltaGrid).BeginInit();
            ((System.ComponentModel.ISupportInitialize)polyGrid).BeginInit();
            SuspendLayout();
            // 
            // action
            // 
            action.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            action.FormattingEnabled = true;
            action.Location = new System.Drawing.Point(14, 14);
            action.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            action.Name = "action";
            action.Size = new System.Drawing.Size(102, 23);
            action.TabIndex = 1;
            action.SelectedIndexChanged += action_SelectedIndexChanged;
            // 
            // ok
            // 
            ok.DialogResult = System.Windows.Forms.DialogResult.OK;
            ok.Location = new System.Drawing.Point(622, 310);
            ok.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            ok.Name = "ok";
            ok.Size = new System.Drawing.Size(88, 27);
            ok.TabIndex = 5;
            ok.Text = "Accept";
            ok.UseVisualStyleBackColor = true;
            ok.Click += ok_Click;
            // 
            // cancel
            // 
            cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            cancel.Location = new System.Drawing.Point(527, 310);
            cancel.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            cancel.Name = "cancel";
            cancel.Size = new System.Drawing.Size(88, 27);
            cancel.TabIndex = 4;
            cancel.Text = "Cancel";
            cancel.UseVisualStyleBackColor = true;
            // 
            // deltaGrid
            // 
            deltaGrid.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            deltaGrid.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            deltaGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            deltaGrid.Location = new System.Drawing.Point(14, 233);
            deltaGrid.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            deltaGrid.MultiSelect = false;
            deltaGrid.Name = "deltaGrid";
            deltaGrid.ScrollBars = System.Windows.Forms.ScrollBars.None;
            deltaGrid.Size = new System.Drawing.Size(695, 51);
            deltaGrid.TabIndex = 9;
            // 
            // polyGrid
            // 
            polyGrid.AllowUserToAddRows = false;
            polyGrid.AllowUserToDeleteRows = false;
            polyGrid.AllowUserToResizeRows = false;
            polyGrid.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            polyGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            polyGrid.Location = new System.Drawing.Point(14, 74);
            polyGrid.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            polyGrid.MultiSelect = false;
            polyGrid.Name = "polyGrid";
            polyGrid.ScrollBars = System.Windows.Forms.ScrollBars.None;
            polyGrid.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            polyGrid.Size = new System.Drawing.Size(695, 102);
            polyGrid.TabIndex = 8;
            polyGrid.SelectionChanged += polyGrid_SelectionChanged;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new System.Drawing.Point(14, 52);
            label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            label3.Name = "label3";
            label3.Size = new System.Drawing.Size(59, 15);
            label3.TabIndex = 10;
            label3.Text = "Polynomials";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new System.Drawing.Point(14, 210);
            label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            label4.Name = "label4";
            label4.Size = new System.Drawing.Size(39, 15);
            label4.TabIndex = 11;
            label4.Text = "Deltas";
            // 
            // count
            // 
            count.Location = new System.Drawing.Point(59, 207);
            count.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            count.Name = "count";
            count.Size = new System.Drawing.Size(58, 23);
            count.TabIndex = 6;
            count.TextChanged += count_TextChanged;
            // 
            // ExtendSetupDialog
            // 
            AcceptButton = ok;
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            CancelButton = cancel;
            ClientSize = new System.Drawing.Size(733, 354);
            Controls.Add(label4);
            Controls.Add(label3);
            Controls.Add(deltaGrid);
            Controls.Add(polyGrid);
            Controls.Add(count);
            Controls.Add(ok);
            Controls.Add(cancel);
            Controls.Add(action);
            FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            Name = "ExtendSetupDialog";
            ShowInTaskbar = false;
            Text = "Extend Setup";
            Load += ExtendSetupDialog_Load;
            ((System.ComponentModel.ISupportInitialize)deltaGrid).EndInit();
            ((System.ComponentModel.ISupportInitialize)polyGrid).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private System.Windows.Forms.ComboBox action;
        private System.Windows.Forms.Button ok;
        private System.Windows.Forms.Button cancel;
        private System.Windows.Forms.DataGridView deltaGrid;
        private System.Windows.Forms.DataGridView polyGrid;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox count;
    }
}