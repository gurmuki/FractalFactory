using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace FractalFactory
{
    public partial class ProjectSaveAsDialog : Form
    {
        public ProjectSaveAsDialog(Control control, int offset, List<string> existingProjects)
        {
            InitializeComponent();

            FormLocator.Locate(this, control, offset);

            foreach (string project in existingProjects)
            {
                projects.Add(project);
            }
        }

        private void ValueChanged(object sender, EventArgs e)
        {
            accept.Enabled = (projects.Text.Length > 0);
        }

        public string ProjectName
        {
            get { return projects.Text; }
            set { projects.Text = value; }
        }

        //=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=

        private void projectSaveAsDialog_Load(object sender, EventArgs e)
        {
            AcceptEnable();
        }

        private void projectSaveAsDialog_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = (this.DialogResult == DialogResult.No);
        }

        private void accept_Click(object sender, EventArgs e)
        {
            if (projects.Contains(projects.Text))
            {
                string msg = $"Overwrite the existing project ({projects.Text})?";
                this.DialogResult = MessageDialog.Show(this, 10, "Warning!", msg, MessageBoxButtons.YesNo);
            }

            if (this.DialogResult == DialogResult.Yes)
            {
                ProjectName = projects.Text;
                this.DialogResult = DialogResult.OK;
            }
        }

        private void AcceptEnable()
        {
            accept.Enabled = (projects.Text.Length > 0);
        }

        private void ProjectSaveAsDialog_Click(object sender, EventArgs e)
        {
            projects.DropdownClose();
        }
    }
}
