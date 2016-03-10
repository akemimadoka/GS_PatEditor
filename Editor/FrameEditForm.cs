using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GS_PatEditor.Editor
{
    public partial class FrameEditForm : Form
    {
        public FrameEditForm()
        {
            InitializeComponent();
            textBox1.SetIntegerMode(1);
        }

        public int FrameCount
        {
            get
            {
                return textBox1.GetIntegerValue(1);
            }
            set
            {
                textBox1.Text = value.ToString();
            }
        }

        public bool UseImage
        {
            get
            {
                return checkBox1.Checked;
            }
            set
            {
                checkBox1.Checked = value;
            }
        }

        public bool SetDuationForAllEnabled
        {
            get
            {
                return checkBox2.Enabled;
            }
            set
            {
                checkBox2.Enabled = value;
            }
        }

        public bool SetDurationForAll
        {
            get
            {
                return checkBox2.Checked;
            }
            set
            {
                checkBox2.Checked = value;
            }
        }
    }
}
