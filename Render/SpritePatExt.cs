﻿using GS_PatEditor.Editor.Panels;
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
        public static void SetupActor(this Sprite sprite, Texture txt, Simulation.Actor actor, EditingPoint editing)
        {
            //TODO do not use Setup (which only supports Scale but not Size)
            var frame = actor.CurrentFrame;

            //handle invalid frame
            if (frame == null)
            {
                sprite.Texture = null;
                return;
            }

            var dirScale = actor.InversedDirection ? -1.0f : 1.0f;

            sprite.Setup(txt,
                OriginX: frame.OriginX + editing.OffsetX + 0.5f,
                OriginY: frame.OriginY + editing.OffsetY + 0.5f,
                ScaleX: frame.ScaleX / 100.0f * actor.ScaleX * dirScale,
                ScaleY: frame.ScaleY / 100.0f * actor.ScaleY);
            sprite.SetupPosition(actor.X, actor.Y, frame.Rotation / 180.0f * 3.1415926f + actor.Rotation);
            sprite.SetupColor(actor.Alpha * frame.Alpha,
                actor.Red * frame.Red,
                actor.Green * frame.Green,
                actor.Blue * frame.Blue);
        }

        public static void SetupFrame(this Sprite sprite, Texture txt, Frame frame, EditingPoint editing)
        {
            //TODO do not use Setup (which only supports Scale but not Size)
            sprite.Setup(txt,
                OriginX: frame.OriginX + editing.OffsetX + 0.5f,
                OriginY: frame.OriginY + editing.OffsetY + 0.5f,
                ScaleX: frame.ScaleX / 100.0f,
                ScaleY: frame.ScaleY / 100.0f);
            sprite.SetupPosition(0, 0, frame.Rotation / 180.0f * 3.1415926f);
            sprite.SetupColor(frame.Alpha, frame.Red, frame.Green, frame.Blue);
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
            if (box.IsSelected)
            {
                rect.SetupDashRect(color, hw, hh, 3);
            }
            else
            {
                rect.SetupRect(color, hw, hh);
            }
            rect.SetupPosition(box.Left + hw, box.Top + hh, 0, box.Rotation);
        }

        public static void SetupBorder(this Sprite[] rect, Frame frame, Texture txt, EditingPoint editing)
        {
            var size = txt.GetLevelDescription(0);

            var w = size.Width * frame.ScaleX / 200.0f;
            var h = size.Height * frame.ScaleY / 200.0f;
            rect.SetupRect(0x222222, w, h);
            rect.SetupPosition(-frame.OriginX - editing.OffsetX + w,
                -frame.OriginY - editing.OffsetY + h, 0);
        }
    }
}
