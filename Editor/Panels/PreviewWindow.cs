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

        //share sprites in all AbstractPreviewWindowContent to avoid repeating creating/disposing
        public readonly List<Sprite> SpriteList = new List<Sprite>();

        public AbstractPreviewWindowContent CurrentContent { get; set; }

        private int PreviewX, PreviewY;
        private int PreviewMovingX, PreviewMovingY;
        private float _PreviewScale = 1.0f;

        public int SpriteMovingX, SpriteMovingY;

        private MouseMovable MovablePreview;

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

            PreviewX = ctrl.Width / 2;
            PreviewY = ctrl.Height / 2;
            Render.Transform.X = PreviewX;
            Render.Transform.Y = PreviewY;

            UpdatePreviewMode();

            //move scene
            {
                //TODO use diff version, calc delta * scale
                MovablePreview = new MouseMovable(ctrl, MouseButtons.Middle, PreviewMovingX, PreviewMovingY);
                MovablePreview.OnMovedDiff += delegate(int x, int y)
                {
                    PreviewMovingX = x;
                    PreviewMovingY = y;

                    Render.Transform.X = PreviewX + PreviewMovingX;
                    Render.Transform.Y = PreviewY + PreviewMovingY;
                };
                MovablePreview.OnMoveFinished += delegate()
                {
                    PreviewX += PreviewMovingX;
                    PreviewY += PreviewMovingY;
                    PreviewMovingX = 0;
                    PreviewMovingY = 0;

                    Render.Transform.X = PreviewX;
                    Render.Transform.Y = PreviewY;
                };
            }

            //move tool
            {
                var move = new MouseMovable(ctrl, MouseButtons.Left, 0, 0);
                move.FilterMouseDown += delegate(ref bool result)
                {
                    var node = _Parent.EditorNode.Animation.Frame;
                    //empty animation (no frame)
                    if (node.FrameData == null)
                    {
                        result = false;
                    }
                    if (node.PreviewMode != FrameNode.FramePreviewMode.Pause ||
                        node.EditMode != FrameNode.FrameEditMode.Move)
                    {
                        result = false;
                    }
                };
                move.OnMovedDiff += delegate(int x, int y)
                {
                    SpriteMovingX = (int)(-x / _PreviewScale);
                    SpriteMovingY = (int)(-y / _PreviewScale);
                };
                move.OnMoveFinished += delegate()
                {
                    var node = _Parent.EditorNode.Animation.Frame;
                    node.FrameData.OriginX += SpriteMovingX;
                    node.FrameData.OriginY += SpriteMovingY;
                    SpriteMovingX = 0;
                    SpriteMovingY = 0;
                };
            }
            

            //mouse wheel zoom in/out
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
                if (e.Delta < 0)
                {
                    //_PreviewScale *= 0.9f;
                    //Render.Transform.Scale = _PreviewScale;
                    SetScale(_PreviewScale * 0.9f);
                }
                else if (e.Delta > 0)
                {
                    //_PreviewScale /= 0.9f;
                    //Render.Transform.Scale = _PreviewScale;
                    SetScale(_PreviewScale / 0.9f);
                }
            }
        }

        private void SetScale(float newScale)
        {
            if (newScale < 0.1f || newScale > 10.0f)
            {
                return;
            }

            //client position of mouse
            var pClient = _Control.PointToClient(Control.MousePosition);

            //position on scaled scene
            float pSSx = (pClient.X - PreviewX) / _PreviewScale;
            float pSSy = (pClient.Y - PreviewY) / _PreviewScale;

            //this point is still on pClient
            float newX = pClient.X - pSSx * newScale;
            float newY = pClient.Y - pSSy * newScale;

            PreviewX = (int)newX;
            PreviewY = (int)newY;
            _PreviewScale = newScale;

            MovablePreview.SetPosition(PreviewX, PreviewY);

            Render.Transform.Scale = _PreviewScale;
            Render.Transform.X = PreviewX;
            Render.Transform.Y = PreviewY;
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

        public void EnsureSpriteList(int count)
        {
            while (SpriteList.Count < count)
            {
                SpriteList.Add(Render.GetSprite());
            }
        }
    }
}
