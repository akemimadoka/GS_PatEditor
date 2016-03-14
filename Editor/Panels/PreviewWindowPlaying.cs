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

        public PreviewWindowPlaying(Editor parent)
        {
            _Parent = parent;
            _World = new Simulation.World();
            var animation = parent.EditorNode.Animation.Data;

            var actor = new Simulation.PlayerActor(_World, new Simulation.SystemAnimationProvider())
            {
                Y = -100.0f,
            };

            Simulation.AnimationSetup.SetupActorForAnimation(actor, animation);
            actor.SetMotion(animation, 0);
            _World.Add(actor);

            var sprites = parent.PreviewWindowUI.SpriteManager;
            _Sprite = sprites.GetSprite(0);

            _Parent.PreviewWindowUI.PreviewMoving.ResetScaleForPlay();
        }

        public override void Render()
        {
            _World.Update();

            var window = _Parent.PreviewWindowUI;
            foreach (var actor in _World)
            {
                var frame = actor.CurrentFrame;
                if (frame != null && frame.ImageID != null)
                {
                    var txt = _Parent.Data.ImageList.GetTexture(frame.ImageID, _Parent.PreviewWindowUI.Render);
                    if (txt != null)
                    {
                        _Sprite.SetupFrame(txt, frame, window.SpriteMoving);
                        _Sprite.Left = actor.X;
                        _Sprite.Top = actor.Y;
                        _Sprite.Render();
                    }
                }
            }
        }
    }
}
