using GS_PatEditor.Editor.Nodes;
using GS_PatEditor.Editor.Panels.Tools;
using GS_PatEditor.Editor.Panels.Tools.HitAttack;
using GS_PatEditor.Editor.Panels.Tools.Move;
using GS_PatEditor.Editor.Panels.Tools.Physical;
using GS_PatEditor.Editor.Panels.Tools.Point;
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
        public SpriteMovingHandler SpriteMoving { get; private set; }

        public PhysicalEditingHandler PhysicalEditing { get; private set; }
        public HitAttackBoxesEditingHandler HitEditing { get; private set; }
        public HitAttackBoxesEditingHandler AttackEditing { get; private set; }
        public PointEditingHandler PointEditing { get; private set; }

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

            //global

            Render = new RenderEngine(ctrl);
            Render.OnRender += _Render_OnRender;

            SpriteManager = new PreviewWindowSpriteManager(this);

            //tools

            PreviewMoving = new PreviewMovingHandler(this, ctrl);
            SpriteMoving = new SpriteMovingHandler(_Parent, ctrl);

            PhysicalEditing = new PhysicalEditingHandler(_Parent, ctrl);
            PreviewMoving.SceneMoved += PhysicalEditing.UpdateMouseCursor;

            HitEditing = new HitBoxesEditingHandler(_Parent, ctrl);
            AttackEditing = new AttackBoxesEditingHandler(_Parent, ctrl);

            PointEditing = new PointEditingHandler(_Parent, ctrl);

            UpdatePreviewMode();
        }

        public EventFilter GetFilterForEditMode(FrameEditMode mode)
        {
            return delegate(ref bool result)
            {
                result = _Parent.Frame.EditMode == mode;

                //visible condition

                switch (mode)
                {
                    case FrameEditMode.Physical:
                        result = result && _Parent.Frame.PhysicalBoxVisible;
                        break;
                }
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
                SpriteManager.ResetAll();

                Render.Dispose();
                Render = null;
            }
        }

        //called by FrameNode, when update mode is changed
        public void UpdatePreviewMode()
        {
            SpriteManager.ResetAll();

            switch (_Parent.Frame.PreviewMode)
            {
                case FrameNode.FramePreviewMode.Pause:
                    CurrentContent = new PreviewWindowStatic(_Parent);
                    PreviewMoving.ResetScale(1.0f);
                    break;
                case FrameNode.FramePreviewMode.Play:
                    CurrentContent = new PreviewWindowPlaying(_Parent);
                    break;
                default:
                    CurrentContent = null;
                    break;
            }
        }

        public int ControlWidth
        {
            get
            {
                return _Control == null ? 100 : _Control.Parent.ClientSize.Width;
            }
        }

        public int ControlHeight
        {
            get
            {
                return _Control == null ? 100 : _Control.Parent.ClientSize.Height;
            }
        }
    }
}
