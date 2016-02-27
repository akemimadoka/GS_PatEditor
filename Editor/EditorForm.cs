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

                    frm.timer1.Tick += delegate(object sender, EventArgs e)
                    {
                        editor.PreviewWindowUI.Refresh();
                    };

                    frm.Show();
                    {
                        var x = frm.flowLayoutPanel2.ClientSize.Width - 800;
                        var y = frm.flowLayoutPanel2.ClientSize.Height - 600;
                        frm.flowLayoutPanel2.AutoScrollPosition = new Point(-x / 2, -y / 2);
                    }

                    Application.Run(frm);
                }
            }
        }

        private Editor _Editor;

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

        private bool ChangeEditMode(FrameNode.FrameEditMode mode)
        {
            return _Editor.EditorNode.Animation.Frame.ChangeEditMode(mode);
        }

        private void ClearToolButtonsToolChecked()
        {
            toolStripButtonToolCursor.CheckState = CheckState.Unchecked;
            toolStripButtonToolMove.CheckState = CheckState.Unchecked;
            toolStripButtonToolPhysics.CheckState = CheckState.Unchecked;
        }

        private void toolStripButtonToolCursor_Click(object sender, EventArgs e)
        {
            if (ChangeEditMode(FrameNode.FrameEditMode.None))
            {
                ClearToolButtonsToolChecked();
                toolStripButtonToolCursor.CheckState = CheckState.Checked;
            }
        }

        private void toolStripButtonToolMove_Click(object sender, EventArgs e)
        {
            if (ChangeEditMode(FrameNode.FrameEditMode.Move))
            {
                ClearToolButtonsToolChecked();
                toolStripButtonToolMove.CheckState = CheckState.Checked;
            }
        }

        private void toolStripButtonToolPhysics_Click(object sender, EventArgs e)
        {
            if (ChangeEditMode(FrameNode.FrameEditMode.Physical))
            {
                ClearToolButtonsToolChecked();
                toolStripButtonToolPhysics.CheckState = CheckState.Checked;
            }
        }

        private void physicalToolStripMenuItem_Click(object sender, EventArgs e)
        {
            physicalToolStripMenuItem.Checked = !physicalToolStripMenuItem.Checked;
            //switch
        }
    }
}
