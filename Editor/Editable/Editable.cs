using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GS_PatEditor.Editor.Editable
{
    interface Editable<T>
        where T : class
    {
    }

    interface SingleEditable<T> : Editable<T>
        where T : class
    {
        void Reset(T value);
    }

    interface MultiEditable<T> : Editable<T>
        where T : class
    {
        void Remove(T val);
        void Append(T val);
        bool IsFirst(T val);
        bool IsLast(T val);
        void MoveUp(T val);
        void MoveDown(T val);
    }

    class ListMultiEditable<T> : MultiEditable<T>
        where T : class
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

        public bool IsFirst(T val)
        {
            return List.FindIndex(i => i == val) == 0;
        }

        public bool IsLast(T val)
        {
            return List.FindIndex(i => i == val) == List.Count - 1;
        }

        public void MoveUp(T val)
        {
            var index = List.FindIndex(i => i == val);
            if (index != 0)
            {
                List.RemoveAt(index);
                List.Insert(index - 1, val);
            }
        }

        public void MoveDown(T val)
        {
            var index = List.FindIndex(i => i == val);
            if (index != List.Count - 1)
            {
                List.RemoveAt(index);
                List.Insert(index + 1, val);
            }
        }
    }

    class EditableListMultiEditable<T> : MultiEditable<T>
        where T : class
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

        public bool IsFirst(T val)
        {
            return _List.FindIndex(val) == 0;
        }

        public bool IsLast(T val)
        {
            return _List.FindIndex(val) == _List.Count - 1;
        }

        public void MoveUp(T val)
        {
            var index = _List.FindIndex(val);
            if (index != 0)
            {
                _List.Remove(val);
                _List.Insert(index - 1, val);
            }
        }

        public void MoveDown(T val)
        {
            var index = _List.FindIndex(val);
            if (index != _List.Count - 1)
            {
                _List.Remove(val);
                _List.Insert(index + 1, val);
            }
        }
    }
}
