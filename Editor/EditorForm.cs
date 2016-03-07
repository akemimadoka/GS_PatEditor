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

                    #region init clipboards
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
                    #endregion

                    #region init edit menu visible groups
                    frm._GroupEditPhysical = new VisibleGroup(
                        frm.physicalToolStripMenuItem1,
                        frm.cutPhysicalToolStripMenuItem,
                        frm.copyPhysicalToolStripMenuItem,
                        frm.pastePhysicalToolStripMenuItem,
                        frm.deletePhysicalToolStripMenuItem
                        );
                    frm._GroupEditHit = new VisibleGroup(
                        frm.hitToolStripMenuItem,
                        frm.newHitToolStripMenuItem,
                        frm.cutHitToolStripMenuItem,
                        frm.copyHitToolStripMenuItem,
                        frm.pasteHitToolStripMenuItem,
                        frm.deleteHitToolStripMenuItem
                        );
                    frm._GroupEditAttack = new VisibleGroup(
                        frm.attackToolStripMenuItem2,
                        frm.newAttackToolStripMenuItem,
                        frm.cutAttackToolStripMenuItem,
                        frm.copyAttackToolStripMenuItem,
                        frm.pasteAttackToolStripMenuItem,
                        frm.deleteAttackToolStripMenuItem
                        );
                    #endregion

                    frm._GroupToolAnimationList = new VisibleGroup(
                        frm.toolStripButtonNew,
                        frm.toolStripButtonOpen,
                        frm.toolStripButtonSave,
                        frm.toolStripButtonSaveAs,
                        frm.toolStripButtonExport,
                        frm.toolStripSeparator7,
                        frm.toolStripButtonNewAnimation,
                        frm.toolStripButtonRemoveAnimation,
                        frm.toolStripButtonEditAnimation,
                        frm.toolStripButtonAnimationProperty
                        );
                    frm._GroupToolAnimation = new VisibleGroup(
                        frm.toolStripExpandAll, frm.toolStripCollapseAll,
                        frm.toolStripSeparator1,
                        frm.toolStripButtonToolCursor, frm.toolStripButtonToolMove,
                        frm.toolStripButtonToolPhysics, frm.toolStripButtonToolHit,
                        frm.toolStripButtonToolAttack,
                        frm.toolStripSeparator2,
                        frm.toolStripSplitBoxVisible,
                        frm.toolStripSeparator3,
                        frm.toolStripSplitEdit,
                        frm.toolStripSeparator6,
                        frm.toolStripButtonBack
                    );
                    frm._GroupToolImageList = new VisibleGroup(new ToolStripButton[0]);

                    editor.AnimationListUI.SelectedChange += delegate()
                    {
                        var enabled = editor.AnimationListUI.HasSelected;
                        frm.toolStripButtonRemoveAnimation.Enabled = enabled;
                        frm.toolStripButtonEditAnimation.Enabled = enabled;
                        frm.toolStripButtonAnimationProperty.Enabled = enabled;
                    };

                    editor.UISwitched += delegate()
                    {
                        switch (editor.CurrentUI)
                        {
                            case EditorUI.AnimationList:
                                frm.ChangeActivePanel(0);
                                break;
                            case EditorUI.Animation:
                                frm.ChangeActivePanel(1);
                                frm.ChangeEditMode(FrameEditMode.None);
                                frm.ResetPreviewPosition(1.0f);
                                break;
                        }
                    };
                    editor.ShowAnimationListUI();

                    frm.Show();


                    Application.Run(frm);
                }
            }
        }

        private class VisibleGroup
        {
            private readonly ToolStripItem[] _Items;
            private readonly ToolStripButton[] _Buttons;

            public VisibleGroup(params ToolStripItem[] ctrls)
            {
                _Items = ctrls;
                _Buttons = new ToolStripButton[0];
            }

            public VisibleGroup(params ToolStripButton[] ctrls)
            {
                _Items = new ToolStripItem[0];
                _Buttons = ctrls;
            }

            public bool Visible
            {
                set
                {
                    foreach (var c in _Items)
                    {
                        c.Visible = value;
                    }
                    foreach (var c in _Buttons)
                    {
                        c.Visible = value;
                    }
                }
            }
        }

        private Editor _Editor;
        private ClipboardUIProvider _ClipboardPhysical;
        private ClipboardUIProvider _ClipboardHit;
        private ClipboardUIProvider _ClipboardAttack;

        private VisibleGroup _GroupEditPhysical, _GroupEditHit, _GroupEditAttack;
        private VisibleGroup _GroupToolAnimationList, _GroupToolAnimation, _GroupToolImageList;

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
            _GroupEditPhysical.Visible = false;
            _GroupEditHit.Visible = false;
            _GroupEditAttack.Visible = false;
            switch (mode)
            {
                case FrameEditMode.Physical:
                    _GroupEditPhysical.Visible = true;
                    break;
                case FrameEditMode.Hit:
                    _GroupEditHit.Visible = true;
                    break;
                case FrameEditMode.Attack:
                    _GroupEditAttack.Visible = true;
                    break;
            }
            return _Editor.EditorNode.Animation.Frame.ChangeEditMode(mode);
        }

        private void ChangeActivePanel(int panel)
        {
            switch (panel)
            {
                case 0:
                    _GroupToolAnimationList.Visible = true;
                    _GroupToolAnimation.Visible = false;
                    _GroupToolImageList.Visible = false;
                    panelAnimations.Visible = true;
                    panelAnimationEdit.Visible = false;
                    break;
                case 1:
                    _GroupToolAnimationList.Visible = false;
                    _GroupToolAnimation.Visible = true;
                    _GroupToolImageList.Visible = false;
                    panelAnimations.Visible = false;
                    panelAnimationEdit.Visible = true;
                    break;
            }
        }

        private void ResetPreviewPosition(float scale)
        {
            _Editor.PreviewWindowUI.PreviewMoving.ResetScale(scale);

            var x = previewWindow.Width - panelFramePreviewScroll.ClientSize.Width;
            var y = previewWindow.Height - panelFramePreviewScroll.ClientSize.Height;
            panelFramePreviewScroll.AutoScrollPosition = new Point(x / 2, y / 2);
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

        private void hitToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            hitToolStripMenuItem1.Checked = !hitToolStripMenuItem1.Checked;
            _Editor.EditorNode.Animation.Frame.HitBoxVisible = hitToolStripMenuItem1.Checked;
        }

        private void attackToolStripMenuItem_Click(object sender, EventArgs e)
        {
            attackToolStripMenuItem.Checked = !attackToolStripMenuItem.Checked;
            _Editor.EditorNode.Animation.Frame.AttackBoxVisible = attackToolStripMenuItem.Checked;
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
            _Editor.ShowAnimationListUI();
        }

        private void newHitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _Editor.PreviewWindowUI.HitEditing.New();
        }

        private void newAttackToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _Editor.PreviewWindowUI.AttackEditing.New();
        }

        private void toolStripButtonEditAnimation_Click(object sender, EventArgs e)
        {
            _Editor.AnimationListUI.EditCurrent();
        }

        private void toolStripButtonRemoveAnimation_Click(object sender, EventArgs e)
        {
            _Editor.AnimationListUI.RemoveCurrent();
        }

        private void toolStripButtonNewAnimation_Click(object sender, EventArgs e)
        {
            _Editor.AnimationListUI.AddNew();
        }

        private void toolStripButtonAnimationProperty_Click(object sender, EventArgs e)
        {
            _Editor.AnimationListUI.EditProperty();
        }
    }
}
