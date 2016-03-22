using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GS_PatEditor.Editor.EffectEditable
{
    [EditorSelector(typeof(Pat.PointProvider))]
    class SelectPointProvider : Pat.PointProvider, Pat.Effects.IHideFromEditor
    {
        private readonly Action<Pat.PointProvider> _OnNewPointProvider;

        public SelectPointProvider(Action<Pat.PointProvider> onNewPointProvider)
        {
            _OnNewPointProvider = onNewPointProvider;
        }

        [TypeConverter(typeof(GenericEditorSelectorTypeConverter<Pat.PointProvider>))]
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
                _OnNewPointProvider((Pat.PointProvider)value.Value.GetConstructor(new Type[0]).Invoke(new object[0]));
            }
        }

        public override Pat.FramePoint GetPointForActor(Simulation.Actor actor)
        {
            return new Pat.FramePoint { X = (int)actor.X, Y = (int)actor.Y };
        }
    }
}
