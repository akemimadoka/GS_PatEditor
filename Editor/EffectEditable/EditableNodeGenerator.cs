using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GS_PatEditor.Editor.EffectEditable
{
    class EditableNodeGenerator
    {
        private class GenericEditableTreeNode<T> : EditableTreeNode<T>
            where T : class
        {
            public GenericEditableTreeNode(T data, Editable<T> dest)
                : base(data, dest)
            {
            }

            public GenericEditableTreeNode(MultiEditable<T> dest)
                : base(dest)
            {
            }

            protected override T CreateSelectObject(MultiEditable<T> dest)
            {
                return EditableNodeGenerator.CreateSelectObject<T>(obj =>
                {
                    dest.Append(obj);
                    this.InsertBefore(new GenericEditableTreeNode<T>(obj, dest));
                });
            }

            protected override TreeNode CreateSingleEditableNode(SingleEditable<T> dest)
            {
                GenericEditableTreeNode<T> ret = null;
                ret = new GenericEditableTreeNode<T>(EditableNodeGenerator.CreateSelectObject<T>(obj =>
                {
                    dest.Reset(obj);
                    ret.Replace(new GenericEditableTreeNode<T>(obj, dest));
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
                else if (EditableNodeGenerator._SelectObjectTypes[typeof(T)].IsAssignableFrom(Data.GetType()))
                {
                    Text = "<select>";
                }
                else
                {
                    Text = Data.GetType().Name;
                }

                EditableNodeGenerator.SetupChildren<T>(this, Data);
            }
        }

        private class FieldSingleEditable<T> : SingleEditable<T>
        {
            private object _Object;
            private FieldInfo _Field;

            public FieldSingleEditable(object obj, FieldInfo field)
            {
                _Object = obj;
                _Field = field;
            }

            public void Reset(T value)
            {
                _Field.SetValue(_Object, value);
            }
        }

        public static EditableTreeNode<T> Create<T>(T value, Editable<T> dest)
            where T : class
        {
            return new GenericEditableTreeNode<T>(value, dest);
        }

        public static EditableTreeNode<T> Create<T>(MultiEditable<T> dest)
            where T : class
        {
            return new GenericEditableTreeNode<T>(dest);
        }

        //select object class
        private static Dictionary<Type, Type> _SelectObjectTypes = new Dictionary<Type, Type>();

        static EditableNodeGenerator()
        {
            //TODO all assemblies?
            foreach (var type in typeof(EditableNodeGenerator).Assembly.GetTypes())
            {
                var attr = type.GetCustomAttribute<EditorSelectorAttribute>();
                if (attr != null)
                {
                    _SelectObjectTypes.Add(attr.Type, type);
                }
            }
        }

        private static T CreateSelectObject<T>(Action<T> action)
        {
            var type = _SelectObjectTypes[typeof(T)];
            return (T)(type.GetConstructor(new Type[] { typeof(Action<T>) }).Invoke(new object[] { action }));
        }

        private static void SetupChildren<T>(TreeNode node, T obj)
        {
            if (obj == null)
            {
                return;
            }

            var fields = obj.GetType().GetFields();
            foreach (var f in fields)
            {
                var attr = f.GetCustomAttributes(typeof(EditorChildNodeAttribute), false)
                    .Select(x => (EditorChildNodeAttribute)x)
                    .FirstOrDefault();
                if (attr == null)
                {
                    continue;
                }

                TreeNodeCollection coll;
                if (attr.Name == null)
                {
                    coll = node.Nodes;
                }
                else
                {
                    var newNode = new TreeNode { Text = attr.Name };
                    node.Nodes.Add(newNode);
                    coll = newNode.Nodes;
                }

                var valueF = f.GetValue(obj);
                var typeV = f.FieldType;
                var listInterface = typeV.GetInterfaces()
                    .Where(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IEditableList<>))
                    .Select(i => i.GetGenericArguments()[0])
                    .FirstOrDefault();
                if (listInterface != null)
                {
                    //we've got a list
                    //first create our MultiEditable object
                    var editableListT = typeof(IEditableList<>).MakeGenericType(listInterface);
                    var editableT = typeof(Editable<>).MakeGenericType(listInterface);
                    var treeNodeT = typeof(GenericEditableTreeNode<>).MakeGenericType(listInterface);

                    var me = typeof(EditableListMultiEditable<>).MakeGenericType(listInterface)
                        .GetConstructor(new Type[] { editableListT })
                        .Invoke(new object[] { valueF });
                    var creator1 = treeNodeT
                        .GetConstructor(new Type[] { listInterface, editableT });
                    var creator2 = treeNodeT
                        .GetConstructor(new Type[] { typeof(MultiEditable<>).MakeGenericType(listInterface) });

                    var list = (System.Collections.IEnumerable)valueF;
                    foreach (var item in list)
                    {
                        var newNode = (TreeNode)creator1.Invoke(new object[] { item, me });
                        coll.Add(newNode);
                    }
                    coll.Add((TreeNode)creator2.Invoke(new object[] { me }));
                }
                else
                {
                    var editableT = typeof(Editable<>).MakeGenericType(typeV);
                    var se = typeof(FieldSingleEditable<>).MakeGenericType(typeV)
                        .GetConstructor(new Type[] { typeof(object), typeof(FieldInfo) })
                        .Invoke(new object[] { obj, f });
                    var creator = typeof(GenericEditableTreeNode<>).MakeGenericType(typeV)
                        .GetConstructor(new Type[] { typeV, editableT });
                    var newNode = (IEditableTreeNode)creator.Invoke(new object[] { valueF, se });
                    coll.Add((TreeNode)newNode);
                    if (valueF == null)
                    {
                        newNode.Reset();
                    }
                }
            }
        }
    }

    public interface IEditableList<T> : IEnumerable<T>
    {
        void Add(T val);
        void Remove(T val);
    }

    public class EditorChildNodeAttribute : Attribute
    {
        public string Name { get; private set; }

        public EditorChildNodeAttribute(string name)
        {
            Name = name;
        }
    }

    public class EditorSelectorAttribute : Attribute
    {
        public Type Type { get; set; }

        public EditorSelectorAttribute(Type type)
        {
            Type = type;
        }
    }
}
