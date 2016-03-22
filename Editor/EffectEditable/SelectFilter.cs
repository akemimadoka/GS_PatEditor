using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GS_PatEditor.Editor.EffectEditable
{
    [EditorSelector(typeof(Pat.Filter))]
    class SelectFilter : Pat.Filter, Pat.Effects.IHideFromEditor
    {
        private readonly Action<Pat.Filter> _OnNewFilter;

        public SelectFilter(Action<Pat.Filter> onNewFilter)
        {
            _OnNewFilter = onNewFilter;
        }

        [TypeConverter(typeof(GenericEditorSelectorTypeConverter<Pat.Filter>))]
        public SelectType Type
        {
            get
            {
                return null;
            }
            set
            {
                if (value == null || value.Value == null)
                {
                    return;
                }
                //TODO error handling
                _OnNewFilter((Pat.Filter)value.Value.GetConstructor(new Type[0]).Invoke(new object[0]));
            }
        }

        public override bool Test(Simulation.Actor actor)
        {
            return false;
        }
    }
}
