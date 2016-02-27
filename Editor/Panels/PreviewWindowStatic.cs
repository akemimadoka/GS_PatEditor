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
            _Sprite.OriginX = frame.OriginX;
            _Sprite.OriginY = frame.OriginY;

            _Sprite.Left = window.X;
            _Sprite.Top = window.Y;
            _SpriteLineV.Left = window.X;
            _SpriteLineV.Top = window.Y;
            _SpriteLineH.Left = window.X;
            _SpriteLineH.Top = window.Y;

            _Sprite.Render();
            _SpriteLineV.Render();
            _SpriteLineH.Render();
        }
    }
}
