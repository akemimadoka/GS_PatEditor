using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GS_PatEditor.Simulation
{
    class World : IEnumerable<Actor>
    {
        private readonly List<Actor> _Actors = new List<Actor>();

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
            _Actors.Add(actor);
        }

        public void Update()
        {
            //should check if it is released
            foreach (var actor in this)
            {
                actor.Update();
            }
        }
    }
}
