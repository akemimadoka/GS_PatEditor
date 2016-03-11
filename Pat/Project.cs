using GS_PatEditor.Pat.Editing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace GS_PatEditor.Pat
{
    [Serializable]
    [XmlRoot("GSPatProject")]
    [XmlType("GSPatProject")]
    public class Project
    {
        [XmlElement(ElementName = "Settings")]
        public ProjectSettings Settings;

        [XmlArray]
        public List<FrameImage> Images;

        [XmlArray]
        public List<Animation> Animations;

        [XmlArray]
        public List<Action> Actions;

        [XmlArray]
        public List<Actor> Actors;

        [XmlIgnore]
        public ProjectLocalInfo LocalInformation;

        [XmlIgnore]
        public readonly ProjectImageFileList ImageList;

        [XmlIgnore]
        public string FilePath;

        [XmlIgnore]
        public bool IsEmptyProject;

        public Project()
        {
            ImageList = new ProjectImageFileList(this);
        }
    }
}
