using GS_PatEditor.Pat.Effects;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GS_PatEditor.Editor.Editable
{
    [EditorSelector(typeof(Pat.Effect))]
    class SelectEffect : Pat.Effect, IHideFromEditor, IEditableEnvironment
    {
        private readonly Action<Pat.Effect> _OnNewEffect;

        public SelectEffect(Action<Pat.Effect> onNewEffect)
        {
            _OnNewEffect = onNewEffect;
        }

        [TypeConverter(typeof(GenericEditorSelectorTypeConverter<Pat.Effect>))]
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
                _OnNewEffect(SelectHelper.Create<Pat.Effect>(value.Value, Environment));
            }
        }

        public override void Run(Simulation.Actor actor)
        {
        }

        [Browsable(false)]
        public EditableEnvironment Environment { get; set; }

        public override Exporters.CodeFormat.ILineObject Generate(Exporters.GenerationEnvironment env)
        {
            return Exporters.CodeFormat.SimpleLineObject.Empty;
        }
    }
}
