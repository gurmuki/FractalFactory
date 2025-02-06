using System;
using System.Windows.Forms;

namespace FractalFactory
{
    /// <summary>
    /// The ProjectMovieDirectives dialog allows you to avoid responding
    /// to a series of questions that could be presented in lesser dialogs.
    /// </summary>
    /// <remarks>
    /// This dialog is a little funky. While it's fairly clear the dialog
    /// won't let you create a movie without addressing existing files, it
    /// is not very clear
    /// </remarks>
    public partial class ProjectMovieDirectives : Form
    {
        public ProjectMovieDirectives(Control control, int offset)
        {
            InitializeComponent();

            FormLocator.Locate(this, control, offset);

            overwriteMovieFile.Checked = false;
            overwriteMovieFile.Enabled = false;

            outputSelectedRange.Checked = false;
            outputSelectedRange.Enabled = false;

            viewMovie.Checked = false;
            viewMovie.Enabled = true;
        }

        public bool ImageFilesOverwrite { get; set; } = false;

        public bool MoveFileOverwrite { get; set; } = false;

        public bool UseSelectedRange { get; set; } = false;

        public bool MovieShow { get; set; } = false;

        private void ProjectMovieDirectives_Load(object sender, EventArgs e)
        {
            // ImageFilesOverwrite should be true only if existing image
            // files are detected.
            //
            // When checked, the application should delete the image files,
            // effectively behaving as if the files are being overwritten.
            //
            // When not checked, the application should not save new image
            // files, effectively reusing the existing image files. This is
            // desired behavior if you're simply regenerating a movie with
            // (eg.) a different frame rate.
            if (ImageFilesOverwrite)
            {
                overwriteImageFiles.Checked = true;
                overwriteImageFiles.Enabled = true;

                outputSelectedRange.Enabled = (UseSelectedRange && overwriteImageFiles.Checked);
                outputSelectedRange.Checked = outputSelectedRange.Enabled;
            }
            else
            {
                overwriteImageFiles.Checked = false;
                overwriteImageFiles.Enabled = false;

                // UseSelectedRange should be true only if multiple statements
                // are selected in the grid control.
                outputSelectedRange.Checked = UseSelectedRange;
                outputSelectedRange.Enabled = UseSelectedRange;
            }

            // MoveFileOverwrite should be true only if the target movie exists.
            if (MoveFileOverwrite)
            {
                overwriteMovieFile.Checked = true;
                overwriteMovieFile.Enabled = true;
            }

            AcceptEnable();
        }

        private void overwriteImageFiles_CheckedChanged(object sender, EventArgs e)
        {
            if (ImageFilesOverwrite)
            {
                if (overwriteImageFiles.Checked)
                {
                    outputSelectedRange.Enabled = UseSelectedRange;
                    outputSelectedRange.Checked = false;
                }
                else
                {
                    outputSelectedRange.Enabled = false;
                    outputSelectedRange.Checked = false;
                }
            }
        }

        private void accept_Click(object sender, EventArgs e)
        {
            ImageFilesOverwrite = (overwriteImageFiles.Enabled && overwriteImageFiles.Checked);
            MoveFileOverwrite = (overwriteMovieFile.Enabled && overwriteMovieFile.Checked);
            UseSelectedRange = (outputSelectedRange.Enabled && outputSelectedRange.Checked);
            MovieShow = viewMovie.Checked;
        }

        private void overwriteImgFiles_CheckedChanged(object sender, EventArgs e)
        {
            AcceptEnable();
        }

        private void overwriteMovieFile_CheckedChanged(object sender, EventArgs e)
        {
            AcceptEnable();
        }

        // Provides a little feedback.
        private void AcceptEnable()
        {
            if (overwriteMovieFile.Enabled)
                accept.Enabled = overwriteMovieFile.Checked;
            else
                accept.Enabled = true;
        }
    }
}
