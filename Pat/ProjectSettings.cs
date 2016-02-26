using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace GS_PatEditor.Pat
{
    [Serializable]
    public class ProjectSettings
    {
        [XmlElement(ElementName = "ProjectName")]
        public string ProjectName;

        [XmlArray]
        [XmlArrayItem("Directory")]
        public List<ProjectDirectoryDesc> Directories;

        [XmlArray]
        [XmlArrayItem("Palette")]
        public List<string> Palettes;
    }
}
