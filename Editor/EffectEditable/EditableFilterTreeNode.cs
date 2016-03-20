using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GS_PatEditor.Editor.EffectEditable
{
    class EditableFilterTreeNode : EditableTreeNode<Pat.Filter>
    {
        public EditableFilterTreeNode(Pat.Filter data, Editable<Pat.Filter> dest)
            : base(data, dest)
        {
        }

        public EditableFilterTreeNode(MultiEditable<Pat.Filter> dest)
            : base(dest)
        {
        }

        protected override Pat.Filter CreateSelectObject(MultiEditable<Pat.Filter> dest)
        {
            return new SelectFilter(delegate(Pat.Filter filter)
            {
                dest.Append(filter);
                this.InsertBefore(new EditableFilterTreeNode(filter, dest));
            });
        }

        protected override TreeNode CreateSingleEditableNode(SingleEditable<Pat.Filter> dest)
        {
            EditableFilterTreeNode ret = null;
            ret = new EditableFilterTreeNode(new SelectFilter(delegate(Pat.Filter filter)
            {
                dest.Reset(filter);
                ret.Replace(new EditableFilterTreeNode(filter, dest));
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
            else if (Data is SelectFilter)
            {
                Text = "<select>";
            }
            else
            {
                Text = Data.GetType().Name;
            }

            if (Data is Pat.SimpleListFilter)
            {
                var cf = (Pat.SimpleListFilter)Data;
                var dest = new ListMultiEditable<Pat.Filter>
                {
                    List = cf.FilterList.Filters,
                };
                foreach (var f in cf.FilterList)
                {
                    var nodeFilter = new EditableFilterTreeNode(f, dest);
                    Nodes.Add(nodeFilter);
                }
                Nodes.Add(new EditableFilterTreeNode(dest));
            }
        }
    }
}
