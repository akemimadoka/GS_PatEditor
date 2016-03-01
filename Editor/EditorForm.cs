﻿using GS_PatEditor.Editor.Nodes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GS_PatEditor.Editor
{
    public partial class EditorForm : Form
    {
        public static void ShowEditorForm(Pat.Project proj)
        {
            using (var frm = new EditorForm())
            {
                using (var editor = new Editor(proj))
                {
                    frm._Editor = editor;

                    editor.AnimationFramesUI.Init(frm.animationFrames);
                    editor.PreviewWindowUI.Init(frm.previewWindow);
                    editor.AnimationListUI.Init(frm.animations);

                    frm.timer1.Tick += delegate(object sender, EventArgs e)
                    {
                        editor.PreviewWindowUI.Refresh();
                    };
                    frm._ClipboardPhysical = new ClipboardUIProvider(editor.PreviewWindowUI.PhysicalEditing)
                    {
                        Cut = new ClipboardUIElementToolstripItem(frm.cutPhysicalToolStripMenuItem),
                        Copy = new ClipboardUIElementToolstripItem(frm.copyPhysicalToolStripMenuItem),
                        Paste = new ClipboardUIElementToolstripItem(frm.pastePhysicalToolStripMenuItem),
                        Delete = new ClipboardUIElementToolstripItem(frm.deletePhysicalToolStripMenuItem),
                    };

                    frm.Show();
                    frm.ResetPreviewPosition();

                    Application.Run(frm);
                }
            }
        }

        private Editor _Editor;
        private ClipboardUIProvider _ClipboardPhysical;

        public EditorForm()
        {
            InitializeComponent();
        }

        private void toolStripCollapseAll_Click(object sender, EventArgs e)
        {
            _Editor.AnimationFramesUI.CollapseAll();
        }

        private void toolStripExpandAll_Click(object sender, EventArgs e)
        {
            _Editor.AnimationFramesUI.ExpandAll();
        }

        private bool ChangeEditMode(FrameEditMode mode)
        {
            return _Editor.EditorNode.Animation.Frame.ChangeEditMode(mode);
        }

        private void ResetPreviewPosition()
        {
            _Editor.PreviewWindowUI.PreviewMoving.ResetScale();

            var x = panelFramePreviewScroll.ClientSize.Width - 800;
            var y = panelFramePreviewScroll.ClientSize.Height - 600;
            panelFramePreviewScroll.AutoScrollPosition = new Point(-x / 2, -y / 2);
        }

        private void ClearToolButtonsToolChecked()
        {
            toolStripButtonToolCursor.CheckState = CheckState.Unchecked;
            toolStripButtonToolMove.CheckState = CheckState.Unchecked;
            toolStripButtonToolPhysics.CheckState = CheckState.Unchecked;
        }

        private void toolStripButtonToolCursor_Click(object sender, EventArgs e)
        {
            if (ChangeEditMode(FrameEditMode.None))
            {
                ClearToolButtonsToolChecked();
                toolStripButtonToolCursor.CheckState = CheckState.Checked;
            }
        }

        private void toolStripButtonToolMove_Click(object sender, EventArgs e)
        {
            if (ChangeEditMode(FrameEditMode.Move))
            {
                ClearToolButtonsToolChecked();
                toolStripButtonToolMove.CheckState = CheckState.Checked;
            }
        }

        private void toolStripButtonToolPhysics_Click(object sender, EventArgs e)
        {
            if (ChangeEditMode(FrameEditMode.Physical))
            {
                ClearToolButtonsToolChecked();
                toolStripButtonToolPhysics.CheckState = CheckState.Checked;
            }
        }

        private void physicalToolStripMenuItem_Click(object sender, EventArgs e)
        {
            physicalToolStripMenuItem.Checked = !physicalToolStripMenuItem.Checked;
            _Editor.EditorNode.Animation.Frame.PhysicalBoxVisible = physicalToolStripMenuItem.Checked;
        }

        private void axisToolStripMenuItem_Click(object sender, EventArgs e)
        {
            axisToolStripMenuItem.Checked = !axisToolStripMenuItem.Checked;
            _Editor.EditorNode.Animation.Frame.AxisVisible = axisToolStripMenuItem.Checked;
        }

        private void toolStripSplitEdit_DropDownOpening(object sender, EventArgs e)
        {
            _ClipboardPhysical.UpdateEnable();
        }

        private void resetScaleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ResetPreviewPosition();
        }

        private void toolStripButtonBack_Click(object sender, EventArgs e)
        {
            panelAnimations.Visible = !panelAnimations.Visible;
        }
    }
}
