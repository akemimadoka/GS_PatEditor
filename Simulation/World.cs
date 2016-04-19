using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GS_PatEditor.Simulation
{
    public class World : IEnumerable<Actor>
    {
        public Random Random = new Random();

        public event Action WhenError;
        public event Action WhenFinished;

        private readonly List<Actor> _Actors = new List<Actor>();
        private readonly List<Actor> _AddActors = new List<Actor>();
        private readonly List<Actor> _RemoveActors = new List<Actor>();

        private readonly PhyisicalCollisionDetector _Physical;

        public World(int width, int height)
        {
            _Physical = new PhyisicalCollisionDetector(-width / 2, -height, width, height);
        }

        #region actor list access

        public IEnumerator<Actor> GetEnumerator()
        {
            return _Actors.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return _Actors.GetEnumerator();
        }

        #endregion

        public void Add(Actor actor)
        {
            _AddActors.Add(actor);
        }

        public void Update()
        {
            //should check if it is released
            foreach (var actor in this)
            {
                actor.Update();

                if (actor.IsReleased)
                {
                    _RemoveActors.Add(actor);
                    continue;
                }

                _Physical.TestActor(actor);

                if (actor.IsInAir && actor.HitBottom)
                {
                    if (actor.SitLabel != null)
                    {
                        actor.SitLabel(actor);
                    }
                }
            }

            foreach (var actor in _AddActors)
            {
                _Actors.Add(actor);
            }
            _AddActors.Clear();
            foreach (var actor in _RemoveActors)
            {
                _Actors.Remove(actor);
            }
            _RemoveActors.Clear();
        }

        public void OnError()
        {
            if (WhenError != null)
            {
                WhenError();
            }
        }

        public void OnFinished()
        {
            if (WhenFinished != null)
            {
                WhenFinished();
            }
        }
    }
}
