using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GS_PatEditor.Editor.Panels.Tools.Physical
{
    //TODO merge physical box check logic
    class PhysicalDataProvider : EditingPhysicalBox
    {
        private readonly Editor _Editor;
        private readonly PreviewWindow _Window;

        private bool _IsEditing;
        private float _NewLeft, _NewRight, _NewTop, _NewBottom;

        public PhysicalDataProvider(Editor editor)
        {
            _Editor = editor;
            _Window = editor.PreviewWindowUI;
        }

        public bool IsEditing
        {
            get
            {
                return _IsEditing;
            }
            set
            {
                if (_IsEditing == value)
                {
                    return;
                }
                if (value)
                {
                    BeginEditing();
                }
                else
                {
                    FinishEditing();
                }
            }
        }

        private void BeginEditing()
        {
            _IsEditing = true;

            var frame = _Editor.EditorNode.Animation.Frame.FrameData;
            if (frame == null || frame.PhysicalBox == null)
            {
                return;
            }

            _NewLeft = frame.PhysicalBox.X;
            _NewTop = frame.PhysicalBox.Y;
            _NewRight = frame.PhysicalBox.X + frame.PhysicalBox.W;
            _NewBottom = frame.PhysicalBox.Y + frame.PhysicalBox.H;
        }

        private void FinishEditing()
        {
            _IsEditing = false;

            var frame = _Editor.EditorNode.Animation.Frame.FrameData;
            if (frame == null || frame.PhysicalBox == null)
            {
                return;
            }

            frame.PhysicalBox.X = (int)Math.Round(_NewLeft);
            frame.PhysicalBox.Y = (int)Math.Round(_NewTop);
            frame.PhysicalBox.W = (int)Math.Round(_NewRight - _NewLeft);
            frame.PhysicalBox.H = (int)Math.Round(_NewBottom - _NewTop);
        }

        public float ScreenLeft
        {
            get
            {
                var frame = _Editor.EditorNode.Animation.Frame.FrameData;
                if (frame == null || frame.PhysicalBox == null)
                {
                    return 0;
                }
                if (_IsEditing)
                {
                    return _Window.PreviewMoving.TransformXSpriteToClient(_NewLeft);
                }
                else
                {
                    return _Window.PreviewMoving.TransformXSpriteToClient(frame.PhysicalBox.X);
                }
            }
            set
            {
                var frame = _Editor.EditorNode.Animation.Frame.FrameData;
                if (frame == null || frame.PhysicalBox == null)
                {
                    return;
                }
                if (_IsEditing)
                {
                    _NewLeft = _Window.PreviewMoving.TransformXClientToSprite(value);
                }
                else
                {
                    frame.PhysicalBox.X = (int)_Window.PreviewMoving.TransformXClientToSprite(value);
                }
            }
        }

        public float ScreenTop
        {
            get
            {
                var frame = _Editor.EditorNode.Animation.Frame.FrameData;
                if (frame == null || frame.PhysicalBox == null)
                {
                    return 0;
                }
                if (_IsEditing)
                {
                    return _Window.PreviewMoving.TransformYSpriteToClient(_NewTop);
                }
                else
                {
                    return _Window.PreviewMoving.TransformYSpriteToClient(frame.PhysicalBox.Y);
                }
            }
            set
            {
                var frame = _Editor.EditorNode.Animation.Frame.FrameData;
                if (frame == null || frame.PhysicalBox == null)
                {
                    return;
                }
                if (_IsEditing)
                {
                    _NewTop = _Window.PreviewMoving.TransformYClientToSprite(value);
                }
                else
                {
                    frame.PhysicalBox.Y = (int)_Window.PreviewMoving.TransformYClientToSprite(value);
                }
            }
        }

        public float ScreenRight
        {
            get
            {
                var frame = _Editor.EditorNode.Animation.Frame.FrameData;
                if (frame == null || frame.PhysicalBox == null)
                {
                    return 0;
                }
                if (_IsEditing)
                {
                    return _Window.PreviewMoving.TransformXSpriteToClient(_NewRight);
                }
                else
                {
                    return _Window.PreviewMoving.TransformXSpriteToClient(frame.PhysicalBox.X + frame.PhysicalBox.W);
                }
            }
            set
            {
                var frame = _Editor.EditorNode.Animation.Frame.FrameData;
                if (frame == null || frame.PhysicalBox == null)
                {
                    return;
                }
                if (_IsEditing)
                {
                    _NewRight = _Window.PreviewMoving.TransformXClientToSprite(value);
                }
                else
                {
                    frame.PhysicalBox.W = (int)_Window.PreviewMoving.TransformXClientToSprite(value) - frame.PhysicalBox.X;
                }
            }
        }

        public float ScreenBottom
        {
            get
            {
                var frame = _Editor.EditorNode.Animation.Frame.FrameData;
                if (frame == null || frame.PhysicalBox == null)
                {
                    return 0;
                }
                if (_IsEditing)
                {
                    return _Window.PreviewMoving.TransformYSpriteToClient(_NewBottom);
                }
                else
                {
                    return _Window.PreviewMoving.TransformYSpriteToClient(frame.PhysicalBox.Y + frame.PhysicalBox.H);
                }
            }
            set
            {
                var frame = _Editor.EditorNode.Animation.Frame.FrameData;
                if (frame == null || frame.PhysicalBox == null)
                {
                    return;
                }
                if (_IsEditing)
                {
                    _NewBottom = _Window.PreviewMoving.TransformYClientToSprite(value);
                }
                else
                {
                    frame.PhysicalBox.H = (int)_Window.PreviewMoving.TransformYClientToSprite(value) - frame.PhysicalBox.Y;
                }
            }
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
                return _IsEditing ? _NewLeft : frame.PhysicalBox.X;
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
                return _IsEditing ? _NewTop : frame.PhysicalBox.Y;
            }
        }

        public float Width
        {
            get
            {
                var frame = _Editor.EditorNode.Animation.Frame.FrameData;
                if (frame == null || frame.PhysicalBox == null)
                {
                    return 0;
                }
                return _IsEditing ? _NewRight - _NewLeft : frame.PhysicalBox.W;
            }
        }

        public float Height
        {
            get
            {
                var frame = _Editor.EditorNode.Animation.Frame.FrameData;
                if (frame == null || frame.PhysicalBox == null)
                {
                    return 0;
                }
                return _IsEditing ? _NewBottom - _NewTop : frame.PhysicalBox.H;
            }
        }
    }
}
