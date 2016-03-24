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
        public event EventHandler SelectedNodeChanged;

        public void SetSelectedNodeWithCallback(TreeNode node)
        {
            SelectedNode = node;
            if (SelectedNodeChanged != null)
            {
                SelectedNodeChanged(this, EventArgs.Empty);
            }
        }
    }
}
