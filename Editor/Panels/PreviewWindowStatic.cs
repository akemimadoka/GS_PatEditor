using GS_PatEditor.Render;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GS_PatEditor.Editor.Panels
{
    class PreviewWindowStatic : AbstractPreviewWindowContent
    {
        private readonly Editor _Parent;
        private Sprite _Sprite;

        private Sprite _SpriteLineV, _SpriteLineH;

        public PreviewWindowStatic(Editor parent)
        {
            _Parent = parent;
            var r = parent.PreviewWindowUI.Render;
            _Sprite = new Sprite(r)
            {
            };
            _SpriteLineH = new Sprite(r)
            {
                Texture = r.GetBlackTexture(),
                ScaleX = 10000,
                ScaleY = 1,
                OriginX = 0.5f,
                OriginY = 0.5f,
                Rotation = 0,
            };
            _SpriteLineV = new Sprite(r)
            {
                Texture = r.GetBlackTexture(),
                ScaleX = 1,
                ScaleY = 10000,
                OriginX = 0.5f,
                OriginY = 0.5f,
                Rotation = 0,
            };
        }

        public override void Render()
        {
            var proj = _Parent.Data;
            var frame = _Parent.EditorNode.Animation.Frame.FrameData;
            var id = frame.ImageID;
            var txt = proj.ImageList.GetTexture(id, _Parent.PreviewWindowUI.Render);
            var window = _Parent.PreviewWindowUI;

            _Sprite.Texture = txt;
            _Sprite.OriginX = frame.OriginX + window.SpriteMovingX;
            _Sprite.OriginY = frame.OriginY + window.SpriteMovingY;
            _Sprite.ScaleX = frame.ScaleX / 100.0f;
            _Sprite.ScaleY = frame.ScaleY / 100.0f;
            _Sprite.Rotation = frame.Rotate / 180.0f * 3.1415926f;

            //_Sprite.Left = window.PreviewMovingX;
            //_Sprite.Top = window.PreviewMovingY;
            //_SpriteLineV.Left = window.PreviewMovingX;
            //_SpriteLineV.Top = window.PreviewMovingY;
            //_SpriteLineH.Left = window.PreviewMovingX;
            //_SpriteLineH.Top = window.PreviewMovingY;

            _Sprite.Render();
            _SpriteLineV.Render();
            _SpriteLineH.Render();
        }
    }
}
