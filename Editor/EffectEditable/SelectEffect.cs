using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GS_PatEditor.Editor.EffectEditable
{
    [EditorSelector(typeof(Pat.Effect))]
    class SelectEffect : Pat.Effect, Pat.Effects.IHideFromEditor
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
                //TODO error handling
                _OnNewEffect((Pat.Effect)value.Value.GetConstructor(new Type[0]).Invoke(new object[0]));
            }
        }

        public override void Run(Simulation.Actor actor)
        {
        }
    }
}
