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
    public partial class CardTreeNForm : OKCancelDatForm
    {
        PathTreeN old_parent = null;
        public CardTreeNForm()
        {
            InitializeComponent();
            txtID.DataMember = "ID";
            txtParent_FP.DataMember = "Parent_FPn";
            txtCode.DataMember = "CodeS";
            txtName.DataMember = "Name";
        }
        
        private void CardTreeForm_Load(object sender, EventArgs e)
        {
            ITreeNDat dat = NewValue as ITreeNDat;
            if (dat != null)
            {
                old_parent = dat.Parent_FPn;
                chkRoot.Enabled = ((BaseDat)dat).IsNew && old_parent != null;
                chkRoot.Checked = (dat.Parent_FPn == null);
                BindControls(this);
            }
            chkRoot_CheckedChanged(this, EventArgs.Empty);
        }

        public virtual void chkRoot_CheckedChanged(object sender, EventArgs e)
        {
            ITreeNDat dat = NewValue as ITreeNDat;
            if (dat != null)
            {
                dat.Parent_FPn = (chkRoot.Checked) ? null : old_parent;
                dat.FPn = new PathTreeN(dat.Parent_FPn, dat.FPn.CodeS);
                Text = (((BaseDat)dat).IsNew)
                    ? "Новый" + ((dat.Parent_FPn == null) ? " корневой узел" : " подузел в ветке " + dat.Parent_FPn.ToString())
                    : string.Format("Изменение узла {0}", dat);
            } 
        }
    }
}

