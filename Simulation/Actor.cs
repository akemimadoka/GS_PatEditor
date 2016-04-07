using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GS_PatEditor.Simulation
{
    public enum ActorLabelType
    {
        Fall,
        Sit,
        Hit,
    }

    public delegate void ActorLabel(Actor actor);

    public enum ActorVariableType
    {
        //Integer,
        Float,
        //Label
        ActorLabel,
        //ActorReference,
    }

    public class ActorVariable
    {
        public ActorVariableType Type;
        public object Value;
    }

    public abstract class Actor
    {
        //world
        public World World { get; private set; }

        public SystemAnimationProvider SystemAnimations { get; private set; }
        public AnimationProvider Animations { get; private set; }
        public ActionProvider Actions { get; private set; }

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
        public Pat.Action CurrentAction { get; private set; }
        public int CurrentSegmentIndex { get; private set; }
        public int CurrentFrameIndex { get; private set; }
        public int CurrentFrameCounter { get; private set; }

        //count, used in Action Code
        public int ActionCount;

        public Pat.Frame CurrentFrame
        {
            get
            {
                if (CurrentAction == null || CurrentAction.Segments.Count <= CurrentSegmentIndex ||
                    CurrentAction.Segments[CurrentSegmentIndex].Frames.Count <= CurrentFrameIndex)
                {
                    return Pat.Frame.EmptyFrame;
                }
                return CurrentAction.Segments[CurrentSegmentIndex].Frames[CurrentFrameIndex];
            }
        }

        //also update ActorActions.ClearLabel
        public ActorLabel UpdateLabel;
        public ActorLabel SitLabel;
        public ActorLabel FallLabel; //not used in simulation
        public ActorLabel[] EndKeyFrameLabel; //TODO IMPORTANT if it will be executed in looping animation, if EndMotion will be executed in looping animation
        public ActorLabel HitEvent; //bullet only

        public Dictionary<string, ActorVariable> Variables = new Dictionary<string, ActorVariable>();

        public Actor(World theWorld, AnimationProvider animations, SystemAnimationProvider sysanimations,
            ActionProvider actions)
        {
            this.World = theWorld;

            this.Animations = animations;
            this.SystemAnimations = sysanimations;
            this.Actions = actions;

            this.ScaleX = 1;
            this.ScaleY = 1;

            this.ImmuneGravity = true;

            this.IsInAir = true;
        }

        public abstract void Update();

        public void SetMotion(SystemAnimationType sys, int segment)
        {
            SetMotion(SystemAnimations.GetSystemAction(sys), segment);
        }

        public void SetMotion(string id, int segment)
        {
            SetMotion(Animations.GetActionByID(id), segment);
        }

        public void SetMotion(Pat.Action action, int segment)
        {
            if (action == null)
            {
                World.OnError();
                return;
            }

            CurrentAction = action;
            CurrentSegmentIndex = segment;
            CurrentFrameIndex = 0;
            CurrentFrameCounter = 0;
        }

        public void Release()
        {
            IsReleased = true;
        }

        protected void StepAnimation()
        {
            if (++CurrentFrameCounter == CurrentFrame.Duration)
            {
                CurrentFrameCounter = 0;

                var seg = CurrentAction.Segments[CurrentSegmentIndex];

                //next frame
                if (++CurrentFrameIndex == seg.Frames.Count)
                {
                    //CurrentFrameIndex = 0;
                    //make a flag on CurrentFrameIndex
                    CurrentFrameIndex = -1;

                    //next segment or loop

                    //anyway, first trigget key callback
                    if (EndKeyFrameLabel != null && CurrentSegmentIndex < EndKeyFrameLabel.Length)
                    {
                        EndKeyFrameLabel[CurrentSegmentIndex](this);
                    }

                    if (CurrentFrameIndex == -1)
                    {
                        //SetMotion not called in EndKeyFrameLabel
                        CurrentFrameIndex = 0;

                        if (!seg.IsLoop)
                        {
                            //next segment
                            ++CurrentSegmentIndex;
                        }

                        if (CurrentSegmentIndex == CurrentAction.Segments.Count)
                        {
                            //end of animation
                            World.OnFinished();
                        }
                    }
                }
            }
        }

        protected void UpdateGravity()
        {
            if (!ImmuneGravity && IsInAir)
            {
                if (Gravity.HasValue)
                {
                    VY += Gravity.Value;
                    Gravity = null;
                }
                else
                {
                    VY += DefaultGravity;
                }
                if (VY > 15)
                {
                    VY = 15;
                }
            }
        }

        protected void RunUpdateLabel()
        {
            if (UpdateLabel != null)
            {
                UpdateLabel(this);
            }
        }
    }
}
