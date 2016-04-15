using GS_PatEditor.Editor.Editable;
using GS_PatEditor.Pat.Effects;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace GS_PatEditor.Pat.Behaviors
{
    [Serializable]
    [SerializationBaseClassAttribute]
    public abstract class PlayerGroundSpeedCtrlBehaviorEntry
    {
        [XmlElement]
        //TODO browsable?
        public SegmentSelector Segments { get; set; }

        [XmlIgnore]
        [Browsable(false)]
        public abstract Effect Effect { get; }
    }

    //public class EntrySelectTypeDescriptor : CustomTypeDescriptor
    //{
    //    class CustomFieldPropertyDescriptor : PropertyDescriptor
    //    {
    //        private readonly PropertyInfo _PI;

    //        public CustomFieldPropertyDescriptor(PropertyInfo pi)
    //            : base(pi.Name, pi.GetCustomAttributes().ToArray())
    //        {
    //            _PI = pi;
    //        }

    //        public override bool CanResetValue(object component)
    //        {
    //            return false;
    //        }

    //        public override Type ComponentType
    //        {
    //            get
    //            {
    //                return _PI.DeclaringType;
    //            }
    //        }

    //        public override object GetValue(object component)
    //        {
    //            return _PI.GetValue(component);
    //        }

    //        public override bool IsReadOnly
    //        {
    //            get
    //            {
    //                return _PI.CanRead && !_PI.CanWrite;
    //            }
    //        }

    //        public override Type PropertyType
    //        {
    //            get
    //            {
    //                return _PI.PropertyType;
    //            }
    //        }

    //        public override void ResetValue(object component)
    //        {
    //            throw new NotImplementedException();
    //        }

    //        public override void SetValue(object component, object value)
    //        {
    //            _PI.SetValue(component, value);
    //        }

    //        public override bool ShouldSerializeValue(object component)
    //        {
    //            return false;
    //        }
    //    }

    //    public EntrySelectTypeDescriptor()
    //    {
    //    }

    //    public override PropertyDescriptorCollection GetProperties(Attribute[] attributes)
    //    {
    //        return new PropertyDescriptorCollection(new PropertyDescriptor[]
    //        {
    //            new CustomFieldPropertyDescriptor(typeof(PlayerGroundSpeedCtrlBehaviorEntrySelect).GetProperty("Type")),
    //        });
    //    }
    //}

    //public class EntrySelectTypeDescriptionProvider : TypeDescriptionProvider
    //{
    //    public override ICustomTypeDescriptor GetTypeDescriptor(Type objectType, object instance)
    //    {
    //        return new EntrySelectTypeDescriptor();
    //    }
    //}

    //[EditorSelector(typeof(PlayerGroundSpeedCtrlBehaviorEntry))]
    //[TypeDescriptionProvider(typeof(EntrySelectTypeDescriptionProvider))]
    //public class PlayerGroundSpeedCtrlBehaviorEntrySelect : PlayerGroundSpeedCtrlBehaviorEntry, IHideFromEditor, IEditableEnvironment
    //{
    //    private readonly Action<PlayerGroundSpeedCtrlBehaviorEntry> _OnNewEntry;

    //    public PlayerGroundSpeedCtrlBehaviorEntrySelect(Action<PlayerGroundSpeedCtrlBehaviorEntry> onNewEntry)
    //    {
    //        _OnNewEntry = onNewEntry;
    //    }

    //    [TypeConverter(typeof(GenericEditorSelectorTypeConverter<PlayerGroundSpeedCtrlBehaviorEntry>))]
    //    public SelectType Type
    //    {
    //        get
    //        {
    //            return null;
    //        }
    //        set
    //        {
    //            if (value == null || value.Value == null)
    //            {
    //                return;
    //            }
    //            _OnNewEntry(SelectHelper.Create<PlayerGroundSpeedCtrlBehaviorEntry>(value.Value, Environment));
    //        }
    //    }

    //    [Browsable(false)]
    //    public EditableEnvironment Environment { get; set; }

    //    public override Effect Effect
    //    {
    //        get { return null; }
    //    }
    //}

    [Serializable]
    public class PlayerGroundSpeedCtrlBehaviorEntryFriction : PlayerGroundSpeedCtrlBehaviorEntry
    {
        [XmlIgnore]
        public override Effect Effect
        {
            get
            {
                return new Effects.PlayerSkillStopMovingEffect { ReduceSpeed = 0.2f };
            }
        }
    }

    public class EntryList : IEditableList<PlayerGroundSpeedCtrlBehaviorEntry>
    {
        public List<PlayerGroundSpeedCtrlBehaviorEntry> Entries = new List<PlayerGroundSpeedCtrlBehaviorEntry>();

        public void Add(PlayerGroundSpeedCtrlBehaviorEntry val)
        {
            Entries.Add(val);
        }

        public void Remove(PlayerGroundSpeedCtrlBehaviorEntry val)
        {
            Entries.Remove(val);
        }

        public int FindIndex(PlayerGroundSpeedCtrlBehaviorEntry val)
        {
            return Entries.IndexOf(val);
        }

        public void Insert(int index, PlayerGroundSpeedCtrlBehaviorEntry val)
        {
            Entries.Insert(index, val);
        }

        public int Count
        {
            get { return Entries.Count; }
        }

        public IEnumerator<PlayerGroundSpeedCtrlBehaviorEntry> GetEnumerator()
        {
            return Entries.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return Entries.GetEnumerator();
        }
    }

    [Serializable]
    public class PlayerGroundSpeedCtrlBehavior : Behavior
    {
        [XmlAttribute]
        [DefaultValue(true)]
        public bool ReduceInitialSpeed { get; set; }

        [XmlArray]
        [EditorChildNode(null)]
        public EntryList Entries = new EntryList();

        public PlayerGroundSpeedCtrlBehavior()
        {
            ReduceInitialSpeed = true;
        }

        public override void MakeEffects(ActionEffects effects)
        {
        }
    }
}
