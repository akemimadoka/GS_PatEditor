using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace GS_PatEditor.Pat
{
    [Serializable]
    [XmlRoot("GSPatLocal")]
    [XmlType("GSPatLocal")]
    public class ProjectLocalInfo
    {
        [XmlArray]
        [XmlArrayItem("Directory")]
        public List<ProjectDirectoryPath> Directories;
    }
}
