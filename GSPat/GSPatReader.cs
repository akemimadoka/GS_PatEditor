using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GS_PatEditor.GSPat
{
    class GSPatReader
    {
        public static GSPatFile ReadFromStream(Stream stream)
        {
            using (var br = new BinaryReader(stream))
            {
                if (br.ReadByte() != 5)
                {
                    throw new Exception("File format incorrect.");
                }

                var ret = new GSPatFile
                {
                    Images = new List<string>(),
                    Animations = new List<Animation>(),
                };

                {
                    int imageCount = br.ReadInt16();
                    ret.Images.Capacity = imageCount;

                    var encoding = Encoding.GetEncoding(932);
                    byte[] buffer = new byte[0x80];
                    for (int i = 0; i < imageCount; ++i)
                    {
                        if (br.Read(buffer, 0, 0x80) != 0x80)
                        {
                            throw new Exception("File format incorrect.");
                        }
                        int countBytes = Array.FindIndex(buffer, b => b == 0);
                        ret.Images.Add(encoding.GetString(buffer, 0, countBytes));
                    }
                }
                {
                    int animationCount = br.ReadInt32();
                    ret.Animations.Capacity = animationCount;

                    for (int i = 0; i < animationCount; ++i)
                    {
                        ret.Animations.Add(ReadAnimation(br));

                        //sometimes the stream ends before getting all the animations
                        if (br.BaseStream.Position == br.BaseStream.Length)
                        {
                            break;
                        }
                    }
                }

                return ret;
            }
        }

        private static Animation ReadAnimation(BinaryReader br)
        {
            var ret = new Animation();

            int animationID = br.ReadInt32();
            ret.AnimationID = animationID;

            if (animationID == -1)
            {
                ret.Type = AnimationType.Clone;
                ret.CloneFrom = (short)br.ReadInt32();
                ret.CloneTo = (short)br.ReadInt32();
                return ret;
            }

            ret.AttackLevel = br.ReadInt16();
            ret.CancelLevel = br.ReadInt16();
            ret.IsLoop = br.ReadBoolean();

            var frameCount = br.ReadInt32();
            ret.Frames = new List<Frame>(frameCount);
            for (int i = 0; i < frameCount; ++i)
            {
                ret.Frames.Add(ReadFrame(br));
            }

            return ret;
        }

        private static Frame ReadFrame(BinaryReader br)
        {
            var ret = new Frame();

            ret.SpriteID = br.ReadInt32();
            ret.ViewOffsetX = br.ReadInt16();
            ret.ViewOffsetY = br.ReadInt16();
            ret.ViewWidth = br.ReadInt16();
            ret.ViewHeight = br.ReadInt16();
            ret.OriginX = br.ReadInt16();
            ret.OriginY = br.ReadInt16();
            ret.DisplayTime = br.ReadInt16();

            ret.ImageManipulation = ReadIM(br);

            ret.Damage = br.ReadInt16();
            ret.HitstopOpponent = br.ReadInt16();
            ret.HitstopSelf = br.ReadInt16();
            ret.Bind = br.ReadInt16();
            ReadZero16(br); //1
            ReadZero16(br); //2
            ReadZero16(br); //3
            ReadZero16(br); //4
            ReadZero16(br); //5
            ReadZero16(br); //6
            ReadZero16(br); //7
            ReadZero16(br); //8
            ReadZero16(br); //9
            ReadZero16(br); //10
            ReadZero16(br); //11
            ReadZero16(br); //12
            ReadZero16(br); //13
            ret.HitSoundEffect = br.ReadInt16();
            ret.AttackType = br.ReadInt16();
            ReadZero16(br); //14
            ReadZero8(br);  //15
            ret.StateFlag = br.ReadInt32();
            ret.AttackFlag = br.ReadInt32();
            ret.PhysicsBox = ReadPhysics(br);
            {
                int hitCount = br.ReadByte();
                ret.HitBoxes = new List<Box>(hitCount);
                for (int i = 0; i < hitCount; ++i)
                {
                    ret.HitBoxes.Add(ReadBox(br));
                }
            }
            {
                int attackCount = br.ReadByte();
                ret.AttackBoxes = new List<Box>(attackCount);
                for (int i = 0; i < attackCount; ++i)
                {
                    ret.AttackBoxes.Add(ReadBox(br));
                }
            }
            ret.Point0 = ReadPoint(br);
            ret.Point1 = ReadPoint(br);
            ret.Point2 = ReadPoint(br);
            ret.HitVX = br.ReadInt16();
            ret.HitVY = br.ReadInt16();
            ret.HitG = br.ReadInt16();

            return ret;
        }

        private static ImageManipulation ReadIM(BinaryReader br)
        {
            if (br.ReadByte() != 2)
            {
                return null;
            }

            var ret = new ImageManipulation();

            ret.AlphaBlend = br.ReadInt16();
            ret.Alpha = br.ReadByte();
            ret.Red = br.ReadByte();
            ret.Green = br.ReadByte();
            ret.Blue = br.ReadByte();
            ret.ScaleX = br.ReadInt16();
            ret.ScaleY = br.ReadInt16();
            ret.SkewX = br.ReadInt16();
            ret.SkewY = br.ReadInt16();
            ret.Rotation = br.ReadInt16();

            return ret;
        }

        private static void ReadZero16(BinaryReader br)
        {
            if (br.ReadInt16() != 0)
            {
                throw new Exception("File format incorrect.");
            }
        }

        private static void ReadZero8(BinaryReader br)
        {
            if (br.ReadByte() != 0)
            {
                throw new Exception("File format incorrect.");
            }
        }

        private static PhysicsBox ReadPhysics(BinaryReader br)
        {
            if (!br.ReadBoolean())
            {
                return null;
            }

            var ret = new PhysicsBox();

            ret.X1 = br.ReadInt32();
            ret.Y1 = br.ReadInt32();
            ret.X2 = br.ReadInt32();
            ret.Y2 = br.ReadInt32();

            return ret;
        }

        private static Box ReadBox(BinaryReader br)
        {
            var ret = new Box();
            ret.X1 = br.ReadInt32();
            ret.Y1 = br.ReadInt32();
            ret.X2 = br.ReadInt32();
            ret.Y2 = br.ReadInt32();
            ret.Rotation = br.ReadInt16();
            return ret;
        }

        private static PointReference ReadPoint(BinaryReader br)
        {
            var x = br.ReadInt16();
            var sgx = br.ReadInt16();
            var y = br.ReadInt16();
            var sgy = br.ReadInt16();

            CheckSign(x, sgx);
            CheckSign(y, sgy);

            return new PointReference
            {
                X = x,
                Y = y,
            };
        }

        private static void CheckSign(short val, short sg)
        {
            if (sg == -1)
            {
                if (val >= 0)
                {
                    throw new Exception("File format incorrect.");
                }
            }
            else if (sg == 0)
            {
                if (val < 0)
                {
                    throw new Exception("File format incorrect.");
                }
            }
            else
            {
                throw new Exception("File format incorrect.");
            }
        }
    }
}
