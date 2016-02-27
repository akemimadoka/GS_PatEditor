namespace GS_PatEditor.Editor
{
    partial class EditorForm
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
            this.components = new System.ComponentModel.Container();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.animationFrames = new System.Windows.Forms.PictureBox();
            this.previewWindow = new System.Windows.Forms.PictureBox();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.flowLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.animationFrames)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.previewWindow)).BeginInit();
            this.SuspendLayout();
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.AutoScroll = true;
            this.flowLayoutPanel1.BackColor = System.Drawing.Color.White;
            this.flowLayoutPanel1.Controls.Add(this.animationFrames);
            this.flowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.flowLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(587, 100);
            this.flowLayoutPanel1.TabIndex = 1;
            // 
            // animationFrames
            // 
            this.animationFrames.Location = new System.Drawing.Point(0, 0);
            this.animationFrames.Margin = new System.Windows.Forms.Padding(0);
            this.animationFrames.Name = "animationFrames";
            this.animationFrames.Size = new System.Drawing.Size(433, 69);
            this.animationFrames.TabIndex = 1;
            this.animationFrames.TabStop = false;
            // 
            // previewWindow
            // 
            this.previewWindow.Location = new System.Drawing.Point(82, 121);
            this.previewWindow.Name = "previewWindow";
            this.previewWindow.Size = new System.Drawing.Size(402, 219);
            this.previewWindow.TabIndex = 2;
            this.previewWindow.TabStop = false;
            // 
            // timer1
            // 
            this.timer1.Enabled = true;
            this.timer1.Interval = 16;
            // 
            // EditorForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(587, 371);
            this.Controls.Add(this.previewWindow);
            this.Controls.Add(this.flowLayoutPanel1);
            this.Name = "EditorForm";
            this.Text = "EditorForm";
            this.flowLayoutPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.animationFrames)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.previewWindow)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.PictureBox animationFrames;
        private System.Windows.Forms.PictureBox previewWindow;
        private System.Windows.Forms.Timer timer1;

    }
}