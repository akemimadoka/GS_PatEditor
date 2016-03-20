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
                var nodeFilter = new EditableFilterTreeNode(ce.Filter,
                    new DelegateSingleEditable<Pat.Filter>
                    {
                        OnReset = ffilter =>
                        {
                            ce.Filter = ffilter;
                        }
                    });
                var nodeEffect = new EditableEffectTreeNode(ce.Effect,
                    new DelegateSingleEditable<Pat.Effect>
                    {
                        OnReset = eeffect =>
                        {
                            ce.Effect = eeffect;
                        }
                    });
                Nodes.Add(TreeNodeExt.CreateNodeWithChild("Filter",
                    nodeFilter));
                Nodes.Add(TreeNodeExt.CreateNodeWithChild("Effect",
                    nodeEffect));
                if (ce.Filter == null)
                {
                    nodeFilter.Reset();
                }
                if (ce.Effect == null)
                {
                    nodeEffect.Reset();
                }
            }
            else if (Data is Pat.SimpleListEffect)
            {
                var ce = (Pat.SimpleListEffect)Data;
                var dest = new ListMultiEditable<Pat.Effect>
                {
                    List = ce.EffectList.Effects,
                };
                foreach (var e in ce.EffectList)
                {
                    var nodeEffect = new EditableEffectTreeNode(e, dest);
                    Nodes.Add(nodeEffect);
                }
                Nodes.Add(new EditableEffectTreeNode(dest));
            }
            else if (Data is Pat.Effects.CreateBulletEffect)
            {
                var ce = (Pat.Effects.CreateBulletEffect)Data;
                var nodePP = new EditablePointProviderTreeNode(ce.Position,
                    new DelegateSingleEditable<Pat.PointProvider>
                    {
                        OnReset = epp =>
                        {
                            ce.Position = epp;
                        }
                    });
                Nodes.Add(TreeNodeExt.CreateNodeWithChild("Position", nodePP));
                if (ce.Position == null)
                {
                    nodePP.Reset();
                }
            }
        }
    }
}
