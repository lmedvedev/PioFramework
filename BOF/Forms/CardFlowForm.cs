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
    public partial class CardFlowForm : CardForm
    {
        public CardFlowForm()
        {
            InitializeComponent();
        }

        protected virtual void Init()
        {
        }

        public virtual void Form_Load(object sender, EventArgs e)
        {
            Init();
            if (NewValue != null)
                this.Text = string.Format("{0}: {1}", (NewValue.IsNew) ? "Новая запись" : string.Format("Редактируем запись [{0}]", ((IDat)NewValue).ID), NewValue.ToString());
        }

    }
}

