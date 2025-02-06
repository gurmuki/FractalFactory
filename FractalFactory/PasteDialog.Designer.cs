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
            okBTN = new System.Windows.Forms.Button();
            cancelBTN = new System.Windows.Forms.Button();
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
            // okBTN
            // 
            okBTN.DialogResult = System.Windows.Forms.DialogResult.OK;
            okBTN.Location = new System.Drawing.Point(28, 91);
            okBTN.Name = "okBTN";
            okBTN.Size = new System.Drawing.Size(75, 23);
            okBTN.TabIndex = 1;
            okBTN.Text = "OK";
            okBTN.UseVisualStyleBackColor = true;
            // 
            // cancelBTN
            // 
            cancelBTN.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            cancelBTN.Location = new System.Drawing.Point(109, 91);
            cancelBTN.Name = "cancelBTN";
            cancelBTN.Size = new System.Drawing.Size(75, 23);
            cancelBTN.TabIndex = 2;
            cancelBTN.Text = "Cancel";
            cancelBTN.UseVisualStyleBackColor = true;
            // 
            // PasteDialog
            // 
            AcceptButton = okBTN;
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            CancelButton = cancelBTN;
            ClientSize = new System.Drawing.Size(209, 126);
            Controls.Add(cancelBTN);
            Controls.Add(okBTN);
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
        private System.Windows.Forms.Button okBTN;
        private System.Windows.Forms.Button cancelBTN;
    }
}