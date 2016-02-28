using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GS_PatEditor.Editor.Panels.Tools.Physical
{
    class PhysicalDataProvider : EditingPhysicalBox
    {
        private readonly Editor _Editor;
        private readonly PreviewWindow _Window;

        private bool _IsEditing;
        private float _NewLeft, _NewRight, _NewTop, _NewBottom;

        private static readonly Pat.PhysicalBox NullBox = new Pat.PhysicalBox();

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

        private Pat.PhysicalBox GetPhysicalBox()
        {
            var frame = _Editor.EditorNode.Animation.Frame.FrameData;
            if (frame == null || frame.PhysicalBox == null)
            {
                NullBox.X = 0;
                NullBox.Y = 0;
                NullBox.W = 0;
                NullBox.H = 0;

                return NullBox;
            }
            return frame.PhysicalBox;
        }

        public float ScreenLeft
        {
            get
            {
                if (_IsEditing)
                {
                    return _Window.PreviewMoving.TransformXSpriteToClient(_NewLeft);
                }
                else
                {
                    return _Window.PreviewMoving.TransformXSpriteToClient(GetPhysicalBox().X);
                }
            }
            set
            {
                if (_IsEditing)
                {
                    _NewLeft = _Window.PreviewMoving.TransformXClientToSprite(value);
                }
                else
                {
                    GetPhysicalBox().X = (int)_Window.PreviewMoving.TransformXClientToSprite(value);
                }
            }
        }

        public float ScreenTop
        {
            get
            {
                if (_IsEditing)
                {
                    return _Window.PreviewMoving.TransformYSpriteToClient(_NewTop);
                }
                else
                {
                    return _Window.PreviewMoving.TransformYSpriteToClient(GetPhysicalBox().Y);
                }
            }
            set
            {
                if (_IsEditing)
                {
                    _NewTop = _Window.PreviewMoving.TransformYClientToSprite(value);
                }
                else
                {
                    GetPhysicalBox().Y = (int)_Window.PreviewMoving.TransformYClientToSprite(value);
                }
            }
        }

        public float ScreenRight
        {
            get
            {
                if (_IsEditing)
                {
                    return _Window.PreviewMoving.TransformXSpriteToClient(_NewRight);
                }
                else
                {
                    var box = GetPhysicalBox();
                    return _Window.PreviewMoving.TransformXSpriteToClient(box.X + box.W);
                }
            }
            set
            {
                if (_IsEditing)
                {
                    _NewRight = _Window.PreviewMoving.TransformXClientToSprite(value);
                }
                else
                {
                    var box = GetPhysicalBox();
                    box.W = (int)_Window.PreviewMoving.TransformXClientToSprite(value) - box.X;
                }
            }
        }

        public float ScreenBottom
        {
            get
            {
                if (_IsEditing)
                {
                    return _Window.PreviewMoving.TransformYSpriteToClient(_NewBottom);
                }
                else
                {
                    var box = GetPhysicalBox();
                    return _Window.PreviewMoving.TransformYSpriteToClient(box.Y + box.H);
                }
            }
            set
            {
                if (_IsEditing)
                {
                    _NewBottom = _Window.PreviewMoving.TransformYClientToSprite(value);
                }
                else
                {
                    var box = GetPhysicalBox();
                    box.H = (int)_Window.PreviewMoving.TransformYClientToSprite(value) - box.Y;
                }
            }
        }

        public float Left
        {
            get
            {
                return _IsEditing ? _NewLeft : GetPhysicalBox().X;
            }
        }

        public float Top
        {
            get
            {
                return _IsEditing ? _NewTop : GetPhysicalBox().Y;
            }
        }

        public float Width
        {
            get
            {
                return _IsEditing ? _NewRight - _NewLeft : GetPhysicalBox().W;
            }
        }

        public float Height
        {
            get
            {
                return _IsEditing ? _NewBottom - _NewTop : GetPhysicalBox().H;
            }
        }
    }
}
