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
                SetLinkedStatus(nnode.CanDelete, nnode.CanReset, node.Tag);
            }
            else
            {
                SetLinkedStatus(false, false, node != null ? node.Tag : null);
            }
        }

        private void SetLinkedStatus(bool bd, bool br, object p)
        {
            if (LinkedDeleteButton != null)
            {
                LinkedDeleteButton.Enabled = bd;
            }
            if (LinkedResetButton != null)
            {
                LinkedResetButton.Enabled = br;
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
    }
}
