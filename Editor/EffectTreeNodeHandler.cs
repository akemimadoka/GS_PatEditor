using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GS_PatEditor.Editor
{
    class EditableTreeNode : TreeNode
    {
        private readonly Pat.Effect Data;
        private readonly Editable<Pat.Effect> Dest;

        public EditableTreeNode(Pat.Effect data, Editable<Pat.Effect> dest)
        {
            Data = data;
            Dest = dest;

            SetupCommon();
        }

        public EditableTreeNode(MultiEditable<Pat.Effect> dest)
        {
            var data = new SelectEffect(delegate(Pat.Effect effect)
            {
                dest.Append(effect);
                this.InsertBefore(new EditableTreeNode(effect, dest));
            });
            Data = data;
            Dest = null;

            SetupCommon();
        }

        private void SetupCommon()
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
                var nodeEffect = new EditableTreeNode(ce.Effect,
                    new DelegateSingleEditable<Pat.Effect>
                    {
                        OnReset = delegate(Pat.Effect eeffect)
                        {
                            ce.Effect = eeffect;
                        }
                    });
                Nodes.Add(EffectTreeNodeHandler.CreateNodeWithChild("Filter",
                    EffectTreeNodeHandler.CreateNode(ce.Filter)));
                Nodes.Add(EffectTreeNodeHandler.CreateNodeWithChild("Effect", nodeEffect));
                if (ce.Effect == null)
                {
                    (nodeEffect as EditableTreeNode).Reset();
                }
            }
        }

        public void Reset()
        {
            if (Dest == null)
            {
                return;
            }
            if (Dest is SingleEditable<Pat.Effect>)
            {
                var ddest = (SingleEditable<Pat.Effect>)Dest;
                ddest.Reset(null);
                EditableTreeNode newNode = null;
                newNode = new EditableTreeNode(new SelectEffect(delegate(Pat.Effect effect)
                {
                    ddest.Reset(effect);
                    newNode.Replace(new EditableTreeNode(effect, Dest));
                }), null);
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
            if (Dest is MultiEditable<Pat.Effect>)
            {
                var ddest = (MultiEditable<Pat.Effect>)Dest;
                ddest.Remove(Data);
                this.RemoveFromParent();
            }
        }
        public bool CanDelete { get { return Dest != null && Dest is MultiEditable<Pat.Effect>; } }
    }

    interface Editable<T>
    {
    }

    interface SingleEditable<T> : Editable<T>
    {
        void Reset(T value);
    }

    interface MultiEditable<T> : Editable<T>
    {
        void Remove(T val);
        void Append(T val);
    }

    class DelegateSingleEditable<T> : SingleEditable<T>
    {
        public Action<T> OnReset;

        public void Reset(T value)
        {
            if (OnReset != null)
            {
                OnReset(value);
            }
        }
    }

    class ListMultiEditable<T> : MultiEditable<T>
    {
        public List<T> List;

        public void Remove(T val)
        {
            List.Remove(val);
        }

        public void Append(T val)
        {
            List.Add(val);
        }
    }

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
    }
    
    #region Effect Add

    [TypeConverter(typeof(EffectTypeConverter))]
    class EffectType
    {
        public Type Value;

        public override string ToString()
        {
            return Value.Name;
        }
    }

    class EffectTypeConverter : TypeConverter
    {
        private static List<Type> _EffectTypes;
        private static List<Type> EffectTypes
        {
            get
            {
                if (_EffectTypes == null)
                {
                    _EffectTypes = typeof(EffectTypeConverter).Assembly.GetTypes()
                        .Where(t =>
                            !t.IsAbstract &&
                            typeof(Pat.Effect).IsAssignableFrom(t) &&
                            t != typeof(SelectEffect))
                        .ToList();
                }
                return _EffectTypes;
            }
        }

        public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
        {
            return true;
        }

        public override StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
        {
            return new StandardValuesCollection(EffectTypes.Select(type => type.Name).ToArray());
        }

        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            if (sourceType == typeof(string))
            {
                return true;
            }
            return base.CanConvertFrom(context, sourceType);
        }

        public override object ConvertFrom(ITypeDescriptorContext context,
            System.Globalization.CultureInfo culture, object value)
        {
            if (value is string)
            {
                foreach (var t in EffectTypes)
                {
                    if (t.Name == (string)value)
                    {
                        return new EffectType { Value = t };
                    }
                }
            }
            return base.ConvertFrom(context, culture, value);
        }
    }

    class SelectEffect : Pat.Effect
    {
        private readonly Action<Pat.Effect> _OnNewEffect;

        public SelectEffect(Action<Pat.Effect> onNewEffect)
        {
            _OnNewEffect = onNewEffect;
        }

        public EffectType Type
        {
            get
            {
                return null;
            }
            set
            {
                if (value == null || value.Value == null)
                {
                    return;
                }
                _OnNewEffect((Pat.Effect)value.Value.GetConstructor(new Type[0]).Invoke(new object[0]));
            }
        }

        public override void Run(Simulation.Actor actor)
        {
        }
    }

    #endregion

    class EffectTreeNodeHandler
    {
        public static TreeNode CreateFinalEffectNode(MultiEditable<Pat.Effect> dest)
        {
            return new EditableTreeNode(dest);
        }

        public static TreeNode CreateNode(Pat.Effect effect, MultiEditable<Pat.Effect> dest)
        {
            return new EditableTreeNode(effect, dest);
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
