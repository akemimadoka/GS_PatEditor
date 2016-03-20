using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GS_PatEditor.Editor.EffectEditable
{
    [TypeConverter(typeof(PointProviderTypeConverter))]
    class PointProviderType
    {
        public Type Value;

        public override string ToString()
        {
            return Value.Name;
        }
    }

    class PointProviderTypeConverter : TypeConverter
    {
        private static List<Type> _PointProviderTypes;
        private static List<Type> PointProviderTypes
        {
            get
            {
                if (_PointProviderTypes == null)
                {
                    _PointProviderTypes = typeof(PointProviderTypeConverter).Assembly.GetTypes()
                        .Where(t =>
                            !t.IsAbstract &&
                            typeof(Pat.PointProvider).IsAssignableFrom(t) &&
                            t != typeof(SelectPointProvider))
                        .Where(t =>
                            !typeof(Pat.Effects.IHideFromEditor).IsAssignableFrom(t))
                        .OrderBy(t => t.Name)
                        .ToList();
                }
                return _PointProviderTypes;
            }
        }

        public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
        {
            return true;
        }

        public override StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
        {
            return new StandardValuesCollection(PointProviderTypes.Select(type => type.Name).ToArray());
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
                foreach (var t in PointProviderTypes)
                {
                    if (t.Name == (string)value)
                    {
                        return new PointProviderType { Value = t };
                    }
                }
            }
            return base.ConvertFrom(context, culture, value);
        }
    }

    class SelectPointProvider : Pat.PointProvider
    {
        private readonly Action<Pat.PointProvider> _OnNewPointProvider;

        public SelectPointProvider(Action<Pat.PointProvider> onNewPointProvider)
        {
            _OnNewPointProvider = onNewPointProvider;
        }

        public PointProviderType Type
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
