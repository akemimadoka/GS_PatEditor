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
            parent.PreviewModeChanged += OnPreviewModeChanged;
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

            OnPreviewModeChanged();
        }

        public EventFilter GetFilterForEditMode(FrameEditMode mode)
        {
            return delegate(ref bool result)
            {
                result = _Parent.PreviewMode == FramePreviewMode.Pause && _Parent.EditMode == mode;

                //visible condition

                switch (mode)
                {
                    case FrameEditMode.Physical:
                        result = result && _Parent.PhysicalBoxVisible;
                        break;
                    case FrameEditMode.Attack:
                        result = result && _Parent.AttackBoxVisible;
                        break;
                    case FrameEditMode.Hit:
                        result = result && _Parent.HitBoxVisible;
                        break;
                    case FrameEditMode.Point:
                        result = result && _Parent.PointVisible;
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

        private void OnPreviewModeChanged()
        {
            SpriteManager.ResetAll();

            switch (_Parent.PreviewMode)
            {
                case FramePreviewMode.Pause:
                    CurrentContent = new PreviewWindowStatic(_Parent);
                    PreviewMoving.ResetScale(1.0f);
                    break;
                case FramePreviewMode.Play:
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
