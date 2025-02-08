using FractalFactory.Common;
using FractalFactory.Database;
using FractalFactory.Graphics;
using FractalFactory.Statements;
using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FractalFactory
{
    // Fractal project management functionality.
    public partial class FractalFactory : Form
    {
        private void WorkspaceNew()
        {
            update.Enabled = false;

            DialogResult status = DirtyFileSave(projectMenu, 20, "Save changes before switching projects?");
            if (status == DialogResult.Cancel)
                return;

            OptionsDialog dialog = new OptionsDialog(projectMenu, 20, true);
            dialog.Settings = workspaceSettings;
            dialog.Settings.DefaultParametersSet();
            dialog.Calibrating = calibrating;

            if (dialog.ShowDialog() == DialogResult.Cancel)
                return;

            workspaceSettings = dialog.Settings;
            workspaceSettings.movieFolder = string.Empty;

            calibrating = dialog.Calibrating;
            if (calibrating)
            {
                CalibrationInit();
                return;
            }

            WaitCursorStart(false);

            int dy = grid.MaxVisibleRows;

            bool usingSeedFile = dialog.UseSeedFile && !Stringy.IsEmpty(workspaceSettings.projPath);
            if (usingSeedFile && !calibrating)
            {
                GridInitialize();

                string[] contents = File.ReadAllLines(workspaceSettings.projPath);
                List<string> statements = new List<string>(contents.Length);
                foreach (string str in contents)
                {
                    string statement = fractalDb.Preprocess(str);
                    if (Stringy.IsEmpty(statement))
                        continue;

                    statements.Add(statement);
                }

                fractalDb.WorkspacePopulate(statements, GridAdd);
            }
            else
            {
                fractalDb.WorkspaceClear();
                workspaceSettings.projPath = string.Empty;
            }

            string json = workspaceSettings.Serialize();
            fractalDb.SettingsSave(fractalDb.WorkspaceID, json);

            // While we may have read a seed file, it's possible nothing in
            // the seed file represented a valid statement and so no records
            // would have been added to the database.
            fractalDb.IsDirty = (fractalDb.WorkspaceRecordCount() > 0);

            // This next statement affects the Single/Multi Frame radio buttons.
            workspaceSettings.multi = (fractalDb.WorkspaceRecordCount() > 0);

            ControlPanelInitialize(workspaceSettings);
            MethodControlsUpdate(workspaceSettings.method);

            NewProject();

            WaitCursorStop(false);

            ProjectMenuItemsEnable();
        }

        /// <summary>For use with fractalDb.WorkspacePopulate()</summary>
        private void GridAdd(int indx, string str)
        {
            Invoke((Action)(() =>
            {
                if (indx > grid.MaxVisibleRows)
                    return;

                grid.StatementInsert(indx, str);

                bool update = ((grid.RowCount > 10) && ((grid.RowCount % 10) == 0));

                if (update)
                    progressBar.PerformStep();

                if ((indx == grid.MaxVisibleRows) || update)
                    grid.Update();
            }));
        }

        private void NewProject()
        {
            projectID = fractalDb.WorkspaceID;
            projectName = UNNAMED;

            singleFrame.Checked = true;

            ProjectStateReflect(true);
        }

        private void ProjectStateReflect(bool gridPopulate)
        {
            if (gridPopulate)
            {
                // ASSUMPTION: We arrived here via File/New or File/Open.
                ImageClear();
                GridPopulate();
                grid.Focus();

                if (grid.Rows.Count > 0)
                {
                    // Show the image associated with the 0th grid row (if any).
                    grid.ActiveRow = 0;
                    grid_MouseUp(new object(), new MouseEventArgs(MouseButtons.Left, 0, 0, 0, 0));

                    // Update all of the control panel parameters.
                    SetParameters(true);
                }

                ViewDataClear();

                theCamera = BaseCameraCopy();
                theDomain = DomainFromInputs();

                generate.Enabled = true;
            }

            AppTitleUpdate();
            ProjectMenuItemsEnable();
        }

        private void ProjectMenuItemsEnable()
        {
            List<string> projectNames = fractalDb.ProjectNames();
            openProjectMenuItem.Enabled = (projectNames.Count > 0);

            // The Save option should be disabled if the active project is just the workspace.
            saveProjectMenuItem.Enabled = (fractalDb.IsDirty && !fractalDb.IsWorkspaceID(projectID));

            saveAsProjectMenuItem.Enabled = (fractalDb.IsDirty || !fractalDb.IsWorkspaceID(projectID));
            saveTextToolStripMenuItem.Enabled = (grid.RowCount > 0);

            movieProjectMenuItem.Enabled = (fractalDb.WorkspaceImageCount() > 0);

            bool projectsExist = (fractalDb.ProjectsCount() > 0);
            renameProjectMenuItem.Enabled = projectsExist;
            deleteProjectMenuItem.Enabled = projectsExist;
        }

        private DialogResult DirtyFileSave(Control control, int offset, string msg)
        {
            DialogResult status = DialogResult.OK;

            if (fractalDb.IsDirty)
            {
                status = MessageDialog.Show(control, offset, "Warning!", msg, MessageBoxButtons.YesNoCancel);
                if (status == DialogResult.Yes)
                    status = ProjectSaveAs();
            }

            return status;
        }

        private void ProjectOpen()
        {
            List<string> projectNames = fractalDb.ProjectNames();
            Debug.Assert((projectNames.Count > 0), "ProjectOpen()");  // Should not arrive here because the Open menu item should be disabled.
            if (projectNames.Count < 1)
                return;

            DialogResult status = DirtyFileSave(projectMenu, 20, "Save changes before switching projects?");
            if (status == DialogResult.Cancel)
                return;

            ProjectOpenDialog dialog = new ProjectOpenDialog(projectMenu, 20);
            dialog.ProjectNames = projectNames;

            if ((dialog.ShowDialog() != DialogResult.OK) || (dialog.ProjectName == string.Empty))
                return;

            WaitCursorStart(false);

            // Assuming no errors, an existing project has been selected by name.
            long pid = fractalDb.ProjectID(dialog.ProjectName);
            if (pid < 0)
            {
                WaitCursorStop();
                return;
            }

            DateTime start = DateTime.Now;

            // Set the temporary project based upon the selected project.
            projectID = pid;
            pid = fractalDb.WorkspacePopulate(projectID);
            if (pid < 0)
            {
                WaitCursorStop();
                return;
            }

            Debug.WriteLine($"ProjectOpen: {DateTime.Now - start}");

            string json = fractalDb.SerializedSettings(projectID);
            workspaceSettings.Deserialize(json);

            ControlPanelInitialize(workspaceSettings);
            MethodControlsUpdate(workspaceSettings.method);

            ViewBase();

            GLModelUpdate(workspaceSettings.orientation);
            FormResize(workspaceSettings.orientation, workspaceSettings.reduced);

            projectName = dialog.ProjectName;
            ProjectStateReflect(true);

            WaitCursorStop();
        }

        private DialogResult ProjectSaveAs()
        {
            DialogResult status = DialogResult.Cancel;

            List<string> existingProjects = fractalDb.ProjectNames();

            ProjectSaveAsDialog dialog = new ProjectSaveAsDialog(projectMenu, 20, existingProjects);
            dialog.ProjectName = ((projectName == UNNAMED) ? string.Empty : projectName);

            status = dialog.ShowDialog();
            if (status == DialogResult.OK)
            {
                long candidateProjectID = fractalDb.ProjectID(dialog.ProjectName);
                if (candidateProjectID < 0)
                    candidateProjectID = fractalDb.ProjectNew(dialog.ProjectName);

                ProgressBarTask pbt = new ProgressBarTask("Saving", SavingTime());
                pbt.task = Task.Run(() => { ProjectSaveCore(candidateProjectID); });
                TaskProgressShow(pbt);

                projectID = candidateProjectID;
                projectName = dialog.ProjectName;

                fractalDb.IsDirty = false;

                AppTitleUpdate();
                RunEnable();

                ProjectStateReflect(false);
            }

            return status;
        }

        private bool ProjectSave()
        {
            bool okay = !fractalDb.IsWorkspaceID(projectID);
            Debug.Assert(okay, "ProjectSave()");
            if (!okay)
                return false;

            ProgressBarTask pbt = new ProgressBarTask("Saving", SavingTime());
            pbt.task = Task.Run(() => { ProjectSaveCore(projectID); });
            TaskProgressShow(pbt);

            fractalDb.IsDirty = false;

            AppTitleUpdate();
            RunEnable();

            return true;
        }

        private void ProjectSaveCore(long projectID)
        {
            string json = ProjectSettingsSerialize();
            fractalDb.SettingsSave(projectID, json);
            fractalDb.WorkspaceSaveTo(projectID);
            // fractalDb.Compact();  -- This takes a while so defer it to app shutdown?
        }

        /// <summary>Compute empirical processing time for saving a project.</summary>
        /// <returns>Guesstimated processing time (in seconds).</returns>
        /// <remarks>
        /// You should adjust these calculations to account
        /// for your hardware performance.
        /// </remarks>
        private float SavingTime()
        {
            int imageCount = fractalDb.WorkspaceImageCount();
            int noImageCount = grid.RowCount - imageCount;
            float estimatedTimeAsSeconds = (0.025f * imageCount) + (0.006f * noImageCount);
            return estimatedTimeAsSeconds;
        }

        /// <summary>Compute empirical processing time for compacting the database.</summary>
        /// <returns>Guesstimated processing time (in seconds).</returns>
        /// <remarks>
        /// You should adjust these calculations to account
        /// for your hardware performance.
        /// </remarks>
        private float CompactingTime(string dbFolder)
        {
            string dbPath = Path.Combine(dbFolder, "fractal.db");
            FileInfo fileInfo = new FileInfo(dbPath);
            float estimatedTimeAsSeconds = fileInfo.Length * 1.1e-8f;
            return estimatedTimeAsSeconds;
        }

        private DialogResult ProjectTextSave()
        {
            DialogResult status = DialogResult.Cancel;

            if (grid.RowCount < 1)
                return status;

            SaveFileDialog dialog = new SaveFileDialog();
            dialog.Title = "Save Project Text";
            dialog.InitialDirectory = workspaceSettings.ProjectFolder();
            dialog.FileName = workspaceSettings.ProjectFilename();
            dialog.Filter = "txt files (*.txt)|*.txt";

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                workspaceSettings.projPath = dialog.FileName;

                WaitCursorStart();

                if (!fractalDb.IsWorkspaceID(projectID))
                {
                    string json = workspaceSettings.Serialize();
                    fractalDb.SettingsSave(projectID, json);
                }

                fractalDb.WorkspaceSaveTo(dialog.FileName);
                status = DialogResult.OK;

                WaitCursorStop();
            }

            return status;
        }

        private void ProjectRename()
        {
            string currProjectName = fractalDb.ProjectName(projectID);

            List<string> existingProjects = fractalDb.ProjectNames();
            ProjectRenameDialog dialog = new ProjectRenameDialog(projectMenu, 20, existingProjects);
            dialog.ActiveProjectName = currProjectName;

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                if (ConditionalProjectRename(dialog.FromProjectName, dialog.ToProjectName))
                {
                    if (currProjectName == dialog.FromProjectName)
                    {
                        projectName = dialog.ToProjectName;
                        AppTitleUpdate();
                    }
                }
            }
        }

        private bool ConditionalProjectRename(string existingProjectName, string newProjectName)
        {
            long existingProjectID = fractalDb.ProjectID(existingProjectName);
            long newProjectID = fractalDb.ProjectID(newProjectName);

            if (existingProjectID <= 0)
            {
                MessageDialog.Show("Error!", MsgFormat("No project named", existingProjectName), MessageBoxButtons.OK);
                return false;
            }

            if (existingProjectID == newProjectID)
                throw new InvalidOperationException($"{existingProjectName} is already named {newProjectName}!");

            if (newProjectID > 0)
            {
                DialogResult answer = MessageDialog.Show("Warning!", MsgFormat("Overwrite the exiting project", newProjectName), MessageBoxButtons.YesNo);
                if (answer == DialogResult.No)
                    return false;

                fractalDb.ProjectDelete(newProjectID);
            }

            return fractalDb.ProjectRename(existingProjectName, newProjectName);
        }

        private void ProjectDelete()
        {
            List<string> projectNames = fractalDb.ProjectNames();
            if (projectNames.Count < 1)
                return;

            ProjectDeleteDialog dialog = new ProjectDeleteDialog(projectMenu, 20);
            dialog.ProjectNames = projectNames;

            if ((dialog.ShowDialog() != DialogResult.OK) || (dialog.ProjectName == string.Empty))
                return;

            long deleteID = fractalDb.ProjectID(dialog.ProjectName);
            if (deleteID < 1)
                return;  // something is very wrong

            fractalDb.ProjectDelete(deleteID);

            ProgressBarTask pbt = new ProgressBarTask("Compacting", CompactingTime(workspaceSettings.databaseFolder));
            pbt.task = Task.Run(() => { fractalDb.Compact(); });
            TaskProgressShow(pbt);

            if (deleteID == projectID)
                NewProject();
        }

        private void ProjectOptions()
        {
            OptionsDialog dialog = new OptionsDialog(projectMenu, 20, false);
            dialog.Settings = workspaceSettings;
            dialog.Calibrating = calibrating;

            if (dialog.ShowDialog(this) == DialogResult.OK)
            {
                bool viewOptionsChanging = ((workspaceSettings.orientation != dialog.Settings.orientation)
                    || (workspaceSettings.reduced != dialog.Settings.reduced));

                calibrating = dialog.Calibrating;
                if (calibrating)
                {
                    workspaceSettings = dialog.Settings;
                    CalibrationInit();
                    return;
                }

                if (viewOptionsChanging)
                {
                    if (grid.RowCount > 0)
                    {
                        int rowNumber = grid.ActiveRow;

                        // NOTE: There's likely a more visually aesthetic way of doing this.
                        grid.SelectAll();

                        string msg = "This action will clear all workspace images from the database.\n    Proceed?";
                        bool success = SelectedImagesClear(msg);
                        grid.ClearSelection();
                        grid.StatementSelect(rowNumber, true);

                        if (!success)
                            return;
                    }
                }

                workspaceSettings.ExternalPathsSave();

                string previousDbFolder = workspaceSettings.databaseFolder;
                workspaceSettings = dialog.Settings;

                if (viewOptionsChanging)
                {
                    GLModelUpdate(workspaceSettings.orientation);
                    FormResize(workspaceSettings.orientation, workspaceSettings.reduced);
                }

                string json = workspaceSettings.Serialize();
                if (dialog.Settings.databaseFolder != previousDbFolder)
                {
                    // Alternatively, SQLite could be used to copy the active workspace
                    // (images and all) to the new database. Hardly worth the bother at
                    // this time.
                    string msg = "Changing the database location has the follow effects:\n"
                        + "1. It initially creates an empty database in the new location.\n"
                        + "2. It only populates the new workspace with the grid contents.\n"
                        + "   Any images related to the current project will not be copied.\n"
                        + "3. Your current database will (essentially) be orphaned but\n"
                        + "   remaining at its current location.\n"
                        + "4. The active project (if any) will not be copied to the new\n"
                        + "   database. Instead, you follow up this action with File/Save As\n"
                        + "   to save your project to the new database.";

                    DialogResult status = MessageDialog.Show(projectMenu, 20, "Warning!", msg, MessageBoxButtons.YesNo);
                    if (status == DialogResult.Yes)
                    {
                        ViewDataClear();

                        fractalDb.WorkspaceClear();

                        ProgressBarTask pbt = new ProgressBarTask("Compacting", CompactingTime(previousDbFolder));
                        pbt.task = Task.Run(() =>
                        {
                            fractalDb.Compact();
                            fractalDb.Close();
                        });
                        TaskProgressShow(pbt);

                        // Update the Windows registry.
                        workspaceSettings.DatabaseFolder(dialog.Settings.databaseFolder);

                        fractalDb = new FractalDB();
                        fractalDb.DbInit(Path.Combine(dialog.Settings.databaseFolder, "fractal.db"));

                        // We could, instead, create a grid method to create/populate contents[].
                        List<string> contents = new List<string>(grid.RowCount); ;
                        for (int indx = 0; indx < grid.RowCount; ++indx)
                        { contents.Add(grid.StatementAt(indx)); }

                        fractalDb.WorkspacePopulate(contents, null!);

                        if (fractalDb.IsNewDatabase)
                        {
                            fractalDb.SettingsSave(fractalDb.WorkspaceID, json);
                            fractalDb.IsDirty = false;  // Prevent File/SaveAs from being enabled.
                        }

                        ControlPanelInitialize(workspaceSettings);

                        theDomain = DomainFromInputs();

                        // CRITICAL: theCamera must be established before calling FormResize()
                        // Why? Because FormResize() indirectly calls ImageClear(). In turn,
                        // ImageClear() requires theCamera because the shader is used when
                        // rendering the glControl.
                        theCamera = BaseCameraCopy();

                        FormResize(workspaceSettings.orientation, workspaceSettings.reduced);
                    }
                    else
                    {
                        workspaceSettings.DatabaseFolder(previousDbFolder);
                    }
                }
                else if (fractalDb.IsWorkspaceID(projectID))
                {
                    fractalDb.SettingsSave(fractalDb.WorkspaceID, json);
                    fractalDb.IsDirty = false;  // Prevent File/SaveAs from being enabled.
                }
                else
                {
                    fractalDb.SettingsSave(projectID, json);
                    fractalDb.IsDirty = true;
                }
            }

            ProjectMenuItemsEnable();
        }

        /// <summary>Get the current settings as a json string.</summary>
        /// <remarks>This includes the polynomials, domain, ProjectPath and MovieFolder.</remarks>
        /// <returns>a json string</returns>
        private string ProjectSettingsSerialize()
        {
            ProjectSettings settings = new ProjectSettings();

            // Preserve settings managed by the OptionsDialog.
            settings = workspaceSettings;

            // Update settings managed in the controlPanel.
            settings.numer = numerPoly.Text;
            settings.denom = denomPoly.Text;

            settings.xmin = xmin.Text;
            settings.ymin = ymin.Text;
            settings.xmax = xmax.Text;
            settings.ymax = ymax.Text;

            settings.multi = multiFrame.Checked;

            settings.precision = precision.Text;
            settings.divs = divs.Text;

            return settings.Serialize();
        }

        private void GLModelUpdate(int orientation)
        {
            // Matrix4.CreateRotationZ(+/- HALF_PI) doesn't create ideal matrices, so ....
            if (orientation == 90)
            {
                Vector4 xvec = new Vector4(0, 1, 0, 0);
                Vector4 yvec = new Vector4(-1, 0, 0, 0);
                model = new Matrix4(xvec, yvec, Vector4.UnitZ, Vector4.UnitW);
            }
            else if (orientation == -90)
            {
                Vector4 xvec = new Vector4(0, -1, 0, 0);
                Vector4 yvec = new Vector4(1, 0, 0, 0);
                model = new Matrix4(xvec, yvec, Vector4.UnitZ, Vector4.UnitW);
            }
            else
            {
                model = Matrix4.Identity;
            }

            modelInverse = new Matrix3(model);
            modelInverse.Invert();
        }

        /// <summary>Sets the values of the equation and domain controls as well as the ProjectPath and MovieFolder.</summary>
        /// <param name="json">a json string representing project parameters</param>
        private void ControlPanelInitialize(ProjectSettings settings)
        {
            initializingControlPanel = true;

            numerPoly.Text = settings.numer;
            denomPoly.Text = settings.denom;

            xmin.Text = settings.xmin;
            ymin.Text = settings.ymin;
            xmax.Text = settings.xmax;
            ymax.Text = settings.ymax;

            multiFrame.Checked = settings.multi;
            singleFrame.Checked = !multiFrame.Checked;

            precision.Text = settings.precision;

            int prec = PrecisionAsInt();
            statementFormatter = new StatementFormatter(prec);
            statementProcessor = new StatementProcessor(prec);
            gridStatementProcessor = new StatementProcessor(prec);

            divs.Text = settings.divs;

            initializingControlPanel = false;
        }

        /// <summary>Populates the grid using Workspace statements.</summary>
        private void GridPopulate()
        {
            GridInitialize();

            List<string> statements = fractalDb.WorkspaceStatements();
            for (int indx = 0; indx < statements.Count; ++indx)
            {
                grid.StatementInsert(indx, statements[indx]);
            }
        }

        private void GridInitialize()
        {
            grid.Clear();

            List<string> visibleColumns = new List<string>();
            visibleColumns.Add("Text");

            grid.GridInitialize(visibleColumns);
        }

        // Why? Because the baseCamera is immutable.
        private Camera BaseCameraCopy()
        {
            return new Camera(baseCamera);
        }

        private void CalibrationInit()
        {
            GLModelUpdate(workspaceSettings.orientation);
            FormResize(workspaceSettings.orientation, workspaceSettings.reduced);

            ControlPanelInitialize(workspaceSettings);
            MethodControlsUpdate(workspaceSettings.method);

            NewProject();

            ProjectMenuItemsEnable();
        }
    }
}
