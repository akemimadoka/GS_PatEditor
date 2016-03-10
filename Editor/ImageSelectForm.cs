using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
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
            if (checkBox1.Checked)
            {
                var dirList = _Project.LocalInformation.Directories.Join(
                    _Project.Settings.Directories.Where(d => d.Usage == Pat.ProjectDirectoryUsage.Image),
                    d => d.Name, d => d.Name,
                    (Pat.ProjectDirectoryPath d1, Pat.ProjectDirectoryDesc d2) => d1.Path);
                foreach (var dir in dirList)
                {
                    if (!Directory.Exists(dir))
                    {
                        continue;
                    }
                    //TODO handle duplicate filename
                    foreach (var image in Directory.EnumerateFiles(dir, "*.cv2", SearchOption.TopDirectoryOnly))
                    {
                        listView1.Items.Add(CreateFile(image));
                    }
                    foreach (var image in Directory.EnumerateFiles(dir, "*.dds", SearchOption.TopDirectoryOnly))
                    {
                        listView1.Items.Add(CreateFile(image));
                    }
                }
            }
        }

        private ListViewItem CreateFile(string file)
        {
            var filename = Path.GetFileName(file);
            var ret = new ListViewItem(new string[] { "<create>", filename }, -1);
            ret.StateImageIndex = 0;
            ret.ForeColor = Color.Gray;
            ret.Tag = file;
            ret.ToolTipText = file;
            return ret;
        }

        private ListViewItem CreateItem(Pat.FrameImage img)
        {
            var ret = new ListViewItem(new string[] { img.ImageID, img.Resource.ResourceID }, -1);
            ret.StateImageIndex = 0;
            ret.Tag = img;
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
                    item.EnsureVisible();
                }
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            RefreshList();
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count == 0)
            {
                textBox1.Text = "";
                textBox1.Enabled = false;
                buttonOK.Enabled = false;
            }
            else if (listView1.SelectedItems[0].Tag is Pat.FrameImage)
            {
                textBox1.Enabled = true;
                buttonOK.Enabled = true;
                textBox1.ReadOnly = false;
                //image item
                var img = (Pat.FrameImage)listView1.SelectedItems[0].Tag;
                SetupImage(img);
            }
            else if (listView1.SelectedItems[0].Tag is string)
            {
                textBox1.Enabled = true;
                buttonOK.Enabled = true;
                textBox1.ReadOnly = true;
                buttonOK.Enabled = false;
                var file = (string)listView1.SelectedItems[0].Tag;
                //TODO handle duplicate file
                textBox1.Text = Path.GetFileNameWithoutExtension(file);
            }
        }

        private void SetupImage(Pat.FrameImage img)
        {
            textBox1.Text = img.ImageID;
        }

        private void MakeNewImageFromFile(string str)
        {
            //TODO implement this in ImageList
            Images.AbstractImage img;
            if (Path.GetExtension(str) == ".cv2")
            {
                img = new Images.CV2Image(str);
            }
            else if (Path.GetExtension(str) == ".dds")
            {
                img = new Images.DDSImage(str);
            }
            else
            {
                //error
                return;
            }
            var frame = new GSPat.Frame()
            {
                ViewOffsetX = 0,
                ViewOffsetY = 0,
                ViewWidth = (short)img.Width,
                ViewHeight = (short)img.Height,
            };
            img.Dispose();
            var image = ProjectGenerater.AddImageToProject(_Project, str, frame);
            var item = CreateItem(image);
            listView1.Items.Add(item);
            listView1.SelectedItems.Clear();
            item.Selected = true;
            item.EnsureVisible();
        }

        private void textBox1_Enter(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count == 0)
            {
                return;
            }
            if (listView1.SelectedItems[0].Tag is string)
            {
                var str = (string)listView1.SelectedItems[0].Tag;
                if (MessageBox.Show("Create a new image?", "Image",
                    MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.Yes)
                {
                    MakeNewImageFromFile(str);
                    textBox1.ReadOnly = false;
                }
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count != 0 && listView1.SelectedItems[0].Tag is Pat.FrameImage)
            {
                var image = (Pat.FrameImage)listView1.SelectedItems[0].Tag;
                var oldValue = image.ImageID;
                var newValue = textBox1.Text;
                if (oldValue != newValue)
                {
                    //replace all image ids
                    //TODO move to ImageList
                    image.ImageID = newValue;
                    foreach (var animation in _Project.Animations)
                    {
                        if (animation.ImageID == oldValue)
                        {
                            animation.ImageID = newValue;
                        }
                    }
                    foreach (var frame in _Project.Animations
                        .SelectMany(p => p.Segments)
                        .SelectMany(s => s.Frames))
                    {
                        if (frame.ImageID == oldValue)
                        {
                            frame.ImageID = newValue;
                        }
                    }

                    listView1.SelectedItems[0].Text = newValue;
                }
            }
        }

        private void ImageSelectForm_Load(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count != 0)
            {
                listView1.SelectedItems[0].EnsureVisible();
            }
        }
    }
}
