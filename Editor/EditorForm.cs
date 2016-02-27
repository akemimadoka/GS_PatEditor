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
    public partial class EditorForm : Form
    {
        public static void ShowEditorForm(Pat.Project proj)
        {
            using (var frm = new EditorForm())
            {
                using (var editor = new Editor(proj))
                {
                    editor.AnimationFramesUI.Init(frm.animationFrames);
                    editor.PreviewWindowUI.Init(frm.previewWindow);

                    frm.timer1.Tick += delegate(object sender, EventArgs e)
                    {
                        editor.PreviewWindowUI.Refresh();
                    };

                    Application.Run(frm);
                }
            }
        }

        public EditorForm()
        {
            InitializeComponent();
        }
    }
}
