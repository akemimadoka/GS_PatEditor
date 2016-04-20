using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GS_PatEditor.Simulation
{
    class PhyisicalCollisionDetector
    {
        private float left, top, width, height;

        //results
        public bool HitGround;

        public PhyisicalCollisionDetector(int left, int top, int width, int height)
        {
            this.left = left;
            this.top = top;
            this.width = width;
            this.height = height;
        }

        public void PutActorOnGround(Actor actor)
        {
            var frame = actor.CurrentFrame;
            if (actor.CollisionEnabled == false || frame.PhysicalBox == null)
            {
                return;
            }
            var box = frame.PhysicalBox;

            //TODO check whether physics box is really scaled
            float scaleX = actor.ScaleX, scaleY = actor.ScaleY;
            float cleft = box.X * scaleX, cright = (box.X + box.W) * scaleX;
            float ctop = box.Y * scaleX, cbottom = (box.Y + box.H) * scaleY;

            actor.Y = top + height - cbottom;
            actor.VY = 0;
            actor.IsInAir = false;
        }

        public void TestActor(Actor actor)
        {
            //TODO support collisionMask

            HitGround = false; //!actor.IsInAir;

            var frame = actor.CurrentFrame;
            if (actor.CollisionEnabled == false || frame.PhysicalBox == null)
            {
                actor.X += actor.VX;
                actor.Y += actor.VY;
                return;
            }
            var box = frame.PhysicalBox;

            //TODO check whether physics box is really scaled
            float scaleX = actor.ScaleX, scaleY = actor.ScaleY;
            scaleX *= frame.ScaleX / 100.0f;
            scaleY *= frame.ScaleY / 100.0f;
            float cleft = box.X * scaleX, cright = (box.X + box.W) * scaleX;
            float ctop = box.Y * scaleX, cbottom = (box.Y + box.H) * scaleY;

            float newX = actor.X + actor.VX, newY = actor.Y + actor.VY;

            actor.HitLeft = false;
            actor.HitRight = false;
            actor.HitTop = false;
            actor.HitBottom = false;

            if (newX + cleft < left)
            {
                newX = left - cleft;
                if (actor.VX < 0)
                {
                    //TODO check if speed is reset
                    actor.VX = 0;
                    actor.HitLeft = true;
                }
            }
            if (newX + cright > left + width)
            {
                newX = left + width - cright;
                if (actor.VX > 0)
                {
                    actor.VX = 0;
                    actor.HitRight = true;
                }
            }
            if (newY + ctop < top)
            {
                newY = top - ctop;
                if (actor.VY < 0)
                {
                    actor.VY = 0;
                    actor.HitTop = true;
                }
            }
            if (newY + cbottom > top + height)
            {
                newY = top + height - cbottom;
                if (actor.VY > 0)
                {
                    actor.VY = 0;
                    actor.HitBottom = true;
                }
            }
            actor.X = newX;
            actor.Y = newY;
        }
    }
}
