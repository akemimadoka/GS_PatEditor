using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GS_PatEditor.Editor.EffectEditable
{
    [TypeConverter(typeof(FilterTypeConverter))]
    class FilterType
    {
        public Type Value;

        public override string ToString()
        {
            return Value.Name;
        }
    }

    class FilterTypeConverter : TypeConverter
    {
        private static List<Type> _FilterTypes;
        private static List<Type> FilterTypes
        {
            get
            {
                if (_FilterTypes == null)
                {
                    _FilterTypes = typeof(FilterTypeConverter).Assembly.GetTypes()
                        .Where(t =>
                            !t.IsAbstract &&
                            typeof(Pat.Filter).IsAssignableFrom(t) &&
                            t != typeof(SelectFilter))
                        .Where(t =>
                            !typeof(Pat.Effects.IHideFromEditor).IsAssignableFrom(t))
                        .OrderBy(t => t.Name)
                        .ToList();
                }
                return _FilterTypes;
            }
        }

        public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
        {
            return true;
        }

        public override StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
        {
            return new StandardValuesCollection(FilterTypes.Select(type => type.Name).ToArray());
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
                foreach (var t in FilterTypes)
                {
                    if (t.Name == (string)value)
                    {
                        return new FilterType { Value = t };
                    }
                }
            }
            return base.ConvertFrom(context, culture, value);
        }
    }

    class SelectFilter : Pat.Filter
    {
        private readonly Action<Pat.Filter> _OnNewFilter;

        public SelectFilter(Action<Pat.Filter> onNewFilter)
        {
            _OnNewFilter = onNewFilter;
        }

        public FilterType Type
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
                _OnNewFilter((Pat.Filter)value.Value.GetConstructor(new Type[0]).Invoke(new object[0]));
            }
        }

        public override bool Test(Simulation.Actor actor)
        {
            return false;
        }
    }
}
