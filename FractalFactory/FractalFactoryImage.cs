using FractalFactory.Common;
using FractalFactory.Generators;
using FractalFactory.Graphics;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;

namespace FractalFactory
{
    // Image saving and viewing functionality.
    public partial class FractalFactory : Form
    {
        private void  PhotoGenerate()
        {
            bool haveImageViewer = true;

            if (Stringy.IsEmpty(workspaceSettings.imageViewer))
            {
                string msg =
                      "An image will be generated but will not be presented by an image viewer\n"
                    + "because one has not been chosen. Use the File/Options dialog to select one.\n\n"
                    + "        Continue?";

                if (MessageDialog.Show(this, 20, "Warning", msg, MessageBoxButtons.YesNo) == DialogResult.No)
                    return;

                haveImageViewer = false;
            }
            
            if (haveImageViewer && !File.Exists(workspaceSettings.imageViewer))
            {
                string msg =
                      "An image will be generated but will not be presented because the selected\n"
                    + "image viewer can not be found. Use the File/Options dialog to verify the\n"
                    + "selected image viewer.\n\n"
                    + "        Continue?";

                if (MessageDialog.Show(this, 20, "Warning", msg, MessageBoxButtons.YesNo) == DialogResult.No)
                    return;

                haveImageViewer = false;
            }

            string folder = Stringy.IsEmpty(workspaceSettings.movieFolder)
                ? workspaceSettings.defMovFolder : workspaceSettings.movieFolder;

            SaveFileDialog dialog = new SaveFileDialog();
            dialog.Filter = "Image|*.png";
            dialog.Title = "Save Photograph";
            dialog.InitialDirectory = folder;
            dialog.FileName = ImageNameFormat();

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                // This next statement is a hack to work around the case where <enter>
                // is used to close the dialog. In this case, the key-up event somehow
                // makes its way to the grid control, causing it to insert a line. So,
                // here we simply take focus away from the grid. This can be annoying,
                // but a robust solution is hardly worth the effort.
                this.ActiveControl = null;

                isTaskRunning = true;
                WaitCursorStart(false);

                PhotoGenerate(dialog.FileName);

                WaitCursorStop(false);
                isTaskRunning = false;

                if (haveImageViewer)
                    PictureOpen(dialog.FileName);
            }
        }

        private void PhotoGenerate(string path)
        {
            // Recommended setting from quick web search.
            // However, following is a calculator that might be better(?)
            //    https://toolstud.io/photo/dpi.php?width=12&width_unit=inch&height=18&height_unit=inch&dpi=300&bleed=0&bleed_unit=mm

            // int height = 1080;
            // int width = 1620;
            int width = ((workspaceSettings.orientation == 0) ? 2682 : 1788);
            int height = ((workspaceSettings.orientation == 0) ? 1788 : 2682);

            int size = width * height * 4;  // rgba
            byte[] tempBitmap = new byte[size];
            Texture tempTexture = Texture.LoadFromMemory(tempBitmap, width, height);

            // Why use DomainFromInputs() and not ViewDataPeek()?
            //    Because the text of the domain controls may have been edited.
            theDomain = DomainFromInputs();

            FractalGenerator generator = GeneratorCreate();

            generator.NumeratorPoly(Poly.TermsGet(numerPoly.Text, PolyFunction.VERBATIM));
            if (workspaceSettings.method == OptionsDialog.USER_DEFINED2)
                generator.DenominatorPoly(Poly.TermsGet(denomPoly.Text, PolyFunction.VERBATIM));

            cancelTokenSource = new CancellationTokenSource();
            cancelToken = cancelTokenSource.Token;

            generator.BitmapGenerate(cancelToken, theDomain, tempTexture);

            if (cancelToken.IsCancellationRequested)
                return;

            TimesUpdate(generator.ExecutionTime);

            tempTexture.Orientation = workspaceSettings.orientation;
            ImageSave(path, tempTexture);
        }

