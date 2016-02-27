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
        private MouseButtons _Button;

        private int _X, _Y;
        private int _DownMouseX, _DownMouseY, _LastX, _LastY;

        public event Action<int, int> OnMoved;

        public MouseMovable(Control ctrl, MouseButtons button, int x, int y)
        {
            _Button = button;

            ctrl.MouseDown += ctrl_MouseDown;
            ctrl.MouseMove += ctrl_MouseMove;
            ctrl.MouseUp += ctrl_MouseUp;

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
        }

        void ctrl_MouseUp(object sender, MouseEventArgs e)
        {
        }

        void ctrl_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button.HasFlag(_Button))
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
                _LastX = _X;
                _LastY = _Y;
                _DownMouseX = e.X;
                _DownMouseY = e.Y;
            }
        }
    }
}
