using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace FractalFactory
{
    public partial class ProjectOpenDialog : Form
    {
        public ProjectOpenDialog(Control control, int offset)
        {
            InitializeComponent();

            FormLocator.Locate(this, control, offset);
        }

        public List<string> ProjectNames { private get; set; } = new List<string>();

        public string ProjectName { private set; get; } = string.Empty;

        private void OpenProjectDialog_Load(object sender, EventArgs e)
        {
            if (ProjectNames == null)
                return;

            ok.Enabled = false;
            project.Items.AddRange(ProjectNames.ToArray());
        }

        private void ok_Click(object sender, EventArgs e)
        {
            if (project.SelectedItem == null)
                return;

            ProjectName = project.SelectedItem.ToString()!;
        }

        private void project_SelectedIndexChanged(object sender, EventArgs e)
        {
            ok.Enabled = true;
        }
    }
}
