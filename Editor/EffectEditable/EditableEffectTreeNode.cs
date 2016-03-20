using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GS_PatEditor.Editor.EffectEditable
{
    class EditableEffectTreeNode : EditableTreeNode<Pat.Effect>
    {
        public EditableEffectTreeNode(Pat.Effect data, Editable<Pat.Effect> dest)
            : base(data, dest)
        {
        }

        public EditableEffectTreeNode(MultiEditable<Pat.Effect> dest)
            : base(dest)
        {
        }

        protected override Pat.Effect CreateSelectObject(MultiEditable<Pat.Effect> dest)
        {
            return new SelectEffect(delegate(Pat.Effect effect)
            {
                dest.Append(effect);
                this.InsertBefore(new EditableEffectTreeNode(effect, dest));
            });
        }

        protected override void SetupCommon()
        {
            Tag = Data;
            if (Data == null)
            {
                Text = "<null>";
            }
            else if (Data is SelectEffect)
            {
                Text = "<select>";
            }
            else
            {
                Text = Data.GetType().Name;
            }

            if (Data is Pat.FilteredEffect)
            {
                var ce = (Pat.FilteredEffect)Data;
                var nodeEffect = new EditableEffectTreeNode(ce.Effect,
                    new DelegateSingleEditable<Pat.Effect>
                    {
                        OnReset = delegate(Pat.Effect eeffect)
                        {
                            ce.Effect = eeffect;
                        }
                    });
                Nodes.Add(TreeNodeExt.CreateNodeWithChild("Filter",
                    TreeNodeExt.CreateNode(ce.Filter)));
                Nodes.Add(TreeNodeExt.CreateNodeWithChild("Effect", nodeEffect));
                if (ce.Effect == null)
                {
                    (nodeEffect as EditableEffectTreeNode).Reset();
                }
            }
        }

        protected override TreeNode CreateSingleEditableNode(SingleEditable<Pat.Effect> dest)
        {
            EditableEffectTreeNode ret = null;
            ret = new EditableEffectTreeNode(new SelectEffect(delegate(Pat.Effect effect)
            {
                dest.Reset(effect);
                ret.Replace(new EditableEffectTreeNode(effect, dest));
            }), null);
            return ret;
        }
    }
}
