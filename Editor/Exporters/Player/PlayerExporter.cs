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
        public string PlayerName { get; set; }

        [XmlElement]
        public string ScriptFileName { get; set; }

        [XmlElement]
        public PlayerExporterAnimations Animations = new PlayerExporterAnimations();

        [XmlElement]
        public PlayerInformation PlayerInformation = new PlayerInformation();

        [XmlArray]
        public List<Skill> Skills = new List<Skill>();

        private Pat.Project _Project;
        private Dictionary<string, int> _GeneratedActionID = new Dictionary<string, int>();
        private int _NextFreeActionID;
        private SegmentStartEventRecorder _SSERecorder;

        [XmlIgnore]
        [Browsable(false)]
        public GenerationEnvironment GenEnv { get; private set; }

        public override void ShowOptionDialog(Pat.Project proj)
        {
            Environment = new EditableEnvironment(proj);
            Animations.Environment = Environment;

            var dialog = new PlayerExporterOptionsForm(proj, this);
            dialog.ShowDialog();
        }

        public override void Export(Pat.Project proj)
        {
            _Project = proj;
            _GeneratedActionID.Clear();
            _SSERecorder = new SegmentStartEventRecorder();

            _NextFreeActionID = 20;

            //init script
            var actorCommon = base.AddCodeFile("actorCommon.add.cv4");
            PlayerInitFunctionGenerator.Generate(this, proj, actorCommon);

            //actions
            ExportAction(Animations.Stand, 0);
            ExportAction(Animations.Walk, 1);
            ExportAction(Animations.JumpUp, 3);
            ExportAction(Animations.JumpFront, 4);
            ExportAction(Animations.Fall, 8);
            ExportAction(Animations.Damage, 10);
            ExportAction(Animations.Dead, 18);
            ExportAction(Animations.Lost, 19);
            //TODO make Revive action here

            var playerScript = base.AddCodeFile(ScriptFileName + ".cv4");

            SystemActionFunctionGenerator.Generate(this, proj, playerScript);

            this.GenEnv = SkillGenerator.CreateEnv(this, playerScript);
            SkillGenerator.GenerateSkills(this, playerScript);
            this.GenEnv = null;

            SkillGenerator.GenerateStartMotionFunction(_SSERecorder, playerScript);

            ExportAction(Animations.Stand, 100);
            ExportAction(Animations.Stand, 101);
            ExportAction(Animations.Stand, 102);
            ExportAction(Animations.Stand, 103);
        }

        private void ExportAction(string name, int id)
        {
            if (name == null || name.Length == 0)
            {
                return;
            }

            var action = _Project.Actions.FirstOrDefault(a => a.ActionID == name);
            if (action != null)
            {
                base.AddNormalAnimation(action, id + BaseIndex);
                if (this.GenEnv != null)
                {
                    _SSERecorder.AddAction(action, id + BaseIndex, this.GenEnv);
                }
            }
        }

        public int GetActionID(string name)
        {
            int ret;
            if (_GeneratedActionID.TryGetValue(name, out ret))
            {
                return ret;
            }
            ret = _NextFreeActionID++;
            ExportAction(name, ret);
            return ret;
        }

        public Pat.Action GetAction(string name)
        {
            return _Project.Actions.FirstOrDefault(a => a.ActionID == name);
        }

        [XmlIgnore]
        [Browsable(false)]
        public EditableEnvironment Environment { get; set; }
    }
}
