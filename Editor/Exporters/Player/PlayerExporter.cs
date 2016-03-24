using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace GS_PatEditor.Editor.Exporters.Player
{
    [Serializable]
    public class PlayerExporter : AbstractExporter
    {
        [XmlAttribute]
        public int BaseIndex { get; set; }

        public override void ShowOptionDialog(Pat.Project proj)
        {
            var dialog = new PlayerExporterOptionsForm(proj, this);
            dialog.ShowDialog();
        }

        public override void Export(Pat.Project proj)
        {
        }
    }
}
