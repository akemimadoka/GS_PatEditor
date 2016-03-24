using GS_PatEditor.Pat.Effects;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GS_PatEditor.Editor.Editable
{
    [EditorSelector(typeof(Pat.PointProvider))]
    class SelectPointProvider : Pat.PointProvider, IHideFromEditor, IEditableEnvironment
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
                _OnNewPointProvider(SelectHelper.Create<Pat.PointProvider>(value.Value, Environment));
            }
        }

        public override Pat.FramePoint GetPointForActor(Simulation.Actor actor)
        {
            return new Pat.FramePoint { X = (int)actor.X, Y = (int)actor.Y };
        }


        [Browsable(false)]
        public EditableEnvironment Environment { get; set; }
    }
}
