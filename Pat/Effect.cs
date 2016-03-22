using GS_PatEditor.Editor.EffectEditable;
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
    public abstract class PointProvider
    {
        public abstract FramePoint GetPointForActor(Simulation.Actor actor);
    }

    [Serializable]
    public abstract class Value
    {
        public abstract float Get();
        public int GetInt()
        {
            return (int)Get();
        }
    }

    [Serializable]
    public class FilteredEffect : Effect
    {
        [XmlElement]
        [EditorChildNode("Filter")]
        public Filter Filter;
        [XmlElement]
        [EditorChildNode("Effect")]
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
        [EditorChildNode(null)]
        public readonly EffectList EffectList = new EffectList();

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
        [EditorChildNode(null)]
        public readonly FilterList FilterList = new FilterList();

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

    [Serializable]
    public class EffectList : IEditableList<Effect>, IEnumerable<Effect>
    {
        [XmlElement(ElementName = "Effect")]
        public List<Effect> Effects = new List<Effect>();

        public IEnumerator<Effect> GetEnumerator()
        {
            return Effects.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return Effects.GetEnumerator();
        }

        public void Add(Effect effect)
        {
            Effects.Add(effect);
        }

        public void Remove(Effect val)
        {
            Effects.Remove(val);
        }
    }

    [Serializable]
    public class FilterList : IEditableList<Filter>, IEnumerable<Filter>
    {
        [XmlElement(ElementName = "Filter")]
        public List<Filter> Filters = new List<Filter>();

        public IEnumerator<Filter> GetEnumerator()
        {
            return Filters.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return Filters.GetEnumerator();
        }

        public void Add(Filter filter)
        {
            Filters.Add(filter);
        }

        public void AddRange(IEnumerable<Filter> filters)
        {
            Filters.AddRange(filters);
        }

        public void Remove(Filter val)
        {
            Filters.Remove(val);
        }
    }

    [Serializable]
    public class ConstValue : Value
    {
        [XmlAttribute]
        public float Value { get; set; }

        public override float Get()
        {
            return Value;
        }
    }


    //tests

    [Serializable]
    public class TestEffect : Effect
    {
        public override void Run(Simulation.Actor actor)
        {
            actor.Y -= 20;
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
