using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace FractalFactory.CustomControls
{
    /// <summary>
    /// A CComboBox behaves somewhat like a Microsoft CombBox.
    /// </summary>
    /// <remarks>
    /// CComboBox was introduced because some behaviors of the Microsoft
    /// ComboBox are nearly impossible to work around.
    /// </remarks>
    public partial class CComboBox : UserControl
    {
        public enum LBoxPos { Left, Above }
        private LBoxPos labelPos = LBoxPos.Left;
        private int maxDisplayItems = 0;
        private bool readOnly = false;

        private char[] WHITESPACE = { ' ' };

        // Sans this inelegant solution, if the control receives an Enter
        // or Esc key, the dialog behaves as if its Ok/Cancel button was
        // pressed. I was not able to find a better solution.
        private bool specialKey = false;

        public CComboBox()
        {
            InitializeComponent();

            listBox.Visible = false;
            dropButton.Visible = false;

            // This is necessary to prevent the button
            // border from overlying the textbox border.
            dropButton.FlatStyle = FlatStyle.Flat;
            dropButton.FlatAppearance.BorderSize = 0;
        }

        private void CComboBox_Load(object sender, EventArgs e)
        {
            ControlResize();

            dropButton.Height = textBox.Height - 2;
            dropButton.Width = dropButton.Height;

            dropButton.Location = new Point(textBox.Right - dropButton.Width - 1, textBox.Location.Y + 1);
            dropButton.Visible = (listBox.Items.Count > 0);

            ListBoxShow(false);
        }

        //=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=
        // https://learn.microsoft.com/en-us/dotnet/desktop/winforms/controls/defining-an-event-in-windows-forms-controls?view=netframeworkdesktop-4.8
        //=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=

        // The event does not have any data, so EventHandler is adequate
        // as the event delegate.  
        private EventHandler? onValueChanged;

        // Define the event member using the 'event' keyword.  
        // In this case, for efficiency, the event is defined
        // using the event property construct.  
        public event EventHandler ValueChanged
        {
            add { onValueChanged += value; }
            remove { onValueChanged -= value; }
        }

        // The protected method that raises the ValueChanged  
        // event when the value has actually
        // changed. Derived controls can override this method.
        protected virtual void OnValueChanged(EventArgs e)
        {
            onValueChanged?.Invoke(this, e);
        }

        //=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=

        /// <summary>
        /// Allows a client to force closure of an open dropdown list.
        /// </summary>
        /// <remarks>
        /// This hack allows an application to emulate a Microsoft ComboBox
        /// which will close when a mouse click occurs outside of the control.
        /// </remarks>
        public void DropdownClose()
        {
            ListBoxShow(false);
        }

        // Sans 'override', warning CS0114: 'CComboBox.Text' hides inherited member 'UserControl.Text'.
        public override string Text
        {
            get { return textBox.Text; }
            set { textBox.Text = value; }
        }

        public void Add(string text)
        {
            string candidate = text.Trim(WHITESPACE);
            if (candidate.Length < 1)
                return;

            listBox.Items.Add(candidate);
        }

        public bool Contains(string text)
        {
            return listBox.Items.Contains(text);
        }

        [
        Category("Appearance"),
        Description("Label text.")
        ]
        public string Label
        {
            get { return label.Text; }

            set
            {
                label.Text = value;

                ControlResize();
            }
        }

        [
        Category("Layout"),
        Description("Relative position of label.")
        ]
        public LBoxPos LabelPosition
        {
            get { return labelPos; }

            set
            {
                labelPos = value;

                ControlResize();
            }
        }

        [
        Category("Behavior"),
        Description("Controls ability to change the edit control text.")
        ]
        public bool ReadOnly
        {
            get { return readOnly; }

            set
            {
                readOnly = value;
                textBox.ReadOnly = readOnly;
            }
        }

        [
        Category("Behavior"),
        Description("Maximum number of items to display in dropdown.")
        ]
        public int MaxDisplayItems
        {
            get { return maxDisplayItems; }

            set
            {
                maxDisplayItems = ((value < 0) ? 0 : value);

                ControlResize();
            }
        }

        private void CComboBox_FontChanged(object sender, EventArgs e)
        {
            label.Font = this.Font;
            textBox.Font = this.Font;
            listBox.Font = this.Font;

            ControlResize();
        }

        private void textBox_TextChanged(object sender, EventArgs e)
        {
            // Raise an event informing the client the textbox text has changed.
            OnValueChanged(EventArgs.Empty);

            if (listBox.Items.Count < 1)
                return;

            if (textBox.Text.Length < 1)
            {
                ListBoxShow(false);
                return;
            }

            ListItemSelect();
        }

        private void listBox_DoubleClick(object sender, EventArgs e)
        {
            textBox.Text = listBox.Text;
            ListBoxShow(false);
        }

        private void textBox_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                textBox.Text = textBox.Text.Trim(WHITESPACE);
                return;
            }

            if ((textBox.Text.Length < 1) && (e.KeyCode == Keys.Space))
            {
                e.Handled = true;
                return;
            }

            if (e.KeyCode == Keys.Escape)
            {
                ListBoxShow(false);
                return;
            }

            // Avoid showing the dropdown when there's nothing to display.
            if ((listBox.Items.Count > 0) && !listBox.Visible && (textBox.Text.Length > 0))
            {
                ListBoxShow(true);
            }
        }

        private void textBox_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            specialKey = IsSpecialKey(e.KeyCode);

            if ((e.KeyCode == Keys.Tab) || specialKey)
            {
                ListBoxShow(false);

                char[] whitespace = { ' ' };
                textBox.Text = textBox.Text.Trim(whitespace);

                SendKeys.Send("{Tab}");  // Advance to next control
            }
        }

        private void listBox_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                textBox.Text = listBox.Text;

            specialKey = IsSpecialKey(e.KeyCode);
            if (specialKey)
                ListBoxShow(false);
        }

        private void textBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if ((e.KeyChar == ' ') && (textBox.Text.Length == 0))
                e.Handled = true;  // Disallow whitespace at start of name.
        }

        private void textBox_Validating(object sender, CancelEventArgs e)
        {
            if (specialKey)
            {
                specialKey = false;
                e.Cancel = true;
            }
        }

        private void ListItemSelect()
        {
            int indx, count = listBox.Items.Count;
            for (indx = 0; indx < count; ++indx)
            {
                string candidate = listBox.Items[indx].ToString();
                if (candidate[0] < textBox.Text[0])
                    continue;

                if (candidate[0] > textBox.Text[0])
                {
                    indx = count;
                    break;
                }

                if (candidate.StartsWith(textBox.Text))
                    break;
            }

            listBox.SelectedIndex = ((indx < count) ? indx : -1);
        }

        private void dropButton_Click(object sender, EventArgs e)
        {
            ListBoxShow(true);
        }

        private void ControlResize()
        {
            if (labelPos == LBoxPos.Left)
            {
                textBox.Location = new Point(this.Padding.Left + label.Width, this.Padding.Top);
                label.Location = new Point(this.Padding.Left, textBox.Top + ((textBox.Height - label.Height) / 2) + 2);
            }
            else
            {
                label.Location = new Point(this.Padding.Left, this.Padding.Top);
                textBox.Location = new Point(label.Left, label.Bottom);
            }

            listBox.Location = new Point(textBox.Location.X, textBox.Bottom);

            int controlWidth = label.Location.X + textBox.Width;
            if (labelPos == LBoxPos.Left)
                controlWidth += label.Width;

            int textHeight = ControlTextSize("QYqy", label.Font).Height + 1;
            int itemCount = ((listBox.Items.Count > maxDisplayItems) ? maxDisplayItems : listBox.Items.Count);
            listBox.Height = textHeight * (itemCount + 1);  // TODO: Fix the +1 kludge

            this.Height = textBox.Bottom + listBox.Height + this.Padding.Bottom;
            this.Width = controlWidth + this.Padding.Right;

            // Invalidate() calls the OnPaint().
            Invalidate();
        }

        private void CComboBox_PaddingChanged(object sender, EventArgs e)
        {
            ControlResize();
        }

        private Size ControlTextSize(string text, Font font)
        {
            string copy = text.Replace(' ', 'x');
            return TextRenderer.MeasureText(copy, font);
        }

        private void ListBoxShow(bool show)
        {
            if (show)
            {
                this.Height = textBox.Height + listBox.Height;
                listBox.Visible = true;
            }
            else
            {
                this.Height = textBox.Height;
                listBox.Visible = false;
            }

            this.BringToFront();
        }

        private bool IsSpecialKey(Keys key)
        {
            return ((key == Keys.Enter) || (key == Keys.Escape));
        }
    }

    internal class SpecialListBox : System.Windows.Forms.ListBox
    {
        private bool mShowScroll;

        protected override System.Windows.Forms.CreateParams CreateParams
        {
            get
            {
                CreateParams cp = base.CreateParams;
                if (!mShowScroll)
                    cp.Style = cp.Style & ~0x200000;
                return cp;
            }
        }

        public bool ShowScrollbar
        {
            get { return mShowScroll; }
            set
            {
                if (value != mShowScroll)
                {
                    mShowScroll = value;
                    if (IsHandleCreated)
                        RecreateHandle();
                }
            }
        }
    }

}
