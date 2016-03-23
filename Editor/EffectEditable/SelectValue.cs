﻿using GS_PatEditor.Pat.Effects;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GS_PatEditor.Editor.EffectEditable
{
    [EditorSelector(typeof(Pat.Value))]
    class SelectValue : Pat.Value, IHideFromEditor, IEditableEnvironment
    {
        private readonly Action<Pat.Value> _OnNewValue;

        public SelectValue(Action<Pat.Value> onNewValue)
        {
            _OnNewValue = onNewValue;
        }

        [TypeConverter(typeof(GenericEditorSelectorTypeConverter<Pat.Value>))]
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
                _OnNewValue(SelectHelper.Create<Pat.Value>(value.Value, Environment));
            }
        }

        public override float Get(Simulation.Actor actor)
        {
            return 0.0f;
        }

        [Browsable(false)]
        public EditableEnvironment Environment { get; set; }
    }
}
