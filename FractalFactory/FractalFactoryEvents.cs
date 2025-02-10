using FractalFactory.Math;
using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace FractalFactory
{
    // Keyboard processing functionality.
    public partial class FractalFactory : Form
    {
        // Hookup the key and mouse event handlers.
        private void GLControlEventHandlersBind()
        {
            // Log any focus changes.
            #region UnusedEventHandlers
#if false
            glControl.GotFocus += (sender, e) =>
                Log("Focus in");

            glControl.LostFocus += (sender, e) =>
                Log("Focus out");

            // See also FractalFactory_KeyPress() because it intercepts key presses.
            glControl.PreviewKeyDown += (sender, e) =>
            {
                PreviewKeyEvent(e.KeyCode);
            };

            glControl.KeyDown += (sender, e) =>
            {
                // Log($"Key down: {e.KeyCode}");
            };

            glControl.KeyUp += (sender, e) =>
            {
                // Log($"Key up: {e.KeyCode}");
            };

            glControl.KeyPress += (sender, e) =>
            {
                // Log($"Key press: {e.KeyChar}");
            };
#endif
            #endregion

            glControl.MouseDown += MouseDownHandler!;
            glControl.MouseUp += MouseUpHandler!;
            glControl.MouseMove += MouseMoveHandler!;
            glControl.MouseWheel += MouseWheelHandler!;
        }

        private void MouseDownHandler(object sender, MouseEventArgs e)
        {
            // Log($"Mouse down: ({e.X},{e.Y})");
            glControl.Focus();

            if (IsCameraMode(CameraMode.Static))
                return;

            if (IsCameraMode(CameraMode.Windowing))
            {
                RectUpdate(MouseEvent.LButtonDown, e.Location);
                RectDraw();
            }
            else if (IsCameraMode(CameraMode.Panning))
            {
                panPt = e.Location;
                panPtValid = true;
            }
        }

        private void MouseUpHandler(object sender, MouseEventArgs e)
        {
            // Log($"Mouse up: ({e.X},{e.Y})");
            if (e.Button == MouseButtons.Right)
            {
                panToolStripMenuItem.Visible = generate.Enabled;
                windowToolStripMenuItem.Visible = generate.Enabled;
                zoomToolStripMenuItem.Visible = generate.Enabled;
                toolStripSeparator4.Visible = generate.Enabled;
                photoGenerateToolStripMenuItem.Visible = generate.Enabled;

                this.ContextMenuStrip = viewPopMenu;
                this.ContextMenuStrip.Show(Cursor.Position);
                return;
            }

            if (IsCameraMode(CameraMode.Static))
                return;

            if (IsCameraMode(CameraMode.Windowing))
            {
                List<PointD> pts = rect.Points(workspaceSettings.orientation);
                RectUpdate(MouseEvent.LButtonUp, e.Location);

                double dx = System.Math.Abs(pts[1].X - pts[0].X) / 2;
                double dy = System.Math.Abs(pts[0].Y - pts[1].Y) / 2;

                double xFactor = (workspaceSettings.orientation == 0)
                    ? System.Math.Abs(XClip(glControl.Width) / dx)
                    : System.Math.Abs(YClip(glControl.Height) / dy);

                double yFactor = (workspaceSettings.orientation == 0)
                    ? System.Math.Abs(XClip(glControl.Height) / dx)
                    : System.Math.Abs(YClip(glControl.Width) / dy);

                double z = theCamera.Position.Z;

                double scaleFactor = (xFactor > yFactor) ? xFactor : yFactor;
                Vector3 delta = new Vector3(
                    (float)(pts[0].X + pts[1].X) / 2,
                    (float)(pts[0].Y + pts[1].Y) / 2,
                    (float)((z / scaleFactor) - z));

                if (workspaceSettings.orientation == 90)
                    delta = cwMatrix * delta;
                else if (workspaceSettings.orientation == -90)
                    delta = ccwMatrix * delta;

                theCamera.Position += delta;

                //=-=-=-=
                // pts[0] is bottom left / pts[1] is top right
                double bldx = (pts[0].X + 1) / 2;
                double bldy = (pts[0].Y + 1) / 2;
                double trdx = (pts[1].X + 1) / 2;
                double trdy = (pts[1].Y + 1) / 2;

                double xmin = theDomain.Xmin + (bldx * theDomain.Dx());
                double ymin = theDomain.Ymin + (bldy * theDomain.Dy());
                double xmax = theDomain.Xmin + (trdx * theDomain.Dx());
                double ymax = theDomain.Ymin + (trdy * theDomain.Dy());
                //=-=-=-=

                theDomain = new Domain(xmin, ymin, xmax, ymax);
                DomainValuesReflect(theDomain);

                Render();

                // The user must re-establish windowing mode.
                cameraMode = CameraMode.Static;
            }
            else if (IsCameraMode(CameraMode.Panning))
            {
                panPtValid = false;

                // The user must re-establish panning mode.
                cameraMode = CameraMode.Static;
            }
            else if (IsCameraMode(CameraMode.Zooming))
            {
                panPtValid = false;

                if (!zoomPtLocked)
                {
                    ZoomPtInit(e.Location);
                    zoomPtLocked = true;
                }
            }
        }

        private void MouseMoveHandler(object sender, MouseEventArgs e)
        {
            // Log($"Mouse move: ({e.X},{e.Y})");
            Matrix4 m = theCamera.GetViewMatrix();
            Vector3 pos = theCamera.Position;
            Vector3 vec = new Vector3((float)XClip(e.X), (float)YClip(e.Y), 0) * m.M43;
            Vector3 tot = (pos - vec);

            lbInfo.Text = $"x:{tot.X} y:{tot.Y}";

            if (!zoomPtLocked)
                ZoomPtInit(e.Location);

            if (IsCameraMode(CameraMode.Static))
                return;

            if (IsCameraMode(CameraMode.Windowing))
            {
                RectUpdate(MouseEvent.LButtonDownMoving, e.Location);
                RectDraw();
            }
            else if (IsCameraMode(CameraMode.Panning))
            {
                if (!panPtValid)
                    return;

                int dx = e.X - panPt.X;
                int dy = e.Y - panPt.Y;
                if ((dx == 0) && (dy == 0))
                    return;

                panPt = e.Location;

                //=-=-=-=
                DomainFromView(dx, dy);
                DomainValuesReflect(theDomain);
                //=-=-=-=

                float fdz = theCamera.Position.Z;
                float fdx = (dx / (float)glControl.Width) * fdz * 2f;
                float fdy = (dy / (float)glControl.Height) * fdz * 2f;

                // The drag point of image must follow the cursor.
                theCamera.Position += new Vector3(-fdx, fdy, 0);

                Render();
            }
        }

        private void MouseWheelHandler(object sender, MouseEventArgs e)
        {
            if (!IsCameraMode(CameraMode.Zooming))
                return;

            zoomPtLocked = true;

            Vector3 glvec = new Vector3(glZoomPt);
            if (workspaceSettings.orientation == 90)
                glvec = cwMatrix * glvec;
            else if (workspaceSettings.orientation == -90)
                glvec = ccwMatrix * glvec;

            Matrix4 viewA = theCamera.GetViewMatrix();
            Vector3 vecA = glvec * viewA.M43;

            float dz = ((e.Delta > 0) ? -ZOOMDELTA : ZOOMDELTA);
            theCamera.Position += new Vector3(0, 0, dz);

            Matrix4 viewB = theCamera.GetViewMatrix();
            Vector3 vecB = glvec * viewB.M43;

            Vector3 delta = vecB - vecA;
            theCamera.Position += delta;

            //=-=-=

            Domain forShow = new Domain(theDomain);
            forShow.ScaleAbout(zoomPt.X, zoomPt.Y, theCamera.Position.Z);

            DomainValuesReflect(forShow);

            //=-=-=

            Render();
        }

        private void KeyPressProcess(object sender, KeyPressEventArgs e)
        {
            if (isTaskRunning)
                return;

            if (e.KeyChar == '\u0016')  // <ctrl> v
            {
                if ((ActiveControl == xmin)
                    || (ActiveControl == xmax)
                    || (ActiveControl == ymin)
                    || (ActiveControl == ymax)
                    || (ActiveControl == numerPoly)
                    || (ActiveControl == denomPoly))
                {
                    char[] EOL = { '\n' };
                    string text = Clipboard.GetText().Replace("\r", "");

                    string[] statements = text.Split(EOL, StringSplitOptions.RemoveEmptyEntries);
                    if (statements.Length < 1)
                        return;  // nothing to paste

                    e.Handled = (TextProcess(statements[0]) > 0);
                }
                else
                {
                    // We want e.Handled to be false when editing grid cell text.
                    // Otherwise, this action will be cancelled, preventing (eg.)
                    // pasting text into the active grid cell.
                    e.Handled = (grid.EditingControl == null);  // ASSUMPTION: Was handled by the grid control
                }
            }
            else if (grid.EditingControl == null)
            {
                Keys keyCode = (Keys)(Char.IsLetter(e.KeyChar) ? Char.ToUpper(e.KeyChar) : e.KeyChar);
                PreviewKeyEvent(keyCode);
            }
        }

        private void KeyPreviewExecute(Keys keyCode)
        {
            // Again, the generate button is enabled only by ensuring the
            // input controls correspond to the selected grid statement.
            if (!generate.Enabled)
                return;

            if (keyCode == Keys.Escape)
            {
                if (IsCameraMode(CameraMode.Static))
                    return;  // Nothing to do.

                if (IsCameraMode(CameraMode.Windowing))
                    rect.Clear();
                else
                    ViewDataPeek();  // ASSUMPTION: We're actively panning or zooming.

                cameraMode = CameraMode.Static;
                Render();
            }
            else if (keyCode == Keys.W)
            {
                cameraMode = CameraMode.Windowing;
                ViewDataPush();
            }
            else if (keyCode == Keys.C)
            {
                cameraMode = CameraMode.Panning;
                ViewDataPush();
            }
            else if (keyCode == Keys.Z)
            {
                cameraMode = CameraMode.Zooming;
                ViewDataPush();
            }
            else if (keyCode == Keys.U)
            {
                cameraMode = CameraMode.Static;

                if (cameras.Count > 1)
                    ViewDataPop();
                else
                    ViewDataPeek();

                Render();
            }
            else if (keyCode == Keys.B)
            {
                cameraMode = CameraMode.Static;
                ViewBase();
                Render();
            }
        }
    }
}
