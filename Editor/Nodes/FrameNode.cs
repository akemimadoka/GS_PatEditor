using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GS_PatEditor.Editor.Nodes
{
    class FrameNode
    {
        public enum EditMode
        {
            None,
            Hit,
            Attack,
        }

        public Pat.Frame Data;

        public EditMode Mode;
        public List<int> EditingBoxes = new List<int>();

        public event Action OnReset;

        public void Reset()
        {
            Mode = EditMode.None;
            if (OnReset != null)
            {
                OnReset();
            }
        }

        public void AddHitBox(Pat.Box box)
        {

        }

        public void AddAttackBox(Pat.Box box)
        {

        }

        public void RemoveHitBox(int index)
        {

        }

        public void RemoveAttackBox(int index)
        {

        }
    }
}
