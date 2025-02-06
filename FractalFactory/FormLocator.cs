using System.Windows.Forms;

namespace FractalFactory
{
    public class FormLocator
    {
        public static void Locate(Form form, Control control, int offset)
        {
            if (control == null)
            {
                form.StartPosition = FormStartPosition.WindowsDefaultLocation;
            }
            else
            {
                form.StartPosition = FormStartPosition.Manual;
                form.Location = control.PointToScreen(new System.Drawing.Point(offset, offset));
            }
        }
    }
}
