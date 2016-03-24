using GS_PatEditor.Editor.Editable;
using GS_PatEditor.Pat.Effects;
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
    public partial class EffectListEditForm : Form
    {
        private readonly Pat.Project _Project;
        private readonly Pat.EffectList _Effects;

        public EffectListEditForm(Pat.Project proj, Pat.EffectList effects)
        {
            InitializeComponent();

            treeView1.LinkedPropertyGrid = propertyGrid1;
            treeView1.LinkedDeleteButton = button1;
            treeView1.LinkedResetButton = button2;

            _Project = proj;
            _Effects = effects;
            RefreshList();
        }

        private void RefreshList()
        {
            treeView1.ResetList<Pat.Effect>(new EditableEnvironment(_Project), _Effects.Effects);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            var node = treeView1.SelectedNode as IEditableTreeNode;
            if (node != null)
            {
                node.Reset();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var node = treeView1.SelectedNode as IEditableTreeNode;
            if (node != null)
            {
                node.Delete();
            }
        }
    }
}
