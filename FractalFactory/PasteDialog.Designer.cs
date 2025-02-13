namespace FractalFactory
{
    partial class PasteDialog
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
            groupBox = new System.Windows.Forms.GroupBox();
            below = new System.Windows.Forms.RadioButton();
            above = new System.Windows.Forms.RadioButton();
            accept = new System.Windows.Forms.Button();
            cancel = new System.Windows.Forms.Button();
            groupBox.SuspendLayout();
            SuspendLayout();
            // 
            // groupBox
            // 
            groupBox.Controls.Add(below);
            groupBox.Controls.Add(above);
            groupBox.Location = new System.Drawing.Point(12, 12);
            groupBox.Name = "groupBox";
            groupBox.Size = new System.Drawing.Size(182, 68);
            groupBox.TabIndex = 0;
            groupBox.TabStop = false;
            groupBox.Text = "Option";
            // 
            // below
            // 
            below.AutoSize = true;
            below.Location = new System.Drawing.Point(105, 30);
            below.Name = "below";
            below.Size = new System.Drawing.Size(57, 19);
            below.TabIndex = 1;
            below.TabStop = true;
            below.Text = "Below";
            below.UseVisualStyleBackColor = true;
            // 
            // above
            // 
            above.AutoSize = true;
            above.Location = new System.Drawing.Point(22, 30);
            above.Name = "above";
            above.Size = new System.Drawing.Size(59, 19);
            above.TabIndex = 0;
            above.TabStop = true;
            above.Text = "Above";
            above.UseVisualStyleBackColor = true;
            // 
            // accept
            // 
            accept.DialogResult = System.Windows.Forms.DialogResult.OK;
            accept.Location = new System.Drawing.Point(109, 91);
            accept.Name = "accept";
            accept.Size = new System.Drawing.Size(75, 23);
            accept.TabIndex = 1;
            accept.Text = "Accept";
            accept.UseVisualStyleBackColor = true;
            // 
            // cancel
            // 
            cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            cancel.Location = new System.Drawing.Point(28, 91);
            cancel.Name = "cancel";
            cancel.Size = new System.Drawing.Size(75, 23);
            cancel.TabIndex = 2;
            cancel.Text = "Cancel";
            cancel.UseVisualStyleBackColor = true;
            // 
            // PasteDialog
            // 
            AcceptButton = accept;
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            CancelButton = cancel;
            ClientSize = new System.Drawing.Size(209, 126);
            Controls.Add(cancel);
            Controls.Add(accept);
            Controls.Add(groupBox);
            FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            Name = "PasteDialog";
            ShowInTaskbar = false;
            Text = "Paste Option";
            FormClosing += PasteDialog_FormClosing;
            Load += PasteDialog_Load;
            groupBox.ResumeLayout(false);
            groupBox.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox;
        private System.Windows.Forms.RadioButton below;
        private System.Windows.Forms.RadioButton above;
        private System.Windows.Forms.Button accept;
        private System.Windows.Forms.Button cancel;
    }
}