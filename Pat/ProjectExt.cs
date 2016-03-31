using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GS_PatEditor.Pat
{
    static class ProjectExt
    {
        public static string FindResource(this Project proj, ProjectDirectoryUsage usage, string id)
        {
            //TODO cache dir list
            return proj.Settings.Directories
                .Where(dir => dir.Usage == usage && dir.Path != null && dir.Path.Length > 0)
                .Select(d => Path.Combine(d.Path, id))
                .Where(file => File.Exists(file))
                .FirstOrDefault();
        }
    }
}
