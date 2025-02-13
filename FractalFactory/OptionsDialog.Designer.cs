namespace FractalFactory
{
    partial class OptionsDialog
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            label1 = new System.Windows.Forms.Label();
            method = new System.Windows.Forms.ComboBox();
            defaultFolders = new System.Windows.Forms.GroupBox();
            foldersBrowse = new System.Windows.Forms.Button();
            label4 = new System.Windows.Forms.Label();
            database = new System.Windows.Forms.TextBox();
            label3 = new System.Windows.Forms.Label();
            defMovieFolder = new System.Windows.Forms.TextBox();
            label2 = new System.Windows.Forms.Label();
            defProjFolder = new System.Windows.Forms.TextBox();
            seedFileOptions = new System.Windows.Forms.Panel();
            useSeedFile = new System.Windows.Forms.CheckBox();
            select = new System.Windows.Forms.Button();
            seedFilePath = new System.Windows.Forms.TextBox();
            processingOptions = new System.Windows.Forms.Panel();
            externals = new System.Windows.Forms.GroupBox();
            externalsBrowse = new System.Windows.Forms.Button();
            imageViewerLBL = new System.Windows.Forms.Label();
            imageViewer = new System.Windows.Forms.TextBox();
            movieViewerLBL = new System.Windows.Forms.Label();
            movieViewer = new System.Windows.Forms.TextBox();
            movieGeneratorLBL = new System.Windows.Forms.Label();
            movieGenerator = new System.Windows.Forms.TextBox();
            imageGroupBox = new System.Windows.Forms.GroupBox();
            label5 = new System.Windows.Forms.Label();
            orientation = new System.Windows.Forms.ComboBox();
            reducedImage = new System.Windows.Forms.CheckBox();
            previewMode = new System.Windows.Forms.CheckBox();
            exeGroupBox = new System.Windows.Forms.GroupBox();
            limitIterations = new System.Windows.Forms.CheckBox();
            parallelExecution = new System.Windows.Forms.RadioButton();
            serialExecution = new System.Windows.Forms.RadioButton();
            calibrate = new System.Windows.Forms.CheckBox();
            cancel = new System.Windows.Forms.Button();
            accept = new System.Windows.Forms.Button();
            defaultFolders.SuspendLayout();
            seedFileOptions.SuspendLayout();
            processingOptions.SuspendLayout();
            externals.SuspendLayout();
            imageGroupBox.SuspendLayout();
            exeGroupBox.SuspendLayout();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new System.Drawing.Point(22, 18);
            label1.Name = "label1";
            label1.Size = new System.Drawing.Size(49, 15);
            label1.TabIndex = 1;
            label1.Text = "Method";
            // 
            // method
            // 
            method.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            method.FormattingEnabled = true;
            method.Location = new System.Drawing.Point(72, 14);
            method.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            method.Name = "method";
            method.Size = new System.Drawing.Size(150, 23);
            method.TabIndex = 2;
            // 
            // defaultFolders
            // 
            defaultFolders.Controls.Add(foldersBrowse);
            defaultFolders.Controls.Add(label4);
            defaultFolders.Controls.Add(database);
            defaultFolders.Controls.Add(label3);
            defaultFolders.Controls.Add(defMovieFolder);
            defaultFolders.Controls.Add(label2);
            defaultFolders.Controls.Add(defProjFolder);
            defaultFolders.Location = new System.Drawing.Point(16, 47);
            defaultFolders.Name = "defaultFolders";
            defaultFolders.Size = new System.Drawing.Size(479, 117);
            defaultFolders.TabIndex = 3;
            defaultFolders.TabStop = false;
            defaultFolders.Text = "Default folders";
            defaultFolders.Leave += defaultFolders_Leave;
            // 
            // foldersBrowse
            // 
            foldersBrowse.Location = new System.Drawing.Point(384, 51);
            foldersBrowse.Name = "foldersBrowse";
            foldersBrowse.Size = new System.Drawing.Size(75, 27);
            foldersBrowse.TabIndex = 10;
            foldersBrowse.Text = "Browse";
            foldersBrowse.UseVisualStyleBackColor = true;
            foldersBrowse.Click += foldersBrowse_Click;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new System.Drawing.Point(22, 28);
            label4.Name = "label4";
            label4.Size = new System.Drawing.Size(58, 15);
            label4.TabIndex = 4;
            label4.Text = "Database:";
            // 
            // database
            // 
            database.Location = new System.Drawing.Point(83, 24);
            database.Name = "database";
            database.ReadOnly = true;
            database.Size = new System.Drawing.Size(293, 23);
            database.TabIndex = 5;
            database.Enter += database_Enter;
            database.MouseUp += database_MouseUp;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new System.Drawing.Point(37, 86);
            label3.Name = "label3";
            label3.Size = new System.Drawing.Size(43, 15);
            label3.TabIndex = 8;
            label3.Text = "Movie:";
            // 
            // defMovieFolder
            // 
            defMovieFolder.Location = new System.Drawing.Point(83, 82);
            defMovieFolder.Name = "defMovieFolder";
            defMovieFolder.ReadOnly = true;
            defMovieFolder.Size = new System.Drawing.Size(293, 23);
            defMovieFolder.TabIndex = 9;
            defMovieFolder.Enter += defMovieFolder_Enter;
            defMovieFolder.MouseUp += defMovieFolder_MouseUp;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new System.Drawing.Point(32, 57);
            label2.Name = "label2";
            label2.Size = new System.Drawing.Size(47, 15);
            label2.TabIndex = 6;
            label2.Text = "Project:";
            // 
            // defProjFolder
            // 
            defProjFolder.Location = new System.Drawing.Point(83, 53);
            defProjFolder.Name = "defProjFolder";
            defProjFolder.ReadOnly = true;
            defProjFolder.Size = new System.Drawing.Size(293, 23);
            defProjFolder.TabIndex = 7;
            defProjFolder.Enter += defProjFolder_Enter;
            defProjFolder.MouseUp += defProjFolder_MouseUp;
            // 
            // seedFileOptions
            // 
            seedFileOptions.Controls.Add(useSeedFile);
            seedFileOptions.Controls.Add(select);
            seedFileOptions.Controls.Add(seedFilePath);
            seedFileOptions.Location = new System.Drawing.Point(16, 165);
            seedFileOptions.Name = "seedFileOptions";
            seedFileOptions.Size = new System.Drawing.Size(479, 45);
            seedFileOptions.TabIndex = 11;
            // 
            // useSeedFile
            // 
            useSeedFile.AutoSize = true;
            useSeedFile.Location = new System.Drawing.Point(8, 14);
            useSeedFile.Name = "useSeedFile";
            useSeedFile.Size = new System.Drawing.Size(72, 19);
            useSeedFile.TabIndex = 12;
            useSeedFile.Text = "Seed File";
            useSeedFile.UseVisualStyleBackColor = true;
            useSeedFile.CheckedChanged += useSeedFile_CheckedChanged;
            // 
            // select
            // 
            select.Location = new System.Drawing.Point(384, 10);
            select.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            select.Name = "select";
            select.Size = new System.Drawing.Size(75, 27);
            select.TabIndex = 14;
            select.Text = "Select";
            select.UseVisualStyleBackColor = true;
            select.Click += select_Click;
            // 
            // seedFilePath
            // 
            seedFilePath.Location = new System.Drawing.Point(83, 12);
            seedFilePath.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            seedFilePath.Name = "seedFilePath";
            seedFilePath.ReadOnly = true;
            seedFilePath.Size = new System.Drawing.Size(293, 23);
            seedFilePath.TabIndex = 13;
            seedFilePath.Enter += seedFilePath_Enter;
            seedFilePath.MouseUp += seedFilePath_MouseUp;
            // 
            // processingOptions
            // 
            processingOptions.Controls.Add(externals);
            processingOptions.Controls.Add(imageGroupBox);
            processingOptions.Controls.Add(exeGroupBox);
            processingOptions.Location = new System.Drawing.Point(6, 218);
            processingOptions.Name = "processingOptions";
            processingOptions.Size = new System.Drawing.Size(503, 285);
            processingOptions.TabIndex = 15;
            // 
            // externals
            // 
            externals.Controls.Add(externalsBrowse);
            externals.Controls.Add(imageViewerLBL);
            externals.Controls.Add(imageViewer);
            externals.Controls.Add(movieViewerLBL);
            externals.Controls.Add(movieViewer);
            externals.Controls.Add(movieGeneratorLBL);
            externals.Controls.Add(movieGenerator);
            externals.Location = new System.Drawing.Point(11, 149);
            externals.Name = "externals";
            externals.Size = new System.Drawing.Size(478, 120);
            externals.TabIndex = 21;
            externals.TabStop = false;
            externals.Text = "Externals";
            externals.Leave += externals_Leave;
            // 
            // externalsBrowse
            // 
            externalsBrowse.Location = new System.Drawing.Point(383, 50);
            externalsBrowse.Name = "externalsBrowse";
            externalsBrowse.Size = new System.Drawing.Size(75, 27);
            externalsBrowse.TabIndex = 17;
            externalsBrowse.Text = "Browse";
            externalsBrowse.UseVisualStyleBackColor = true;
            externalsBrowse.Click += externalsBrowse_Click;
            // 
            // imageViewerLBL
            // 
            imageViewerLBL.AutoSize = true;
            imageViewerLBL.Location = new System.Drawing.Point(29, 26);
            imageViewerLBL.Name = "imageViewerLBL";
            imageViewerLBL.Size = new System.Drawing.Size(81, 15);
            imageViewerLBL.TabIndex = 11;
            imageViewerLBL.Text = "Image Viewer:";
            // 
            // imageViewer
            // 
            imageViewer.Location = new System.Drawing.Point(113, 23);
            imageViewer.Name = "imageViewer";
            imageViewer.ReadOnly = true;
            imageViewer.Size = new System.Drawing.Size(265, 23);
            imageViewer.TabIndex = 12;
            imageViewer.Enter += imageViewer_Enter;
            imageViewer.MouseUp += imageViewer_MouseUp;
            // 
            // movieViewerLBL
            // 
            movieViewerLBL.AutoSize = true;
            movieViewerLBL.Location = new System.Drawing.Point(29, 84);
            movieViewerLBL.Name = "movieViewerLBL";
            movieViewerLBL.Size = new System.Drawing.Size(81, 15);
            movieViewerLBL.TabIndex = 15;
            movieViewerLBL.Text = "Movie Viewer:";
            // 
            // movieViewer
            // 
            movieViewer.Location = new System.Drawing.Point(113, 81);
            movieViewer.Name = "movieViewer";
            movieViewer.ReadOnly = true;
            movieViewer.Size = new System.Drawing.Size(265, 23);
            movieViewer.TabIndex = 16;
            movieViewer.Enter += movieViewer_Enter;
            movieViewer.MouseUp += movieViewer_MouseUp;
            // 
            // movieGeneratorLBL
            // 
            movieGeneratorLBL.AutoSize = true;
            movieGeneratorLBL.Location = new System.Drawing.Point(12, 55);
            movieGeneratorLBL.Name = "movieGeneratorLBL";
            movieGeneratorLBL.Size = new System.Drawing.Size(98, 15);
            movieGeneratorLBL.TabIndex = 13;
            movieGeneratorLBL.Text = "Movie Generator:";
            // 
            // movieGenerator
            // 
            movieGenerator.Location = new System.Drawing.Point(113, 52);
            movieGenerator.Name = "movieGenerator";
            movieGenerator.ReadOnly = true;
            movieGenerator.Size = new System.Drawing.Size(265, 23);
            movieGenerator.TabIndex = 14;
            movieGenerator.Enter += movieGenerator_Enter;
            movieGenerator.MouseUp += movieGenerator_MouseUp;
            // 
            // imageGroupBox
            // 
            imageGroupBox.Controls.Add(label5);
            imageGroupBox.Controls.Add(orientation);
            imageGroupBox.Controls.Add(reducedImage);
            imageGroupBox.Controls.Add(previewMode);
            imageGroupBox.Location = new System.Drawing.Point(11, 75);
            imageGroupBox.Name = "imageGroupBox";
            imageGroupBox.Size = new System.Drawing.Size(478, 57);
            imageGroupBox.TabIndex = 20;
            imageGroupBox.TabStop = false;
            imageGroupBox.Text = "Image";
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new System.Drawing.Point(44, 26);
            label5.Name = "label5";
            label5.Size = new System.Drawing.Size(67, 15);
            label5.TabIndex = 21;
            label5.Text = "Orientation";
            // 
            // orientation
            // 
            orientation.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            orientation.FormattingEnabled = true;
            orientation.Location = new System.Drawing.Point(113, 22);
            orientation.MaxDropDownItems = 3;
            orientation.Name = "orientation";
            orientation.Size = new System.Drawing.Size(96, 23);
            orientation.TabIndex = 22;
            // 
            // reducedImage
            // 
            reducedImage.AutoSize = true;
            reducedImage.Location = new System.Drawing.Point(256, 24);
            reducedImage.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            reducedImage.Name = "reducedImage";
            reducedImage.Size = new System.Drawing.Size(72, 19);
            reducedImage.TabIndex = 23;
            reducedImage.Text = "Reduced";
            reducedImage.UseVisualStyleBackColor = true;
            // 
            // previewMode
            // 
            previewMode.AutoSize = true;
            previewMode.Location = new System.Drawing.Point(339, 24);
            previewMode.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            previewMode.Name = "previewMode";
            previewMode.Size = new System.Drawing.Size(67, 19);
            previewMode.TabIndex = 24;
            previewMode.Text = "Preview";
            previewMode.UseVisualStyleBackColor = true;
            // 
            // exeGroupBox
            // 
            exeGroupBox.Controls.Add(limitIterations);
            exeGroupBox.Controls.Add(parallelExecution);
            exeGroupBox.Controls.Add(serialExecution);
            exeGroupBox.Location = new System.Drawing.Point(11, 11);
            exeGroupBox.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            exeGroupBox.Name = "exeGroupBox";
            exeGroupBox.Padding = new System.Windows.Forms.Padding(4, 3, 4, 3);
            exeGroupBox.Size = new System.Drawing.Size(478, 59);
            exeGroupBox.TabIndex = 16;
            exeGroupBox.TabStop = false;
            exeGroupBox.Text = "Execution";
            // 
            // limitIterations
            // 
            limitIterations.AutoSize = true;
            limitIterations.Location = new System.Drawing.Point(256, 26);
            limitIterations.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            limitIterations.Name = "limitIterations";
            limitIterations.Size = new System.Drawing.Size(105, 19);
            limitIterations.TabIndex = 19;
            limitIterations.Text = "Limit Iterations";
            limitIterations.UseVisualStyleBackColor = true;
            // 
            // parallelExecution
            // 
            parallelExecution.AutoSize = true;
            parallelExecution.Location = new System.Drawing.Point(73, 26);
            parallelExecution.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            parallelExecution.Name = "parallelExecution";
            parallelExecution.Size = new System.Drawing.Size(63, 19);
            parallelExecution.TabIndex = 17;
            parallelExecution.TabStop = true;
            parallelExecution.Text = "Parallel";
            parallelExecution.UseVisualStyleBackColor = true;
            // 
            // serialExecution
            // 
            serialExecution.AutoSize = true;
            serialExecution.Location = new System.Drawing.Point(145, 26);
            serialExecution.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            serialExecution.Name = "serialExecution";
            serialExecution.Size = new System.Drawing.Size(53, 19);
            serialExecution.TabIndex = 18;
            serialExecution.TabStop = true;
            serialExecution.Text = "Serial";
            serialExecution.UseVisualStyleBackColor = true;
            // 
            // calibrate
            // 
            calibrate.AutoSize = true;
            calibrate.Location = new System.Drawing.Point(44, 526);
            calibrate.Name = "calibrate";
            calibrate.Size = new System.Drawing.Size(73, 19);
            calibrate.TabIndex = 25;
            calibrate.Text = "Calibrate";
            calibrate.UseVisualStyleBackColor = true;
            // 
            // cancel
            // 
            cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            cancel.Location = new System.Drawing.Point(321, 522);
            cancel.Name = "cancel";
            cancel.Size = new System.Drawing.Size(75, 27);
            cancel.TabIndex = 26;
            cancel.Text = "Cancel";
            cancel.UseVisualStyleBackColor = true;
            // 
            // accept
            // 
            accept.DialogResult = System.Windows.Forms.DialogResult.OK;
            accept.Location = new System.Drawing.Point(400, 522);
            accept.Name = "accept";
            accept.Size = new System.Drawing.Size(75, 27);
            accept.TabIndex = 27;
            accept.Text = "Accept";
            accept.UseVisualStyleBackColor = true;
            accept.Click += accept_Click;
            // 
            // OptionsDialog
            // 
            AcceptButton = accept;
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            CancelButton = cancel;
            ClientSize = new System.Drawing.Size(515, 558);
            Controls.Add(calibrate);
            Controls.Add(accept);
            Controls.Add(cancel);
            Controls.Add(seedFileOptions);
            Controls.Add(processingOptions);
            Controls.Add(defaultFolders);
            Controls.Add(label1);
            Controls.Add(method);
            FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            Name = "OptionsDialog";
            ShowInTaskbar = false;
            Text = "[context dependent]";
            Load += OptionsDialog_Load;
            defaultFolders.ResumeLayout(false);
            defaultFolders.PerformLayout();
            seedFileOptions.ResumeLayout(false);
            seedFileOptions.PerformLayout();
            processingOptions.ResumeLayout(false);
            externals.ResumeLayout(false);
            externals.PerformLayout();
            imageGroupBox.ResumeLayout(false);
            imageGroupBox.PerformLayout();
            exeGroupBox.ResumeLayout(false);
            exeGroupBox.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox method;
        private System.Windows.Forms.GroupBox defaultFolders;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox database;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox defProjFolder;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox defMovieFolder;
        private System.Windows.Forms.Button foldersBrowse;
        private System.Windows.Forms.Panel seedFileOptions;
        private System.Windows.Forms.CheckBox useSeedFile;
        private System.Windows.Forms.TextBox seedFilePath;
        private System.Windows.Forms.Button select;
        private System.Windows.Forms.Panel processingOptions;
        private System.Windows.Forms.GroupBox exeGroupBox;
        private System.Windows.Forms.RadioButton parallelExecution;
        private System.Windows.Forms.RadioButton serialExecution;
        private System.Windows.Forms.CheckBox limitIterations;
        private System.Windows.Forms.GroupBox imageGroupBox;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ComboBox orientation;
        private System.Windows.Forms.CheckBox reducedImage;
        private System.Windows.Forms.CheckBox previewMode;
        private System.Windows.Forms.CheckBox calibrate;
        private System.Windows.Forms.Button cancel;
        private System.Windows.Forms.Button accept;
        private System.Windows.Forms.GroupBox externals;
        private System.Windows.Forms.Button externalsBrowse;
        private System.Windows.Forms.Label imageViewerLBL;
        private System.Windows.Forms.TextBox imageViewer;
        private System.Windows.Forms.Label movieViewerLBL;
        private System.Windows.Forms.TextBox movieViewer;
        private System.Windows.Forms.Label movieGeneratorLBL;
        private System.Windows.Forms.TextBox movieGenerator;
    }
}