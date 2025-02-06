namespace FractalFactory
{
    partial class ProjectOpenDialog
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
            ok = new System.Windows.Forms.Button();
            cancel = new System.Windows.Forms.Button();
            project = new System.Windows.Forms.ComboBox();
            label1 = new System.Windows.Forms.Label();
            SuspendLayout();
            // 
            // ok
            // 
            ok.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
            ok.DialogResult = System.Windows.Forms.DialogResult.OK;
            ok.Location = new System.Drawing.Point(216, 51);
            ok.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            ok.Name = "ok";
            ok.Size = new System.Drawing.Size(88, 27);
            ok.TabIndex = 16;
            ok.Text = "Accept";
            ok.UseVisualStyleBackColor = true;
            ok.Click += ok_Click;
            // 
            // cancel
            // 
            cancel.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
            cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            cancel.Location = new System.Drawing.Point(121, 51);
            cancel.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            cancel.Name = "cancel";
            cancel.Size = new System.Drawing.Size(88, 27);
            cancel.TabIndex = 15;
            cancel.Text = "Cancel";
            cancel.UseVisualStyleBackColor = true;
            // 
            // project
            // 
            project.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            project.FormattingEnabled = true;
            project.Location = new System.Drawing.Point(69, 14);
            project.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            project.Name = "project";
            project.Size = new System.Drawing.Size(234, 23);
            project.Sorted = true;
            project.TabIndex = 19;
            project.SelectedIndexChanged += project_SelectedIndexChanged;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new System.Drawing.Point(16, 18);
            label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            label1.Name = "label1";
            label1.Size = new System.Drawing.Size(44, 15);
            label1.TabIndex = 20;
            label1.Text = "Project";
            // 
            // ProjectOpenDialog
            // 
            AcceptButton = ok;
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            CancelButton = cancel;
            ClientSize = new System.Drawing.Size(328, 91);
            Controls.Add(label1);
            Controls.Add(project);
            Controls.Add(ok);
            Controls.Add(cancel);
            FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            Name = "ProjectOpenDialog";
            ShowInTaskbar = false;
            Text = "Open Project";
            Load += OpenProjectDialog_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.Button ok;
        private System.Windows.Forms.Button cancel;
        private System.Windows.Forms.ComboBox project;
        private System.Windows.Forms.Label label1;
    }
}