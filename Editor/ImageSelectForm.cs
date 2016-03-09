using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GS_PatEditor.Editor
{
    public partial class ImageSelectForm : Form
    {
        private Pat.Project _Project;

        public ImageSelectForm(Pat.Project project)
        {
            InitializeComponent();

            _Project = project;
            RefreshList();
        }

        private void RefreshList()
        {
            listView1.Items.Clear();
            foreach (var image in _Project.Images)
            {
                listView1.Items.Add(CreateItem(image));
            }
        }

        private ListViewItem CreateItem(Pat.FrameImage img)
        {
            var ret = new ListViewItem(new string[] { img.ImageID, img.Resource.ResourceID }, -1);
            ret.StateImageIndex = 0;
            return ret;
        }

        private ListViewItem GetSelectedItem()
        {
            if (listView1.SelectedItems.Count == 0)
            {
                return null;
            }
            return listView1.SelectedItems[0];
        }

        public string SelectedImage
        {
            get
            {
                var sel = GetSelectedItem();
                if (sel == null)
                {
                    return null;
                }
                return sel.Text;
            }
            set
            {
                var item = listView1.Items.Cast<ListViewItem>()
                    .FirstOrDefault(i => i.Text == value);
                listView1.SelectedItems.Clear();
                if (item != null)
                {
                    item.Selected = true;
                }
            }
        }
    }
}
