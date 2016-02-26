﻿using GS_PatEditor.GSPat;
using GS_PatEditor.Pat;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GS_PatEditor
{
    class ProjectGenerater
    {
        [STAThread]
        private static void Main()
        {
            var proj = Generate();
            using (var f = File.Open(@"E:\proj.xml", FileMode.CreateNew))
            {
                Pat.PatSerialization.ProjectSerializer.Serialize(f, proj);
            }
            using (var f = File.Open(@"E:\proj_local.xml", FileMode.CreateNew))
            {
                Pat.PatSerialization.LocalSerializer.Serialize(f, proj.LocalInformation);
            }
        }
        public static Project Generate()
        {
            var patfile = OpenHomuraPat();
            if (patfile == null)
            {
                return null;
            }

            GSPatFile gspat;
            using (var file = File.OpenRead(patfile))
            {
                gspat = GSPatReader.ReadFromStream(file);
            }

            var proj = new Project();
            proj.Images = new List<FrameImage>();
            proj.Animations = new List<Pat.Animation>();
            proj.Actions = new List<Pat.Action>();
            proj.Actors = new List<Pat.Actor>();
            proj.Settings = new ProjectSettings()
            {
                ProjectName = Path.GetFileNameWithoutExtension(patfile),
                Directories = new List<ProjectDirectoryDesc>()
                {
                    new ProjectDirectoryDesc
                    {
                        Name = "images",
                        Usage = ProjectDirectoryUsage.Image,
                    }
                },
                Palettes = new List<string>()
                {
                    "palette000.pal",
                },
            };

            proj.LocalInformation = new ProjectLocalInfo
            {
                Directories = new List<ProjectDirectoryPath>()
                {
                    new ProjectDirectoryPath
                    {
                        Name = "images",
                        Path = Path.GetDirectoryName(patfile),
                    }
                },
            };

            //import animation 0
            GSPat.Animation animation0 = gspat.Animations[0];
            ImportSimpleAnimation(proj, gspat, animation0, "walk");

            CheckImageResources(proj, Path.GetDirectoryName(patfile));
            return proj;
        }
        private static void CheckImageResources(Project proj, string dir)
        {
            for (int i = 0; i < proj.Images.Count; ++i)
            {
                var res = proj.Images[i].Resource.ResourceID;
                var fullPath = Path.Combine(dir, res);
                if (File.Exists(fullPath))
                {
                    continue;
                }
                if (Path.GetExtension(res) == ".png")
                {
                    if (File.Exists(Path.ChangeExtension(fullPath, ".cv2")))
                    {
                        proj.Images[i].Resource.ResourceID = Path.ChangeExtension(res, ".cv2");
                        continue;
                    }
                }
                //TODO warning
            }
        }
        private static string OpenHomuraPat()
        {
            var defaultPath = @"E:\Games\[game]GRIEFSYNDROME\griefsyndrome\gs00\data\actor\homura\homura.pat";
            OpenFileDialog dialog = new OpenFileDialog()
            {
                Filter = "homura.pat|homura.pat",

            };
            if (File.Exists(defaultPath))
            {
                dialog.InitialDirectory = Path.GetDirectoryName(defaultPath);
            }
            if (dialog.ShowDialog() == DialogResult.Cancel)
            {
                return null;
            }
            return dialog.FileName;
        }
        private static void ImportSimpleAnimation(Pat.Project proj, GSPat.GSPatFile pat, GSPat.Animation animation, string id)
        {
            var patAnimation = new Pat.Animation
            {
                AnimationID = id,
                Segments = new List<AnimationSegment>()
                {
                    ImportSegment(proj, pat, animation),
                },
            };
            proj.Animations.Add(patAnimation);
        }
        private static AnimationSegment ImportSegment(Pat.Project proj, GSPat.GSPatFile pat, GSPat.Animation animation)
        {
            return new AnimationSegment()
            {
                Damage = ImportDamageInfo(animation),
                CancelLevel = animation.CancelLevel == 0 ? CancelLevel.Free :
                                animation.CancelLevel == 10 ? CancelLevel.Light :
                                animation.CancelLevel == 30 ? CancelLevel.Long :
                                animation.CancelLevel == 31 ? CancelLevel.Heavy :
                                animation.CancelLevel == 50 ? CancelLevel.Magic : CancelLevel.None,
                IsLoop = animation.IsLoop,
                JumpCancellable = ImportCancellable(animation, 0x200000),
                SkillCancellable = ImportCancellable(animation, 0x20),
                Frames = animation.Frames.Select(f => ImportFrame(proj, pat, f)).ToList(),
            };
        }
        private static Pat.Frame ImportFrame(Pat.Project proj, GSPat.GSPatFile pat, GSPat.Frame frame)
        {
            return new Pat.Frame
            {
                ImageID = AddImageToProject(proj, pat.Images[frame.SpriteID], frame).ImageID,
                OriginX = frame.OriginX,
                OriginY = frame.OriginY,
                ScaleX = frame.ImageManipulation != null ? frame.ImageManipulation.ScaleX : 100,
                ScaleY = frame.ImageManipulation != null ? frame.ImageManipulation.ScaleY : 100,
                Duration = frame.DisplayTime,
                Rotate = frame.ImageManipulation != null ? frame.ImageManipulation.Rotation : 0,
                PhysicalBox = ImportPhysicalBox(frame.PhysicsBox),
                HitBoxes = frame.HitBoxes.Select(ImportBox).ToList(),
                AttackBoxes = frame.AttackBoxes.Select(ImportBox).ToList(),
                Points = new List<FramePoint>()
                {
                    ImportPoint(frame.Point0),
                    ImportPoint(frame.Point1),
                    ImportPoint(frame.Point2),
                },
            };
        }
        private static Pat.FramePoint ImportPoint(GSPat.PointReference point)
        {
            if (point == null)
            {
                return new FramePoint { X = 0, Y = 0 };
            }
            return new FramePoint { X = point.X, Y = point.Y };
        }
        private static Pat.Box ImportBox(GSPat.Box box)
        {
            if (box == null)
            {
                return null;
            }
            return new Pat.Box
            {
                X = Math.Min(box.X1, box.X2),
                Y = Math.Min(box.Y1, box.Y2),
                W = Math.Abs(box.X1 - box.X2),
                H = Math.Abs(box.Y1 - box.Y2),
                R = box.Rotation,
            };
        }
        private static Pat.PhysicalBox ImportPhysicalBox(GSPat.PhysicsBox box)
        {
            if (box == null)
            {
                return null;
            }
            return new PhysicalBox
            {
                X = Math.Min(box.X1, box.X2),
                Y = Math.Min(box.Y1, box.Y2),
                W = Math.Abs(box.X1 - box.X2),
                H = Math.Abs(box.Y1 - box.Y2),
            };
        }
        private static AnimationCancellableInfo ImportCancellable(GSPat.Animation animation, int mask)
        {
            var first = animation.Frames.FindIndex(f => (f.StateFlag & mask) != 0);
            if (first == -1)
            {
                return null;
            }
            return new AnimationCancellableInfo { StartFrom = animation.Frames.Take(first).Sum(f => f.DisplayTime) };
        }
        private static AnimationDamageInfo ImportDamageInfo(GSPat.Animation animation)
        {
            var atkIndex = animation.Frames.FindIndex(f => f.AttackBoxes != null && f.AttackBoxes.Count > 0);
            if (atkIndex == -1)
            {
                return null;
            }
            var atk = animation.Frames[atkIndex];
            var atkLength = animation.Frames
                .Skip(atkIndex)
                .TakeWhile(f => f.AttackBoxes == null || f.AttackBoxes.Count == 0)
                .Sum(f => f.DisplayTime);
            return new AnimationDamageInfo()
            {
                AttackType = atk.AttackType == 0 ? AttackType.None :
                                atk.AttackType == 1 ? AttackType.Light :
                                atk.AttackType == 2 ? AttackType.Heavy :
                                atk.AttackType == 3 ? AttackType.Smash : AttackType.None,
                BaseDamage = atk.Damage,
                HitStop = new HitStop { Self = atk.HitstopSelf, Opponent = atk.HitstopOpponent },
                Knockback = new HitKnockback { SpeedX = atk.HitVX, SpeedY = atk.HitVY, Gravity = atk.HitG },
                SoundEffect = atk.HitSoundEffect,
                Duration = atkLength,
            };
        }
        private static Pat.FrameImage AddImageToProject(Project proj, string filename, GSPat.Frame frame)
        {
            var find = proj.Images.FirstOrDefault(
                img => 
                    img.Resource.ResourceID == filename &&
                    img.X == frame.ViewOffsetX && img.Y == frame.ViewOffsetY &&
                    img.W == frame.ViewWidth && img.H == frame.ViewHeight
            );
            if (find != null)
            {
                return find;
            }
            var id = MakeImageID(proj, filename);
            var ret = new Pat.FrameImage()
            {
                ImageID = id,
                AlphaBlendMode = false, //not supported
                Resource = new ResourceReference { ResourceID = filename },
                X = frame.ViewOffsetX,
                Y = frame.ViewOffsetY,
                W = frame.ViewWidth,
                H = frame.ViewHeight,
            };
            proj.Images.Add(ret);
            return ret;
        }
        private static string MakeImageID(Project proj, string filename)
        {
            var defName = Path.GetFileNameWithoutExtension(filename);
            if (!proj.Images.Any(img => img.ImageID == defName))
            {
                return defName;
            }
            int i = 2;
            while (proj.Images.Any(img => img.ImageID == defName + "_" + i.ToString()))
            {
                ++i;
            }
            return defName + "_" + i.ToString();
        }
    }
}
