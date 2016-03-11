using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GS_PatEditor
{
    class ProjectExporter
    {
        public static GSPat.GSPatFile Export(Pat.Project proj)
        {
            var ret = new GSPat.GSPatFile();

            //first export images
            ret.Images = new List<string>();
            Dictionary<string, int> imageToFile = new Dictionary<string, int>();
            Dictionary<string, Pat.FrameImage> imageIDToObj = new Dictionary<string, Pat.FrameImage>();
            foreach (var img in proj.Images)
            {
                imageToFile.Add(img.ImageID, ret.Images.Count);
                ret.Images.Add(img.Resource.ResourceID);
                imageIDToObj.Add(img.ImageID, img);
            }

            //export animations
            ret.Animations = new List<GSPat.Animation>();
            int nextAnimationID = 0;
            foreach (var animation in proj.Animations)
            {
                if (animation.Segments.Count == 0)
                {
                    continue;
                }

                {
                    var eAnimation = new GSPat.Animation()
                    {
                        AnimationID = nextAnimationID++,
                        AttackLevel = 0,
                        CancelLevel = (short)animation.Segments[0].CancelLevel,
                        IsLoop = animation.Segments[0].IsLoop,
                        Type = GSPat.AnimationType.Normal,
                    };
                    ExportFrames(eAnimation, animation.Segments[0], imageIDToObj, imageToFile);
                    ret.Animations.Add(eAnimation);
                }
                
                for (int i = 1; i < animation.Segments.Count - 1; ++i)
                {
                    var eAnimation = new GSPat.Animation()
                    {
                        AnimationID = -2,
                        AttackLevel = 0,
                        CancelLevel = (short)animation.Segments[i].CancelLevel,
                        IsLoop = animation.Segments[i].IsLoop,
                        Type = GSPat.AnimationType.Normal,
                    };
                    ExportFrames(eAnimation, animation.Segments[i], imageIDToObj, imageToFile);
                    ret.Animations.Add(eAnimation);
                }
            }

            return ret;
        }

        private static void ExportFrames(GSPat.Animation toList, Pat.AnimationSegment fromList,
            Dictionary<string, Pat.FrameImage> images,
            Dictionary<string, int> imageIndex)
        {
            toList.Frames = new List<GSPat.Frame>();
            var attackType = fromList.Damage == null ? Pat.AttackType.None : fromList.Damage.AttackType;
            var damage = fromList.Damage == null ? 0 : fromList.Damage.BaseDamage;
            var hitG = fromList.Damage == null ? 0 : fromList.Damage.Knockback.Gravity;
            var se = fromList.Damage == null ? 0 : fromList.Damage.SoundEffect;
            var hso = fromList.Damage == null ? 0 : fromList.Damage.HitStop.Opponent;
            var hss = fromList.Damage == null ? 0 : fromList.Damage.HitStop.Self;
            var hitX = fromList.Damage == null ? 0 : fromList.Damage.Knockback.SpeedX;
            var hitY = fromList.Damage == null ? 0 : fromList.Damage.Knockback.SpeedY;
            var cancelJump = fromList.JumpCancellable == null ? Int32.MaxValue : fromList.JumpCancellable.StartFrom;
            var cancelSkill = fromList.SkillCancellable == null ? Int32.MaxValue : fromList.SkillCancellable.StartFrom;

            var frameTime = 0;
            foreach (var frame in fromList.Frames)
            {
                var img = images[frame.ImageID];

                var eFrame = new GSPat.Frame()
                {
                    AttackBoxes = ExportBoxs(frame.AttackBoxes),
                    AttackFlag = 0,
                    AttackType = (short)attackType,
                    Bind = 0,
                    Damage = (short)damage,
                    DisplayTime = (short)frame.Duration,
                    HitBoxes = ExportBoxs(frame.HitBoxes),
                    HitG = (short)hitG,
                    HitSoundEffect = (short)se,
                    HitstopOpponent = (short)hso,
                    HitstopSelf = (short)hss,
                    HitVX = (short)hitX,
                    HitVY = (short)hitY,

                    //TODO
                    ImageManipulation = null,

                    OriginX = (short)frame.OriginX,
                    OriginY = (short)frame.OriginY,
                    PhysicsBox = ExportPhysical(frame.PhysicalBox),
                    Point0 = new GSPat.PointReference(),
                    Point1 = new GSPat.PointReference(),
                    Point2 = new GSPat.PointReference(),
                    SpriteID = imageIndex[frame.ImageID],
                    StateFlag = (frameTime >= cancelSkill ? 0x20 : 0) + (frameTime >= cancelJump ? 0x200000 : 0),
                    ViewHeight = (short)img.H,
                    ViewOffsetX = (short)img.X,
                    ViewOffsetY = (short)img.Y,
                    ViewWidth = (short)img.W,
                };
                toList.Frames.Add(eFrame);

                frameTime += frame.Duration;
            }
        }

        private static List<GSPat.Box> ExportBoxs(List<Pat.Box> boxes)
        {
            return boxes.Select(b => new GSPat.Box()
                {
                    X1 = b.X,
                    X2 = b.X + b.W,
                    Y1 = b.Y,
                    Y2 = b.Y + b.H,
                    Rotation = b.R,
                }).ToList();
        }

        private static GSPat.PhysicsBox ExportPhysical(Pat.PhysicalBox box)
        {
            if (box == null)
            {
                return null;
            }
            return new GSPat.PhysicsBox()
            {
                X1 = box.X,
                X2 = box.X + box.W,
                Y1 = box.Y,
                Y2 = box.Y + box.H,
            };
        }
    }
}
