using GS_PatEditor.Pat;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace GS_PatEditor
{
    class ProjectSerializer
    {
        private static XmlSerializer _Serializer;
        public static XmlSerializer Serializer
        {
            get
            {
                if (_Serializer == null)
                {
                    var types = System.Reflection.Assembly.GetExecutingAssembly().GetTypes().Where(
                        t => (typeof(Effect).IsAssignableFrom(t) || typeof(Filter).IsAssignableFrom(t)) &&
                            !t.IsAbstract).ToArray();
                    _Serializer = new XmlSerializer(typeof(Project), types);
                }
                return _Serializer;
            }
        }
        public static void SaveProject(Project proj, string filename)
        {
            if (File.Exists(filename))
            {
                File.Delete(filename);
            }
            using (var file = File.Open(filename, FileMode.CreateNew))
            {
                Serializer.Serialize(file, proj);
            }
        }
        public static Project OpenProject(string filename)
        {
            using (var file = File.OpenRead(filename))
            {
                return (Project)Serializer.Deserialize(file);
            }
        }
    }
}
