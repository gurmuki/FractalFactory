using FractalFactory.Common;
using FractalFactory.Database;
using FractalFactory.Graphics;
using FractalFactory.Math;
using FractalFactory.Statements;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FractalFactory
{
    using CameraStack = GenericStack<Camera>;
    using DomainStack = GenericStack<Domain>;

    using Keys = System.Windows.Forms.Keys;

    /// <summary>
    /// By default, a SQLite project database "fractal.db" will be created in the folder
    /// containing the executable. The default location can be modified via File/Options.
    /// Being necessarily persistent data, the location of the database is recorded in
    /// the Windows registry under HKEY_CURRENT_USER\Software\<exe name>. 
    /// </summary>
    public partial class FractalFactory : Form
    {
        // Because we're adding a texture, we modify the vertex array to include texture
        // coordinates which (incidentally and intentional) span the entire model space.
        // Texture coordinates range from 0.0 to 1.0, with (0.0, 0.0) representing the
        // bottom left, and (1.0, 1.0) representing the top right. This layout is three
        // floats for a vertex followed by two floats for corresponding texture coordinates.
        private readonly float[] vertices =
        {
            // Position | Texture coordinates
             1f,  1f, 0f, 1f, 1f, // top right
             1f, -1f, 0f, 1f, 0f, // bottom right
            -1f, -1f, 0f, 0f, 0f, // bottom left
            -1f,  1f, 0f, 0f, 1f  // top left
        };

        // MY NOTE: The indices representing two triangles
        //  (0, 1, 3) - upper right triangle
        //  (1, 2, 3) - lower left triangle
        private readonly uint[] indices =
        {
            0, 1, 3,
            1, 2, 3
        };

        // The shader code could (of course) be loaded from files.
        // https://opentk.net/learn/chapter1/8-coordinate-systems.html
        #region ShaderCode

        private string vertexShaderCode =
            @"#version 330 core

            layout(location = 0) in vec3 aPosition;

            // We add another input variable for the texture coordinates.

            layout(location = 1) in vec2 aTexCoord;

            // ...However, they aren't needed for the vertex shader itself.
            // Instead, we create an output variable so we can send that data to the fragment shader.

            out vec2 texCoord;

            uniform mat4 model;
            uniform mat4 view;
            uniform mat4 projection;

            void main(void)
            {
                // Then, we further the input texture coordinate to the output one.
                // texCoord can now be used in the fragment shader.
    
                texCoord = aTexCoord;

                gl_Position = vec4(aPosition, 1.0) * model * view * projection;
            }";

        private string fragmentShaderCode =
            @"#version 330

            out vec4 outputColor;

            in vec2 texCoord;

            // A sampler2d is the representation of a texture in a shader.
            // Each sampler is bound to a texture unit (texture units are described in Texture.cs on the Use function).
            // By default, the unit is 0, so no code-related setup is actually needed.
            // Multiple samplers will be demonstrated in section 1.5.
            uniform sampler2D texture0;

            void main()
            {
                // To use a texture, you call the texture() function.
                // It takes two parameters: the sampler to use, and a vec2, used as texture coordinates.
                outputColor = texture(texture0, texCoord);
            }";

        private string vLineStripShaderCode =
            @"#version 330 core

            layout(location = 0) in vec2 aCorner;

            vec3 vertex;

            void main(void)
            {
                vertex = vec3(aCorner, 0.1);

                gl_Position = vec4(vertex, 1.0);
            }";

        private string fLineStripShaderCode =
            @"#version 330 core

            out vec4 outputColor;

            void main()
            {
                // To use a texture, you call the texture() function.
                // It takes two parameters: the sampler to use, and a vec2, used as texture coordinates.
                outputColor = vec4(1.0, 0.0, 0.0, 1.0);
            }";

        #endregion

        private int elementBufferObject;
        private int vertexBufferObject;
        private int vertexArrayObject;
        private int lineStripBufferObject;
        private int lineStripArrayObject;

        // Properly initialized in Form1_Load().
        // shader is used to generate texture.
        private Shader shader = new InvalidShader();
        private Texture texture = new InvalidTexture();

        // Properly initialized in Form1_Load().
        // rectShader is used when drawing a windowing rectangle on top of texture.
        private Shader rectShader = new InvalidShader();

        // rect is used for windowing.
        private Rect rect = new Rect();

        // center is used by GLxord() and GLyord() so they
        // can return coordinates in clip space [-1..+1]
        private PointD center;

        //=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=
        // The parameters defining a view consists of a Domain and a Camera.
        //     See also: ViewDataPush(), ViewDataPop(), ViewDataPeek()
        //=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=

        // Properly initialized in Form1_Load().
        private readonly Camera baseCamera = new Camera(new Vector3(0f, 0f, 1f) * 1f, 1f);
        private Camera theCamera = new InvalidCamera();
        private Domain theDomain = new Domain();

        // For managing view changes (recording/undoing).
        // The stacks are also reset upon clicking the Generate button.
        private DomainStack domains = new DomainStack();
        private CameraStack cameras = new CameraStack();

        private Matrix4 model;
        private Matrix3 modelInverse;
        private Matrix3 cwMatrix;
        private Matrix3 ccwMatrix;

        //=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=

        enum MouseEvent { None, LButtonDown, LButtonDownMoving, LButtonUp }

        enum CameraMode { Static, Zooming, Windowing, Panning }
        private CameraMode cameraMode { get; set; } = CameraMode.Static;

        private Point panPt = new Point();
        private bool panPtValid = false;

        // The gl delta-z for each mouse wheel
        const float ZOOMDELTA = 0.01f;

        // The limit below which any gl ordinate is considered to be zero.
        const float RESOLUTION = 1E-6F;

        private Vector3 glZoomPt;
        private Vector3 zoomPt;

        // While it would be possible to zoom about the cursor location as it
        // is moved, supporting that behavior complicates things. Instead, the
        // solution employed is to "lock the about point". See ZoomPtInit()
        // for a better explanation.
        private bool zoomPtLocked = false;

        //=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=

        private string appTitle = string.Empty;

        const string UNNAMED = "unnamed";
        private string projectName = string.Empty;
        private long projectID = -1;

        // Properly initialized in Form1_Load().
        private ProjectSettings workspaceSettings = new ProjectSettings();

        // Properly initialized in Form1_Load().
        private FractalDB fractalDb = new FractalDB();

        enum ImageType { Bmp, Png }

        private byte[] theBitmap = { };

        // Properly updated in MultiExecution().
        private FractalDBWalker dbWalker = new InvalidFractalDBWalker();
        private bool isTaskRunning = false;

        private TimeSpan frameTime;
        private TimeSpan totalTime;
        private DateTime overallStart;

        private const int DEFAULT_PRECISION = 6;
        // Properly initialized in ControlsInitialize().
        private StatementFormatter statementFormatter = new StatementFormatter(DEFAULT_PRECISION);
        private StatementProcessor statementProcessor = new StatementProcessor(DEFAULT_PRECISION);

        // A separate instance of StatementProcessor is necessary
        // to avoid overwriting the state of statementProcessor.
        private StatementProcessor gridStatementProcessor = new StatementProcessor(DEFAULT_PRECISION);
        private string activeStatement = string.Empty;

        // For user-intiated termination of tasks.
        private CancellationTokenSource cancelTokenSource = new CancellationTokenSource();
        private CancellationToken cancelToken;

        // Upon selecting the Generate button, imagePending is set to true,
        // helping to ensure the correct image is saved with a recorded statement.
        private bool imagePending = false;

        private bool initializingControlPanel;
        private const float MINIMUM_TIME = 3;

        private bool calibrating = false;  // A debugging feature.

        //=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=

        public FractalFactory()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.Location = new Point(40, 20);

            progressPanel.Location = new Point(recordingGroupBox.Location.X, recordingGroupBox.Bottom);
            progressPanel.Visible = false;

            // CRITICAL: KeyPreview must be set 'true' to intercept
            // key presses for view changing.
            KeyPreview = true;

            // CRITICAL: Otherwise the grid exhibits undesirable behavior.
            // eg. Various actions result in calls to CustomDbGridCtrl.OnCellBeginEdit()
            // which, in turn, cannot be canceled .. inhibiting further app interaction.
            grid.EditMode = DataGridViewEditMode.EditProgrammatically;

            // Make sure that when the GLControl is resized or needs to be painted,
            // we update our projection matrix or re-render its contents, respectively.
            glControl.Resize += glControl_Resize;
            glControl.Paint += glControl_Paint;

            // Ensure that the viewport and projection matrix are set correctly initially.
            glControl_Resize(glControl, EventArgs.Empty);

            // Hookup the key and mouse event handlers.
            GLControlEventHandlersBind();

            GLSetup();

            run.Enabled = false;
            lbInfo.Text = string.Empty;

            //=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=

            // Among other things, this next statement gets the
            // database folder from the Windows registry
            workspaceSettings = ProjectSettings.DefaultSettings();

            // Create/Connect to the database as necessary.
            fractalDb = new FractalDB();
            fractalDb.DbInit(Path.Combine(workspaceSettings.databaseFolder, "fractal.db"));

            // Get the settings as a json string.
            string json = (fractalDb.IsNewDatabase
                ? workspaceSettings.Serialize()
                : fractalDb.SerializedSettings(fractalDb.WorkspaceID));

            if (fractalDb.IsNewDatabase)
            {
                // Establish the initial settings in the database.
                fractalDb.SettingsSave(fractalDb.WorkspaceID, json);
                fractalDb.IsDirty = false;  // Prevent File/SaveAs from being enabled.
            }

            // Update the workspace settings using the json string.
            workspaceSettings.Deserialize(json);

            // Initialize various UI controls.
            ControlPanelInitialize(workspaceSettings);
            MethodControlsUpdate(workspaceSettings.method);

            theDomain = DomainFromInputs();

            // CRITICAL: theCamera must be established before calling FormResize()
            // Why? Because FormResize() indirectly calls ImageClear(). In turn,
            // ImageClear() requires theCamera because the shader is used when
            // rendering the glControl.
            theCamera = BaseCameraCopy();

            GLModelUpdate(workspaceSettings.orientation);
            FormResize(workspaceSettings.orientation, workspaceSettings.reduced);

            ccwMatrix = new Matrix3(new Vector3(0, 1, 0), new Vector3(-1, 0, 0), Vector3.UnitZ);
            cwMatrix = new Matrix3(new Vector3(0, -1, 0), new Vector3(1, 0, 0), Vector3.UnitZ);

            //=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=

            interpolate.Enabled = false;
            divs.Enabled = false;

            smooth.Enabled = false;
            steps.Enabled = false;

            stop.Enabled = false;
            steps.Text = "2";

            clear.Enabled = false;
            update.Enabled = false;

            GridInitialize();

            appTitle = this.Text;
            NewProject();

            WaitCursorStop();
        }

        private void FractalFactory_FormClosing(object sender, FormClosingEventArgs e)
        {
            DialogResult status = DirtyFileSave(null!, 0, "Save changes before exiting?");
            e.Cancel = (status == DialogResult.Cancel);
            if (e.Cancel)
                return;

            DateTime start = DateTime.Now;

            fractalDb.WorkspaceClear();

            Debug.WriteLine($"FormClosed start: {DateTime.Now - start}");

            ProgressBarTask pbt = new ProgressBarTask("Compacting", CompactingTime(workspaceSettings.databaseFolder));
            pbt.task = Task.Run(() =>
            {
                fractalDb.Compact();
                fractalDb.Close();
            });
            TaskProgressShow(pbt);

            Debug.WriteLine($"FormClosed: {DateTime.Now - start}");

            Thread.Sleep(100);
        }

        private void AppTitleUpdate()
        {
            if (fractalDb.IsDirty)
                this.Text = $"{appTitle} - ({projectName}*)";
            else
                this.Text = $"{appTitle} - ({projectName})";
        }

        private void GLSetup()
        {
            vertexBufferObject = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, vertexBufferObject);
            GL.BufferData(BufferTarget.ArrayBuffer, vertices.Length * sizeof(float), vertices, BufferUsageHint.StaticDraw);

            vertexArrayObject = GL.GenVertexArray();
            GL.BindVertexArray(vertexArrayObject);

            elementBufferObject = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, elementBufferObject);
            GL.BufferData(BufferTarget.ElementArrayBuffer, indices.Length * sizeof(uint), indices, BufferUsageHint.StaticDraw);

            // Alternatively, shader code can be loaded from a file.
            shader = Shader.FromCode(vertexShaderCode, fragmentShaderCode);

            // Because there's now 5 floats between the start of the first vertex and the start of the second,
            // we modify the stride from 3 * sizeof(float) to 5 * sizeof(float).
            // This will now pass the new vertex array to the buffer.
            var vertexLocation = shader.GetAttribLocation("aPosition");
            GL.EnableVertexAttribArray(vertexLocation);
            GL.VertexAttribPointer(vertexLocation, 3, VertexAttribPointerType.Float, false, 5 * sizeof(float), 0);

            // Next, we also setup texture coordinates. It works in much the same way.
            // We add an offset of 3, since the texture coordinates comes after the position data.
            // We also change the amount of data to 2 because there's only 2 floats for texture coordinates.
            var texCoordLocation = shader.GetAttribLocation("aTexCoord");
            GL.EnableVertexAttribArray(texCoordLocation);
            GL.VertexAttribPointer(texCoordLocation, 2, VertexAttribPointerType.Float, false, 5 * sizeof(float), 3 * sizeof(float));

            //=-=-=-=

            lineStripBufferObject = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, lineStripBufferObject);
            GL.BufferData(BufferTarget.ArrayBuffer, rect.ords.Length * sizeof(float), rect.ords, BufferUsageHint.StaticDraw);

            lineStripArrayObject = GL.GenVertexArray();
            GL.BindVertexArray(lineStripArrayObject);

            rectShader = Shader.FromCode(vLineStripShaderCode, fLineStripShaderCode);

            var cornerLocation = rectShader.GetAttribLocation("aCorner");
            GL.EnableVertexAttribArray(cornerLocation);
            GL.VertexAttribPointer(cornerLocation, 2, VertexAttribPointerType.Float, false, 2 * sizeof(float), 0);
        }

        private void glControl_Resize(object? sender, EventArgs e)
        {
            glControl.MakeCurrent();

            if (glControl.ClientSize.Height == 0)
                glControl.ClientSize = new System.Drawing.Size(glControl.ClientSize.Width, 1);

            GL.Viewport(0, 0, glControl.ClientSize.Width, glControl.ClientSize.Height);
        }

        private void glControl_Paint(object? sender, PaintEventArgs e)
        {
            Render();
        }

        private void FormResize(int orientation, bool useReducedSize)
        {
            // 1.5 is the ratio required to support a (12 x 18) photo
            int STANDARD_HEIGHT = 850;
            int STANDARD_WIDTH = (int)(1.5 * (double)STANDARD_HEIGHT);
            int PICTURE_PANEL_SHIFT = 4;

            controlPanel.Height = ((orientation == 0) ? STANDARD_HEIGHT : STANDARD_WIDTH);

            picturePanel.Location = new Point(controlPanel.Width + PICTURE_PANEL_SHIFT, controlPanel.Top);
            picturePanel.Height = controlPanel.Height;
            picturePanel.Width = ((orientation == 0) ? STANDARD_WIDTH : STANDARD_HEIGHT);

            PictureSizeAdjust(useReducedSize);

            this.ClientSize = new Size((controlPanel.Width + picturePanel.Width + PICTURE_PANEL_SHIFT), controlPanel.Height);
            this.Invalidate();
        }

        private void DomainFromView(int dx, int dy)
        {
            double vdx, vdy;

            if (workspaceSettings.orientation != 0)
            {
                vdx = (dx / (double)glControl.Width) * (double)theDomain.Dy();
                vdy = (dy / (double)glControl.Height) * (double)theDomain.Dx();
            }
            else
            {
                vdx = (dx / (double)glControl.Width) * (double)theDomain.Dx();
                vdy = (dy / (double)glControl.Height) * (double)theDomain.Dy();
            }

            Vector3 vec = modelInverse * new Vector3((float)vdx, (float)vdy, 0f);

            theDomain.Xmin -= vec.X;
            theDomain.Ymin += vec.Y;
            theDomain.Xmax -= vec.X;
            theDomain.Ymax += vec.Y;
        }

        private void PreviewKeyEvent(Keys keyCode)
        {
            KeyPreviewExecute(keyCode);
        }

        // CRITICAL: The KeyPreview property must be set 'true' for this to work.
        private void FractalFactory_KeyPress(object sender, KeyPressEventArgs e)
        {
            KeyPressProcess(sender, e);
        }

        private void record_Click(object sender, EventArgs e)
        {
            RecordClicked();
        }

        private void generate_Click(object sender, EventArgs e)
        {
            GenerateClicked();
        }

        private void run_Click(object sender, EventArgs e)
        {
            RunClicked();
        }

        // ASSUMPTION: The Update button is enabled only after successfully Generate'ing an image.
        // In this scenario, the statement may (or may not) have changed; it's not relevant.
        private void update_Click(object sender, EventArgs e)
        {
            UpdateClicked();
        }

        private void newProjectMenuItem_Click(object sender, EventArgs e)
        {
            WorkspaceNew();
        }

        private void openProjectMenuItem_Click(object sender, EventArgs e)
        {
            ProjectOpen();
        }

        private void saveProjectMenuItem_Click(object sender, EventArgs e)
        {
            ProjectSave();
        }

        private void saveAsProjectMenuItem_Click(object sender, EventArgs e)
        {
            ProjectSaveAs();
        }

        private void saveTextToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ProjectTextSave();
        }

        private void renameProjectMenuItem_Click(object sender, EventArgs e)
        {
            ProjectRename();
        }

        private void deleteProjectMenuItem_Click(object sender, EventArgs e)
        {
            ProjectDelete();
        }

        private void optionsProjectMenuItem_Click(object sender, EventArgs e)
        {
            ProjectOptions();
        }

        private void movieProjectMenuItem_Click(object sender, EventArgs e)
        {
            MovieSave();
        }

        private void gridPopMenu_Opening(object sender, CancelEventArgs e)
        {
            if (grid.RowCount < 1)
            {
                e.Cancel = true;
            }
            else
            {
                Point pt = grid.PointToClient(Cursor.Position);
                e.Cancel = !grid.ClientRectangle.Contains(pt);
            }
        }

        private void setParametersToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SetParameters(true);
        }

        private void setPolynomialsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SetParameters(false);
        }

        private void saveTextToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            ProjectTextSave();
        }

        private void panToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PreviewKeyEvent(Keys.C);
        }

        private void windowToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PreviewKeyEvent(Keys.W);
        }

        private void zoomToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PreviewKeyEvent(Keys.Z);
        }

        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ImageSave();
        }

        private void photoGenerateToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PhotoGenerate();
        }

        private void interpolate_Click(object sender, EventArgs e)
        {
            InterpolateExecute(sender, e);
        }

        private void smooth_Click(object sender, EventArgs e)
        {
            SmoothExecute();
        }

        private void extend_Click(object sender, EventArgs e)
        {
            ExtendExecute();
        }

        private void clear_Click(object sender, EventArgs e)
        {
            SelectedImagesClear("Clear the selected images?");
        }

        private void stop_Click(object sender, EventArgs e)
        {
            StopClicked();
        }

        private void singleFrame_CheckedChanged(object sender, EventArgs e)
        {
            stop.Enabled = false;

            if (singleFrame.Checked)
            {
                generate.Enabled = true;
                run.Enabled = false;
            }
            else
            {
                run.Enabled = true;
                generate.Enabled = false;
            }
        }

        private void Copy_Click(object sender, EventArgs e)
        {
            string statement = statementFormatter.DomainStatementCreate(RawDomain());
            Clipboard.SetText(statement);
        }

        private void precision_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !KeyValidator.IsIntChar(e.KeyChar);
            toolTip.Hide(precision);
        }

        private void divs_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !KeyValidator.IsIntChar(e.KeyChar);
        }

        private void steps_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !KeyValidator.IsIntChar(e.KeyChar);
        }

        // BEWARE: Enforcing proper floating point sytax is left as an exercise to someone else.
        private void xmin_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !KeyValidator.IsFloatingPointInput(e.KeyChar);
        }

        // BEWARE: Enforcing proper floating point sytax is left as an exercise to someone else.
        private void ymin_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !KeyValidator.IsFloatingPointInput(e.KeyChar);
        }

        // BEWARE: Enforcing proper floating point sytax is left as an exercise to someone else.
        private void xmax_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !KeyValidator.IsFloatingPointInput(e.KeyChar);
        }

        // BEWARE: Enforcing proper floating point sytax is left as an exercise to someone else.
        private void ymax_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !KeyValidator.IsFloatingPointInput(e.KeyChar);
        }

        private void numerPoly_Leave(object sender, EventArgs e)
        {
            if (workspaceSettings.method == OptionsDialog.NEWTON1)
            {
                PolyTerms terms = Poly.TermsGet(numerPoly.Text, PolyFunction.DERIVATIVE);
                denomPoly.Text = statementFormatter.FunctionStatementCreate(terms);  // TODO: Use PolyFormatter instead?
            }
        }

        private void precision_Validating(object sender, CancelEventArgs e)
        {
            int[] limits = { 5, 16 };
            int prec = PrecisionAsInt();

            // Perhaps a bit cheesy but limiting the bounds here
            // allows us to avoid checking myriad checks elsewhere.
            if ((prec < limits[0]) || (prec > limits[1]))
            {
                e.Cancel = true;
                toolTip.Show($"The precision must be in the range of [{limits[0]}..{limits[1]}] digits.", precision);
            }
            else
            {
                statementFormatter = new StatementFormatter(prec);
                statementProcessor = new StatementProcessor(prec);
                gridStatementProcessor = new StatementProcessor(prec);
            }
        }

        private void divs_Validating(object sender, CancelEventArgs e)
        {
            int divisions = 0;
            Int32.TryParse(divs.Text, out divisions);
            e.Cancel = (divisions < 1);
        }

        private void steps_Validating(object sender, CancelEventArgs e)
        {
            int divisions = 0;
            Int32.TryParse(steps.Text, out divisions);
            e.Cancel = (divisions < 1);
        }

        private void WaitCursorStart(bool useAppWaitCursor = true)
        {
            if (useAppWaitCursor)
                Application.UseWaitCursor = true;
            else
                Cursor.Current = Cursors.WaitCursor;
        }

        private void WaitCursorStop(bool usingAppWaitCursor = true)
        {
            if (usingAppWaitCursor)
            {
                Application.UseWaitCursor = false;
                Cursor.Position += new Size(1, 1);  // <--- necessary to dismiss the WaitCursor
            }
            else
            {
                Cursor.Current = Cursors.Default;
            }
        }

        private void Render()
        {
            glControl.MakeCurrent();

            GL.Clear(ClearBufferMask.ColorBufferBit);
            GL.BindVertexArray(vertexArrayObject);

            texture.Use(TextureUnit.Texture0);
            shader.Use();

            shader.SetMatrix4("model", model);
            shader.SetMatrix4("view", theCamera.GetViewMatrix());
            shader.SetMatrix4("projection", theCamera.GetProjectionMatrix());

            GL.DrawElements(PrimitiveType.Triangles, indices.Length, DrawElementsType.UnsignedInt, 0);

            if (IsCameraMode(CameraMode.Windowing) && rect.AllowDraw)
            {
                // Draw the windowing rectangle on top of the bitmap.
                GL.BindVertexArray(lineStripArrayObject);

                rectShader.Use();
                GL.LineWidth(3);
                GL.DrawArrays(PrimitiveType.LineLoop, 0, 4);
            }

            glControl.SwapBuffers();
        }

        private void TimesUpdate(TimeSpan executionTime)
        {
            Invoke((Action)(() =>
            {
                if (executionTime == TimeSpan.MaxValue)
                {
                    frameTime = TimeSpan.Zero;
                    totalTime = TimeSpan.Zero;
                }
                else
                {
                    frameTime = executionTime;
                    totalTime += executionTime;
                }

                time.Text = string.Format("time: {0}", frameTime.ToString());
                total.Text = string.Format("total: {0}", totalTime.ToString());

                time.Update();
                total.Update();
            }));
        }

        private int PrecisionAsInt()
        {
            int prec = 0;
            Int32.TryParse(precision.Text, out prec);
            return prec;
        }

        private float PrecisionAsFloat()
        {
            string fmt = "0." + new string('0', PrecisionAsInt() - 1) + "1";
            return Single.Parse(fmt);
        }

        // Prior to introducing grid_SelectionChanged (and this problem frequently
        // occurred), if you wern't paying attention when you clicked the Generate
        // button, the generated fractal didn't correspond to the selected statement
        // (because the values in the input controls didn't correspond to the selected
        // statement). It was then possible to save the (wrong) generated image with
        // the selected statement. So now, after selecting a new row, you must use
        // the Set Polynomials or Set All Parameters option of the grid pop-menu to
        // enable the Generate button.
        private void grid_SelectionChanged(object sender, EventArgs e)
        {
            update.Enabled = false;

            lbInfo.Text = string.Format("Selected: {0} lines", grid.SelectedRows.Count);

            if (!ConditionalInterpolationEnable())
            {
                ViewBase();
                BitmapShowByRowNumber(grid.ActiveRow);
            }

            generate.Enabled = false;
        }

        private void grid_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                this.ContextMenuStrip = gridPopMenu;
                this.ContextMenuStrip.Show(Cursor.Position);
            }
        }

        private void grid_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            int rowNumber = grid.ActiveRow;

            if (grid.EditingCanceled)
            {
                // emptyStatement will be true if you manually insert a blank statement using e.g. (F1, F2)
                if (activeStatement == string.Empty)
                {
                    // delete what would appear as an empty statement.
                    grid.StatementDelete(rowNumber);
                }
                else
                {
                    // Otherwise, restore the existing statement.
                    StatementUpdate(rowNumber, activeStatement);
                }

                return;
            }

            string statement = grid.StatementAt(rowNumber);
            if (statement == activeStatement)
                return;  // The statement has not changed.

            StatementUpdate(rowNumber, statement);
        }

        private void grid_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.Modifiers == Keys.Control)
            {
                if (e.KeyCode == Keys.A)
                {
                    grid.SelectAll();
                    lbInfo.Text = string.Format("Selected: {0} lines", grid.SelectedRows.Count);
                    ConditionalInterpolationEnable();
                }
                else if (e.KeyCode == Keys.C)
                {
                    // Copy the selected rows into the system Clipboard.
                    ClipboardCopy();
                }
                else if (e.KeyCode == Keys.X)
                {
                    ClipboardCopy();
                    SelectedRowsDelete(true);
                }
                else if (e.KeyCode == Keys.V)
                {
                    ClipboardPaste();
                }
            }
            else if (e.KeyCode == Keys.Up)
            {
                // Row navigation.
                int rowNumber = grid.ActiveRow - 1;
                if (rowNumber < 0)
                    return;

                BitmapShowByRowNumber(rowNumber);
                clear.Enabled = HaveSelectedImages();
            }
            else if (e.KeyCode == Keys.Down)
            {
                // Row navigation.
                int rowNumber = grid.ActiveRow + 1;
                if (rowNumber >= grid.RowCount)
                    return;

                BitmapShowByRowNumber(rowNumber);
                clear.Enabled = HaveSelectedImages();
            }
            else if (e.KeyCode == Keys.F1)
            {
                // Insert a blank statement above the currently select row.
                activeStatement = string.Empty;
                grid.StatementInsert(false, string.Empty);
                grid.BeginEdit(false);
            }
            else if (e.KeyCode == Keys.F2)
            {
                // Insert a blank statement below the currently select row.
                activeStatement = string.Empty;
                grid.StatementInsert(true, string.Empty);
                grid.BeginEdit(false);
            }
            else if (e.KeyCode == Keys.Delete)
            {
                SelectedRowsDelete(false);
            }
        }

        private void grid_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            activeStatement = grid.StatementAt(grid.ActiveRow);
            grid.BeginEdit(false);
        }

        private void grid_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            if (e.FormattedValue == null)  // Shouldn't happen, so shush the compiler
            {
                e.Cancel = true;
                return;
            }

            // TODO: Ideally, Parse() should also validate the syntactical correctness of the polynomials.
            if (gridStatementProcessor.Parse((string)e.FormattedValue) == ParseStatus.PARSE_FAILED)
                e.Cancel = true;

            // TODO: Ideally, prettify the statement (FormattedValue?) before allowing it to be displayed.
        }

        private void Log(string message)
        {
            Debug.WriteLine(message);
        }
    }
}
