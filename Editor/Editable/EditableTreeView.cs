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
        public Button LinkedResetButton { get; set; }
        public Button LinkedDeleteButton { get; set; }
        
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
                SetLinkedStatus(false, false, null);
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

        public void ResetList<T>(EditableEnvironment env, List<T> list)
            where T : class
        {
            Nodes.Clear();
            var me = new ListMultiEditable<T> { List = list };
            foreach (var effect in list)
            {
                Nodes.Add(EditableNodeGenerator.Create<T>(env, effect, me));
            }
            Nodes.Add(EditableNodeGenerator.Create<T>(env, me));
        }
    }
}
