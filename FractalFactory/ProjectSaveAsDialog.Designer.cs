
//=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=
// NOTE: Compilation issues will arise after modifying the dialog using the Designer.
//   Whatever the reason, all references to CustomControls.CComboBox will be changed
//   to FractalFactory.CustomControls.CComboBox, causing an error. I have not been
//   able to find any reference/solution to such behavior on the web. As such, the
//   current solution is simply to edit the offending code.
//=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=

namespace FractalFactory
{
    partial class ProjectSaveAsDialog
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
            projects = new CustomControls.CComboBox();
            accept = new System.Windows.Forms.Button();
            cancel = new System.Windows.Forms.Button();
            SuspendLayout();
            // 
            // projects
            // 
            projects.Label = "Existing Projects";
            projects.LabelPosition = CustomControls.CComboBox.LBoxPos.Left;
            projects.Location = new System.Drawing.Point(13, 12);
            projects.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            projects.MaxDisplayItems = 4;
            projects.Name = "projects";
            projects.ReadOnly = false;
            projects.Size = new System.Drawing.Size(355, 23);
            projects.TabIndex = 1;
            projects.ValueChanged += ValueChanged;
            // 
            // accept
            // 
            accept.Anchor = System.Windows.Forms.AnchorStyles.None;
            accept.DialogResult = System.Windows.Forms.DialogResult.OK;
            accept.Location = new System.Drawing.Point(284, 122);
            accept.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            accept.Name = "accept";
            accept.Size = new System.Drawing.Size(84, 27);
            accept.TabIndex = 2;
            accept.Text = "Accept";
            accept.UseVisualStyleBackColor = true;
            accept.Click += accept_Click;
            // 
            // cancel
            // 
            cancel.Anchor = System.Windows.Forms.AnchorStyles.None;
            cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            cancel.Location = new System.Drawing.Point(195, 122);
            cancel.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            cancel.Name = "cancel";
            cancel.Size = new System.Drawing.Size(84, 27);
            cancel.TabIndex = 3;
            cancel.Text = "Cancel";
            cancel.UseVisualStyleBackColor = true;
            // 
            // ProjectSaveAsDialog
            // 
            AcceptButton = accept;
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            CancelButton = cancel;
            ClientSize = new System.Drawing.Size(384, 161);
            Controls.Add(projects);
            Controls.Add(accept);
            Controls.Add(cancel);
            FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            Name = "ProjectSaveAsDialog";
            ShowInTaskbar = false;
            Text = "Save As Project";
            FormClosing += projectSaveAsDialog_FormClosing;
            Load += projectSaveAsDialog_Load;
            Click += ProjectSaveAsDialog_Click;
            ResumeLayout(false);
        }

        #endregion

        private CustomControls.CComboBox projects;
        private System.Windows.Forms.Button accept;
        private System.Windows.Forms.Button cancel;
    }
}