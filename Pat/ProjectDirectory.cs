using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace GS_PatEditor.Pat
{
    public enum ProjectDirectoryUsage
    {
        SoundEffect,
        Image,
    }

    [Serializable]
    public class ProjectDirectoryDesc
    {
        [XmlAttribute]
        public string Name;

        [XmlAttribute]
        public ProjectDirectoryUsage Usage;

        [XmlIgnore]
        public string Path;
    }

    [Serializable]
    public class ProjectDirectoryPath
    {
        [XmlAttribute]
        public string Name;
        [XmlAttribute]
        public string Path;
    }
}
