using GS_PatEditor.Pat.Effects;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GS_PatEditor.Editor.Editable
{
    [EditorSelector(typeof(Pat.Behavior))]
    class SelectBehavior : Pat.Behavior, IHideFromEditor, IEditableEnvironment
    {
        private readonly Action<Pat.Behavior> _OnNewBehavior;

        public SelectBehavior(Action<Pat.Behavior> onNewBehavior)
        {
            _OnNewBehavior = onNewBehavior;
        }

        [TypeConverter(typeof(GenericEditorSelectorTypeConverter<Pat.Behavior>))]
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
                _OnNewBehavior(SelectHelper.Create<Pat.Behavior>(value.Value, Environment));
            }
        }

        [Browsable(false)]
        public EditableEnvironment Environment { get; set; }

        public override void MakeEffects(Pat.ActionEffects effects)
        {
        }
    }
}
