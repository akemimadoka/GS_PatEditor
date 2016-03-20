using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GS_PatEditor.Editor.EffectEditable
{
    abstract class EditableTreeNode<T> : TreeNode
        where T : class
    {
        protected readonly T Data;
        protected readonly Editable<T> Dest;

        protected EditableTreeNode(T data, Editable<T> dest)
        {
            Data = data;
            Dest = dest;

            SetupCommon();
        }

        protected EditableTreeNode(MultiEditable<T> dest)
        {
            Data = CreateSelectObject(dest);
            Dest = null;

            SetupCommon();
        }

        protected abstract T CreateSelectObject(MultiEditable<T> dest);
        protected abstract TreeNode CreateSingleEditableNode(SingleEditable<T> dest);

        protected abstract void SetupCommon();

        public void Reset()
        {
            if (Dest == null)
            {
                return;
            }
            if (Dest is SingleEditable<T>)
            {
                var ddest = (SingleEditable<T>)Dest;
                ddest.Reset(null);
                TreeNode newNode = CreateSingleEditableNode(ddest);
                this.Replace(newNode);
            }
        }
        public bool CanReset { get { return Dest != null && Dest is SingleEditable<Pat.Effect>; } }

        public void Delete()
        {
            if (Dest == null)
            {
                return;
            }
            if (Dest is MultiEditable<T>)
            {
                var ddest = (MultiEditable<T>)Dest;
                ddest.Remove(Data);
                this.RemoveFromParent();
            }
        }
        public bool CanDelete { get { return Dest != null && Dest is MultiEditable<Pat.Effect>; } }
    }
}
