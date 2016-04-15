using GS_PatEditor.Pat.Effects;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GS_PatEditor.Editor.Editable
{
    class AbstractSelectObject
    {
        public Action<object> Callback;
        public EditableEnvironment Env;
        public Type GenericType;
    }

    [TypeDescriptionProvider(typeof(SelectObjectTypeDescriptionProvider))]
    class SelectObject<T> : AbstractSelectObject
    {
        public SelectObject(Action<object> cb, EditableEnvironment env)
        {
            base.Callback = cb;
            base.Env = env;
            base.GenericType = typeof(T);
        }

        public int Value { get; set; }
    }

    class SelectObjectTypeDescriptionProvider : TypeDescriptionProvider
    {
        public override ICustomTypeDescriptor GetTypeDescriptor(Type objectType, object instance)
        {
            return new SelectObjectTypeDescriptor(objectType);
        }
    }

    class SelectObjectTypeDescriptor : CustomTypeDescriptor
    {
        class CustomFieldPropertyDescriptor : PropertyDescriptor
        {
            private readonly Type _Comp;

            private static Type MakeConverter(Type comp)
            {
                if (comp.IsGenericType && comp.GetGenericTypeDefinition() == typeof(SelectObject<>))
                {
                    return typeof(GenericEditorSelectorTypeConverter<>).MakeGenericType(comp.GetGenericArguments()[0]);
                }
                return null;
            }

            public CustomFieldPropertyDescriptor(Type comp)
                : base("Type", new Attribute[] { new TypeConverterAttribute(MakeConverter(comp)) })
            {
                _Comp = comp;
            }

            public override bool CanResetValue(object component)
            {
                return false;
            }

            public override Type ComponentType
            {
                get
                {
                    return _Comp;
                }
            }

            public override object GetValue(object component)
            {
                return null;
            }

            public override bool IsReadOnly
            {
                get
                {
                    return false;
                }
            }

            public override Type PropertyType
            {
                get
                {
                    return typeof(SelectType);
                }
            }

            public override void ResetValue(object component)
            {
                throw new NotImplementedException();
            }

            public override void SetValue(object component, object value)
            {
                if (component is AbstractSelectObject && value is SelectType)
                {
                    var cc = (AbstractSelectObject)component;
                    var cv = (SelectType)value;

                    var created = cv.Value.GetConstructor(new Type[0]).Invoke(new object[0]);
                    if (created is IEditableEnvironment)
                    {
                        ((IEditableEnvironment)created).Environment = cc.Env;
                    }

                    cc.Callback(created);
                }
            }

            public override bool ShouldSerializeValue(object component)
            {
                return false;
            }
        }

        private readonly Type _Comp;

        public SelectObjectTypeDescriptor(Type comp)
        {
            _Comp = comp;
        }

        public override PropertyDescriptorCollection GetProperties(Attribute[] attributes)
        {
            return new PropertyDescriptorCollection(new PropertyDescriptor[]
            {
                new CustomFieldPropertyDescriptor(_Comp),
            });
        }
    }
}
