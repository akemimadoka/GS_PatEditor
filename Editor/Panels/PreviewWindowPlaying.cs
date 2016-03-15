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
            _World = new Simulation.World(_Parent.PreviewWindowUI.ControlWidth, _Parent.PreviewWindowUI.ControlHeight);
            var animation = parent.EditorNode.Animation.Data;

            var actor = new Simulation.PlayerActor(_World, new Simulation.SystemAnimationProvider())
            {
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
