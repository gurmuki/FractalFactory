using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace FractalFactory
{
    public partial class ProjectRenameDialog : Form
    {
        private DialogResult overwrite = DialogResult.None;

        public ProjectRenameDialog(Control control, int offset, List<string> existingProjects)
        {
            InitializeComponent();

            FormLocator.Locate(this, control, offset);

            foreach (string project in existingProjects)
            {
                fromProjects.Add(project);
                toProjects.Add(project);
            }

            fromProjects.ReadOnly = true;
            toProjects.ReadOnly = false;

            FromProjectName = string.Empty;
            OkEnable();
        }

        public string ActiveProjectName { get; set; } = string.Empty;

        public string FromProjectName
        {
            get { return fromProjects.Text; }
            set { fromProjects.Text = value; }
        }

        public string ToProjectName
        {
            get { return toProjects.Text; }
            set { toProjects.Text = value; }
        }

        private void RenameProjectDialog_Load(object sender, EventArgs e)
        {
            ok.Enabled = false;
        }

        private void ProjectRenameDialog_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (overwrite == DialogResult.No)
            {
                e.Cancel = true;
                overwrite = DialogResult.None;
            }
        }

        private void ok_Click(object sender, EventArgs e)
        {
            if (fromProjects.Contains(fromProjects.Text))
            {
                string msg = (ActiveProjectName == fromProjects.Text)
                    ? $"Rename the active project [{fromProjects.Text}] to [{toProjects.Text}]?"
                    : $"Rename [{fromProjects.Text}] to [{toProjects.Text}]?";

                overwrite = MessageDialog.Show(this, 10, "Warning!", msg, MessageBoxButtons.YesNo);
            }

            if (overwrite != DialogResult.No)
            {
                FromProjectName = fromProjects.Text;
                ToProjectName = toProjects.Text;
                this.DialogResult = DialogResult.OK;
            }
        }

        private void FromValueChanged(object sender, EventArgs e)
        {
            OkEnable();
        }

        private void ToValueChanged(object sender, EventArgs e)
        {
            OkEnable();
        }

        private void OkEnable()
        {
            ok.Enabled = ((fromProjects.Text.Length > 0) && (toProjects.Text != fromProjects.Text));
        }

        private void ProjectRenameDialog_Click(object sender, EventArgs e)
        {
            fromProjects.DropdownClose();
            toProjects.DropdownClose();
        }
    }
}
