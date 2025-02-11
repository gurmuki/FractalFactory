namespace FractalFactory
{
    partial class ProjectMovieDialog
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
            pathsGroupBox = new System.Windows.Forms.GroupBox();
            watermark = new System.Windows.Forms.CheckBox();
            moviePath = new System.Windows.Forms.Label();
            movie = new System.Windows.Forms.TextBox();
            browse = new System.Windows.Forms.Button();
            watermarkPath = new System.Windows.Forms.TextBox();
            frameRate = new System.Windows.Forms.TextBox();
            frameRateLBL = new System.Windows.Forms.Label();
            cancel = new System.Windows.Forms.Button();
            accept = new System.Windows.Forms.Button();
            showProcess = new System.Windows.Forms.CheckBox();
            pathsGroupBox.SuspendLayout();
            SuspendLayout();
            // 
            // pathsGroupBox
            // 
            pathsGroupBox.Controls.Add(watermark);
            pathsGroupBox.Controls.Add(moviePath);
            pathsGroupBox.Controls.Add(movie);
            pathsGroupBox.Controls.Add(browse);
            pathsGroupBox.Controls.Add(watermarkPath);
            pathsGroupBox.Location = new System.Drawing.Point(12, 12);
            pathsGroupBox.Name = "pathsGroupBox";
            pathsGroupBox.Size = new System.Drawing.Size(442, 90);
            pathsGroupBox.TabIndex = 1;
            pathsGroupBox.TabStop = false;
            pathsGroupBox.Text = "Paths";
            // 
            // watermark
            // 
            watermark.AutoSize = true;
            watermark.Location = new System.Drawing.Point(6, 53);
            watermark.Name = "watermark";
            watermark.Size = new System.Drawing.Size(84, 19);
            watermark.TabIndex = 9;
            watermark.Text = "Watermark";
            watermark.UseVisualStyleBackColor = true;
            watermark.CheckedChanged += watermark_CheckedChanged;
            // 
            // moviePath
            // 
            moviePath.AutoSize = true;
            moviePath.Location = new System.Drawing.Point(40, 26);
            moviePath.Name = "moviePath";
            moviePath.Size = new System.Drawing.Size(40, 15);
            moviePath.TabIndex = 2;
            moviePath.Text = "Movie";
            // 
            // movie
            // 
            movie.Location = new System.Drawing.Point(90, 22);
            movie.Name = "movie";
            movie.ReadOnly = true;
            movie.Size = new System.Drawing.Size(265, 23);
            movie.TabIndex = 3;
            movie.Enter += movie_Enter;
            movie.MouseUp += movie_MouseUp;
            // 
            // browse
            // 
            browse.Location = new System.Drawing.Point(361, 36);
            browse.Name = "browse";
            browse.Size = new System.Drawing.Size(75, 23);
            browse.TabIndex = 8;
            browse.Text = "Browse";
            browse.UseVisualStyleBackColor = true;
            browse.Click += browse_Click;
            // 
            // watermarkPath
            // 
            watermarkPath.Location = new System.Drawing.Point(90, 51);
            watermarkPath.Name = "watermarkPath";
            watermarkPath.ReadOnly = true;
            watermarkPath.Size = new System.Drawing.Size(265, 23);
            watermarkPath.TabIndex = 5;
            watermarkPath.Enter += watermarkPath_Enter;
            watermarkPath.MouseUp += watermarkPath_MouseUp;
            // 
            // frameRate
            // 
            frameRate.Location = new System.Drawing.Point(95, 117);
            frameRate.Name = "frameRate";
            frameRate.Size = new System.Drawing.Size(39, 23);
            frameRate.TabIndex = 10;
            frameRate.TextChanged += frameRate_TextChanged;
            frameRate.KeyPress += frameRate_KeyPress;
            // 
            // frameRateLBL
            // 
            frameRateLBL.AutoSize = true;
            frameRateLBL.Location = new System.Drawing.Point(29, 121);
            frameRateLBL.Name = "frameRateLBL";
            frameRateLBL.Size = new System.Drawing.Size(63, 15);
            frameRateLBL.TabIndex = 9;
            frameRateLBL.Text = "Frame rate";
            // 
            // cancel
            // 
            cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            cancel.Location = new System.Drawing.Point(293, 146);
            cancel.Name = "cancel";
            cancel.Size = new System.Drawing.Size(75, 27);
            cancel.TabIndex = 12;
            cancel.Text = "Cancel";
            cancel.UseVisualStyleBackColor = true;
            // 
            // accept
            // 
            accept.DialogResult = System.Windows.Forms.DialogResult.OK;
            accept.Location = new System.Drawing.Point(373, 146);
            accept.Name = "accept";
            accept.Size = new System.Drawing.Size(75, 27);
            accept.TabIndex = 11;
            accept.Text = "Accept";
            accept.UseVisualStyleBackColor = true;
            accept.Click += accept_Click;
            // 
            // showProcess
            // 
            showProcess.AutoSize = true;
            showProcess.Location = new System.Drawing.Point(310, 119);
            showProcess.Name = "showProcess";
            showProcess.Size = new System.Drawing.Size(98, 19);
            showProcess.TabIndex = 13;
            showProcess.Text = "Show Process";
            showProcess.UseVisualStyleBackColor = true;
            // 
            // ProjectMovieDialog
            // 
            AcceptButton = accept;
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            CancelButton = cancel;
            ClientSize = new System.Drawing.Size(472, 188);
            Controls.Add(showProcess);
            Controls.Add(accept);
            Controls.Add(cancel);
            Controls.Add(frameRateLBL);
            Controls.Add(frameRate);
            Controls.Add(pathsGroupBox);
            FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            Name = "ProjectMovieDialog";
            ShowInTaskbar = false;
            Text = "Create Movie";
            Load += ProjectMovieDialog_Load;
            pathsGroupBox.ResumeLayout(false);
            pathsGroupBox.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.GroupBox pathsGroupBox;
        private System.Windows.Forms.TextBox watermarkPath;
        private System.Windows.Forms.Button browse;
        private System.Windows.Forms.Label moviePath;
        private System.Windows.Forms.TextBox movie;
        private System.Windows.Forms.TextBox frameRate;
        private System.Windows.Forms.Label frameRateLBL;
        private System.Windows.Forms.Button cancel;
        private System.Windows.Forms.Button accept;
        private System.Windows.Forms.CheckBox showProcess;
        private System.Windows.Forms.CheckBox watermark;
    }
}