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
            this.flowLayoutPanelFrameList = new System.Windows.Forms.FlowLayoutPanel();
            this.animationFrames = new System.Windows.Forms.PictureBox();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.flowLayoutPanelFramePreview = new System.Windows.Forms.FlowLayoutPanel();
            this.previewWindow = new System.Windows.Forms.PictureBox();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toolStripExpandAll = new System.Windows.Forms.ToolStripButton();
            this.toolStripCollapseAll = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripButtonToolCursor = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonToolMove = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonToolPhysics = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripSplitBoxVisible = new System.Windows.Forms.ToolStripDropDownButton();
            this.axisToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.physicalToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripSplitEdit = new System.Windows.Forms.ToolStripDropDownButton();
            this.physicalToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.cutPhysicalToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.copyPhysicalToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.pastePhysicalToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.deletePhysicalToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.resetScaleToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.flowLayoutPanelAnimationList = new System.Windows.Forms.FlowLayoutPanel();
            this.toolStripButtonBack = new System.Windows.Forms.ToolStripButton();
            this.flowLayoutPanelFrameList.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.animationFrames)).BeginInit();
            this.flowLayoutPanelFramePreview.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.previewWindow)).BeginInit();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // flowLayoutPanelFrameList
            // 
            this.flowLayoutPanelFrameList.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.flowLayoutPanelFrameList.AutoScroll = true;
            this.flowLayoutPanelFrameList.BackColor = System.Drawing.Color.White;
            this.flowLayoutPanelFrameList.Controls.Add(this.animationFrames);
            this.flowLayoutPanelFrameList.Location = new System.Drawing.Point(0, 28);
            this.flowLayoutPanelFrameList.Name = "flowLayoutPanelFrameList";
            this.flowLayoutPanelFrameList.Size = new System.Drawing.Size(612, 100);
            this.flowLayoutPanelFrameList.TabIndex = 1;
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
            // flowLayoutPanelFramePreview
            // 
            this.flowLayoutPanelFramePreview.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.flowLayoutPanelFramePreview.AutoScroll = true;
            this.flowLayoutPanelFramePreview.Controls.Add(this.previewWindow);
            this.flowLayoutPanelFramePreview.Location = new System.Drawing.Point(0, 136);
            this.flowLayoutPanelFramePreview.Margin = new System.Windows.Forms.Padding(0);
            this.flowLayoutPanelFramePreview.Name = "flowLayoutPanelFramePreview";
            this.flowLayoutPanelFramePreview.Size = new System.Drawing.Size(612, 245);
            this.flowLayoutPanelFramePreview.TabIndex = 3;
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
            this.toolStripExpandAll,
            this.toolStripCollapseAll,
            this.toolStripSeparator1,
            this.toolStripButtonToolCursor,
            this.toolStripButtonToolMove,
            this.toolStripButtonToolPhysics,
            this.toolStripSeparator2,
            this.toolStripSplitBoxVisible,
            this.toolStripSeparator3,
            this.toolStripSplitEdit,
            this.toolStripButtonBack});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.RenderMode = System.Windows.Forms.ToolStripRenderMode.Professional;
            this.toolStrip1.Size = new System.Drawing.Size(611, 25);
            this.toolStrip1.TabIndex = 4;
            this.toolStrip1.Text = "toolStrip1";
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
            // toolStripCollapseAll
            // 
            this.toolStripCollapseAll.Image = ((System.Drawing.Image)(resources.GetObject("toolStripCollapseAll.Image")));
            this.toolStripCollapseAll.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripCollapseAll.Name = "toolStripCollapseAll";
            this.toolStripCollapseAll.Size = new System.Drawing.Size(92, 22);
            this.toolStripCollapseAll.Text = "CollapseAll";
            this.toolStripCollapseAll.Click += new System.EventHandler(this.toolStripCollapseAll_Click);
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
            this.resetScaleToolStripMenuItem,
            this.toolStripMenuItem1,
            this.axisToolStripMenuItem,
            this.physicalToolStripMenuItem});
            this.toolStripSplitBoxVisible.Image = ((System.Drawing.Image)(resources.GetObject("toolStripSplitBoxVisible.Image")));
            this.toolStripSplitBoxVisible.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripSplitBoxVisible.Name = "toolStripSplitBoxVisible";
            this.toolStripSplitBoxVisible.Size = new System.Drawing.Size(75, 22);
            this.toolStripSplitBoxVisible.Text = "Visible";
            // 
            // axisToolStripMenuItem
            // 
            this.axisToolStripMenuItem.Checked = true;
            this.axisToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.axisToolStripMenuItem.Name = "axisToolStripMenuItem";
            this.axisToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.axisToolStripMenuItem.Text = "Axis";
            this.axisToolStripMenuItem.Click += new System.EventHandler(this.axisToolStripMenuItem_Click);
            // 
            // physicalToolStripMenuItem
            // 
            this.physicalToolStripMenuItem.Checked = true;
            this.physicalToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.physicalToolStripMenuItem.Name = "physicalToolStripMenuItem";
            this.physicalToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.physicalToolStripMenuItem.Text = "Physical";
            this.physicalToolStripMenuItem.Click += new System.EventHandler(this.physicalToolStripMenuItem_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(6, 25);
            // 
            // toolStripSplitEdit
            // 
            this.toolStripSplitEdit.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.physicalToolStripMenuItem1,
            this.cutPhysicalToolStripMenuItem,
            this.copyPhysicalToolStripMenuItem,
            this.pastePhysicalToolStripMenuItem,
            this.deletePhysicalToolStripMenuItem});
            this.toolStripSplitEdit.Image = ((System.Drawing.Image)(resources.GetObject("toolStripSplitEdit.Image")));
            this.toolStripSplitEdit.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripSplitEdit.Name = "toolStripSplitEdit";
            this.toolStripSplitEdit.Size = new System.Drawing.Size(59, 22);
            this.toolStripSplitEdit.Text = "Edit";
            this.toolStripSplitEdit.DropDownOpening += new System.EventHandler(this.toolStripSplitEdit_DropDownOpening);
            // 
            // physicalToolStripMenuItem1
            // 
            this.physicalToolStripMenuItem1.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.physicalToolStripMenuItem1.Enabled = false;
            this.physicalToolStripMenuItem1.Name = "physicalToolStripMenuItem1";
            this.physicalToolStripMenuItem1.Size = new System.Drawing.Size(152, 22);
            this.physicalToolStripMenuItem1.Text = "Physical";
            // 
            // cutPhysicalToolStripMenuItem
            // 
            this.cutPhysicalToolStripMenuItem.Name = "cutPhysicalToolStripMenuItem";
            this.cutPhysicalToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.cutPhysicalToolStripMenuItem.Text = "Cut";
            // 
            // copyPhysicalToolStripMenuItem
            // 
            this.copyPhysicalToolStripMenuItem.Name = "copyPhysicalToolStripMenuItem";
            this.copyPhysicalToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.copyPhysicalToolStripMenuItem.Text = "Copy";
            // 
            // pastePhysicalToolStripMenuItem
            // 
            this.pastePhysicalToolStripMenuItem.Name = "pastePhysicalToolStripMenuItem";
            this.pastePhysicalToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.pastePhysicalToolStripMenuItem.Text = "Paste";
            // 
            // deletePhysicalToolStripMenuItem
            // 
            this.deletePhysicalToolStripMenuItem.Name = "deletePhysicalToolStripMenuItem";
            this.deletePhysicalToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.deletePhysicalToolStripMenuItem.Text = "Delete";
            // 
            // resetScaleToolStripMenuItem
            // 
            this.resetScaleToolStripMenuItem.Name = "resetScaleToolStripMenuItem";
            this.resetScaleToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.resetScaleToolStripMenuItem.Text = "Reset scale";
            this.resetScaleToolStripMenuItem.Click += new System.EventHandler(this.resetScaleToolStripMenuItem_Click);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(149, 6);
            // 
            // flowLayoutPanelAnimationList
            // 
            this.flowLayoutPanelAnimationList.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.flowLayoutPanelAnimationList.Location = new System.Drawing.Point(0, 28);
            this.flowLayoutPanelAnimationList.Margin = new System.Windows.Forms.Padding(0);
            this.flowLayoutPanelAnimationList.Name = "flowLayoutPanelAnimationList";
            this.flowLayoutPanelAnimationList.Size = new System.Drawing.Size(612, 353);
            this.flowLayoutPanelAnimationList.TabIndex = 5;
            this.flowLayoutPanelAnimationList.Visible = false;
            // 
            // toolStripButtonBack
            // 
            this.toolStripButtonBack.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonBack.Image")));
            this.toolStripButtonBack.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonBack.Name = "toolStripButtonBack";
            this.toolStripButtonBack.Size = new System.Drawing.Size(56, 22);
            this.toolStripButtonBack.Text = "Back";
            // 
            // EditorForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(611, 382);
            this.Controls.Add(this.flowLayoutPanelAnimationList);
            this.Controls.Add(this.toolStrip1);
            this.Controls.Add(this.flowLayoutPanelFramePreview);
            this.Controls.Add(this.flowLayoutPanelFrameList);
            this.Name = "EditorForm";
            this.Text = "AnimationEditor";
            this.flowLayoutPanelFrameList.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.animationFrames)).EndInit();
            this.flowLayoutPanelFramePreview.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.previewWindow)).EndInit();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanelFrameList;
        private System.Windows.Forms.PictureBox animationFrames;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanelFramePreview;
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
        private System.Windows.Forms.ToolStripMenuItem axisToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripDropDownButton toolStripSplitEdit;
        private System.Windows.Forms.ToolStripMenuItem physicalToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem cutPhysicalToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem copyPhysicalToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem pastePhysicalToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem deletePhysicalToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem resetScaleToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanelAnimationList;
        private System.Windows.Forms.ToolStripButton toolStripButtonBack;

    }
}