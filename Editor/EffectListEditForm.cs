﻿using GS_PatEditor.Editor.Editable;
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
        public EffectListEditForm(Pat.Project proj, Pat.EffectList effects)
        {
            InitializeComponent();

            treeView1.LinkedPropertyGrid = propertyGrid1;
            treeView1.LinkedDeleteButton = button1;
            treeView1.LinkedResetButton = button2;

            treeView1.Nodes.AddEditableList(new EditableEnvironment(proj), effects.Effects);
        }
    }
}
