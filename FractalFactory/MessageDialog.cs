using System;
using System.Drawing;
using System.Windows.Forms;

namespace FractalFactory
{
    public partial class MessageDialog : Form
    {
        private MessageBoxButtons btns;

        private MessageDialog(Control control, int offset)
        {
            InitializeComponent();

            FormLocator.Locate(this, control, offset);
        }

        /// <summary>Show the message box at the WindowsDefaultLocation.</summary>
        public static DialogResult Show(string caption, string msg, MessageBoxButtons buttons)
        {
            MessageDialog dialog = new MessageDialog(null!, 0);

            dialog.Text = caption;
            dialog.message.Text = msg;
            dialog.btns = buttons;

            return dialog.ShowDialog();
        }

        /// <summary>Show the message box at a location relative to a control.</summary>
        public static DialogResult Show(Control control, int offset, string caption, string msg, MessageBoxButtons buttons)
        {
            MessageDialog dialog = new MessageDialog(control, offset);

            dialog.Text = caption;
            dialog.message.Text = msg;
            dialog.btns = buttons;

            return dialog.ShowDialog();
        }

        private void MessageDialog_Load(object sender, EventArgs e)
        {
            Size size = MessageDimensions();

            int padding = 20;
            message.Location = new Point(20, 20);
            message.Size = new Size(size.Width + padding, size.Height + padding);

            panelOkCancel.Visible = false;
            panelYesNo.Visible = false;
            panelYesNoCancel.Visible = false;

            Panel? panel = null;
            if (btns == MessageBoxButtons.OK)
                panel = panelOk;
            else if (btns == MessageBoxButtons.OKCancel)
                panel = panelOkCancel;
            else if (btns == MessageBoxButtons.YesNo)
                panel = panelYesNo;
            else if (btns == MessageBoxButtons.YesNoCancel)
                panel = panelYesNoCancel;

            if (panel != null)
            {
                int minWidth = (int)(1.5 * panel.Width);
                if (size.Width < minWidth)
                    size.Width = minWidth;

                this.ClientSize = new Size(size.Width + (2 * padding), size.Height + (2 * padding) + panel.Height);
                panel.Location = new Point(this.ClientSize.Width - panel.Width, this.ClientSize.Height - panel.Height);
                panel.Visible = true;
            }
        }

        private Size MessageDimensions()
        {
            // Font font = new Font("Courier New", 10.0F);
            Image fakeImage = new Bitmap(1, 1); //As we cannot use CreateGraphics() in a class library, so the fake image is used to load the Graphics.
            System.Drawing.Graphics graphics = System.Drawing.Graphics.FromImage(fakeImage);
            SizeF size = graphics.MeasureString(message.Text, this.Font);
            return size.ToSize();
        }
    }
}
