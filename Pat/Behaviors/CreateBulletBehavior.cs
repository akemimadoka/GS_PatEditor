﻿using GS_PatEditor.Editor.Editable;
using GS_PatEditor.Pat.Effects;
using GS_PatEditor.Pat.Effects.Converter;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace GS_PatEditor.Pat.Behaviors
{
    [Serializable]
    public class CreateBulletBehavior : Behavior, IEditableEnvironment
    {
        [XmlElement]
        [EditorChildNode("Time")]
        public Time Time;

        [XmlElement]
        [TypeConverter(typeof(ActionIDConverter))]
        public string Bullet { get; set; }

        [XmlElement]
        public CreateBulletDirection Direction { get; set; }

        [XmlElement]
        [EditorChildNode("Position")]
        public PointProvider Position;

        public override void MakeEffects(ActionEffects effects)
        {
            var effect = new CreateBulletEffect
            {
                ActionName = Bullet,
                Direction = Direction,
                Position = Position,
            };
            Time.MakeEffects(effects, effect);
        }

        [XmlIgnore]
        [Browsable(false)]
        public EditableEnvironment Environment { get; set; }
    }
}
