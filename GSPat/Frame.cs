using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GS_PatEditor.GSPat
{
    class ImageManipulation
    {
        public short AlphaBlend;
        public byte Alpha, Red, Green, Blue;
        public short ScaleX, ScaleY;
        public short SkewX, SkewY;
        public short Rotation;
    }
    class PhysicsBox
    {
        public int X1, Y1, X2, Y2;
    }
    class Box
    {
        public int X1, Y1, X2, Y2;
        public int Rotation;
    }
    class PointReference
    {
        public short X, Y;
    }
    class Frame
    {
        public int SpriteID;
        public short ViewOffsetX, ViewOffsetY;
        public short ViewWidth, ViewHeight;
        public short OriginX, OriginY;
        public short DisplayTime;
        public ImageManipulation ImageManipulation;
        public short Damage;
        public short HitstopOpponent, HitstopSelf;
        public short Bind;
        public short HitSoundEffect;
        public short AttackType;
        public int StateFlag, AttackFlag;
        public PhysicsBox PhysicsBox;
        public List<Box> HitBoxes;
        public List<Box> AttackBoxes;
        public PointReference Point0, Point1, Point2;
        public short HitVX, HitVY, HitG;
    }
}
