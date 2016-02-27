using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GS_PatEditor.Editor.Panels
{
    class MouseMovable
    {
        public delegate void MouseDownFilterDelegate(ref bool value);

        private MouseButtons _Button;

        private int _X, _Y;
        private int _DownMouseX, _DownMouseY, _LastX, _LastY;
        private bool _IsButtonDown;

        public event Action<int, int> OnMoved;
        public event Action<int, int> OnMovedDiff;
        public event Action OnMoveFinished;
        public event MouseDownFilterDelegate FilterMouseDown;

        public void SetPosition(int x, int y)
        {
            _X = x;
            _Y = y;
        }

        public MouseMovable(Control ctrl, MouseButtons button, int x, int y)
        {
            _Button = button;

            ctrl.MouseDown += ctrl_MouseDown;
            ctrl.MouseMove += ctrl_MouseMove;
            ctrl.MouseUp += ctrl_MouseUp;
            //TODO this event handler is never removed
            //be careful of memory leakage

            _X = x;
            _Y = y;
            _LastX = x;
            _LastY = y;
        }

        private void Update()
        {
            if (OnMoved != null)
            {
                OnMoved(_X, _Y);
            }
            if (OnMovedDiff != null)
            {
                OnMovedDiff(_X - _LastX, _Y - _LastY);
            }
        }

        void ctrl_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button.HasFlag(_Button) && _IsButtonDown)
            {
                _IsButtonDown = false;
                if (OnMoveFinished != null)
                {
                    OnMoveFinished();
                }
            }
        }

        void ctrl_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button.HasFlag(_Button) && _IsButtonDown)
            {
                _X = _LastX + e.X - _DownMouseX;
                _Y = _LastY + e.Y - _DownMouseY;
                Update();
            }
        }

        void ctrl_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button.HasFlag(_Button))
            {
                //TODO filter
                if (FilterMouseDown != null)
                {
                    bool success = true;
                    FilterMouseDown(ref success);
                    if (!success)
                    {
                        return;
                    }
                }

                _IsButtonDown = true;

                _LastX = _X;
                _LastY = _Y;
                _DownMouseX = e.X;
                _DownMouseY = e.Y;
            }
        }
    }
}
