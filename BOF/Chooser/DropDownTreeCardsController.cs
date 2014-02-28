using System;
using System.Collections.Generic;
using System.Text;
using BO;
using DA;
using System.Windows.Forms;

namespace BOF
{
    /// <summary>
    /// Контроллер для выбора Card или Tree - из DropDown-списка.
    /// </summary>
    /// <typeparam name="TS">Тип TreeSet-класса</typeparam>
    /// <typeparam name="TD">Тип TreeDat-класса</typeparam>
    /// <typeparam name="CS">Тип CardSet-класса</typeparam>
    /// <typeparam name="CD">Тип CardDat-класса</typeparam>
    public class DropDownTreeCardsController<TS, TD, CS, CD> : DropDownController<CS, CD>
        where TS : BaseSet<TD, TS>, new()
        where CS : BaseSet<CD, CS>, new()
        where TD : BaseDat<TD>, ITreeDat, new()
        where CD : BaseDat<CD>, ICardDat, new()
    {

        #region Constructors
        protected DropDownTreeCardsController() { }

        public DropDownTreeCardsController(ChooserEditor editor)
            : this(editor, new DropDownGridStatus(), true) { }

        public DropDownTreeCardsController(ChooserEditor editor, bool reload)
            : this(editor, new DropDownGridStatus(), reload) { }
        
        //public DropDownTreeCardsController(ChooserEditor editor, IDropDownControl ctl)
        //    : base(editor, ctl)
        //{
        //    ddController = new CardGridChooserController<TS, TD, CS, CD>(ctl.Grid);
        //    ddController.Reload(null);
        //    ddController.DatValueChanged += new DatEventDelegate(ddController_DatValueChanged);
        //    ddController.DatValueSelected += new DatEventDelegate(ddController_DatValueSelected);
        //    ddController.ListReloaded += new EventHandler<EventArgs<TD>>(ddController_ListReloaded);
        //}
        
        public DropDownTreeCardsController(ChooserEditor editor, IDropDownControl ctl, bool reload)
            : base(editor, ctl, reload)
        {
            ddController = new CardGridChooserController<TS, TD, CS, CD>(ctl.Grid);
            ddController.Reload(null);
            ddController.DatValueChanged += new DatEventDelegate(ddController_DatValueChanged);
            ddController.DatValueSelected += new DatEventDelegate(ddController_DatValueSelected);
            ddController.ListReloaded += new EventHandler<EventArgs<TD>>(ddController_ListReloaded);
        }
        #endregion

        void ddController_ListReloaded(object sender, EventArgs<TD> e)
        {
            ddControl.WriteText(BaseDat.ToString(e.Data));
        }

        void ddController_DatValueSelected(object sender, DatEventArgs e)
        {
            BaseDat dat = e.DatEntity;
            Editor.Value = dat;
            CloseDropDown();
            //Console.WriteLine("ddController_DatValueSelected");
        }

        void ddController_DatValueChanged(object sender, DatEventArgs e)
        {
            //Console.WriteLine("ddController_DatValueChanged {0}",e.DatEntity);
        }
        protected override void OnRowEntered(object sender, EventArgs<int> e)
        {
            // Просто перекрываем базовый функционал - обработка RowEntered 
            // здесь происходит в CardGridChooserController
            return;
        }
        protected override void OnRowSelected(object sender, EventArgs<int> e)
        {
            BaseDat dat = ddController.GetValue();
            Editor.Value = dat;
            CloseDropDown();
            //Console.WriteLine("OnRowSelected");
        }
        protected override void OnValueChanged(object sender, EventArgs e)
        {
            if (ddControl != null && Editor != null)
                ddControl.WriteText(BaseDat.ToString(Editor.Value));
            //Console.WriteLine("OnValueChanged");
        }
        protected override void OnValueDropDown(object sender, EventArgs<BaseDat> e)
        {
            if (e.Data == null)
            {
                if (string.IsNullOrEmpty(Editor.Text))
                    ddController.Reload();
                else
                    ddController.ReloadSearch(Editor.Text);
            }
            DropDownControl((Control)ddControl);
        }
        public void Reload()
        {
            ddController.Reload(null);
        }
        CardGridChooserController<TS, TD, CS, CD> ddController;

        public void Reload(PathTree root)
        {
            ddController.Reload(root);
        }
    }
}
