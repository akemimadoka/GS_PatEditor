using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GS_PatEditor.GSPat
{
    class GSPatWriter
    {
        public static void Write(GSPatFile file, BinaryWriter writer)
        {
            writer.Write((byte)5);

            writer.Write((short)file.Images.Count);
            var encoding = Encoding.GetEncoding(932);
            byte[] buffer = new byte[0x80];
            foreach (var img in file.Images)
            {
                int c = encoding.GetBytes(img, 0, img.Length, buffer, 0);
                buffer[c] = 0;
                writer.Write(buffer);
            }

            writer.Write((int)file.Animations.Count);
            foreach (var animation in file.Animations)
            {
                writer.Write((int)animation.AnimationID);

                if (animation.Type == AnimationType.Clone)
                {
                    writer.Write((short)animation.CloneFrom);
                    writer.Write((short)animation.CloneTo);
                    continue;
                }

                writer.Write((short)animation.AttackLevel);
                writer.Write((short)animation.CancelLevel);
                writer.Write((bool)animation.IsLoop);

                writer.Write((int)animation.Frames.Count);
                foreach (var f in animation.Frames)
                {
                    WriteFrame(writer, f);
                }
            }
        }

        private static void WriteFrame(BinaryWriter bw, Frame frame)
        {
            bw.Write((int)frame.SpriteID);
            bw.Write((short)frame.ViewOffsetX);
            bw.Write((short)frame.ViewOffsetY);
            bw.Write((short)frame.ViewWidth);
            bw.Write((short)frame.ViewHeight);
            bw.Write((short)frame.OriginX);
            bw.Write((short)frame.OriginY);
            bw.Write((short)frame.DisplayTime);

            WriteIM(bw, frame.ImageManipulation);

            bw.Write((short)frame.Damage);
            bw.Write((short)frame.HitstopOpponent);
            bw.Write((short)frame.HitstopSelf);
            bw.Write((short)frame.Bind);
            bw.Write((short)0); //1
            bw.Write((short)0);
            bw.Write((short)0);
            bw.Write((short)0);
            bw.Write((short)0); //5
            bw.Write((short)0);
            bw.Write((short)0);
            bw.Write((short)0);
            bw.Write((short)0); //9
            bw.Write((short)0);
            bw.Write((short)0);
            bw.Write((short)0);
            bw.Write((short)0); //13
            bw.Write((short)frame.HitSoundEffect);
            bw.Write((short)frame.AttackType);
            bw.Write((short)0);
            bw.Write((byte)0);
            bw.Write((int)frame.StateFlag);
            bw.Write((int)frame.AttackFlag);

            bw.Write((bool)(frame.PhysicsBox != null));
            if (frame.PhysicsBox != null)
            {
                bw.Write((int)frame.PhysicsBox.X1);
                bw.Write((int)frame.PhysicsBox.Y1);
                bw.Write((int)frame.PhysicsBox.X2);
                bw.Write((int)frame.PhysicsBox.Y2);
            }
            WriteBoxes(bw, frame.HitBoxes);
            WriteBoxes(bw, frame.AttackBoxes);
            WritePoint(bw, frame.Point0);
            WritePoint(bw, frame.Point1);
            WritePoint(bw, frame.Point2);
            bw.Write((short)frame.HitVX);
            bw.Write((short)frame.HitVY);
            bw.Write((short)frame.HitG);
        }

        private static void WriteIM(BinaryWriter bw, ImageManipulation im)
        {
            bw.Write((byte)(im != null ? 2 : 0));
            if (im != null)
            {
                bw.Write((short)im.AlphaBlend);
                bw.Write((byte)im.Alpha);
                bw.Write((byte)im.Red);
                bw.Write((byte)im.Green);
                bw.Write((byte)im.Blue);
                bw.Write((short)im.ScaleX);
                bw.Write((short)im.ScaleY);
                bw.Write((short)im.SkewX);
                bw.Write((short)im.SkewY);
                bw.Write((short)im.Rotation);
            }
        }

        private static void WriteBoxes(BinaryWriter bw, List<Box> boxes)
        {
            bw.Write((byte)boxes.Count);
            foreach (var b in boxes)
            {
                bw.Write((int)b.X1);
                bw.Write((int)b.Y1);
                bw.Write((int)b.X2);
                bw.Write((int)b.Y2);
                bw.Write((short)b.Rotation);
            }
        }

        private static void WritePoint(BinaryWriter bw, PointReference p)
        {
            bw.Write((short)p.X);
            bw.Write((short)(p.X < 0 ? -1 : 0));
            bw.Write((short)p.Y);
            bw.Write((short)(p.Y < 0 ? -1 : 0));
        }
    }
}
