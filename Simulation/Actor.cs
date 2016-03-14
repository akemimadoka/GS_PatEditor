using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GS_PatEditor.Simulation
{
    enum ActorType
    {
        Player,
        Enemy,
    }

    delegate void ActorLabel(Actor actor);

    enum ActorVariableType
    {
        //Integer,
        Float,
        //Label
        ActorLabel,
        //ActorReference,
    }

    class ActorVariable
    {
        public ActorVariableType Type;
        public object Value;
    }

    abstract class Actor
    {
        public event Action<Actor> BeforeRelease, AfterRelease;

        //world
        public World World { get; private set; }
        public SystemAnimationProvider SystemAnimations { get; private set; }
        public bool IsReleased { get; private set; }
        public bool IsVisible { get; private set; }

        //rendering
        public int Priority;
        public float ScaleX, ScaleY;
        public float Rotation;

        //position
        public float DefaultGravity;
        public float X, Y;
        public float VX, VY;
        public float? Gravity;
        public bool ImmuneGravity;

        //collision
        public bool CollisionEnabled;
        public bool HitLeft, HitRight, HitTop, HitBottom;

        //
        public bool IsInAir;

        //animation
        //public ActionList Actions { get; private set; }
        public Pat.Animation CurrentAnimation { get; private set; }
        public int CurrentSegmentIndex { get; private set; }
        public int CurrentFrameIndex { get; private set; }
        public int CurrentFrameCounter { get; private set; }

        //count, used in Action Code
        public int ActionCount;

        public Pat.Frame CurrentFrame
        {
            get
            {
                return CurrentAnimation.Segments[CurrentSegmentIndex].Frames[CurrentFrameIndex];
            }
        }

        //also update ActorActions.ClearLabel
        public ActorLabel UpdateLabel;
        public ActorLabel SitLabel;
        public ActorLabel FallLabel; //not used in simulation
        public ActorLabel EndMotionLabel;
        public ActorLabel[] EndKeyFrameLabel;
        public ActorLabel HitEvent; //bullet only

        public Dictionary<string, ActorVariable> Variables = new Dictionary<string, ActorVariable>();

        public Actor(World theWorld, SystemAnimationProvider animation)
        {
            this.World = theWorld;
            this.SystemAnimations = animation;

            this.ScaleX = 1;
            this.ScaleY = 1;

            this.ImmuneGravity = true;

            this.IsInAir = true;
        }

        public abstract void Update();

        public void SetMotion(Pat.Animation animation, int segment)
        {
            CurrentAnimation = animation;
            CurrentSegmentIndex = segment;
            CurrentFrameIndex = 0;
            CurrentFrameCounter = 0;
        }

        protected void StepAnimation()
        {
            //TODO callback
            if (++CurrentFrameCounter == CurrentFrame.Duration)
            {
                CurrentFrameCounter = 0;

                var seg = CurrentAnimation.Segments[CurrentSegmentIndex];
                if (++CurrentFrameIndex == seg.Frames.Count)
                {
                    CurrentFrameIndex = 0;
                    if (!seg.IsLoop)
                    {
                        if (++CurrentSegmentIndex == CurrentAnimation.Segments.Count)
                        {

                        }
                    }
                }
            }
        }
    }
}
