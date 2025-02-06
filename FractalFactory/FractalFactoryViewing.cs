using FractalFactory.Graphics;
using FractalFactory.Math;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

namespace FractalFactory
{
    public partial class FractalFactory : Form
    {
        private void RectUpdate(MouseEvent me, Point pt)
        {
            if (me == MouseEvent.LButtonUp)
            {
                rect.Clear();
            }
            else
            {
                PointD glPt = new PointD(XClip(pt.X), YClip(pt.Y));

                if (me == MouseEvent.LButtonDown)
                    rect.FirstPt(glPt);
                else if (me == MouseEvent.LButtonDownMoving)
                    rect.SecondPt(glPt);
            }
        }

        private float[] fords = { 0, 0, 0, 0, 0, 0, 0, 0 };

        private void RectDraw()
        {
            for (int indx = 0; indx < rect.ords.Length; ++indx)
            { fords[indx] = (float)rect.ords[indx]; }

            GL.BindBuffer(BufferTarget.ArrayBuffer, lineStripBufferObject);
            GL.BufferData(BufferTarget.ArrayBuffer, fords.Length * sizeof(float), fords, BufferUsageHint.StaticDraw);

            Render();
        }

        // NOTE: The app should always have a single set of viewing parameters.
        // CAVEAT: Upon creating a new session, the stacks will be temporarily empty.
        private void ViewDataPush()
        {
            Debug.Assert(cameras.Count == domains.Count, "ViewDataPush()");

            if (cameras.Count < 1)
            {
                // ASSUMPTION: Pushing the parameters describing the initial project view.
                Debug.Assert((theCamera == baseCamera), "Assertion: ViewDataPush() -- theCamera != baseCamera");
                cameras.Push(new Camera(theCamera));
                domains.Push(new Domain(theDomain));
            }
            else
            {
                if ((theCamera != cameras.Peek()) && (theDomain != domains.Peek()))
                {
                    cameras.Push(new Camera(theCamera));
                    domains.Push(new Domain(theDomain));
                }
            }
        }

        private void ViewDataPop()
        {
            Debug.Assert((cameras.Count > 1) && (cameras.Count == domains.Count), "ViewDataPop()");

            theCamera = cameras.Pop();
            theDomain = domains.Pop();

            DomainValuesReflect(theDomain);
        }

        private void ViewDataPeek()
        {
            Debug.Assert((cameras.Count > 0) && (cameras.Count == domains.Count), "ViewDataPeek()");

            theCamera = cameras.Peek();
            theDomain = domains.Peek();

            DomainValuesReflect(theDomain);
        }

        private void ViewDataClear()
        {
            // ASSUMPTION: We're establishing new base viewing parameters.
            cameras.Clear();
            domains.Clear();
        }

        private void ViewBase()
        {
            ViewDataClear();

            theCamera = BaseCameraCopy();
            theDomain = DomainFromInputs();
        }

        // NOTE: From the user's perspective, there are two ways to initiate zooming:
        // 1) Place the cursor where you want the "about point" to be and then
        //    start spinning the mouse wheel.
        // 2) Place the cursor where you want the "about point" to be, left mouse
        //    button click and then start spinning the mouse wheel.
        //
        // Why these two methods? I found myself frequently forgetting to mouse click
        // and found it the behavior to be very annoying.
        private void ZoomPtInit(Point cursorLocation)
        {
            Vector3 vec = new Vector3((float)XClip(cursorLocation.X), (float)YClip(cursorLocation.Y), 0);
            if (workspaceSettings.orientation == 90)
                vec = ccwMatrix * vec;
            else if (workspaceSettings.orientation == -90)
                vec = cwMatrix * vec;

            glZoomPt = new Vector3(vec.X, vec.Y, 0);

            double zx = theDomain.Xc() + (glZoomPt.X * (theDomain.Dx() / 2));
            double zy = theDomain.Yc() + (glZoomPt.Y * (theDomain.Dy() / 2));
            zoomPt = new Vector3((float)zx, (float)zy, 0);
        }

        private bool IsCameraMode(CameraMode mode)
        {
            return (cameraMode == mode);
        }

        // Clips space (viewport coordinates)
        // -1, 1 -------- 1, 1
        //    |             |
        //    |             |
        //    |      +      |
        //    |             |
        //    |             |
        // -1,-1 -------- 1,-1

        /// <summary>Get the cursor X ordinate as a value in clip space (-1 .. 1)</summary>
        private double XClip(int x)
        {
            double tmp = ((double)x - center.X) / center.X;
            return ((System.Math.Abs(tmp) < RESOLUTION) ? 0f : tmp);
        }

        /// <summary>Get the cursor Y ordinate as a value in clip space (-1 .. 1)</summary>
        private double YClip(int y)
        {
            double tmp = ((double)y - center.Y) / -center.Y;
            return ((System.Math.Abs(tmp) < RESOLUTION) ? 0f : tmp);
        }
    }
}
