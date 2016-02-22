using GS_PatEditor.Pat;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace GS_PatEditor
{
    class PatXmlSerializationTest
    {
        private static Project CreateProject()
        {
            Project proj = new Project();
            proj.Settings = new ProjectSettings()
            {
                ProjectName = "TestProject",
                Directories = new List<ProjectDirectoryDesc>()
                {
                    new ProjectDirectoryDesc { Name = "images", Usage = ProjectDirectoryUsage.Image },
                    new ProjectDirectoryDesc { Name = "sounds", Usage = ProjectDirectoryUsage.SoundEffect },
                },
            };
            proj.Images = new List<FrameImage>()
            {
                new FrameImage { ImageID = "Walk000", Resource = new ResourceReference { ResourceID = "Walk000.png" }, W = -1, H = -1 },
                new FrameImage { ImageID = "Walk001", Resource = new ResourceReference { ResourceID = "Walk001.png" }, W = -1, H = -1 },
            };
            proj.Animations = new List<Animation>()
            {
                new Animation {
                    AnimationID = "Walk",
                    Segments = new List<AnimationSegment>() {
                        new AnimationSegment() {
                            Frames = new List<Frame>() {
                                new Frame() {
                                    ImageID = "Walk000",
                                    ScaleX = 100,
                                    ScaleY = 100,
                                    Rotate = 0,
                                    OriginX = 98,
                                    OriginY = 34,
                                    Duration = 3,
                                    Points = new List<FramePoint>(),
                                    PhysicalBox = new PhysicalBox { X = -20, W = 40, Y = -80, H = 80 },
                                    HitBoxes = new List<Box>() { new Box { X = -20, W = 40, Y = -80, H = 80, R = 0 } },
                                    AttackBoxes = new List<Box>(),
                                },
                                new Frame() {
                                    ImageID = "Walk001",
                                    ScaleX = 100,
                                    ScaleY = 100,
                                    Rotate = 0,
                                    OriginX = 98,
                                    OriginY = 34,
                                    Duration = 3,
                                    Points = new List<FramePoint>(),
                                    PhysicalBox = new PhysicalBox { X = -20, W = 40, Y = -80, H = 80 },
                                    HitBoxes = new List<Box>() { new Box { X = -20, W = 40, Y = -80, H = 80, R = 0 } },
                                    AttackBoxes = new List<Box>(),
                                },
                            },
                            JumpCancellable = new AnimationCancellableInfo { StartFrom = 0 },
                            SkillCancellable = new AnimationCancellableInfo { StartFrom = 0 },
                            CancelLevel = CancelLevel.Free,
                            Damage = null,
                        },
                        new AnimationSegment() {
                            Frames = new List<Frame>() {
                                new Frame() {
                                    ImageID = "Walk002",
                                    ScaleX = 100,
                                    ScaleY = 100,
                                    Rotate = 0,
                                    OriginX = 98,
                                    OriginY = 34,
                                    Duration = 3,
                                    Points = new List<FramePoint>(),
                                    PhysicalBox = new PhysicalBox { X = -20, W = 40, Y = -80, H = 80 },
                                    HitBoxes = new List<Box>() { new Box { X = -20, W = 40, Y = -80, H = 80, R = 0 } },
                                    AttackBoxes = new List<Box>(),
                                },
                                new Frame() {
                                    ImageID = "Walk003",
                                    ScaleX = 100,
                                    ScaleY = 100,
                                    Rotate = 0,
                                    OriginX = 98,
                                    OriginY = 34,
                                    Duration = 3,
                                    Points = new List<FramePoint>(),
                                    PhysicalBox = new PhysicalBox { X = -20, W = 40, Y = -80, H = 80 },
                                    HitBoxes = new List<Box>() { new Box { X = -20, W = 40, Y = -80, H = 80, R = 0 } },
                                    AttackBoxes = new List<Box>(),
                                },
                            },
                            JumpCancellable = new AnimationCancellableInfo { StartFrom = 0 },
                            SkillCancellable = new AnimationCancellableInfo { StartFrom = 0 },
                            CancelLevel = CancelLevel.Free,
                            Damage = null,
                        },
                    },
                },
            };
            proj.Actions = new List<Pat.Action>()
            {
                new Pat.Action
                {
                    InitEffects = new List<Effect>()
                    {
                        new TestEffect(),
                        new FilteredEffect { Filter = new TestFilter(), Effect = new TestEffect() },
                    }
                },
            };
            return proj;
        }

        private static void Main()
        {
            var types = System.Reflection.Assembly.GetExecutingAssembly().GetTypes().Where(
                t => (typeof(Effect).IsAssignableFrom(t) || typeof(Filter).IsAssignableFrom(t)) &&
                    !t.IsAbstract).ToArray();
            var writer = new XmlSerializer(typeof(Project), types);
            var dest = new StringWriter();
            writer.Serialize(dest, CreateProject());
            var str = dest.ToString();
            var obj = (Project)writer.Deserialize(new StringReader(str));
        }
    }
}
