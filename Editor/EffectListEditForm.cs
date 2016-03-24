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
            treeView1.Tag = (Action)UpdateSelectedNode;

            _Project = proj;
            _Effects = effects;
            RefreshList();
        }

        private void RefreshList()
        {
            treeView1.Nodes.Clear();
            var me = new ListMultiEditable<Pat.Effect> { List = _Effects.Effects };
            var env = new EditableEnvironment(_Project);
            foreach (var effect in _Effects)
            {
                //treeView1.Nodes.Add(new EditableEffectTreeNode(effect,
                //    new ListMultiEditable<Pat.Effect> { List = _Effects.Effects }));
                treeView1.Nodes.Add(EditableNodeGenerator.Create<Pat.Effect>(env, effect, me));
            }
            treeView1.Nodes.Add(EditableNodeGenerator.Create<Pat.Effect>(env, me));
        }

        private void treeView1_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            treeView1.SelectedNode = e.Node;
            UpdateSelectedNode();
        }

        private void UpdateSelectedNode()
        {
            var node = treeView1.SelectedNode;
            if (node == null)
            {
                propertyGrid1.SelectedObject = null;
                button1.Enabled = false;
                button2.Enabled = false;
                return;
            }

            propertyGrid1.SelectedObject = node.Tag;

            var nnode = node as IEditableTreeNode;
            if (nnode != null)
            {
                button2.Enabled = nnode.CanReset;
                button1.Enabled = nnode.CanDelete;
            }
            else
            {
                button2.Enabled = false;
                button1.Enabled = false;
            }
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
