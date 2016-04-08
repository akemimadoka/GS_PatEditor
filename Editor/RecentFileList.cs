using GS_PatEditor.Properties;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GS_PatEditor.Editor
{
    class RecentFileList
    {
        private ToolStripSplitButton _Button;

        public event Action<string> OpenFile;

        private StringCollection GetList()
        {
            var list = Settings.Default.RecentFiles;
            if (list == null)
            {
                list = new StringCollection();
                Settings.Default.RecentFiles = list;
                Settings.Default.Save();
            }
            return list;
        }

        private void SaveList(StringCollection coll)
        {
            Settings.Default.RecentFiles = coll;
            Settings.Default.Save();
        }

        public RecentFileList(ToolStripSplitButton button)
        {
            _Button = button;
            button.DropDownOpening += button_DropDownOpening;
        }

        private void button_DropDownOpening(object sender, EventArgs e)
        {
            _Button.DropDownItems.Clear();
            foreach (var item in GetList())
            {
                var menu = new ToolStripMenuItem(item);
                menu.Click += delegate(object ssender, EventArgs ee)
                {
                    if (OpenFile != null)
                    {
                        OpenFile(item);
                    }
                };
                _Button.DropDownItems.Add(menu);
            }
        }

        public void AddToRecentList(string file)
        {
            var list = GetList();
            if (list.Contains(file))
            {
                list.Remove(file);
            }
            list.Insert(0, file);
            SaveList(list);
        }
    }
}