        /// <summary>
        /// Creates a file name from the current input control values,
        /// the result looking much like a statement in the grid control.
        /// </summary>
        //
        //  The file names are formatted this way to provide information
        //  about the parameters used to generate the image. After a little
        //  editing, you can paste the parametric part of the file name into
        //  any Polynomial or Domain input control, magically populating all
        //  of the inputs. Then you can regenerate the image.
        //
        //  Why would you do this? If you happen to loose the original project
        //  data (database of text file) this can provide a starting point for
        //  creating a new project.
        //
        //  The general form of the generated file name is:
        //      [project name],[numerator],[denominator],[domain]
        //
        //  but ':' and '*' are not a valid file characters and so are either
        //  converted or deleted. For example:
        // 
        //     before: test,numer:x^3-1,denom:3*x^2.3,xmin:-2.5,ymin:-1.6,xmax:2.5,ymax:1.6
        //     after:  test,numer=x^3-1,denom=3x^2.3,xmin=-2.5,ymin=-1.6,xmax=2.5,ymax=1.6
        //
        private string ImageNameFormat()
        {
            string projectName = fractalDb.ProjectName(projectID);
            if (Stringy.IsEmpty(projectName))
                projectName = UNNAMED;

            string numer = numerPoly.Text.Replace("*", "");
            string denom = denomPoly.Text.Replace("*", "");

            string dom = statementFormatter.DomainStatementCreate(RawDomain());
            dom = dom.Replace(':', '=');

            return string.Format($"{projectName},{Stringy.NUMER}={numer},{Stringy.DENOM}={denom},{dom}").Replace(" ", "");
        }

#if false
        // Open for display using the default associated application.
        //   See also: https://csharpforums.net/threads/process-start-more-image.5557/
        private void PictureOpen(string path)
        {
            ProcessStartInfo Process_Info = new ProcessStartInfo(path, @"%SystemRoot%\System32\rundll32.exe %ProgramFiles%\Windows Photo Viewer\PhotoViewer.dll, ImageView_Fullscreen %1")
            {
                UseShellExecute = true,
                WorkingDirectory = Path.GetDirectoryName(path),
                FileName = path,
                Verb = "runas"
            };
            Process.Start(Process_Info);
        }
#else
        // This solution is more generic, allowing to use whatever image viewer you desire.
        //
        // BEWARE: ImageNameFormat() preserves the '^' character in file names. While '^' is
        // a valid file name character, it appears some applications don't like it, acting
        // as if the '^' isn't present and attempt to open a non-existing file. This may
        // require you to modify ImageNameFormat() for your purposes.
        //
        //     See also: the comments associated with ImageNameFormat().
        private void PictureOpen(string path)
        {
            int showCmdWindow = 0;  // set to zero to hide the command window

            Process process = new Process();
            process.StartInfo.Verb = "runas";

            process.StartInfo.Arguments = $"\"{path}\" {showCmdWindow}";
            process.StartInfo.FileName = workspaceSettings.imageViewer;

            if (showCmdWindow == 0)
                process.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;

            process.Start();
            Debug.WriteLine($"PictureOpen({process.Id})");
            process.WaitForExit();
            Debug.WriteLine("PictureOpen(exited)");
        }
#endif

        private void ImageSave()
        {
            SaveFileDialog dialog = new SaveFileDialog();
            dialog.Filter = "Image|*.png";
            dialog.Title = "Save an Image File";
            dialog.FileName = ImageNameFormat();

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                // This next statement is a hack to work around the case where <enter>
                // is used to close the dialog. In this case, the key-up event somehow
                // makes its way to the grid control, causing it to insert a line. So,
                // here we simply take focus away from the grid. This can be annoying,
                // but a robust solution is hardly worth the effort.
                this.ActiveControl = null;

                ImageSave(dialog.FileName, texture);
            }
        }

        private void ImageSave(string path, Texture texture)
        {
            string ext = Path.GetExtension(path).ToLower();

            ImageType imageType;
            if (ext == ".png")
                imageType = ImageType.Png;
            else if (ext == ".bmp")
                imageType = ImageType.Bmp;
            else
                return;

            byte[] brga = RGBA_to_BRGA(texture);
            Image img = ImageExtensions.ImageFromRawBgraArray(brga, texture.Cols, texture.Rows, PixelFormat.Format32bppArgb);

            Bitmap bmp;
            if (texture.Orientation == 0)
                bmp = new Bitmap(img, texture.Cols, texture.Rows);
            else
                bmp = new Bitmap(img, texture.Rows, texture.Cols);

            img.Dispose();

            if (workspaceSettings.orientation == 0)
                bmp.RotateFlip(RotateFlipType.RotateNoneFlipY);
            else if (workspaceSettings.orientation == 90)
                bmp.RotateFlip(RotateFlipType.Rotate270FlipX);
            else if (workspaceSettings.orientation == -90)
                bmp.RotateFlip(RotateFlipType.Rotate90FlipX);

            if (imageType == ImageType.Png)
                bmp.Save(path, ImageFormat.Png);
            else if (imageType == ImageType.Bmp)
                bmp.Save(path, ImageFormat.Bmp);
        }

        // OpenGL is RGBA but bitmaps are BRGA.
        private byte[] RGBA_to_BRGA(Texture texture)
        {
            byte[] rgba = texture.Values;
            byte[] brga = new byte[texture.Size];

            int size = texture.Size;
            for (int i = 0; i < size; i += 4)
            {
                brga[i + 0] = rgba[i + 2];
                brga[i + 1] = rgba[i + 1];
                brga[i + 2] = rgba[i + 0];
                brga[i + 3] = rgba[i + 3];
            }

            return brga;
        }
    }

    // Much thanks to https://stackoverflow.com/questions/9173904/byte-array-to-image-conversion
    internal static class ImageExtensions
    {
        public static Image ImageFromRawBgraArray(this byte[] arr, int width, int height, PixelFormat pixelFormat)
        {
            var output = new Bitmap(width, height, pixelFormat);
            var rect = new Rectangle(0, 0, width, height);
            var bmpData = output.LockBits(rect, ImageLockMode.ReadWrite, output.PixelFormat);

            // Row-by-row copy
            var arrRowLength = width * Image.GetPixelFormatSize(output.PixelFormat) / 8;
            var ptr = bmpData.Scan0;
            for (var i = 0; i < height; i++)
            {
                Marshal.Copy(arr, i * arrRowLength, ptr, arrRowLength);
                ptr += bmpData.Stride;
            }

            output.UnlockBits(bmpData);
            return output;
        }
    }
}
