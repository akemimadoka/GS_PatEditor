using GS_PatEditor.Render;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GS_PatEditor
{
    class RenderingTest
    {
        private static void Main()
        {
            var form = new Form();

            var pic = new PictureBox()
            {
                Width = 100,
                Height = 100,
                BackColor = System.Drawing.Color.White,
            };
            form.Controls.Add(pic);

            var rand = new Random();

            var tmr = new Timer(new System.ComponentModel.Container());
            tmr.Enabled = true;
            tmr.Interval = 16;

            using (var re = new RenderEngine(pic))
            {
                tmr.Tick += delegate(object sender, EventArgs e)
                {
                    re.RenderAll();
                };
                form.Show();
                Application.Run(form);
            }
        }
    }
}
