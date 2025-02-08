using System;
using System.Diagnostics;
using System.Windows.Forms;

namespace FractalFactory
{
    public partial class FractalFactory : Form
    {
        private void RecordClicked()
        {
            string statement = StatementGenerate(false);
            if (statement != string.Empty)
            {
                int rowNumber = grid.ActiveRow;
                int rowMax = grid.RowCount - 1;  // Because row indices a zero-based.

                if (rowNumber < rowMax)
                    fractalDb.DisplayOrderUpdate(rowNumber, 1);

                StatementRecord(true, statement);

                AppTitleUpdate();
                ProjectMenuItemsEnable();

                // ASSUMPTION: A statement corresponding to the input controls was recorded.
                // Though the image may have been previously generated, it may also have not been.
                generate.Enabled = true;
                update.Enabled = true;
            }
        }

        private void GenerateClicked()
        {
            Debug.Assert(singleFrame.Checked);

            if (IsCameraMode(CameraMode.Zooming))
            {
                theDomain.ScaleAbout(zoomPt.X, zoomPt.Y, theCamera.Position.Z);

                DomainValuesReflect(theDomain);
            }

            SingleBitmapGenerate();
            imagePending = true;

            zoomPtLocked = false;

            ProjectMenuItemsEnable();
        }

        private void RunClicked()
        {
            Debug.Assert(multiFrame.Checked);

            overallStart = DateTime.Now;
            isTaskRunning = true;
            MultiExecution();
        }

        private void UpdateClicked()
        {
            string statement = StatementGenerate(true);
            if (statement != string.Empty)
                StatementUpdate(grid.ActiveRow, statement);

            update.Enabled = false;

            AppTitleUpdate();
            ProjectMenuItemsEnable();
        }

        private void StopClicked()
        {
            cancelTokenSource.Cancel();
            isTaskRunning = false;

            stop.Enabled = false;

            RunEnable();

            // To the Up Arrow key to work (as opposed to traversing other controls).
            grid.Focus();
        }
    }
}
