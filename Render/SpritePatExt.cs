using GS_PatEditor.Editor.Panels;
using GS_PatEditor.Editor.Panels.Tools;
using GS_PatEditor.Pat;
using SharpDX.Direct3D9;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Frame = GS_PatEditor.Pat.Frame;

namespace GS_PatEditor.Render
{
    static class SpritePatExt
    {
        public static void SetupFrame(this Sprite sprite, Texture txt, Frame frame, EditingPoint editing)
        {
            sprite.Setup(txt,
                OriginX: frame.OriginX + editing.OffsetX,
                OriginY: frame.OriginY + editing.OffsetY,
                ScaleX: frame.ScaleX / 100.0f,
                ScaleY: frame.ScaleY / 100.0f);
            sprite.SetupPosition(0, 0, frame.Rotation / 180.0f * 3.1415926f);
        }

        public static void SetupPhysical(this Sprite[] rect, uint color, EditingPhysicalBox box)
        {
            var hw = box.Width / 2;
            var hh = box.Height / 2;
            rect.SetupRect(color, hw, hh);
            rect.SetupPosition(box.Left + hw, box.Top + hh, 0);
        }

        public static void SetupHit(this Sprite[] rect, uint color, EditingHitAttackBox box)
        {
            var hw = box.Width / 2;
            var hh = box.Height / 2;
            rect.SetupRect(color, hw, hh);
            rect.SetupPosition(box.Left + hw, box.Top + hh, 0, box.Rotation);
        }
    }
}
