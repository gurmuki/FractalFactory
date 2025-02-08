using FractalFactory.Common;
using Microsoft.WindowsAPICodePack.Dialogs;
using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace FractalFactory
{
    public partial class OptionsDialog : Form
    {
        // Different options are presented based on use-case (File/New vs. File/Options)
        private bool forNewProject;

        private ProjectSettings settings = ProjectSettings.DefaultSettings();
        // The active textbox in the defaultFolders groupbox.
        private System.Windows.Forms.TextBox foldersTextbox = null!;
        // The active textbox in the externals groupbox.
        private System.Windows.Forms.TextBox externalsTextbox = null!;

        private string fractalFactoryFolder = string.Empty;

        public const string NEWTON1 = "Newton(1)";
        public const string NEWTON2 = "Newton(2)";
        public const string SECANT = "Secant";
        public const string MANDELBROT = "Mandelbrot";

        public const string STANDARD = "Standard";
        public const string ROT_CCW = "Rotate CCW";
        public const string ROT_CW = "Rotate CW";

        public OptionsDialog(Control control, int offset, bool newProject)
        {
            InitializeComponent();

            forNewProject = newProject;
            calibrate.Checked = false;

            FormLocator.Locate(this, control, offset);
        }

        public ProjectSettings Settings
        {
            get { return settings; }
            set { settings.Copy(value); }
        }

        public bool UseSeedFile { get; private set; } = false;

        public bool Calibrating
        {
            get { return calibrate.Checked; }
            set { calibrate.Checked = value; }
        }

        private void OptionsDialog_Load(object sender, EventArgs e)
        {
            if (forNewProject)
            {
                Text = "New Project Options";
                processingOptions.Visible = false;
                seedFileOptions.Visible = true;

                method.Visible = true;
                defaultFolders.Location = new Point(defaultFolders.Left, method.Bottom + 10);

                seedFileOptions.Location = new Point(seedFileOptions.Left, defaultFolders.Bottom);

                ok.Top = seedFileOptions.Bottom + 10;
            }
            else
            {
                Text = "Options";
                seedFileOptions.Visible = false;
                processingOptions.Visible = true;

                method.Visible = false;
                defaultFolders.Top = method.Top;

                processingOptions.Top = defaultFolders.Bottom;

                ok.Top = processingOptions.Bottom + 10;

                string exePath = System.Reflection.Assembly.GetExecutingAssembly().Location;
                string exeFolder = Path.GetDirectoryName(exePath)!;
                // ASSUMPTION: The application is being run from where it was built.
                int indx = exeFolder.LastIndexOf("FractalFactory");
                if (indx> 0)
                    fractalFactoryFolder = exeFolder.Substring(0, indx + "FractalFactory".Length);
            }

#if DEBUG
            calibrate.Visible = true;
            calibrate.Top = ok.Top;
#else
            calibrate.Visible = false;
#endif

            cancel.Top = ok.Top;
            this.Height = ok.Bottom + ok.Height + 25;

            MethodDropdownInit();

            // seedFilePath.Text = string.Empty;
            seedFilePath.Text = settings.projPath;
            seedFilePath.Enabled = false;
            select.Enabled = false;

            parallelExecution.Checked = settings.parallel;
            serialExecution.Checked = !parallelExecution.Checked;

            limitIterations.Checked = settings.limit;

            orientation.Items.Add(ROT_CCW);
            orientation.Items.Add(STANDARD);
            orientation.Items.Add(ROT_CW);
            OrientationItemSelect(settings.orientation);

            reducedImage.Checked = settings.reduced;
            previewMode.Checked = settings.preview;

            database.Text = settings.databaseFolder;
            defProjFolder.Text = settings.defProjFolder;
            defMovieFolder.Text = settings.defMovFolder;
            foldersBrowse.Enabled = false;

            imageViewer.Text = settings.imageViewer;
            movieGenerator.Text = settings.movieGenerator;
            movieViewer.Text = settings.movieViewer;
            externalsBrowse.Enabled = false;
        }

        private void ok_Click(object sender, EventArgs e)
        {
            settings.DatabaseFolder(database.Text);
            settings.defProjFolder = defProjFolder.Text;
            settings.defMovFolder = defMovieFolder.Text;

            settings.imageViewer = imageViewer.Text;
            settings.movieGenerator = movieGenerator.Text;
            settings.movieViewer = movieViewer.Text;

            if (forNewProject)
            {
                settings.method = method.SelectedItem!.ToString()!;
                if (UseSeedFile && !Stringy.IsEmpty(seedFilePath.Text))
                    settings.projPath = seedFilePath.Text;
            }
            else
            {
                settings.parallel = parallelExecution.Checked;
                settings.limit = limitIterations.Checked;

                settings.orientation = SelectedOrientation();
                settings.reduced = reducedImage.Checked;
                settings.preview = previewMode.Checked;
            }
        }

        private void MethodDropdownInit()
        {
            method.Items.Add(NEWTON1);
            method.Items.Add(NEWTON2);
            method.Items.Add(SECANT);
            method.Items.Add(MANDELBROT);

            method.SelectedItem = settings.method;
        }

        private void seedFilePath_Enter(object sender, EventArgs e) { seedFilePath.SelectAll(); }
        private void seedFilePath_MouseUp(object sender, MouseEventArgs e) { seedFilePath.SelectAll(); }

        private void database_Enter(object sender, EventArgs e) { FoldersBrowseEnable(database); }
        private void database_MouseUp(object sender, MouseEventArgs e) { FoldersBrowseEnable(database); }

        private void defProjFolder_Enter(object sender, EventArgs e) { FoldersBrowseEnable(defProjFolder); }
        private void defProjFolder_MouseUp(object sender, MouseEventArgs e) { FoldersBrowseEnable(defProjFolder); }

        private void defMovieFolder_Enter(object sender, EventArgs e) { FoldersBrowseEnable(defMovieFolder); }
        private void defMovieFolder_MouseUp(object sender, MouseEventArgs e) { FoldersBrowseEnable(defMovieFolder); }

        private void defaultFolders_Leave(object sender, EventArgs e)
        {
            foldersBrowse.Enabled = false;
            foldersTextbox = null!;
        }

        private void FoldersBrowseEnable(TextBox textBox)
        {
            foldersTextbox = textBox;
            foldersTextbox.SelectAll();
            foldersBrowse.Enabled = true;
        }

        private void foldersBrowse_Click(object sender, EventArgs e)
        {
            // For an explanation of how to use (instead) the CommonOpenFileDialog, see
            //     https://stackoverflow.com/questions/11624298/how-do-i-use-openfiledialog-to-select-a-folder
            CommonOpenFileDialog dialog = new CommonOpenFileDialog();
            dialog.InitialDirectory = foldersTextbox.Text;
            dialog.DefaultFileName = PathFuncs.FolderName(foldersTextbox.Text);
            dialog.IsFolderPicker = true;

            if (dialog.ShowDialog() != CommonFileDialogResult.Ok)
                return;

            foldersTextbox.Text = dialog.FileName;
        }

        private void useSeedFile_CheckedChanged(object sender, EventArgs e)
        {
            UseSeedFile = useSeedFile.Checked;
            seedFilePath.Enabled = UseSeedFile;
            select.Enabled = UseSeedFile;

            if (UseSeedFile)
                SendKeys.Send("{Tab}");  // Advance to seedFilePath
        }

        private void select_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Title = "Open Project Text";
            // dialog.InitialDirectory = defProjFolder.Text;
            dialog.InitialDirectory = (Stringy.IsEmpty(settings.projPath) ? defProjFolder.Text : settings.projPath);
            dialog.Filter = "txt files (*.txt)|*.txt";

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                seedFilePath.Text = dialog.FileName;
            }
        }

        private void OrientationItemSelect(int orientationSetting)
        {
            if (orientationSetting == 90)
                orientation.SelectedIndex = orientation.FindStringExact(ROT_CCW);
            else if (orientationSetting == -90)
                orientation.SelectedIndex = orientation.FindStringExact(ROT_CW);
            else
                orientation.SelectedIndex = orientation.FindStringExact(STANDARD);
        }

        private int SelectedOrientation()
        {
            if (orientation.SelectedIndex == 0)
                return 90;
            else if (orientation.SelectedIndex == 2)
                return -90;
            else
                return 0;
        }

        private void imageViewer_Enter(object sender, EventArgs e) { ExternalsBrowseEnable(imageViewer); }
        private void imageViewer_MouseUp(object sender, MouseEventArgs e) { ExternalsBrowseEnable(imageViewer); }

        private void movieGenerator_Enter(object sender, EventArgs e) { ExternalsBrowseEnable(movieGenerator); }
        private void movieGenerator_MouseUp(object sender, MouseEventArgs e) { ExternalsBrowseEnable(movieGenerator); }

        private void movieViewer_Enter(object sender, EventArgs e) { ExternalsBrowseEnable(movieViewer); }
        private void movieViewer_MouseUp(object sender, MouseEventArgs e) { ExternalsBrowseEnable(movieViewer); }

        private void externals_Leave(object sender, EventArgs e)
        {
            externalsBrowse.Enabled = false;
            externalsTextbox = null!;
        }

        private void ExternalsBrowseEnable(TextBox textBox)
        {
            externalsTextbox = textBox;
            externalsTextbox.SelectAll();
            externalsBrowse.Enabled = true;
        }

        private void externalsBrowse_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Title = "File Select";
            dialog.InitialDirectory = (Stringy.IsEmpty(externalsTextbox.Text) ? fractalFactoryFolder : externalsTextbox.Text);
            dialog.Filter = "bat files (*.bat)|*.bat";

            if (dialog.ShowDialog() == DialogResult.OK)
                externalsTextbox.Text = dialog.FileName;
        }
    }
}
