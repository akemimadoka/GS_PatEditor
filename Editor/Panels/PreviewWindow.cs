using GS_PatEditor.Editor.Nodes;
using GS_PatEditor.Editor.Panels.Tools;
using GS_PatEditor.Editor.Panels.Tools.Physical;
using GS_PatEditor.Editor.Panels.Tools.Preview;
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

        //global

        public RenderEngine Render { get; private set; }
        public PreviewWindowSpriteManager SpriteManager { get; private set; }
        public AbstractPreviewWindowContent CurrentContent { get; private set; }

        //tools

        public PreviewMovingHandler PreviewMoving { get; private set; }

        public int SpriteMovingX, SpriteMovingY;

        private MouseRectEditable EditPhysical;

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

            PreviewMoving = new PreviewMovingHandler(this, ctrl);

            //sprites
            SpriteManager = new PreviewWindowSpriteManager(this);

            //content
            UpdatePreviewMode();

            //move scene
            {
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
                        node.EditMode != FrameEditMode.Move)
                    {
                        result = false;
                    }
                };
                move.OnMovedDiff += delegate(int x, int y)
                {
                    SpriteMovingX = (int)(-x / PreviewMoving.PreviewScale);
                    SpriteMovingY = (int)(-y / PreviewMoving.PreviewScale);
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

            EditPhysical = new MouseRectEditable(ctrl, new PhysicalDataProvider(_Parent, this));
            EditPhysical.Filter += GetFilterForEditMode(FrameEditMode.Physical);
            PreviewMoving.SceneMoved += EditPhysical.UpdateMouseCursor;
        }

        private EventFilter GetFilterForEditMode(FrameEditMode mode)
        {
            return delegate(ref bool result)
            {
                result = this._Parent.EditorNode.Animation.Frame.EditMode == mode;
            };
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

        //public void EnsureSpriteList(int count)
        //{
        //    while (SpriteList.Count < count)
        //    {
        //        SpriteList.Add(Render.GetSprite());
        //    }
        //}
    }
}
