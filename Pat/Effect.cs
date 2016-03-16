using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace GS_PatEditor.Pat
{
    [Serializable]
    public abstract class Effect
    {
        public abstract void Run(Simulation.Actor actor);
    }

    [Serializable]
    public abstract class Filter
    {
        public abstract bool Test(Simulation.Actor actor);
    }

    [Serializable]
    public class FilteredEffect : Effect
    {
        [XmlElement]
        public Filter Filter;
        [XmlElement]
        public Effect Effect;

        public override void Run(Simulation.Actor actor)
        {
            if (Filter.Test(actor))
            {
                Effect.Run(actor);
            }
        }
    }

    [Serializable]
    public abstract class EffectListEffect : Effect
    {
        protected abstract IEnumerable<Effect> Effects
        {
            get;
        }

        public override void Run(Simulation.Actor actor)
        {
            foreach (var effect in Effects)
            {
                effect.Run(actor);
            }
        }
    }

    [Serializable]
    public class SimpleListEffect : EffectListEffect
    {
        [XmlElement(ElementName = "Effect")]
        public readonly List<Effect> EffectList = new List<Effect>();

        protected override IEnumerable<Effect> Effects
        {
            get
            {
                return EffectList;
            }
        }
    }

    [Serializable]
    public abstract class FilterListFilter : Filter
    {
        protected abstract IEnumerable<Filter> Filters
        {
            get;
        }

        public override bool Test(Simulation.Actor actor)
        {
            return Filters.All(f => f.Test(actor));
        }
    }

    [Serializable]
    public class SimpleListFilter : FilterListFilter
    {
        [XmlElement(ElementName = "Filter")]
        public readonly List<Filter> FilterList = new List<Filter>();

        public SimpleListFilter() { }

        public SimpleListFilter(params Filter[] filters)
        {
            FilterList.AddRange(filters);
        }

        protected override IEnumerable<Filter> Filters
        {
            get
            {
                return FilterList;
            }
        }
    }


    //tests

    [Serializable]
    public class TestEffect : Effect
    {
        public override void Run(Simulation.Actor actor)
        {
            actor.Y -= 100;
        }
    }

    [Serializable]
    public class TestFilter : Filter
    {
        public override bool Test(Simulation.Actor actor)
        {
            return true;
        }
    }
}
