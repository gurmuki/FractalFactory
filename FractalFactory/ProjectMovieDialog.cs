using FractalFactory.Common;
using System;
using System.IO;
using System.Windows.Forms;

namespace FractalFactory
{
    public partial class ProjectMovieDialog : Form
    {
        private ProjectSettings settings = ProjectSettings.DefaultSettings();
        private System.Windows.Forms.TextBox activeTextbox = null!;

        public ProjectMovieDialog(Control control, int offset)
        {
            InitializeComponent();

            FormLocator.Locate(this, control, offset);
        }

        public ProjectSettings Settings
        {
            get { return settings; }
            set { settings.Copy(value); }
        }

        public string MovieName { get; set; } = string.Empty;

        public bool ProcessShow { get; set; } = false;

        private void ProjectMovieDialog_Load(object sender, EventArgs e)
        {
            string folder = (Stringy.IsEmpty(settings.movieFolder) ? settings.defMovFolder : settings.movieFolder);
            movie.Text = Path.Combine(folder, MovieName);
            watermarkPath.Text = settings.movieWater;
            frameRate.Text = settings.movieRate.ToString();

            AcceptEnable();
        }

        private void accept_Click(object sender, EventArgs e)
        {
            settings.movieFolder = Path.GetDirectoryName(movie.Text)!;
            settings.movieName = Path.GetFileName(movie.Text);
            settings.movieWater = watermarkPath.Text;
            settings.movieRate = Int32.Parse(frameRate.Text);

            ProcessShow = showProcess.Checked;
        }

        private void movie_Enter(object sender, EventArgs e) { BrowseEnable(movie); }
        private void movie_MouseUp(object sender, MouseEventArgs e) { BrowseEnable(movie); }

        private void watermarkPath_Enter(object sender, EventArgs e) { BrowseEnable(watermarkPath); }
        private void watermarkPath_MouseUp(object sender, MouseEventArgs e) { BrowseEnable(watermarkPath); }

        private void browse_Click(object sender, EventArgs e)
        {
            if (activeTextbox == movie)
                SaveAs();
            else
                FileSelect();

            AcceptEnable();
        }

        private void BrowseEnable(TextBox textBox)
        {
            activeTextbox = textBox;
            activeTextbox.SelectAll();
            browse.Enabled = true;
        }

        private void frameRate_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !KeyValidator.IsIntChar(e.KeyChar);
            AcceptEnable();
        }

        private void frameRate_TextChanged(object sender, EventArgs e)
        {
            AcceptEnable();
        }

        private void SaveAs()
        {
            SaveFileDialog dialog = new SaveFileDialog();
            dialog.Title = "Save Movie";
            dialog.InitialDirectory = movie.Text;
            dialog.FileName = string.Empty;
            dialog.Filter = "mp4 files (*.mp4)|*.mp4";

            if (dialog.ShowDialog() == DialogResult.Cancel)
                return;

            movie.Text = dialog.FileName;
        }

        private void FileSelect()
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Title = "Select Watermark";
            dialog.InitialDirectory = settings.movieFolder;
            dialog.FileName = string.Empty;
            dialog.Filter = "png files (*.png)|*.png";

            if (dialog.ShowDialog() == DialogResult.Cancel)
                return;

            watermarkPath.Text = dialog.FileName;
        }

        private void AcceptEnable()
        {
            accept.Enabled = false;

            if (Stringy.IsEmpty(Path.GetDirectoryName(movie.Text)!))
                return;

            if (Stringy.IsEmpty(Path.GetFileName(movie.Text)))
                return;

            if (Stringy.IsEmpty(watermarkPath.Text) || (Path.GetExtension(movie.Text).ToLower() != ".mp4"))
                return;

            if (Stringy.IsEmpty(frameRate.Text))
                return;

            accept.Enabled = true;
        }
    }
}
