using GS_PatEditor.Editor.Exporters;
using GS_PatEditor.Editor.Exporters.CodeFormat;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace GS_PatEditor.Pat.Effects
{
    [Serializable]
    public class FrameSinglePointProvider : PointProvider
    {
        [XmlAttribute]
        public int Index { get; set; }

        public override FramePoint GetPointForActor(Simulation.Actor actor)
        {
            if (Index < 0 || Index >= actor.CurrentFrame.Points.Count)
            {
                return new FramePoint();
            }
            var p = actor.CurrentFrame.Points[Index];
            
            float scaleX = actor.ScaleX, scaleY = actor.ScaleY;

            //TODO IMPORTANT check if point should be scaled
            scaleX *= actor.CurrentFrame.ScaleX / 100.0f;
            scaleY *= actor.CurrentFrame.ScaleY / 100.0f;

            return new FramePoint
            {
                X = (int)(actor.X + scaleX * p.X),
                Y = (int)(actor.Y + scaleY * p.Y),
            };
        }

        private static string[] pointXNames = new[] { "point0_x", "point1_x", "point2_x" };
        private static string[] pointYNames = new[] { "point0_y", "point1_y", "point2_y" };

        public override Expression GenerateX(GenerationEnvironment env)
        {
            return new BiOpExpr(ThisExpr.Instance.MakeIndex(pointXNames[Index]),
                ThisExpr.Instance.MakeIndex("vx"), BiOpExpr.Op.Add);
        }

        public override Expression GenerateY(GenerationEnvironment env)
        {
            return new BiOpExpr(ThisExpr.Instance.MakeIndex(pointYNames[Index]),
                ThisExpr.Instance.MakeIndex("vy"), BiOpExpr.Op.Add);
        }
    }
}
