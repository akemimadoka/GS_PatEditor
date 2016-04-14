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
    public partial class AnimationPropertyFrom : Form
    {
        public AnimationPropertyFrom(Pat.Project proj)
        {
            InitializeComponent();
            comboBox1.Items.AddRange(proj.Actions
                .Select(a => a.Category)
                .Where(s => s != null && s.Length != 0)
                .Distinct()
                .ToArray());
        }

        public string AnimationID
        {
            get
            {
                return textBox1.Text;
            }
            set
            {
                textBox1.Text = value;
            }
        }

        public string Category
        {
            get
            {
                return comboBox1.Text;
            }
            set
            {
                comboBox1.Text = value;
            }
        }
    }
}
