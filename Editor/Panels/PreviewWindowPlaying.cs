using GS_PatEditor.Render;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GS_PatEditor.Editor.Panels
{
    class PreviewWindowPlaying : AbstractPreviewWindowContent
    {
        private readonly Simulation.World _World;
        private readonly Editor _Parent;

        private Sprite _Sprite;

        private class PatProjectAnimationProvider : Simulation.AnimationProvider
        {
            public Pat.Project Project;
            public string DefaultAnimation;

            public Pat.Animation GetAnimationByID(string id)
            {
                return Project.Animations.FirstOrDefault(a => a.AnimationID == (id == null ? DefaultAnimation : id));
            }
        }

        public PreviewWindowPlaying(Editor parent)
        {
            _Parent = parent;
            _World = new Simulation.World(_Parent.PreviewWindowUI.ControlWidth, _Parent.PreviewWindowUI.ControlHeight);

            _World.WhenFinished += PlayingFinished;
            _World.WhenError += PlayerError;

            var animation = parent.EditorNode.Animation.Data;

            var actor = new Simulation.PlayerActor(_World,
                new PatProjectAnimationProvider { Project = parent.Data, DefaultAnimation = animation.AnimationID },
                new Simulation.SystemAnimationProvider(_Parent.Data))
            {
            };

            Simulation.AnimationSetup.SetupActorForAnimation(actor, animation);
            actor.SetMotion(animation, 0);

            var action = _Parent.Data.Actions.FirstOrDefault(a => a.ActionID == animation.ActionID);
            if (animation.ActionID != null && action != null)
            {
                Simulation.ActionSetup.SetupActorForAction(actor, action);
            }

            _World.Add(actor);

            var sprites = parent.PreviewWindowUI.SpriteManager;
            _Sprite = sprites.GetSprite(0);

            _Parent.PreviewWindowUI.PreviewMoving.ResetScaleForPlay();
        }

        private void PlayingFinished()
        {
            _Parent.EditorNode.Animation.Frame.ChangePreviewMode(Nodes.FrameNode.FramePreviewMode.Pause);
        }

        private void PlayerError()
        {
            _Parent.EditorNode.Animation.Frame.ChangePreviewMode(Nodes.FrameNode.FramePreviewMode.Pause);
        }

        public override void Render()
        {
            _World.Update();

            var window = _Parent.PreviewWindowUI;
            foreach (var actor in _World.OrderBy(a => a.Priority))
            {
                var frame = actor.CurrentFrame;
                if (frame != null && frame.ImageID != null)
                {
                    var txt = _Parent.Data.ImageList.GetTexture(frame.ImageID, _Parent.PreviewWindowUI.Render);
                    if (txt != null)
                    {
                        _Sprite.SetupActor(txt, actor, window.SpriteMoving);
                        _Sprite.Render();
                    }
                }
            }
        }
    }
}
