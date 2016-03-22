using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GS_PatEditor.Editor.EffectEditable
{
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

    class EditableListMultiEditable<T> : MultiEditable<T>
    {
        private IEditableList<T> _List;

        public EditableListMultiEditable(IEditableList<T> list)
        {
            _List = list;
        }

        public void Remove(T val)
        {
            _List.Remove(val);
        }

        public void Append(T val)
        {
            _List.Add(val);
        }
    }
}
