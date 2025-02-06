namespace FractalFactory
{
    partial class ProjectMovieDirectives
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
            cancel = new System.Windows.Forms.Button();
            accept = new System.Windows.Forms.Button();
            images = new System.Windows.Forms.GroupBox();
            overwriteImageFiles = new System.Windows.Forms.CheckBox();
            outputSelectedRange = new System.Windows.Forms.CheckBox();
            overwriteMovieFile = new System.Windows.Forms.CheckBox();
            viewMovie = new System.Windows.Forms.CheckBox();
            images.SuspendLayout();
            SuspendLayout();
            // 
            // cancel
            // 
            cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            cancel.Location = new System.Drawing.Point(29, 167);
            cancel.Name = "cancel";
            cancel.Size = new System.Drawing.Size(75, 27);
            cancel.TabIndex = 7;
            cancel.Text = "Cancel";
            cancel.UseVisualStyleBackColor = true;
            // 
            // accept
            // 
            accept.DialogResult = System.Windows.Forms.DialogResult.OK;
            accept.Location = new System.Drawing.Point(108, 167);
            accept.Name = "accept";
            accept.Size = new System.Drawing.Size(75, 27);
            accept.TabIndex = 6;
            accept.Text = "Accept";
            accept.UseVisualStyleBackColor = true;
            accept.Click += accept_Click;
            // 
            // images
            // 
            images.Controls.Add(overwriteImageFiles);
            images.Controls.Add(outputSelectedRange);
            images.Location = new System.Drawing.Point(9, 10);
            images.Name = "images";
            images.Size = new System.Drawing.Size(196, 78);
            images.TabIndex = 1;
            images.TabStop = false;
            images.Text = "Images";
            // 
            // overwriteImageFiles
            // 
            overwriteImageFiles.AutoSize = true;
            overwriteImageFiles.Location = new System.Drawing.Point(12, 22);
            overwriteImageFiles.Name = "overwriteImageFiles";
            overwriteImageFiles.Size = new System.Drawing.Size(162, 19);
            overwriteImageFiles.TabIndex = 2;
            overwriteImageFiles.Text = "Overwrite existing images";
            overwriteImageFiles.UseVisualStyleBackColor = true;
            overwriteImageFiles.CheckedChanged += overwriteImageFiles_CheckedChanged;
            // 
            // outputSelectedRange
            // 
            outputSelectedRange.AutoSize = true;
            outputSelectedRange.Location = new System.Drawing.Point(12, 47);
            outputSelectedRange.Name = "outputSelectedRange";
            outputSelectedRange.Size = new System.Drawing.Size(136, 19);
            outputSelectedRange.TabIndex = 3;
            outputSelectedRange.Text = "Output only selected";
            outputSelectedRange.UseVisualStyleBackColor = true;
            // 
            // overwriteMovieFile
            // 
            overwriteMovieFile.AutoSize = true;
            overwriteMovieFile.Location = new System.Drawing.Point(21, 106);
            overwriteMovieFile.Name = "overwriteMovieFile";
            overwriteMovieFile.Size = new System.Drawing.Size(157, 19);
            overwriteMovieFile.TabIndex = 4;
            overwriteMovieFile.Text = "Overwrite existing movie";
            overwriteMovieFile.UseVisualStyleBackColor = true;
            overwriteMovieFile.CheckedChanged += overwriteMovieFile_CheckedChanged;
            // 
            // viewMovie
            // 
            viewMovie.AutoSize = true;
            viewMovie.Location = new System.Drawing.Point(21, 131);
            viewMovie.Name = "viewMovie";
            viewMovie.Size = new System.Drawing.Size(160, 19);
            viewMovie.TabIndex = 5;
            viewMovie.Text = "View movie after creation";
            viewMovie.UseVisualStyleBackColor = true;
            // 
            // ProjectMovieDirectives
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(213, 202);
            Controls.Add(images);
            Controls.Add(cancel);
            Controls.Add(accept);
            Controls.Add(viewMovie);
            Controls.Add(overwriteMovieFile);
            FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            Name = "ProjectMovieDirectives";
            ShowInTaskbar = false;
            Text = "Movie Directives";
            Load += ProjectMovieDirectives_Load;
            images.ResumeLayout(false);
            images.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private System.Windows.Forms.GroupBox images;
        private System.Windows.Forms.CheckBox overwriteImageFiles;
        private System.Windows.Forms.CheckBox outputSelectedRange;
        private System.Windows.Forms.CheckBox overwriteMovieFile;
        private System.Windows.Forms.CheckBox viewMovie;
        private System.Windows.Forms.Button accept;
        private System.Windows.Forms.Button cancel;
    }
}