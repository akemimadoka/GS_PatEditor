using GS_PatEditor.Pat.Effects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GS_PatEditor.Editor.Editable
{
    interface IEditableTreeNode
    {
        void Reset();
        bool CanReset { get; }
        void Delete();
        bool CanDelete { get; }
    }

    abstract class EditableTreeNode<T> : TreeNode, IEditableTreeNode
        where T : class
    {
        protected readonly EditableEnvironment Env;
        protected readonly T Data;
        protected readonly Editable<T> Dest;

        protected EditableTreeNode(EditableEnvironment env, T data, Editable<T> dest)
        {
            Env = env;
            Data = data;
            Dest = dest;

            SetupCommon();
        }

        protected EditableTreeNode(EditableEnvironment env, MultiEditable<T> dest)
        {
            Env = env;
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
        public bool CanReset { get { return Dest != null && Dest is SingleEditable<T>; } }

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
        public bool CanDelete { get { return Dest != null && Dest is MultiEditable<T>; } }
    }
}
