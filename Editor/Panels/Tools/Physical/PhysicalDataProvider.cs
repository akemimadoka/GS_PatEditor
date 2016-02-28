using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GS_PatEditor.Editor.Panels.Tools.Physical
{
    class PhysicalDataProvider : IRectDataProvider
    {
        private readonly Editor _Editor;
        private readonly PreviewWindow _Window;

        public PhysicalDataProvider(Editor editor)
        {
            _Editor = editor;
            _Window = editor.PreviewWindowUI;
        }

        public float Left
        {
            get
            {
                var frame = _Editor.EditorNode.Animation.Frame.FrameData;
                if (frame == null || frame.PhysicalBox == null)
                {
                    return 0;
                }
                return _Window.PreviewMoving.TransformXSpriteToClient(frame.PhysicalBox.X);
            }
            set
            {
                var frame = _Editor.EditorNode.Animation.Frame.FrameData;
                if (frame == null || frame.PhysicalBox == null)
                {
                    return;
                }
                frame.PhysicalBox.X = (int)_Window.PreviewMoving.TransformXClientToSprite(value);
            }
        }

        public float Top
        {
            get
            {
                var frame = _Editor.EditorNode.Animation.Frame.FrameData;
                if (frame == null || frame.PhysicalBox == null)
                {
                    return 0;
                }
                return _Window.PreviewMoving.TransformYSpriteToClient(frame.PhysicalBox.Y);
            }
            set
            {
                var frame = _Editor.EditorNode.Animation.Frame.FrameData;
                if (frame == null || frame.PhysicalBox == null)
                {
                    return;
                }
                frame.PhysicalBox.Y = (int)_Window.PreviewMoving.TransformYClientToSprite(value);
            }
        }

        public float Right
        {
            get
            {
                var frame = _Editor.EditorNode.Animation.Frame.FrameData;
                if (frame == null || frame.PhysicalBox == null)
                {
                    return 0;
                }
                return _Window.PreviewMoving.TransformXSpriteToClient(frame.PhysicalBox.X + frame.PhysicalBox.W);
            }
            set
            {
                var frame = _Editor.EditorNode.Animation.Frame.FrameData;
                if (frame == null || frame.PhysicalBox == null)
                {
                    return;
                }
                frame.PhysicalBox.W = (int)_Window.PreviewMoving.TransformXClientToSprite(value) - frame.PhysicalBox.X;
            }
        }

        public float Bottom
        {
            get
            {
                var frame = _Editor.EditorNode.Animation.Frame.FrameData;
                if (frame == null || frame.PhysicalBox == null)
                {
                    return 0;
                }
                return _Window.PreviewMoving.TransformYSpriteToClient(frame.PhysicalBox.Y + frame.PhysicalBox.H);
            }
            set
            {
                var frame = _Editor.EditorNode.Animation.Frame.FrameData;
                if (frame == null || frame.PhysicalBox == null)
                {
                    return;
                }
                frame.PhysicalBox.H = (int)_Window.PreviewMoving.TransformYClientToSprite(value) - frame.PhysicalBox.Y;
            }
        }
    }
}
