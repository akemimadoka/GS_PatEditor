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
    public partial class DamageEditForm : Form
    {
        public DamageEditForm(Pat.AnimationDamageInfo data)
        {
            InitializeComponent();

            if (data == null)
            {
                AttackType = Pat.AttackType.None;
            }
            else
            {
                AttackType = data.AttackType;
                textBox1.Text = data.BaseDamage.ToString();
                textBox2.Text = data.SoundEffect.ToString();
                textBox3.Text = data.HitStop.Self.ToString();
                textBox4.Text = data.HitStop.Opponent.ToString();
                textBox5.Text = data.Knockback.SpeedX.ToString();
                textBox6.Text = data.Knockback.SpeedY.ToString();
                textBox7.Text = data.Knockback.Gravity.ToString();
            }
        }

        public Pat.AnimationDamageInfo GetData()
        {
            if (AttackType == Pat.AttackType.None)
            {
                return null;
            }
            var ret = new Pat.AnimationDamageInfo()
            {
                AttackType = AttackType,
                BaseDamage = textBox1.GetIntegerValue(0),
                HitStop = new Pat.HitStop
                {
                    Self = textBox3.GetIntegerValue(0),
                    Opponent = textBox4.GetIntegerValue(0),
                },
                Knockback = new Pat.HitKnockback
                {
                    SpeedX = textBox5.GetIntegerValue(0),
                    SpeedY = textBox6.GetIntegerValue(0),
                    Gravity = textBox7.GetIntegerValue(0),
                },
                SoundEffect = textBox2.GetIntegerValue(0),
            };
            return ret;
        }

        public Pat.AttackType AttackType
        {
            get
            {
                Pat.AttackType ret;
                if (Enum.TryParse(comboBox1.Text, out ret))
                {
                    return ret;
                }
                return Pat.AttackType.None;
            }
            set
            {
                comboBox1.SelectedIndex = (int)value;
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            var enabled = comboBox1.SelectedIndex != 0;
            textBox1.Enabled = enabled;
            textBox2.Enabled = enabled;
            textBox3.Enabled = enabled;
            textBox4.Enabled = enabled;
            textBox5.Enabled = enabled;
            textBox6.Enabled = enabled;
            textBox7.Enabled = enabled;
        }
    }
}
