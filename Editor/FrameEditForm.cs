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
    public partial class FrameEditForm : Form
    {
        public FrameEditForm()
        {
            InitializeComponent();
        }

        public int FrameCount
        {
            get
            {
                int ret;
                if (Int32.TryParse(textBox1.Text, out ret))
                {
                    return ret;
                }
                return 0;
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

        private void buttonOK_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
