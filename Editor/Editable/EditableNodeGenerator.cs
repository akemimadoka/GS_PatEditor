using GS_PatEditor.Pat.Effects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GS_PatEditor.Editor.Editable
{
    class EditableNodeGenerator
    {
        private class GenericEditableTreeNode<T> : EditableTreeNode<T>
            where T : class
        {
            public GenericEditableTreeNode(EditableEnvironment env, object data, Editable<T> dest)
                : base(env, data as T, data, dest)
            {
                if (data is IEditableEnvironment)
                {
                    var cv = (IEditableEnvironment)data;
                    if (cv.Environment == null)
                    {
                        cv.Environment = env;
                    }
                }
            }

            public GenericEditableTreeNode(EditableEnvironment env, MultiEditable<T> dest)
                : base(env, dest)
            {
            }

            protected override object CreateSelectObject(MultiEditable<T> dest)
            {
                return new SelectObject<T>(obj =>
                {
                    dest.Append((T)obj);
                    this.InsertBefore(new GenericEditableTreeNode<T>(Env, obj, dest));
                }, Env);
            }

            protected override TreeNode CreateSingleEditableNode(SingleEditable<T> dest)
            {
                GenericEditableTreeNode<T> ret = null;
                ret = new GenericEditableTreeNode<T>(Env, new SelectObject<T>(obj =>
                {
                    dest.Reset((T)obj);
                    ret.Replace(new GenericEditableTreeNode<T>(Env, obj, dest));
                }, Env), null);
                return ret;
            }

            protected override void SetupCommon()
            {
                if (Data == null)
                {
                    Text = "<select>";
                }
                else
                {
                    Text = Data.GetType().Name;
                }

                EditableNodeGenerator.SetupChildren<T>(this, Env, Data);
            }
        }

        private class FieldSingleEditable<T> : SingleEditable<T>
            where T : class
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

        public static EditableTreeNode<T> Create<T>(EditableEnvironment env, T value, Editable<T> dest)
            where T : class
        {
            return new GenericEditableTreeNode<T>(env, value, dest);
        }

        public static EditableTreeNode<T> Create<T>(EditableEnvironment env, MultiEditable<T> dest)
            where T : class
        {
            return new GenericEditableTreeNode<T>(env, dest);
        }

        private static void SetupChildren<T>(TreeNode node, EditableEnvironment env, T obj)
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
                    newNode.Expand();
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
                    var creator1 = treeNodeT.GetConstructor(new Type[] {
                        typeof(EditableEnvironment), listInterface, editableT });
                    var creator2 = treeNodeT.GetConstructor(new Type[] {
                        typeof(EditableEnvironment),
                        typeof(MultiEditable<>).MakeGenericType(listInterface) });

                    var list = (System.Collections.IEnumerable)valueF;
                    foreach (var item in list)
                    {
                        var newNode = (TreeNode)creator1.Invoke(new object[] { env, item, me });
                        coll.Add(newNode);
                    }
                    coll.Add((TreeNode)creator2.Invoke(new object[] { env, me }));
                }
                else
                {
                    var editableT = typeof(Editable<>).MakeGenericType(typeV);
                    var se = typeof(FieldSingleEditable<>).MakeGenericType(typeV)
                        .GetConstructor(new Type[] { typeof(object), typeof(FieldInfo) })
                        .Invoke(new object[] { obj, f });
                    var creator = typeof(GenericEditableTreeNode<>).MakeGenericType(typeV)
                        .GetConstructor(new Type[] { typeof(EditableEnvironment), typeV, editableT });
                    var newNode = (IEditableTreeNode)creator.Invoke(new object[] { env, valueF, se });
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
        where T : class
    {
        void Add(T val);
        void Remove(T val);
        int FindIndex(T val);
        void Insert(int index, T val);
        int Count { get; }
    }

    public class EditorChildNodeAttribute : Attribute
    {
        public string Name { get; private set; }

        public EditorChildNodeAttribute(string name)
        {
            Name = name;
        }
    }
}
