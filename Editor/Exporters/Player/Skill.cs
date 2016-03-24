using GS_PatEditor.Editor.Editable;
using GS_PatEditor.Pat.Effects;
using GS_PatEditor.Pat.Effects.Converter;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace GS_PatEditor.Editor.Exporters.Player
{
    [EditorSelector(typeof(Skill))]
    class SelectEffect : Skill, IHideFromEditor, IEditableEnvironment
    {
        private readonly Action<Skill> _OnNewSkill;

        public SelectEffect(Action<Skill> onNewSkill)
        {
            _OnNewSkill = onNewSkill;
        }

        [TypeConverter(typeof(GenericEditorSelectorTypeConverter<Skill>))]
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
                _OnNewSkill(SelectHelper.Create<Skill>(value.Value, Environment));
            }
        }

        [Browsable(false)]
        public EditableEnvironment Environment { get; set; }
    }

    public enum SkillKey
    {
        KeyA,
        KeyB,
    }

    public enum DirectionHorizontal
    {
        Any,
        Empty,
        Front,
    }

    public enum DirectionVertical
    {
        Any,
        Empty,
        UpOnly,
        DownOnly,
    }

    [Serializable]
    [XmlInclude(typeof(NormalSkill))]
    public abstract class Skill
    {
    }

    [Serializable]
    public class NormalSkill : Skill, IEditableEnvironment
    {
        [XmlAttribute]
        public SkillKey Key { get; set; }

        [XmlAttribute]
        public DirectionHorizontal X { get; set; }
        public bool ShouldSerializeX() { return X != DirectionHorizontal.Any; }

        [XmlAttribute]
        public DirectionVertical Y { get; set; }
        public bool ShouldSerializeY() { return Y != DirectionVertical.Any; }

        [XmlAttribute]
        [TypeConverter(typeof(ActionIDConverter))]
        public string ActionID { get; set; }

        [XmlIgnore]
        [Browsable(false)]
        public EditableEnvironment Environment { get; set; }
    }
}
