using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using BO;

namespace BOF
{
    public partial class CardTreeForm : OKCancelDatForm
    {
        PathTree old_parent = null;
        public CardTreeForm()
        {
            InitializeComponent();
            InitializeComponent();
            txtID.DataMember = "ID";
            txtParent_FP.DataMember = "Parent_FP";
            txtCode.DataMember = "Code";
            txtName.DataMember = "Name";
        }
        
        private void CardTreeForm_Load(object sender, EventArgs e)
        {
            ITreeDat dat = NewValue as ITreeDat;
            if (dat != null)
            {
                old_parent = dat.Parent_FP;
                chkRoot.Enabled = ((BaseDat)dat).IsNew && old_parent != null;
                chkRoot.Checked = (dat.Parent_FP == null);
                BindControls(this);
            }
            chkRoot_CheckedChanged(this, EventArgs.Empty);
        }

        public virtual void chkRoot_CheckedChanged(object sender, EventArgs e)
        {
            ITreeDat dat = NewValue as ITreeDat;
            if (dat != null)
            {
                dat.Parent_FP = (chkRoot.Checked) ? null : old_parent;
                dat.FP = new PathTree(dat.Parent_FP, dat.FP.Code);
                Text = (((BaseDat)dat).IsNew)
                    ? "Новый" + ((dat.Parent_FP == null) ? " корневой узел" : " подузел в ветке " + dat.Parent_FP.ToString())
                    : string.Format("Изменение узла {0}", dat);
            } 
        }
    }
}

