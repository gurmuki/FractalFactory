using System;
using System.Windows.Forms;

namespace FractalFactory
{
    public partial class PasteDialog : Form
    {
        public PasteDialog(Control control, int offset)
        {
            InitializeComponent();

            FormLocator.Locate(this, control, offset);
        }

        public bool PasteAbove { get; private set; } = false;
        public bool PasteBelow { get; private set; } = false;

        private void PasteDialog_Load(object sender, EventArgs e)
        {
            above.Checked = false;
            below.Checked = true;
        }

        private void PasteDialog_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.Cancel)
                return;

            PasteAbove = above.Checked;
            PasteBelow = below.Checked;
        }
    }
}
