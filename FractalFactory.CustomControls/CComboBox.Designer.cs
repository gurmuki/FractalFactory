namespace FractalFactory.CustomControls
{
    partial class CComboBox
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.label = new System.Windows.Forms.Label();
            this.textBox = new System.Windows.Forms.TextBox();
            this.dropButton = new System.Windows.Forms.Button();
            this.listBox = new FractalFactory.CustomControls.SpecialListBox();
            this.SuspendLayout();
            // 
            // label
            // 
            this.label.AutoSize = true;
            this.label.Location = new System.Drawing.Point(3, 7);
            this.label.Name = "label";
            this.label.Size = new System.Drawing.Size(35, 13);
            this.label.TabIndex = 0;
            this.label.Text = "label1";
            // 
            // textBox
            // 
            this.textBox.Location = new System.Drawing.Point(40, 3);
            this.textBox.Name = "textBox";
            this.textBox.Size = new System.Drawing.Size(225, 20);
            this.textBox.TabIndex = 1;
            this.textBox.TextChanged += new System.EventHandler(this.textBox_TextChanged);
            this.textBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBox_KeyPress);
            this.textBox.KeyUp += new System.Windows.Forms.KeyEventHandler(this.textBox_KeyUp);
            this.textBox.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler(this.textBox_PreviewKeyDown);
            this.textBox.Validating += new System.ComponentModel.CancelEventHandler(this.textBox_Validating);
            // 
            // dropButton
            // 
            this.dropButton.Location = new System.Drawing.Point(245, 3);
            this.dropButton.Margin = new System.Windows.Forms.Padding(0);
            this.dropButton.Name = "dropButton";
            this.dropButton.Size = new System.Drawing.Size(20, 20);
            this.dropButton.TabIndex = 3;
            this.dropButton.TabStop = false;
            this.dropButton.Text = "v";
            this.dropButton.UseVisualStyleBackColor = true;
            this.dropButton.Click += new System.EventHandler(this.dropButton_Click);
            // 
            // listBox
            // 
            this.listBox.FormattingEnabled = true;
            this.listBox.Location = new System.Drawing.Point(40, 29);
            this.listBox.Name = "listBox";
            this.listBox.ShowScrollbar = false;
            this.listBox.Size = new System.Drawing.Size(225, 43);
            this.listBox.Sorted = true;
            this.listBox.TabIndex = 2;
            this.listBox.DoubleClick += new System.EventHandler(this.listBox_DoubleClick);
            this.listBox.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler(this.listBox_PreviewKeyDown);
            // 
            // CComboBox
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.dropButton);
            this.Controls.Add(this.listBox);
            this.Controls.Add(this.textBox);
            this.Controls.Add(this.label);
            this.Name = "CComboBox";
            this.Size = new System.Drawing.Size(281, 82);
            this.Load += new System.EventHandler(this.CComboBox_Load);
            this.FontChanged += new System.EventHandler(this.CComboBox_FontChanged);
            this.PaddingChanged += new System.EventHandler(this.CComboBox_PaddingChanged);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label;
        private System.Windows.Forms.TextBox textBox;
        private SpecialListBox listBox;
        private System.Windows.Forms.Button dropButton;
    }
}
