using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GS_PatEditor.Editor.EffectEditable
{
    class EditablePointProviderTreeNode : EditableTreeNode<Pat.PointProvider>
    {
        public EditablePointProviderTreeNode(Pat.PointProvider data, Editable<Pat.PointProvider> dest)
            : base(data, dest)
        {
        }

        public EditablePointProviderTreeNode(MultiEditable<Pat.PointProvider> dest)
            : base(dest)
        {
        }

        protected override Pat.PointProvider CreateSelectObject(MultiEditable<Pat.PointProvider> dest)
        {
            return new SelectPointProvider(delegate(Pat.PointProvider pp)
            {
                dest.Append(pp);
                this.InsertBefore(new EditablePointProviderTreeNode(pp, dest));
            });
        }

        protected override TreeNode CreateSingleEditableNode(SingleEditable<Pat.PointProvider> dest)
        {
            EditablePointProviderTreeNode ret = null;
            ret = new EditablePointProviderTreeNode(new SelectPointProvider(delegate(Pat.PointProvider pp)
            {
                dest.Reset(pp);
                ret.Replace(new EditablePointProviderTreeNode(pp, dest));
            }), null);
            return ret;
        }

        protected override void SetupCommon()
        {
            Tag = Data;
            if (Data == null)
            {
                Text = "<null>";
            }
            else if (Data is SelectPointProvider)
            {
                Text = "<select>";
            }
            else
            {
                Text = Data.GetType().Name;
            }
        }
    }
}
