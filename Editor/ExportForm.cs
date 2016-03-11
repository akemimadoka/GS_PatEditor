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
    public partial class ExportForm : Form
    {
        public ExportForm()
        {
            InitializeComponent();
            textBox1.SetIntegerMode(0);
        }

        public int StartID
        {
            get
            {
                return textBox1.GetIntegerValue(0);
            }
            set
            {
                textBox1.Text = value.ToString();
            }
        }
    }
}
