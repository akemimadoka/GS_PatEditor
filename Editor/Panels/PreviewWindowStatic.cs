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

        private Sprite _Sprite, _SpriteLineV, _SpriteLineH;
        private Sprite[] _SpriteListPhysical;

        public PreviewWindowStatic(Editor parent)
        {
            _Parent = parent;
            var r = parent.PreviewWindowUI.Render;

            parent.PreviewWindowUI.EnsureSpriteList(3 + 4);

            _Sprite = parent.PreviewWindowUI.SpriteList[0];

            _SpriteLineH = parent.PreviewWindowUI.SpriteList[1];
            SpriteGeometry.SetupLine(0, _SpriteLineH, 0, 0, 10000, 0);

            _SpriteLineV = parent.PreviewWindowUI.SpriteList[2];
            SpriteGeometry.SetupLine(0, _SpriteLineV, 0, 0, 10000, 3.1415926f / 2);

            _SpriteListPhysical = new[]
            {
                parent.PreviewWindowUI.SpriteList[3],
                parent.PreviewWindowUI.SpriteList[4],
                parent.PreviewWindowUI.SpriteList[5],
                parent.PreviewWindowUI.SpriteList[6],
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

            _Sprite.Render();
            _SpriteLineV.Render();
            _SpriteLineH.Render();

            if (_Parent.EditorNode.Animation.Frame.PhysicalBoxVisible)
            {
                var physicalBox = frame.PhysicalBox;
                if (physicalBox != null)
                {
                    SpriteGeometry.SetupRect(0x00FF66, _SpriteListPhysical,
                        physicalBox.X + physicalBox.W / 2.0f,
                        physicalBox.Y + physicalBox.H / 2.0f,
                        physicalBox.W / 2.0f,
                        physicalBox.H / 2.0f, 0);
                    foreach (var s in _SpriteListPhysical)
                    {
                        s.Render();
                    }
                }
            }
        }
    }
}
