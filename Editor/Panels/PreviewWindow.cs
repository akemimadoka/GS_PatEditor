using GS_PatEditor.Editor.Nodes;
using GS_PatEditor.Render;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GS_PatEditor.Editor.Panels
{
    abstract class AbstractPreviewWindowContent
    {
        public abstract void Render();
    }

    class PreviewWindow : IDisposable
    {
        private readonly Editor _Parent;
        private Control _Control;

        public RenderEngine Render { get; private set; }
        public AbstractPreviewWindowContent CurrentContent { get; set; }

        public int X { get; private set; }
        public int Y { get; private set; }

        public PreviewWindow(Editor parent)
        {
            _Parent = parent;
        }

        public void Init(Control ctrl)
        {
            if (_Control != null)
            {
                throw new Exception();
            }

            _Control = ctrl;
            Render = new RenderEngine(ctrl);
            Render.OnRender += _Render_OnRender;

            X = ctrl.Width / 2;
            Y = ctrl.Height / 2;

            UpdatePreviewMode();

            //move scene
            var move = new MouseMovable(ctrl, MouseButtons.Middle, X, Y);
            move.OnMoved += delegate(int x, int y)
            {
                this.X = x;
                this.Y = y;
            };

            ctrl.FindForm().MouseWheel += frm_MouseWheel;
        }

        private void frm_MouseWheel(object sender, MouseEventArgs e)
        {
            var parentCtrl = _Control.Parent;
            if (!parentCtrl.ClientRectangle.Contains(parentCtrl.PointToClient(Control.MousePosition)))
            {
                return;
            }
            if (_Control.ClientRectangle.Contains(_Control.PointToClient(Control.MousePosition)))
            {
                if (e.Delta > 0)
                {
                    Render.Transform.Scale *= 0.9f;
                }
                else if (e.Delta < 0)
                {
                    Render.Transform.Scale /= 0.9f;
                }
            }
        }

        private void _Render_OnRender()
        {
            if (CurrentContent != null)
            {
                CurrentContent.Render();
            }
        }

        public void Refresh()
        {
            if (Render != null)
            {
                Render.RenderAll();
            }
        }

        public void Dispose()
        {
            if (Render != null)
            {
                Render.Dispose();
                Render = null;
            }
        }

        //called by FrameNode, when update mode is changed
        public void UpdatePreviewMode()
        {
            switch (_Parent.EditorNode.Animation.Frame.PreviewMode)
            {
                case FrameNode.FramePreviewMode.Pause:
                    CurrentContent = new PreviewWindowStatic(_Parent);
                    break;
                default:
                    CurrentContent = null;
                    break;
            }
        }
    }
}
