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

            UpdatePreviewMode();
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
