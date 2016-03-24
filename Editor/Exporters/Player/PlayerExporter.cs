using GS_PatEditor.Pat.Effects;
using GS_PatEditor.Pat.Effects.Converter;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace GS_PatEditor.Editor.Exporters.Player
{
    [Serializable]
    public class PlayerExporterAnimations : IEditableEnvironment
    {
        [XmlElement]
        [TypeConverter(typeof(ActionIDConverter))]
        public string Stand { get; set; }

        [XmlElement]
        [TypeConverter(typeof(ActionIDConverter))]
        public string Walk { get; set; }

        [XmlElement]
        [TypeConverter(typeof(ActionIDConverter))]
        public string JumpUp { get; set; }

        [XmlElement]
        [TypeConverter(typeof(ActionIDConverter))]
        public string JumpFront { get; set; }

        [XmlElement]
        [TypeConverter(typeof(ActionIDConverter))]
        public string Fall { get; set; }

        [XmlElement]
        [TypeConverter(typeof(ActionIDConverter))]
        public string FallAttack { get; set; }

        [XmlElement]
        [TypeConverter(typeof(ActionIDConverter))]
        public string Damage { get; set; }

        [XmlElement]
        [TypeConverter(typeof(ActionIDConverter))]
        public string Dead { get; set; }

        [XmlElement]
        [TypeConverter(typeof(ActionIDConverter))]
        public string Lost { get; set; }

        [XmlElement]
        [Editor(typeof(ImageIDEditor), typeof(UITypeEditor))]
        public string ReviveLight { get; set; }

        [XmlElement]
        [Editor(typeof(ImageIDEditor), typeof(UITypeEditor))]
        public string ReviveDark { get; set; }

        [XmlIgnore]
        [Browsable(false)]
        public EditableEnvironment Environment { get; set; }
    }

    [Serializable]
    public class PlayerExporter : AbstractExporter, IEditableEnvironment
    {
        [XmlElement]
        public int BaseIndex { get; set; }

        [XmlElement]
        [Browsable(false)]
        public readonly PlayerExporterAnimations Animations = new PlayerExporterAnimations();

        public override void ShowOptionDialog(Pat.Project proj)
        {
            Environment = new EditableEnvironment(proj);
            Animations.Environment = Environment;

            var dialog = new PlayerExporterOptionsForm(proj, this);
            dialog.ShowDialog();
        }

        public override void Export(Pat.Project proj)
        {
        }

        [XmlIgnore]
        [Browsable(false)]
        public EditableEnvironment Environment { get; set; }
    }
}
