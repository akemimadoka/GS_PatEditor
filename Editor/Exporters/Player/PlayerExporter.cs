using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GS_PatEditor.Editor.Exporters.Player
{
    class PlayerExporter : AbstractExporter
    {
        public override void ShowOptionDialog()
        {
            var dialog = new PlayerExporterOptionsForm(this);
            dialog.ShowDialog();
        }

        public override void Export(Pat.Project proj)
        {
        }
    }
}
