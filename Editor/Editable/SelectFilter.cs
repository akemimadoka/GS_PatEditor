using GS_PatEditor.Pat.Effects;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GS_PatEditor.Editor.Editable
{
    [EditorSelector(typeof(Pat.Filter))]
    class SelectFilter : Pat.Filter, IHideFromEditor, IEditableEnvironment
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
                _OnNewFilter(SelectHelper.Create<Pat.Filter>(value.Value, Environment));
            }
        }

        public override bool Test(Simulation.Actor actor)
        {
            return false;
        }


        [Browsable(false)]
        public EditableEnvironment Environment { get; set; }
    }
}
