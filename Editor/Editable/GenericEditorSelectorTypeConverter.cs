using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace GS_PatEditor.Editor.Editable
{
    public class SelectType
    {
        public Type Value;

        public override string ToString()
        {
            var dn = Value.GetCustomAttribute<DisplayNameAttribute>();
            if (dn != null)
            {
                return dn.DisplayName;
            }
            return Value.Name;
        }
    }

    class GenericEditorSelectorTypeConverter<T> : TypeConverter
    {
        private static List<Type> _Types;
        private static List<Type> Types
        {
            get
            {
                if (_Types == null)
                {
                    //TODO find in all assemblies?
                    _Types = typeof(GenericEditorSelectorTypeConverter<T>).Assembly.GetTypes()
                        .Where(t =>
                            !t.IsAbstract &&
                            typeof(T).IsAssignableFrom(t))
                        .Where(t =>
                            !typeof(Pat.Effects.IHideFromEditor).IsAssignableFrom(t))
                        .OrderBy(t => t.Name)
                        .ToList();
                }
                return _Types;
            }
        }

        public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
        {
            return true;
        }

        public override StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
        {
            return new StandardValuesCollection(Types.Select(type => type.Name).ToArray());
        }

        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            if (sourceType == typeof(string))
            {
                return true;
            }
            return base.CanConvertFrom(context, sourceType);
        }

        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            if (value is string)
            {
                foreach (var t in Types)
                {
                    if (t.Name == (string)value)
                    {
                        return new SelectType { Value = t };
                    }
                }
            }
            return base.ConvertFrom(context, culture, value);
        }
    }
}
