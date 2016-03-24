using GS_PatEditor.Pat.Effects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GS_PatEditor.Editor.Editable
{
    class EditableTreeView : TreeView
    {
        public PropertyGrid LinkedPropertyGrid { get; set; }

        private Button _LinkedResetButton;
        public Button LinkedResetButton
        {
            get
            {
                return _LinkedResetButton;
            }
            set
            {
                if (_LinkedResetButton == value)
                {
                    return;
                }
                if (_LinkedResetButton != null)
                {
                    _LinkedResetButton.Click -= ResetButton_Click;
                }
                if (value != null)
                {
                    value.Click += ResetButton_Click;
                }
                _LinkedResetButton = value;
            }
        }

        private Button _LinkedDeleteButton;
        public Button LinkedDeleteButton
        {
            get
            {
                return _LinkedDeleteButton;
            }
            set
            {
                if (_LinkedDeleteButton == value)
                {
                    return;
                }
                if (_LinkedDeleteButton != null)
                {
                    _LinkedDeleteButton.Click -= DeleteButton_Click;
                }
                if (value != null)
                {
                    value.Click += DeleteButton_Click;
                }
                _LinkedDeleteButton = value;
            }
        }

        private Button _LinkedMoveUpButton;
        public Button LinkedMoveUpButton
        {
            get
            {
                return _LinkedMoveUpButton;
            }
            set
            {
                if (_LinkedMoveUpButton == value)
                {
                    return;
                }
                if (_LinkedMoveUpButton != null)
                {
                    _LinkedMoveUpButton.Click -= MoveUpButton_Click;
                }
                if (value != null)
                {
                    value.Click += MoveUpButton_Click;
                }
                _LinkedMoveUpButton = value;
            }
        }

        private Button _LinkedMoveDownButton;
        public Button LinkedMoveDownButton
        {
            get
            {
                return _LinkedMoveDownButton;
            }
            set
            {
                if (_LinkedMoveDownButton == value)
                {
                    return;
                }
                if (_LinkedMoveDownButton != null)
                {
                    _LinkedMoveDownButton.Click -= MoveDownButton_Click;
                }
                if (value != null)
                {
                    value.Click += MoveDownButton_Click;
                }
                _LinkedMoveDownButton = value;
            }
        }

        public EditableTreeView()
        {
            this.NodeMouseClick += EditableTreeView_NodeMouseClick;
        }

        private void EditableTreeView_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            SetSelectedNodeWithCallback(e.Node);
        }

        public void SetSelectedNodeWithCallback(TreeNode node)
        {
            SelectedNode = node;

            if (node != null && node is IEditableTreeNode)
            {
                var nnode = node as IEditableTreeNode;
                SetLinkedStatus(nnode.CanDelete, nnode.CanReset, nnode.CanMoveDown, nnode.CanMoveUp, node.Tag);
            }
            else
            {
                SetLinkedStatus(false, false, false, false, node != null ? node.Tag : null);
            }
        }

        private void SetLinkedStatus(bool bd, bool br, bool bdown, bool bup, object p)
        {
            if (LinkedDeleteButton != null)
            {
                LinkedDeleteButton.Enabled = bd;
            }
            if (LinkedResetButton != null)
            {
                LinkedResetButton.Enabled = br;
            }
            if (LinkedMoveDownButton != null)
            {
                LinkedMoveDownButton.Enabled = bdown;
            }
            if (LinkedMoveUpButton != null)
            {
                LinkedMoveUpButton.Enabled = bup;
            }
            if (LinkedPropertyGrid != null)
            {
                LinkedPropertyGrid.SelectedObject = p;
            }
        }

        private void ResetButton_Click(object sender, EventArgs e)
        {
            var node = SelectedNode as IEditableTreeNode;
            if (node != null)
            {
                node.Reset();
            }
        }

        private void DeleteButton_Click(object sender, EventArgs e)
        {
            var node = SelectedNode as IEditableTreeNode;
            if (node != null)
            {
                node.Delete();
            }
        }

        private void MoveUpButton_Click(object sender, EventArgs e)
        {
            var node = SelectedNode as IEditableTreeNode;
            if (node != null)
            {
                node.MoveUp();
                SetSelectedNodeWithCallback((TreeNode)node);
            }
        }

        private void MoveDownButton_Click(object sender, EventArgs e)
        {
            var node = SelectedNode as IEditableTreeNode;
            if (node != null)
            {
                node.MoveDown();
                SetSelectedNodeWithCallback((TreeNode)node);
            }
        }
    }
}
