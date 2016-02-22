using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace GS_PatEditor.Pat
{
    [Serializable]
    [XmlInclude(typeof(FilteredEffect))]
    public class Effect
    {
    }

    [Serializable]
    public class Filter
    {
    }

    [Serializable]
    public class FilteredEffect : Effect
    {
        [XmlElement]
        public Filter Filter;
        [XmlElement]
        public Effect Effect;
    }

    //tests

    [Serializable]
    public class TestEffect : Effect
    {
    }

    [Serializable]
    public class TestFilter : Filter
    {
    }
}
