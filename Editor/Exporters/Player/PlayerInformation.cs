using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace GS_PatEditor.Editor.Exporters.Player
{
    [Serializable]
    public class PlayerInformation
    {
        public PlayerInformation()
        {
            FallAttack = true;
            RegainCycle = 5;
            RegainRate = 2;
            AtkOffset = 0.1f;
        }

        [XmlElement]
        public bool FallAttack { get; set; }
        public bool ShouldSerializeFallAttack() { return !FallAttack; }

        [XmlElement]
        public int RegainCycle { get; set; }
        public bool ShouldSerializeRegainCycle() { return RegainCycle != 5; }

        [XmlElement]
        public int RegainRate { get; set; }
        public bool ShouldSerializeRegainRate() { return RegainRate != 2; }

        [XmlElement]
        public int MagicUse { get; set; }

        [XmlElement]
        public float AtkOffset { get; set; }
        public bool ShouldSerializeAtkOffset() { return AtkOffset != 0.1f; }
    }
}
