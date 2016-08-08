﻿using GS_PatEditor.Editor;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GS_PatEditor
{
    class Program
    {
        public static Editor.Editor EditorForBackup;

        [STAThread]
        private static void Main()
        {
            try
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);

                var proj = ProjectGenerater.GenerateEmpty("", new List<string>());
                proj.IsEmptyProject = true;

                if (proj != null)
                {
                    EditorForm.ShowEditorForm(proj);
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
                try
                {
                    var file = CreateBackup();
                    if (file != null)
                    {
                        MessageBox.Show("Project file save to " + file + " for recovery.");
                    }
                }
                catch
                {
                }
            }
        }

        private static string CreateBackup()
        {
            if (EditorForBackup != null)
            {
                if (EditorForBackup.Project != null)
                {
                    var filename = GetBackupFileName();
                    ProjectSerializer.SaveProject(EditorForBackup.Project, filename);
                    return filename;
                }
            }
            return null;
        }

        private static string GetBackupFileName()
        {
            var filename = "Backup.patproj";
            return Path.GetFullPath(filename);
        }
    }
}
