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
        public AnimationPropertyFrom()
        {
            InitializeComponent();
        }

        public bool HasResult
        {
            get;
            private set;
        }

        public string AnimationID
        {
            get;
            set;
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            HasResult = true;
            AnimationID = textBox1.Text;

            this.Close();
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            HasResult = false;

            this.Close();
        }

        private void AnimationPropertyFrom_Load(object sender, EventArgs e)
        {
            textBox1.Text = AnimationID;
        }
    }
}
