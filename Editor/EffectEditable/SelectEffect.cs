using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GS_PatEditor.Editor.EffectEditable
{
    [TypeConverter(typeof(EffectTypeConverter))]
    class EffectType
    {
        public Type Value;

        public override string ToString()
        {
            return Value.Name;
        }
    }

    class EffectTypeConverter : TypeConverter
    {
        private static List<Type> _EffectTypes;
        private static List<Type> EffectTypes
        {
            get
            {
                if (_EffectTypes == null)
                {
                    _EffectTypes = typeof(EffectTypeConverter).Assembly.GetTypes()
                        .Where(t =>
                            !t.IsAbstract &&
                            typeof(Pat.Effect).IsAssignableFrom(t) &&
                            t != typeof(SelectEffect))
                        .Where(t =>
                            !typeof(Pat.Effects.IHideFromEditor).IsAssignableFrom(t))
                        .OrderBy(t => t.Name)
                        .ToList();
                }
                return _EffectTypes;
            }
        }

        public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
        {
            return true;
        }

        public override StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
        {
            return new StandardValuesCollection(EffectTypes.Select(type => type.Name).ToArray());
        }

        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            if (sourceType == typeof(string))
            {
                return true;
            }
            return base.CanConvertFrom(context, sourceType);
        }

        public override object ConvertFrom(ITypeDescriptorContext context,
            System.Globalization.CultureInfo culture, object value)
        {
            if (value is string)
            {
                foreach (var t in EffectTypes)
                {
                    if (t.Name == (string)value)
                    {
                        return new EffectType { Value = t };
                    }
                }
            }
            return base.ConvertFrom(context, culture, value);
        }
    }

    class SelectEffect : Pat.Effect
    {
        private readonly Action<Pat.Effect> _OnNewEffect;

        public SelectEffect(Action<Pat.Effect> onNewEffect)
        {
            _OnNewEffect = onNewEffect;
        }

        public EffectType Type
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
