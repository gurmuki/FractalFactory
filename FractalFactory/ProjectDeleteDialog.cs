using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace FractalFactory
{
    public partial class ProjectDeleteDialog : Form
    {
        private bool cancelled = false;

        public ProjectDeleteDialog(Control control, int offset)
        {
            InitializeComponent();

            FormLocator.Locate(this, control, offset);
        }

        public List<string> ProjectNames { private get; set; } = new List<string>();

        public string ProjectName { private set; get; } = string.Empty;

        private void ProjectDeleteDialog_Load(object sender, EventArgs e)
        {
            if (ProjectNames == null)
                return;

            ok.Enabled = false;
            project.Items.AddRange(ProjectNames.ToArray());
        }

        private void ProjectDeleteDialog_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = cancelled;
            cancelled = false;
        }

        private void ok_Click(object sender, EventArgs e)
        {
            if (project.SelectedItem == null)
                return;

            ProjectName = project.SelectedItem.ToString()!;
            cancelled = false;

            DialogResult status = MessageDialog.Show(this, 10, "Warning!", $"Delete the project [{ProjectName}]?", MessageBoxButtons.YesNo);
            if (status == DialogResult.No)
            {
                ProjectName = string.Empty;
                cancelled = true;
            }
        }

        private void project_SelectedIndexChanged(object sender, EventArgs e)
        {
            ok.Enabled = true;
        }
    }
}
