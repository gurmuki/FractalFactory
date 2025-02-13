namespace FractalFactory
{
    partial class InterpolationSetupDialog
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
            numerGrid = new System.Windows.Forms.DataGridView();
            denomGrid = new System.Windows.Forms.DataGridView();
            numerLabel = new System.Windows.Forms.Label();
            denomLabel = new System.Windows.Forms.Label();
            buttonPanel = new System.Windows.Forms.Panel();
            accept = new System.Windows.Forms.Button();
            cancel = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)numerGrid).BeginInit();
            ((System.ComponentModel.ISupportInitialize)denomGrid).BeginInit();
            buttonPanel.SuspendLayout();
            SuspendLayout();
            // 
            // numerGrid
            // 
            numerGrid.AllowUserToAddRows = false;
            numerGrid.AllowUserToDeleteRows = false;
            numerGrid.AllowUserToResizeRows = false;
            numerGrid.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            numerGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            numerGrid.Location = new System.Drawing.Point(14, 29);
            numerGrid.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            numerGrid.MultiSelect = false;
            numerGrid.Name = "numerGrid";
            numerGrid.ScrollBars = System.Windows.Forms.ScrollBars.None;
            numerGrid.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            numerGrid.Size = new System.Drawing.Size(320, 122);
            numerGrid.TabIndex = 0;
            numerGrid.SelectionChanged += fxGrid_SelectionChanged;
            // 
            // denomGrid
            // 
            denomGrid.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            denomGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            denomGrid.Location = new System.Drawing.Point(14, 184);
            denomGrid.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            denomGrid.Name = "denomGrid";
            denomGrid.ScrollBars = System.Windows.Forms.ScrollBars.None;
            denomGrid.Size = new System.Drawing.Size(320, 122);
            denomGrid.TabIndex = 1;
            denomGrid.SelectionChanged += dfxGrid_SelectionChanged;
            // 
            // numerLabel
            // 
            numerLabel.AutoSize = true;
            numerLabel.Location = new System.Drawing.Point(14, 11);
            numerLabel.Name = "numerLabel";
            numerLabel.Size = new System.Drawing.Size(65, 15);
            numerLabel.TabIndex = 4;
            numerLabel.Text = "Numerator";
            // 
            // denomLabel
            // 
            denomLabel.AutoSize = true;
            denomLabel.Location = new System.Drawing.Point(14, 166);
            denomLabel.Name = "denomLabel";
            denomLabel.Size = new System.Drawing.Size(77, 15);
            denomLabel.TabIndex = 5;
            denomLabel.Text = "Denominator";
            // 
            // buttonPanel
            // 
            buttonPanel.Controls.Add(accept);
            buttonPanel.Controls.Add(cancel);
            buttonPanel.Location = new System.Drawing.Point(148, 318);
            buttonPanel.Name = "buttonPanel";
            buttonPanel.Size = new System.Drawing.Size(186, 34);
            buttonPanel.TabIndex = 6;
            // 
            // accept
            // 
            accept.DialogResult = System.Windows.Forms.DialogResult.OK;
            accept.Location = new System.Drawing.Point(97, 4);
            accept.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            accept.Name = "accept";
            accept.Size = new System.Drawing.Size(88, 27);
            accept.TabIndex = 5;
            accept.Text = "Accept";
            accept.UseVisualStyleBackColor = true;
            accept.Click += accept_Click;
            // 
            // cancel
            // 
            cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            cancel.Location = new System.Drawing.Point(2, 4);
            cancel.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            cancel.Name = "cancel";
            cancel.Size = new System.Drawing.Size(88, 27);
            cancel.TabIndex = 4;
            cancel.Text = "Cancel";
            cancel.UseVisualStyleBackColor = true;
            // 
            // InterpolationSetupDialog
            // 
            AcceptButton = accept;
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            CancelButton = cancel;
            ClientSize = new System.Drawing.Size(349, 360);
            Controls.Add(buttonPanel);
            Controls.Add(denomLabel);
            Controls.Add(numerLabel);
            Controls.Add(denomGrid);
            Controls.Add(numerGrid);
            FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            Name = "InterpolationSetupDialog";
            ShowInTaskbar = false;
            Text = "Interpolation Setup";
            Load += InterpolationSetup_Load;
            ((System.ComponentModel.ISupportInitialize)numerGrid).EndInit();
            ((System.ComponentModel.ISupportInitialize)denomGrid).EndInit();
            buttonPanel.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.DataGridView numerGrid;
        private System.Windows.Forms.DataGridView denomGrid;
        private System.Windows.Forms.Label numerLabel;
        private System.Windows.Forms.Label denomLabel;
        private System.Windows.Forms.Panel buttonPanel;
        private System.Windows.Forms.Button accept;
        private System.Windows.Forms.Button cancel;
    }
}