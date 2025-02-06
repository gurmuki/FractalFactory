namespace FractalFactory
{
    partial class ProjectDeleteDialog
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
            label1 = new System.Windows.Forms.Label();
            project = new System.Windows.Forms.ComboBox();
            SuspendLayout();
            // 
            // ok
            // 
            ok.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
            ok.DialogResult = System.Windows.Forms.DialogResult.OK;
            ok.Location = new System.Drawing.Point(224, 52);
            ok.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            ok.Name = "ok";
            ok.Size = new System.Drawing.Size(88, 27);
            ok.TabIndex = 18;
            ok.Text = "Delete";
            ok.UseVisualStyleBackColor = true;
            ok.Click += ok_Click;
            // 
            // cancel
            // 
            cancel.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
            cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            cancel.Location = new System.Drawing.Point(130, 52);
            cancel.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            cancel.Name = "cancel";
            cancel.Size = new System.Drawing.Size(88, 27);
            cancel.TabIndex = 17;
            cancel.Text = "Cancel";
            cancel.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new System.Drawing.Point(16, 18);
            label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            label1.Name = "label1";
            label1.Size = new System.Drawing.Size(44, 15);
            label1.TabIndex = 22;
            label1.Text = "Project";
            // 
            // project
            // 
            project.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            project.FormattingEnabled = true;
            project.Location = new System.Drawing.Point(69, 14);
            project.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            project.Name = "project";
            project.Size = new System.Drawing.Size(234, 23);
            project.TabIndex = 21;
            project.SelectedIndexChanged += project_SelectedIndexChanged;
            // 
            // ProjectDeleteDialog
            // 
            AcceptButton = ok;
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            CancelButton = cancel;
            ClientSize = new System.Drawing.Size(334, 92);
            Controls.Add(label1);
            Controls.Add(project);
            Controls.Add(ok);
            Controls.Add(cancel);
            FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            Name = "ProjectDeleteDialog";
            ShowInTaskbar = false;
            Text = "Delete Project";
            FormClosing += ProjectDeleteDialog_FormClosing;
            Load += ProjectDeleteDialog_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.Button ok;
        private System.Windows.Forms.Button cancel;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox project;
    }
}