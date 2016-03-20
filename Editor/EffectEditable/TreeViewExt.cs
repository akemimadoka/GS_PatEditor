using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GS_PatEditor.Editor.EffectEditable
{
    static class TreeNodeExt
    {
        private static void InvokeSelectedChange(TreeView tv)
        {
            //TODO better solution?
            var action = tv.Tag as Action;
            if (action != null)
            {
                action();
            }
        }

        public static TreeNodeCollection GetParentCollection(this TreeNode node)
        {
            if (node.Parent == null)
            {
                return node.TreeView.Nodes;
            }
            return node.Parent.Nodes;
        }

        public static void Replace(this TreeNode node, TreeNode newNode)
        {
            var shouldSetSelected = node.IsSelected;

            var coll = node.GetParentCollection();
            var index = coll.IndexOf(node);
            if (index == -1)
            {
                return;
            }
            coll.RemoveAt(index);
            coll.Insert(index, newNode);

            if (shouldSetSelected)
            {
                newNode.TreeView.SelectedNode = newNode;
                InvokeSelectedChange(newNode.TreeView);
            }
        }

        public static void RemoveFromParent(this TreeNode node)
        {
            node.GetParentCollection().Remove(node);
        }

        public static void InsertBefore(this TreeNode node, TreeNode newNode)
        {
            var coll = node.GetParentCollection();
            var index = coll.IndexOf(node);
            if (index == -1)
            {
                return;
            }
            coll.Insert(index, newNode);
        }

        public static TreeNode CreateNodeWithChild(string text, TreeNode child)
        {
            var node = new TreeNode { Text = text };
            node.Nodes.Add(child);
            return node;
        }

        public static TreeNode CreateNode(Pat.Filter filter)
        {
            return new TreeNode()
            {
                Text = filter == null ? "<null>" : filter.GetType().Name,
                Tag = filter
            };
        }
    }
}
