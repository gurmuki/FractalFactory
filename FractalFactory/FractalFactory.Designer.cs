
//=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=
// NOTE: Compilation issues will arise after modifying the dialog using the Designer.
//   Whatever the reason, all references to CustomControls.CComboBox will be changed
//   to FractalFactory.CustomControls.CComboBox, causing an error. I have not been
//   able to find any reference/solution to such behavior on the web. As such, the
//   current solution is simply to edit the offending code.
//=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=

using OpenTK.GLControl;

namespace FractalFactory
{
    partial class FractalFactory
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            controlPanel = new System.Windows.Forms.Panel();
            progressPanel = new System.Windows.Forms.Panel();
            progressBar = new System.Windows.Forms.ProgressBar();
            progressLabel = new System.Windows.Forms.Label();
            total = new System.Windows.Forms.Label();
            lbInfo = new System.Windows.Forms.Label();
            recordingGroupBox = new System.Windows.Forms.GroupBox();
            label1 = new System.Windows.Forms.Label();
            steps = new System.Windows.Forms.TextBox();
            divs = new System.Windows.Forms.TextBox();
            precision = new System.Windows.Forms.TextBox();
            update = new System.Windows.Forms.Button();
            clear = new System.Windows.Forms.Button();
            extend = new System.Windows.Forms.Button();
            smooth = new System.Windows.Forms.Button();
            interpolate = new System.Windows.Forms.Button();
            record = new System.Windows.Forms.Button();
            domainSettings = new System.Windows.Forms.GroupBox();
            label7 = new System.Windows.Forms.Label();
            label6 = new System.Windows.Forms.Label();
            label5 = new System.Windows.Forms.Label();
            label4 = new System.Windows.Forms.Label();
            Copy = new System.Windows.Forms.Button();
            ymax = new System.Windows.Forms.TextBox();
            ymin = new System.Windows.Forms.TextBox();
            xmax = new System.Windows.Forms.TextBox();
            xmin = new System.Windows.Forms.TextBox();
            time = new System.Windows.Forms.Label();
            polysGroupBox = new System.Windows.Forms.GroupBox();
            denomLabel = new System.Windows.Forms.Label();
            numerLabel = new System.Windows.Forms.Label();
            denomPoly = new System.Windows.Forms.TextBox();
            numerPoly = new System.Windows.Forms.TextBox();
            exeGroupBox = new System.Windows.Forms.GroupBox();
            stop = new System.Windows.Forms.Button();
            run = new System.Windows.Forms.Button();
            generate = new System.Windows.Forms.Button();
            panel1 = new System.Windows.Forms.Panel();
            multiFrame = new System.Windows.Forms.RadioButton();
            singleFrame = new System.Windows.Forms.RadioButton();
            projectMenu = new System.Windows.Forms.MenuStrip();
            projectToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            newProjectMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            openProjectMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            saveProjectMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            saveAsProjectMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            saveTextToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            renameProjectMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            deleteProjectMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            optionsProjectMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            movieProjectMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            grid = new CustomControls.CustomDbGridCtrl();
            reducedViewCB = new System.Windows.Forms.CheckBox();
            picturePanel = new System.Windows.Forms.Panel();
            glControl = new GLControl();
            viewPopMenu = new System.Windows.Forms.ContextMenuStrip(components);
            panToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            windowToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            zoomToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            saveAsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            photoGenerateToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            gridPopMenu = new System.Windows.Forms.ContextMenuStrip(components);
            setPolynomialsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            setParametersToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            saveTextToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            toolTip = new System.Windows.Forms.ToolTip(components);
            controlPanel.SuspendLayout();
            progressPanel.SuspendLayout();
            recordingGroupBox.SuspendLayout();
            domainSettings.SuspendLayout();
            polysGroupBox.SuspendLayout();
            exeGroupBox.SuspendLayout();
            panel1.SuspendLayout();
            projectMenu.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)grid).BeginInit();
            picturePanel.SuspendLayout();
            viewPopMenu.SuspendLayout();
            gridPopMenu.SuspendLayout();
            SuspendLayout();
            // 
            // controlPanel
            // 
            controlPanel.BackColor = System.Drawing.SystemColors.Control;
            controlPanel.Controls.Add(progressPanel);
            controlPanel.Controls.Add(total);
            controlPanel.Controls.Add(lbInfo);
            controlPanel.Controls.Add(recordingGroupBox);
            controlPanel.Controls.Add(domainSettings);
            controlPanel.Controls.Add(time);
            controlPanel.Controls.Add(polysGroupBox);
            controlPanel.Controls.Add(exeGroupBox);
            controlPanel.Controls.Add(projectMenu);
            controlPanel.Controls.Add(grid);
            controlPanel.Location = new System.Drawing.Point(0, 0);
            controlPanel.Name = "controlPanel";
            controlPanel.Size = new System.Drawing.Size(470, 850);
            controlPanel.TabIndex = 0;
            // 
            // progressPanel
            // 
            progressPanel.Controls.Add(progressBar);
            progressPanel.Controls.Add(progressLabel);
            progressPanel.Location = new System.Drawing.Point(14, 410);
            progressPanel.Name = "progressPanel";
            progressPanel.Size = new System.Drawing.Size(443, 30);
            progressPanel.TabIndex = 41;
            // 
            // progressBar
            // 
            progressBar.Location = new System.Drawing.Point(77, 4);
            progressBar.Name = "progressBar";
            progressBar.Size = new System.Drawing.Size(332, 23);
            progressBar.TabIndex = 1;
            // 
            // progressLabel
            // 
            progressLabel.AutoSize = true;
            progressLabel.Location = new System.Drawing.Point(4, 8);
            progressLabel.Name = "progressLabel";
            progressLabel.Size = new System.Drawing.Size(52, 15);
            progressLabel.TabIndex = 0;
            progressLabel.Text = "Progress";
            // 
            // total
            // 
            total.AutoSize = true;
            total.Location = new System.Drawing.Point(270, 355);
            total.Name = "total";
            total.Size = new System.Drawing.Size(34, 15);
            total.TabIndex = 39;
            total.Text = "total:";
            // 
            // lbInfo
            // 
            lbInfo.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            lbInfo.AutoSize = true;
            lbInfo.Font = new System.Drawing.Font("Segoe UI", 8.25F);
            lbInfo.Location = new System.Drawing.Point(23, 828);
            lbInfo.Name = "lbInfo";
            lbInfo.Size = new System.Drawing.Size(97, 13);
            lbInfo.TabIndex = 6;
            lbInfo.Text = "x:{0} y:{1} t:{2} p:{3}";
            // 
            // recordingGroupBox
            // 
            recordingGroupBox.Controls.Add(label1);
            recordingGroupBox.Controls.Add(steps);
            recordingGroupBox.Controls.Add(divs);
            recordingGroupBox.Controls.Add(precision);
            recordingGroupBox.Controls.Add(update);
            recordingGroupBox.Controls.Add(clear);
            recordingGroupBox.Controls.Add(extend);
            recordingGroupBox.Controls.Add(smooth);
            recordingGroupBox.Controls.Add(interpolate);
            recordingGroupBox.Controls.Add(record);
            recordingGroupBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            recordingGroupBox.Location = new System.Drawing.Point(12, 261);
            recordingGroupBox.Name = "recordingGroupBox";
            recordingGroupBox.Size = new System.Drawing.Size(443, 80);
            recordingGroupBox.TabIndex = 5;
            recordingGroupBox.TabStop = false;
            recordingGroupBox.Text = "Recording";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new System.Drawing.Point(6, 51);
            label1.Name = "label1";
            label1.Size = new System.Drawing.Size(53, 13);
            label1.TabIndex = 9;
            label1.Text = "Precision:";
            // 
            // steps
            // 
            steps.Location = new System.Drawing.Point(202, 47);
            steps.Name = "steps";
            steps.Size = new System.Drawing.Size(36, 20);
            steps.TabIndex = 8;
            steps.KeyPress += steps_KeyPress;
            steps.Validating += steps_Validating;
            // 
            // divs
            // 
            divs.Location = new System.Drawing.Point(202, 21);
            divs.Name = "divs";
            divs.Size = new System.Drawing.Size(36, 20);
            divs.TabIndex = 7;
            divs.KeyPress += divs_KeyPress;
            divs.Validating += divs_Validating;
            // 
            // precision
            // 
            precision.Location = new System.Drawing.Point(61, 48);
            precision.Name = "precision";
            precision.Size = new System.Drawing.Size(36, 20);
            precision.TabIndex = 6;
            precision.KeyPress += precision_KeyPress;
            precision.Validating += precision_Validating;
            // 
            // update
            // 
            update.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            update.Location = new System.Drawing.Point(353, 46);
            update.Name = "update";
            update.Size = new System.Drawing.Size(75, 23);
            update.TabIndex = 5;
            update.Text = "Update";
            update.UseVisualStyleBackColor = true;
            update.Click += update_Click;
            // 
            // clear
            // 
            clear.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            clear.Location = new System.Drawing.Point(353, 19);
            clear.Name = "clear";
            clear.Size = new System.Drawing.Size(75, 23);
            clear.TabIndex = 4;
            clear.Text = "Clear";
            clear.UseVisualStyleBackColor = true;
            clear.Click += clear_Click;
            // 
            // extend
            // 
            extend.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            extend.Location = new System.Drawing.Point(258, 19);
            extend.Name = "extend";
            extend.Size = new System.Drawing.Size(75, 23);
            extend.TabIndex = 3;
            extend.Text = "Extend";
            extend.UseVisualStyleBackColor = true;
            extend.Click += extend_Click;
            // 
            // smooth
            // 
            smooth.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            smooth.Location = new System.Drawing.Point(120, 46);
            smooth.Name = "smooth";
            smooth.Size = new System.Drawing.Size(75, 23);
            smooth.TabIndex = 2;
            smooth.Text = "Smooth";
            smooth.UseVisualStyleBackColor = true;
            smooth.Click += smooth_Click;
            // 
            // interpolate
            // 
            interpolate.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            interpolate.Location = new System.Drawing.Point(120, 19);
            interpolate.Name = "interpolate";
            interpolate.Size = new System.Drawing.Size(75, 23);
            interpolate.TabIndex = 1;
            interpolate.Text = "Interpolate";
            interpolate.UseVisualStyleBackColor = true;
            interpolate.Click += interpolate_Click;
            // 
            // record
            // 
            record.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            record.Location = new System.Drawing.Point(23, 19);
            record.Name = "record";
            record.Size = new System.Drawing.Size(75, 23);
            record.TabIndex = 0;
            record.Text = "Record";
            record.UseVisualStyleBackColor = true;
            record.Click += record_Click;
            // 
            // domainSettings
            // 
            domainSettings.Controls.Add(label7);
            domainSettings.Controls.Add(label6);
            domainSettings.Controls.Add(label5);
            domainSettings.Controls.Add(label4);
            domainSettings.Controls.Add(Copy);
            domainSettings.Controls.Add(ymax);
            domainSettings.Controls.Add(ymin);
            domainSettings.Controls.Add(xmax);
            domainSettings.Controls.Add(xmin);
            domainSettings.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            domainSettings.Location = new System.Drawing.Point(220, 144);
            domainSettings.Name = "domainSettings";
            domainSettings.Size = new System.Drawing.Size(235, 108);
            domainSettings.TabIndex = 4;
            domainSettings.TabStop = false;
            domainSettings.Text = "Domain";
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.Location = new System.Drawing.Point(117, 49);
            label7.Name = "label7";
            label7.Size = new System.Drawing.Size(34, 13);
            label7.TabIndex = 13;
            label7.Text = "ymax:";
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Location = new System.Drawing.Point(120, 23);
            label6.Name = "label6";
            label6.Size = new System.Drawing.Size(31, 13);
            label6.TabIndex = 12;
            label6.Text = "ymin:";
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new System.Drawing.Point(5, 49);
            label5.Name = "label5";
            label5.Size = new System.Drawing.Size(34, 13);
            label5.TabIndex = 11;
            label5.Text = "xmax:";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new System.Drawing.Point(8, 23);
            label4.Name = "label4";
            label4.Size = new System.Drawing.Size(31, 13);
            label4.TabIndex = 10;
            label4.Text = "xmin:";
            // 
            // Copy
            // 
            Copy.Location = new System.Drawing.Point(89, 75);
            Copy.Name = "Copy";
            Copy.Size = new System.Drawing.Size(75, 23);
            Copy.TabIndex = 6;
            Copy.Text = "Copy";
            Copy.UseVisualStyleBackColor = true;
            Copy.Click += Copy_Click;
            // 
            // ymax
            // 
            ymax.Location = new System.Drawing.Point(154, 46);
            ymax.Name = "ymax";
            ymax.Size = new System.Drawing.Size(74, 20);
            ymax.TabIndex = 5;
            ymax.TextChanged += ymax_TextChanged;
            ymax.KeyPress += ymax_KeyPress;
            // 
            // ymin
            // 
            ymin.Location = new System.Drawing.Point(154, 18);
            ymin.Name = "ymin";
            ymin.Size = new System.Drawing.Size(74, 20);
            ymin.TabIndex = 4;
            ymin.TextChanged += ymin_TextChanged;
            ymin.KeyPress += ymin_KeyPress;
            // 
            // xmax
            // 
            xmax.Location = new System.Drawing.Point(40, 47);
            xmax.Name = "xmax";
            xmax.Size = new System.Drawing.Size(74, 20);
            xmax.TabIndex = 3;
            xmax.TextChanged += xmax_TextChanged;
            xmax.KeyPress += xmax_KeyPress;
            // 
            // xmin
            // 
            xmin.Location = new System.Drawing.Point(40, 20);
            xmin.Name = "xmin";
            xmin.Size = new System.Drawing.Size(74, 20);
            xmin.TabIndex = 2;
            xmin.TextChanged += xmin_TextChanged;
            xmin.KeyPress += xmin_KeyPress;
            // 
            // time
            // 
            time.AutoSize = true;
            time.Location = new System.Drawing.Point(23, 355);
            time.Name = "time";
            time.Size = new System.Drawing.Size(34, 15);
            time.TabIndex = 9;
            time.Text = "time:";
            // 
            // polysGroupBox
            // 
            polysGroupBox.Controls.Add(denomLabel);
            polysGroupBox.Controls.Add(numerLabel);
            polysGroupBox.Controls.Add(denomPoly);
            polysGroupBox.Controls.Add(numerPoly);
            polysGroupBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            polysGroupBox.Location = new System.Drawing.Point(12, 144);
            polysGroupBox.Name = "polysGroupBox";
            polysGroupBox.Size = new System.Drawing.Size(201, 107);
            polysGroupBox.TabIndex = 3;
            polysGroupBox.TabStop = false;
            polysGroupBox.Text = "Polynomials";
            // 
            // denomLabel
            // 
            denomLabel.AutoSize = true;
            denomLabel.Location = new System.Drawing.Point(9, 49);
            denomLabel.Name = "denomLabel";
            denomLabel.Size = new System.Drawing.Size(39, 13);
            denomLabel.TabIndex = 11;
            denomLabel.Text = "denom";
            // 
            // numerLabel
            // 
            numerLabel.AutoSize = true;
            numerLabel.Location = new System.Drawing.Point(11, 23);
            numerLabel.Name = "numerLabel";
            numerLabel.Size = new System.Drawing.Size(36, 13);
            numerLabel.TabIndex = 10;
            numerLabel.Text = "numer";
            // 
            // denomPoly
            // 
            denomPoly.Location = new System.Drawing.Point(49, 46);
            denomPoly.Name = "denomPoly";
            denomPoly.Size = new System.Drawing.Size(143, 20);
            denomPoly.TabIndex = 1;
            denomPoly.TextChanged += denomPoly_TextChanged;
            // 
            // numerPoly
            // 
            numerPoly.Location = new System.Drawing.Point(49, 20);
            numerPoly.Name = "numerPoly";
            numerPoly.Size = new System.Drawing.Size(143, 20);
            numerPoly.TabIndex = 0;
            numerPoly.TextChanged += numerPoly_TextChanged;
            numerPoly.Leave += numerPoly_Leave;
            // 
            // exeGroupBox
            // 
            exeGroupBox.Controls.Add(stop);
            exeGroupBox.Controls.Add(run);
            exeGroupBox.Controls.Add(generate);
            exeGroupBox.Controls.Add(panel1);
            exeGroupBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            exeGroupBox.Location = new System.Drawing.Point(12, 44);
            exeGroupBox.Name = "exeGroupBox";
            exeGroupBox.Size = new System.Drawing.Size(282, 90);
            exeGroupBox.TabIndex = 2;
            exeGroupBox.TabStop = false;
            exeGroupBox.Text = "Execution";
            // 
            // stop
            // 
            stop.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            stop.Location = new System.Drawing.Point(204, 45);
            stop.Name = "stop";
            stop.Size = new System.Drawing.Size(59, 23);
            stop.TabIndex = 3;
            stop.Text = "Stop";
            stop.UseVisualStyleBackColor = true;
            stop.Click += stop_Click;
            // 
            // run
            // 
            run.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            run.Location = new System.Drawing.Point(140, 45);
            run.Name = "run";
            run.Size = new System.Drawing.Size(59, 23);
            run.TabIndex = 2;
            run.Text = "Run";
            run.UseVisualStyleBackColor = true;
            run.Click += run_Click;
            // 
            // generate
            // 
            generate.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            generate.Location = new System.Drawing.Point(140, 17);
            generate.Name = "generate";
            generate.Size = new System.Drawing.Size(123, 23);
            generate.TabIndex = 1;
            generate.Text = "Generate";
            generate.UseVisualStyleBackColor = true;
            generate.Click += generate_Click;
            // 
            // panel1
            // 
            panel1.Controls.Add(multiFrame);
            panel1.Controls.Add(singleFrame);
            panel1.Location = new System.Drawing.Point(16, 15);
            panel1.Name = "panel1";
            panel1.Size = new System.Drawing.Size(118, 54);
            panel1.TabIndex = 0;
            // 
            // multiFrame
            // 
            multiFrame.AutoSize = true;
            multiFrame.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            multiFrame.Location = new System.Drawing.Point(12, 30);
            multiFrame.Name = "multiFrame";
            multiFrame.Size = new System.Drawing.Size(98, 17);
            multiFrame.TabIndex = 1;
            multiFrame.TabStop = true;
            multiFrame.Text = "Multiple Frames";
            multiFrame.UseVisualStyleBackColor = true;
            // 
            // singleFrame
            // 
            singleFrame.AutoSize = true;
            singleFrame.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            singleFrame.Location = new System.Drawing.Point(12, 7);
            singleFrame.Name = "singleFrame";
            singleFrame.Size = new System.Drawing.Size(86, 17);
            singleFrame.TabIndex = 0;
            singleFrame.TabStop = true;
            singleFrame.Text = "Single Frame";
            singleFrame.UseVisualStyleBackColor = true;
            singleFrame.CheckedChanged += singleFrame_CheckedChanged;
            // 
            // projectMenu
            // 
            projectMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { projectToolStripMenuItem });
            projectMenu.Location = new System.Drawing.Point(0, 0);
            projectMenu.Name = "projectMenu";
            projectMenu.Size = new System.Drawing.Size(470, 25);
            projectMenu.TabIndex = 1;
            projectMenu.Text = "menuStrip1";
            // 
            // projectToolStripMenuItem
            // 
            projectToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] { newProjectMenuItem, openProjectMenuItem, toolStripSeparator1, saveProjectMenuItem, saveAsProjectMenuItem, saveTextToolStripMenuItem, toolStripSeparator2, renameProjectMenuItem, deleteProjectMenuItem, toolStripSeparator3, optionsProjectMenuItem, movieProjectMenuItem });
            projectToolStripMenuItem.Font = new System.Drawing.Font("Segoe UI", 9.75F);
            projectToolStripMenuItem.Name = "projectToolStripMenuItem";
            projectToolStripMenuItem.Size = new System.Drawing.Size(60, 21);
            projectToolStripMenuItem.Text = "Project";
            // 
            // newProjectMenuItem
            // 
            newProjectMenuItem.Name = "newProjectMenuItem";
            newProjectMenuItem.Size = new System.Drawing.Size(130, 22);
            newProjectMenuItem.Text = "New";
            newProjectMenuItem.ToolTipText = "Start a new workspace";
            newProjectMenuItem.Click += newProjectMenuItem_Click;
            // 
            // openProjectMenuItem
            // 
            openProjectMenuItem.Name = "openProjectMenuItem";
            openProjectMenuItem.Size = new System.Drawing.Size(130, 22);
            openProjectMenuItem.Text = "Open";
            openProjectMenuItem.ToolTipText = "Load a project into the workspace";
            openProjectMenuItem.Click += openProjectMenuItem_Click;
            // 
            // toolStripSeparator1
            // 
            toolStripSeparator1.Name = "toolStripSeparator1";
            toolStripSeparator1.Size = new System.Drawing.Size(127, 6);
            // 
            // saveProjectMenuItem
            // 
            saveProjectMenuItem.Name = "saveProjectMenuItem";
            saveProjectMenuItem.Size = new System.Drawing.Size(130, 22);
            saveProjectMenuItem.Text = "Save";
            saveProjectMenuItem.ToolTipText = "Save the workspace to the active project";
            saveProjectMenuItem.Click += saveProjectMenuItem_Click;
            // 
            // saveAsProjectMenuItem
            // 
            saveAsProjectMenuItem.Name = "saveAsProjectMenuItem";
            saveAsProjectMenuItem.Size = new System.Drawing.Size(130, 22);
            saveAsProjectMenuItem.Text = "Save As";
            saveAsProjectMenuItem.ToolTipText = "Save the workspace to a project";
            saveAsProjectMenuItem.Click += saveAsProjectMenuItem_Click;
            // 
            // saveTextToolStripMenuItem
            // 
            saveTextToolStripMenuItem.Name = "saveTextToolStripMenuItem";
            saveTextToolStripMenuItem.Size = new System.Drawing.Size(130, 22);
            saveTextToolStripMenuItem.Text = "Save Text";
            saveTextToolStripMenuItem.Click += saveTextToolStripMenuItem_Click;
            // 
            // toolStripSeparator2
            // 
            toolStripSeparator2.Name = "toolStripSeparator2";
            toolStripSeparator2.Size = new System.Drawing.Size(127, 6);
            // 
            // renameProjectMenuItem
            // 
            renameProjectMenuItem.Name = "renameProjectMenuItem";
            renameProjectMenuItem.Size = new System.Drawing.Size(130, 22);
            renameProjectMenuItem.Text = "Rename";
            renameProjectMenuItem.ToolTipText = "Rename the active project";
            renameProjectMenuItem.Click += renameProjectMenuItem_Click;
            // 
            // deleteProjectMenuItem
            // 
            deleteProjectMenuItem.Name = "deleteProjectMenuItem";
            deleteProjectMenuItem.Size = new System.Drawing.Size(130, 22);
            deleteProjectMenuItem.Text = "Delete";
            deleteProjectMenuItem.ToolTipText = "Delete a project";
            deleteProjectMenuItem.Click += deleteProjectMenuItem_Click;
            // 
            // toolStripSeparator3
            // 
            toolStripSeparator3.Name = "toolStripSeparator3";
            toolStripSeparator3.Size = new System.Drawing.Size(127, 6);
            // 
            // optionsProjectMenuItem
            // 
            optionsProjectMenuItem.Name = "optionsProjectMenuItem";
            optionsProjectMenuItem.Size = new System.Drawing.Size(130, 22);
            optionsProjectMenuItem.Text = "Options";
            optionsProjectMenuItem.Click += optionsProjectMenuItem_Click;
            // 
            // movieProjectMenuItem
            // 
            movieProjectMenuItem.Name = "movieProjectMenuItem";
            movieProjectMenuItem.Size = new System.Drawing.Size(130, 22);
            movieProjectMenuItem.Text = "Movie";
            movieProjectMenuItem.ToolTipText = "Save a set of sequenced images";
            movieProjectMenuItem.Click += movieProjectMenuItem_Click;
            // 
            // grid
            // 
            grid.ActiveRow = 0;
            grid.AllowUserToResizeRows = false;
            grid.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            grid.BackgroundColor = System.Drawing.SystemColors.ControlLightLight;
            grid.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.None;
            grid.ColumnHeadersHeight = 14;
            grid.ColumnHeadersVisible = false;
            grid.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            grid.Location = new System.Drawing.Point(12, 371);
            grid.Name = "grid";
            grid.RowHeadersVisible = false;
            grid.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.AutoSizeToFirstHeader;
            grid.RowTemplate.Height = 14;
            grid.RowTemplate.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            grid.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            grid.Size = new System.Drawing.Size(443, 448);
            grid.TabIndex = 0;
            grid.CellDoubleClick += grid_CellDoubleClick;
            grid.CellEndEdit += grid_CellEndEdit;
            grid.CellValidating += grid_CellValidating;
            grid.SelectionChanged += grid_SelectionChanged;
            grid.MouseDown += grid_MouseDown;
            grid.MouseUp += grid_MouseUp;
            grid.PreviewKeyDown += grid_PreviewKeyDown;
            // 
            // reducedViewCB
            // 
            reducedViewCB.Location = new System.Drawing.Point(0, 0);
            reducedViewCB.Name = "reducedViewCB";
            reducedViewCB.Size = new System.Drawing.Size(104, 24);
            reducedViewCB.TabIndex = 0;
            // 
            // picturePanel
            // 
            picturePanel.Controls.Add(glControl);
            picturePanel.Location = new System.Drawing.Point(504, 24);
            picturePanel.Name = "picturePanel";
            picturePanel.Size = new System.Drawing.Size(975, 865);
            picturePanel.TabIndex = 1;
            // 
            // glControl
            // 
            glControl.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            glControl.API = OpenTK.Windowing.Common.ContextAPI.OpenGL;
            glControl.APIVersion = new System.Version(3, 3, 0, 0);
            glControl.BackColor = System.Drawing.SystemColors.Control;
            glControl.Flags = OpenTK.Windowing.Common.ContextFlags.Default;
            glControl.IsEventDriven = true;
            glControl.Location = new System.Drawing.Point(176, 204);
            glControl.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            glControl.Name = "glControl";
            glControl.Profile = OpenTK.Windowing.Common.ContextProfile.Compatability;
            glControl.SharedContext = null;
            glControl.Size = new System.Drawing.Size(637, 425);
            glControl.TabIndex = 2;
            glControl.Text = "glControl1";
            // 
            // viewPopMenu
            // 
            viewPopMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { panToolStripMenuItem, windowToolStripMenuItem, zoomToolStripMenuItem, toolStripSeparator4, saveAsToolStripMenuItem, photoGenerateToolStripMenuItem });
            viewPopMenu.Name = "viewPopMenu";
            viewPopMenu.Size = new System.Drawing.Size(169, 120);
            // 
            // panToolStripMenuItem
            // 
            panToolStripMenuItem.Name = "panToolStripMenuItem";
            panToolStripMenuItem.Size = new System.Drawing.Size(168, 22);
            panToolStripMenuItem.Text = "Pan";
            panToolStripMenuItem.Click += panToolStripMenuItem_Click;
            // 
            // windowToolStripMenuItem
            // 
            windowToolStripMenuItem.Name = "windowToolStripMenuItem";
            windowToolStripMenuItem.Size = new System.Drawing.Size(168, 22);
            windowToolStripMenuItem.Text = "Window";
            windowToolStripMenuItem.Click += windowToolStripMenuItem_Click;
            // 
            // zoomToolStripMenuItem
            // 
            zoomToolStripMenuItem.Name = "zoomToolStripMenuItem";
            zoomToolStripMenuItem.Size = new System.Drawing.Size(168, 22);
            zoomToolStripMenuItem.Text = "Zoom";
            zoomToolStripMenuItem.Click += zoomToolStripMenuItem_Click;
            // 
            // toolStripSeparator4
            // 
            toolStripSeparator4.Name = "toolStripSeparator4";
            toolStripSeparator4.Size = new System.Drawing.Size(165, 6);
            // 
            // saveAsToolStripMenuItem
            // 
            saveAsToolStripMenuItem.Name = "saveAsToolStripMenuItem";
            saveAsToolStripMenuItem.Size = new System.Drawing.Size(168, 22);
            saveAsToolStripMenuItem.Text = "Save As ...";
            saveAsToolStripMenuItem.Click += saveAsToolStripMenuItem_Click;
            // 
            // photoGenerateToolStripMenuItem
            // 
            photoGenerateToolStripMenuItem.Name = "photoGenerateToolStripMenuItem";
            photoGenerateToolStripMenuItem.Size = new System.Drawing.Size(168, 22);
            photoGenerateToolStripMenuItem.Text = "Photo Generate ...";
            photoGenerateToolStripMenuItem.Click += photoGenerateToolStripMenuItem_Click;
            // 
            // gridPopMenu
            // 
            gridPopMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { setPolynomialsToolStripMenuItem, setParametersToolStripMenuItem, saveTextToolStripMenuItem1 });
            gridPopMenu.Name = "gridPopMenu";
            gridPopMenu.Size = new System.Drawing.Size(170, 70);
            gridPopMenu.Opening += gridPopMenu_Opening;
            // 
            // setPolynomialsToolStripMenuItem
            // 
            setPolynomialsToolStripMenuItem.Name = "setPolynomialsToolStripMenuItem";
            setPolynomialsToolStripMenuItem.Size = new System.Drawing.Size(169, 22);
            setPolynomialsToolStripMenuItem.Text = "Set Polynomials";
            setPolynomialsToolStripMenuItem.Click += setPolynomialsToolStripMenuItem_Click;
            // 
            // setParametersToolStripMenuItem
            // 
            setParametersToolStripMenuItem.Name = "setParametersToolStripMenuItem";
            setParametersToolStripMenuItem.Size = new System.Drawing.Size(169, 22);
            setParametersToolStripMenuItem.Text = "Set All Parameters";
            setParametersToolStripMenuItem.Click += setParametersToolStripMenuItem_Click;
            // 
            // saveTextToolStripMenuItem1
            // 
            saveTextToolStripMenuItem1.Name = "saveTextToolStripMenuItem1";
            saveTextToolStripMenuItem1.Size = new System.Drawing.Size(169, 22);
            saveTextToolStripMenuItem1.Text = "Save Text";
            saveTextToolStripMenuItem1.Click += saveTextToolStripMenuItem1_Click;
            // 
            // FractalFactory
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            BackColor = System.Drawing.SystemColors.WindowFrame;
            ClientSize = new System.Drawing.Size(1729, 901);
            Controls.Add(picturePanel);
            Controls.Add(controlPanel);
            FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            KeyPreview = true;
            MainMenuStrip = projectMenu;
            MaximizeBox = false;
            Name = "FractalFactory";
            Text = "Fractal Explorer";
            FormClosing += FractalFactory_FormClosing;
            Load += Form1_Load;
            KeyPress += FractalFactory_KeyPress;
            controlPanel.ResumeLayout(false);
            controlPanel.PerformLayout();
            progressPanel.ResumeLayout(false);
            progressPanel.PerformLayout();
            recordingGroupBox.ResumeLayout(false);
            recordingGroupBox.PerformLayout();
            domainSettings.ResumeLayout(false);
            domainSettings.PerformLayout();
            polysGroupBox.ResumeLayout(false);
            polysGroupBox.PerformLayout();
            exeGroupBox.ResumeLayout(false);
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            projectMenu.ResumeLayout(false);
            projectMenu.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)grid).EndInit();
            picturePanel.ResumeLayout(false);
            viewPopMenu.ResumeLayout(false);
            gridPopMenu.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.Panel controlPanel;
        private System.Windows.Forms.Panel picturePanel;
        private GLControl glControl;
        private System.Windows.Forms.Label time;
        private System.Windows.Forms.Label total;
        private System.Windows.Forms.CheckBox reducedViewCB;
        private System.Windows.Forms.MenuStrip projectMenu;
        private System.Windows.Forms.ToolStripMenuItem projectToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem newProjectMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openProjectMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem saveProjectMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveAsProjectMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripMenuItem renameProjectMenuItem;
        private System.Windows.Forms.ToolStripMenuItem deleteProjectMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripMenuItem optionsProjectMenuItem;
        private System.Windows.Forms.ToolStripMenuItem movieProjectMenuItem;
        private System.Windows.Forms.GroupBox exeGroupBox;
        private System.Windows.Forms.GroupBox recordingGroupBox;
        private System.Windows.Forms.GroupBox domainSettings;
        private System.Windows.Forms.GroupBox polysGroupBox;
        private System.Windows.Forms.Button stop;
        private System.Windows.Forms.Button run;
        private System.Windows.Forms.Button generate;
        private CustomControls.CustomDbGridCtrl grid;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button record;
        private System.Windows.Forms.Button smooth;
        private System.Windows.Forms.Button interpolate;
        private System.Windows.Forms.RadioButton multiFrame;
        private System.Windows.Forms.RadioButton singleFrame;
        private System.Windows.Forms.Label lbInfo;
        private System.Windows.Forms.TextBox precision;
        private System.Windows.Forms.Button update;
        private System.Windows.Forms.Button clear;
        private System.Windows.Forms.Button extend;
        private System.Windows.Forms.Button Copy;
        private System.Windows.Forms.TextBox ymax;
        private System.Windows.Forms.TextBox ymin;
        private System.Windows.Forms.TextBox xmax;
        private System.Windows.Forms.TextBox xmin;
        private System.Windows.Forms.TextBox denomPoly;
        private System.Windows.Forms.TextBox numerPoly;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox steps;
        private System.Windows.Forms.TextBox divs;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label denomLabel;
        private System.Windows.Forms.Label numerLabel;
        private System.Windows.Forms.ContextMenuStrip gridPopMenu;
        private System.Windows.Forms.ToolStripMenuItem setPolynomialsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem setParametersToolStripMenuItem;
        private System.Windows.Forms.ContextMenuStrip viewPopMenu;
        private System.Windows.Forms.ToolStripMenuItem panToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem windowToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem zoomToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
        private System.Windows.Forms.ToolStripMenuItem saveAsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem photoGenerateToolStripMenuItem;
        private System.Windows.Forms.ToolTip toolTip;
        private System.Windows.Forms.ToolStripMenuItem saveTextToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveTextToolStripMenuItem1;
        private System.Windows.Forms.Panel progressPanel;
        private System.Windows.Forms.ProgressBar progressBar;
        private System.Windows.Forms.Label progressLabel;
    }
}
