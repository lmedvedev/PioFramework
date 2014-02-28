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
    public partial class CardForm : OKCancelDatForm
    {
        public static Type ExtraInfoForm;
        public static event EventHandler FillExtraInfo;
        public CardForm()
        {
            InitializeComponent();
        }

        private void CardForm_Load(object sender, EventArgs e)
        {
            if (FillExtraInfo != null)
                FillExtraInfo(this, EventArgs.Empty);

            ICardDat dat = NewValue as ICardDat;
            if (dat != null)
            {
                this.Text = (NewValue.IsNew)
                    ? string.Format("Новая карточка в ветке {0}", dat.Parent_FP)
                    : string.Format("Изменение карточки {0}", dat);
                BindControls(this);
            }
        }

        virtual public Dictionary<string, Control> FillExtraControls(ICardDat card)
        {
            return null;
        }

        virtual public void FillExtraFields(ICardDat card, Dictionary<string, Control> ctls)
        {
        }
    }
}

