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
    public partial class ActionEditForm : Form
    {
        private readonly Pat.Project _Project;
        private readonly Pat.Action _Action;

        public ActionEditForm(Pat.Project proj, Pat.Action action)
        {
            InitializeComponent();

            _Project = proj;
            _Action = action;
            RefreshList();
        }

        private void RefreshList()
        {
            listBox1.Items.Clear();
            listBox1.Items.Add("Initialization(" + _Action.InitEffects.Count() + ")");
            listBox1.Items.Add("Update(" + _Action.UpdateEffects.Count() + ")");
            listBox1.Items.Add("-----");
            foreach (var key in _Action.KeyFrameEffects)
            {
                listBox1.Items.Add("KeyFrame(" + key.Count() + ")");
            }

            RefreshButtonEnabled();
        }

        private void RefreshButtonEnabled()
        {
            if (listBox1.SelectedIndex == -1 || listBox1.SelectedIndex == 2)
            {
                button1.Enabled = true;
                button2.Enabled = false;
                button3.Enabled = false;
                button4.Enabled = false;
                button5.Enabled = false;
            }
            else if (listBox1.SelectedIndex <= 1)
            {
                button1.Enabled = true;
                button2.Enabled = false;
                button3.Enabled = true;
                button4.Enabled = false;
                button5.Enabled = false;
            }
            else
            {
                button1.Enabled = true;
                button2.Enabled = true;
                button3.Enabled = true;
                button4.Enabled = false;
                button5.Enabled = false;
                if (listBox1.SelectedIndex != 3)
                {
                    button4.Enabled = true;
                }
                if (listBox1.SelectedIndex != listBox1.Items.Count - 1)
                {
                    button5.Enabled = true;
                }
            }
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            RefreshButtonEnabled();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            _Action.KeyFrameEffects.Add(new Pat.EffectList());
            listBox1.Items.Add("KeyFrame(0)");
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex >= 3)
            {
                _Action.KeyFrameEffects.RemoveAt(listBox1.SelectedIndex - 3);
                listBox1.Items.RemoveAt(listBox1.SelectedIndex);
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            var s = listBox1.SelectedIndex;
            if (s >= 4)
            {
                var key = _Action.KeyFrameEffects[s - 3];
                _Action.KeyFrameEffects.RemoveAt(s - 3);
                _Action.KeyFrameEffects.Insert(s - 4, key);

                var item = listBox1.Items[s];
                listBox1.Items.RemoveAt(s);
                listBox1.Items.Insert(s - 1, item);

                listBox1.SelectedIndex = s - 1;
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            var s = listBox1.SelectedIndex;
            if (s >= 3 && s != listBox1.Items.Count - 1)
            {
                var key = _Action.KeyFrameEffects[s - 3];
                _Action.KeyFrameEffects.RemoveAt(s - 3);
                _Action.KeyFrameEffects.Insert(s - 2, key);

                var item = listBox1.Items[s];
                listBox1.Items.RemoveAt(s);
                listBox1.Items.Insert(s + 1, item);

                listBox1.SelectedIndex = s + 1;
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            var s = listBox1.SelectedIndex;
            Pat.EffectList effects = null;
            switch (s)
            {
                case 0:
                    effects = _Action.InitEffects;
                    break;
                case 1:
                    effects = _Action.UpdateEffects;
                    break;
                case 2:
                    break;
                default:
                    effects = _Action.KeyFrameEffects[s - 3];
                    break;
            }
            if (effects != null)
            {
                var dialog = new EffectListEditForm(_Project, effects);
                dialog.ShowDialog();
                var txt = "KeyFrame";
                if (s == 0)
                {
                    txt = "Initialization";
                }
                else if (s == 1)
                {
                    txt = "Update";
                }
                listBox1.Items[s] = txt + "(" + effects.Count() + ")";
            }
        }
    }
}
