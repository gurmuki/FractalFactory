
//=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=
// NOTE: Compilation issues will arise after modifying the dialog using the Designer.
//   Whatever the reason, all references to CustomControls.CComboBox will be changed
//   to FractalFactory.CustomControls.CComboBox, causing an error. I have not been
//   able to find any reference/solution to such behavior on the web. As such, the
//   current solution is simply to edit the offending code.
//=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=

namespace FractalFactory
{
    partial class ProjectRenameDialog
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
            accept = new System.Windows.Forms.Button();
            cancel = new System.Windows.Forms.Button();
            fromProjects = new CustomControls.CComboBox();
            toProjects = new CustomControls.CComboBox();
            SuspendLayout();
            // 
            // accept
            // 
            accept.DialogResult = System.Windows.Forms.DialogResult.OK;
            accept.Location = new System.Drawing.Point(222, 179);
            accept.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            accept.Name = "accept";
            accept.Size = new System.Drawing.Size(88, 27);
            accept.TabIndex = 18;
            accept.Text = "Accept";
            accept.UseVisualStyleBackColor = true;
            accept.Click += accept_Click;
            // 
            // cancel
            // 
            cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            cancel.Location = new System.Drawing.Point(128, 179);
            cancel.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            cancel.Name = "cancel";
            cancel.Size = new System.Drawing.Size(88, 27);
            cancel.TabIndex = 17;
            cancel.Text = "Cancel";
            cancel.UseVisualStyleBackColor = true;
            // 
            // fromProjects
            // 
            fromProjects.Label = "From";
            fromProjects.LabelPosition = CustomControls.CComboBox.LBoxPos.Left;
            fromProjects.Location = new System.Drawing.Point(13, 22);
            fromProjects.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            fromProjects.MaxDisplayItems = 4;
            fromProjects.Name = "fromProjects";
            fromProjects.ReadOnly = false;
            fromProjects.Size = new System.Drawing.Size(297, 23);
            fromProjects.TabIndex = 26;
            fromProjects.ValueChanged += FromValueChanged;
            // 
            // toProjects
            // 
            toProjects.Label = "To";
            toProjects.LabelPosition = CustomControls.CComboBox.LBoxPos.Left;
            toProjects.Location = new System.Drawing.Point(29, 54);
            toProjects.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            toProjects.MaxDisplayItems = 4;
            toProjects.Name = "toProjects";
            toProjects.ReadOnly = false;
            toProjects.Size = new System.Drawing.Size(281, 23);
            toProjects.TabIndex = 27;
            toProjects.ValueChanged += ToValueChanged;
            // 
            // ProjectRenameDialog
            // 
            AcceptButton = accept;
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            CancelButton = cancel;
            ClientSize = new System.Drawing.Size(336, 218);
            Controls.Add(fromProjects);
            Controls.Add(toProjects);
            Controls.Add(accept);
            Controls.Add(cancel);
            FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            Name = "ProjectRenameDialog";
            ShowInTaskbar = false;
            Text = "Rename Project";
            FormClosing += ProjectRenameDialog_FormClosing;
            Load += RenameProjectDialog_Load;
            Click += ProjectRenameDialog_Click;
            ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.Button accept;
        private System.Windows.Forms.Button cancel;
        private CustomControls.CComboBox fromProjects;
        private CustomControls.CComboBox toProjects;
    }
}