using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace GS_PatEditor.Pat
{
    static class PatSerialization
    {
        private static XmlSerializer _ProjectSerializer;
        public static XmlSerializer ProjectSerializer
        {
            get
            {
                if (_ProjectSerializer == null)
                {
                    var types = System.Reflection.Assembly.GetExecutingAssembly().GetTypes().Where(
                        t => (typeof(Effect).IsAssignableFrom(t) || typeof(Filter).IsAssignableFrom(t)) &&
                            !t.IsAbstract).ToArray();
                    _ProjectSerializer = new XmlSerializer(typeof(Project), types);
                }
                return _ProjectSerializer;
            }
        }

        private static XmlSerializer _LocalSerializer;
        public static XmlSerializer LocalSerializer
        {
            get
            {
                if (_LocalSerializer == null)
                {
                    _LocalSerializer = new XmlSerializer(typeof(ProjectLocalInfo));
                }
                return _LocalSerializer;
            }
        }
    }
}
