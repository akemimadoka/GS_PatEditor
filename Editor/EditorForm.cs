using GS_PatEditor.Editor.Nodes;
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
                    frm._ClipboardHit = new ClipboardUIProvider(editor.PreviewWindowUI.HitEditing)
                    {
                        Cut = new ClipboardUIElementToolstripItem(frm.cutHitToolStripMenuItem),
                        Copy = new ClipboardUIElementToolstripItem(frm.copyHitToolStripMenuItem),
                        Paste = new ClipboardUIElementToolstripItem(frm.pasteHitToolStripMenuItem),
                        Delete = new ClipboardUIElementToolstripItem(frm.deleteHitToolStripMenuItem),
                    };
                    frm._ClipboardAttack = new ClipboardUIProvider(editor.PreviewWindowUI.AttackEditing)
                    {
                        Cut = new ClipboardUIElementToolstripItem(frm.cutAttackToolStripMenuItem),
                        Copy = new ClipboardUIElementToolstripItem(frm.copyAttackToolStripMenuItem),
                        Paste = new ClipboardUIElementToolstripItem(frm.pasteAttackToolStripMenuItem),
                        Delete = new ClipboardUIElementToolstripItem(frm.deleteAttackToolStripMenuItem),
                    };

                    frm.Show();
                    frm.ResetPreviewPosition(1.0f);

                    Application.Run(frm);
                }
            }
        }

        private Editor _Editor;
        private ClipboardUIProvider _ClipboardPhysical;
        private ClipboardUIProvider _ClipboardHit;
        private ClipboardUIProvider _ClipboardAttack;

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

        private void ResetPreviewPosition(float scale)
        {
            _Editor.PreviewWindowUI.PreviewMoving.ResetScale(scale);

            var x = panelFramePreviewScroll.ClientSize.Width - 800;
            var y = panelFramePreviewScroll.ClientSize.Height - 600;
            panelFramePreviewScroll.AutoScrollPosition = new Point(-x / 2, -y / 2);
        }

        private void ClearToolButtonsToolChecked()
        {
            toolStripButtonToolCursor.CheckState = CheckState.Unchecked;
            toolStripButtonToolMove.CheckState = CheckState.Unchecked;
            toolStripButtonToolPhysics.CheckState = CheckState.Unchecked;
            toolStripButtonToolHit.CheckState = CheckState.Unchecked;
            toolStripButtonToolAttack.CheckState = CheckState.Unchecked;
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

        private void toolStripButtonToolHit_Click(object sender, EventArgs e)
        {
            if (ChangeEditMode(FrameEditMode.Hit))
            {
                ClearToolButtonsToolChecked();
                toolStripButtonToolHit.CheckState = CheckState.Checked;
            }
        }

        private void toolStripButtonToolAttack_Click(object sender, EventArgs e)
        {
            if (ChangeEditMode(FrameEditMode.Attack))
            {
                ClearToolButtonsToolChecked();
                toolStripButtonToolAttack.CheckState = CheckState.Checked;
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
            _ClipboardHit.UpdateEnable();
            _ClipboardAttack.UpdateEnable();
        }

        private void resetScaleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ResetPreviewPosition(1.0f);
        }

        private void scale200ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ResetPreviewPosition(2.0f);
        }

        private void scale300ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ResetPreviewPosition(3.0f);
        }

        private void toolStripButtonBack_Click(object sender, EventArgs e)
        {
            panelAnimations.Visible = !panelAnimations.Visible;
        }

        private void newHitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _Editor.PreviewWindowUI.HitEditing.New();
        }

        private void newAttackToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _Editor.PreviewWindowUI.AttackEditing.New();
        }
    }
}
