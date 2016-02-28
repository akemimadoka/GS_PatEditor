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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(EditorForm));
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.animationFrames = new System.Windows.Forms.PictureBox();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.flowLayoutPanel2 = new System.Windows.Forms.FlowLayoutPanel();
            this.previewWindow = new System.Windows.Forms.PictureBox();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toolStripCollapseAll = new System.Windows.Forms.ToolStripButton();
            this.toolStripExpandAll = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripButtonToolCursor = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonToolMove = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonToolPhysics = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripSplitBoxVisible = new System.Windows.Forms.ToolStripDropDownButton();
            this.physicalToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.flowLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.animationFrames)).BeginInit();
            this.flowLayoutPanel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.previewWindow)).BeginInit();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.flowLayoutPanel1.AutoScroll = true;
            this.flowLayoutPanel1.BackColor = System.Drawing.Color.White;
            this.flowLayoutPanel1.Controls.Add(this.animationFrames);
            this.flowLayoutPanel1.Location = new System.Drawing.Point(0, 28);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(627, 100);
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
            // timer1
            // 
            this.timer1.Enabled = true;
            this.timer1.Interval = 16;
            // 
            // flowLayoutPanel2
            // 
            this.flowLayoutPanel2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.flowLayoutPanel2.AutoScroll = true;
            this.flowLayoutPanel2.Controls.Add(this.previewWindow);
            this.flowLayoutPanel2.Location = new System.Drawing.Point(0, 136);
            this.flowLayoutPanel2.Margin = new System.Windows.Forms.Padding(0);
            this.flowLayoutPanel2.Name = "flowLayoutPanel2";
            this.flowLayoutPanel2.Size = new System.Drawing.Size(627, 274);
            this.flowLayoutPanel2.TabIndex = 3;
            // 
            // previewWindow
            // 
            this.previewWindow.Location = new System.Drawing.Point(3, 3);
            this.previewWindow.Name = "previewWindow";
            this.previewWindow.Size = new System.Drawing.Size(800, 600);
            this.previewWindow.TabIndex = 3;
            this.previewWindow.TabStop = false;
            // 
            // toolStrip1
            // 
            this.toolStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripCollapseAll,
            this.toolStripExpandAll,
            this.toolStripSeparator1,
            this.toolStripButtonToolCursor,
            this.toolStripButtonToolMove,
            this.toolStripButtonToolPhysics,
            this.toolStripSeparator2,
            this.toolStripSplitBoxVisible});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.RenderMode = System.Windows.Forms.ToolStripRenderMode.Professional;
            this.toolStrip1.Size = new System.Drawing.Size(626, 25);
            this.toolStrip1.TabIndex = 4;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // toolStripCollapseAll
            // 
            this.toolStripCollapseAll.Image = ((System.Drawing.Image)(resources.GetObject("toolStripCollapseAll.Image")));
            this.toolStripCollapseAll.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripCollapseAll.Name = "toolStripCollapseAll";
            this.toolStripCollapseAll.Size = new System.Drawing.Size(92, 22);
            this.toolStripCollapseAll.Text = "CollapseAll";
            this.toolStripCollapseAll.Click += new System.EventHandler(this.toolStripCollapseAll_Click);
            // 
            // toolStripExpandAll
            // 
            this.toolStripExpandAll.Image = ((System.Drawing.Image)(resources.GetObject("toolStripExpandAll.Image")));
            this.toolStripExpandAll.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripExpandAll.Name = "toolStripExpandAll";
            this.toolStripExpandAll.Size = new System.Drawing.Size(85, 22);
            this.toolStripExpandAll.Text = "ExpandAll";
            this.toolStripExpandAll.Click += new System.EventHandler(this.toolStripExpandAll_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // toolStripButtonToolCursor
            // 
            this.toolStripButtonToolCursor.Checked = true;
            this.toolStripButtonToolCursor.CheckState = System.Windows.Forms.CheckState.Checked;
            this.toolStripButtonToolCursor.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonToolCursor.Image")));
            this.toolStripButtonToolCursor.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonToolCursor.Name = "toolStripButtonToolCursor";
            this.toolStripButtonToolCursor.Size = new System.Drawing.Size(67, 22);
            this.toolStripButtonToolCursor.Text = "Cursor";
            this.toolStripButtonToolCursor.Click += new System.EventHandler(this.toolStripButtonToolCursor_Click);
            // 
            // toolStripButtonToolMove
            // 
            this.toolStripButtonToolMove.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonToolMove.Image")));
            this.toolStripButtonToolMove.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonToolMove.Name = "toolStripButtonToolMove";
            this.toolStripButtonToolMove.Size = new System.Drawing.Size(61, 22);
            this.toolStripButtonToolMove.Text = "Move";
            this.toolStripButtonToolMove.Click += new System.EventHandler(this.toolStripButtonToolMove_Click);
            // 
            // toolStripButtonToolPhysics
            // 
            this.toolStripButtonToolPhysics.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonToolPhysics.Image")));
            this.toolStripButtonToolPhysics.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonToolPhysics.Name = "toolStripButtonToolPhysics";
            this.toolStripButtonToolPhysics.Size = new System.Drawing.Size(73, 22);
            this.toolStripButtonToolPhysics.Text = "Physical";
            this.toolStripButtonToolPhysics.Click += new System.EventHandler(this.toolStripButtonToolPhysics_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 25);
            // 
            // toolStripSplitBoxVisible
            // 
            this.toolStripSplitBoxVisible.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.physicalToolStripMenuItem});
            this.toolStripSplitBoxVisible.Image = ((System.Drawing.Image)(resources.GetObject("toolStripSplitBoxVisible.Image")));
            this.toolStripSplitBoxVisible.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripSplitBoxVisible.Name = "toolStripSplitBoxVisible";
            this.toolStripSplitBoxVisible.Size = new System.Drawing.Size(75, 22);
            this.toolStripSplitBoxVisible.Text = "Visible";
            // 
            // physicalToolStripMenuItem
            // 
            this.physicalToolStripMenuItem.Checked = true;
            this.physicalToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.physicalToolStripMenuItem.Name = "physicalToolStripMenuItem";
            this.physicalToolStripMenuItem.Size = new System.Drawing.Size(121, 22);
            this.physicalToolStripMenuItem.Text = "Physical";
            this.physicalToolStripMenuItem.Click += new System.EventHandler(this.physicalToolStripMenuItem_Click);
            // 
            // EditorForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(626, 411);
            this.Controls.Add(this.toolStrip1);
            this.Controls.Add(this.flowLayoutPanel2);
            this.Controls.Add(this.flowLayoutPanel1);
            this.Name = "EditorForm";
            this.Text = "EditorForm";
            this.flowLayoutPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.animationFrames)).EndInit();
            this.flowLayoutPanel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.previewWindow)).EndInit();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.PictureBox animationFrames;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel2;
        private System.Windows.Forms.PictureBox previewWindow;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton toolStripButtonToolCursor;
        private System.Windows.Forms.ToolStripButton toolStripButtonToolMove;
        private System.Windows.Forms.ToolStripButton toolStripCollapseAll;
        private System.Windows.Forms.ToolStripButton toolStripExpandAll;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripButton toolStripButtonToolPhysics;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripDropDownButton toolStripSplitBoxVisible;
        private System.Windows.Forms.ToolStripMenuItem physicalToolStripMenuItem;

    }
}