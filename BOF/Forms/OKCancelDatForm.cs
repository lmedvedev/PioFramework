using System;
using System.Collections.Generic;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using BO;

namespace BOF
{
    public partial class OKCancelDatForm : OKCancelForm, IHelpLabel
    {
        public OKCancelDatForm()
        {
            InitializeComponent();
        }

        private BaseDat _OldValue = null;
        public BaseDat OldValue
        {
            get { return _OldValue; }
            set
            {
                _OldValue = value;
                if (value == null)
                    throw new Exception("OldValue == null");
                NewValue = value.Clone() as BaseDat;
            }
        }

        public virtual BaseDat NewValue
        {
            get { return (BaseDat)base.Value; }
            set { base.Value = value; }
        }

        bool _IsChanged = false;
        public bool IsChanged
        {
            get { return _IsChanged; }
            set 
            {
                btnOK.Enabled = _IsChanged = value;
            }
        }

        bool _AllowClose = false;
        [Description("Форма сама следит за своим сохранением - переспрашивает на Esc и сохраняет на Ctrl+Enter")]
        public bool AllowClose
        {
            get { return _AllowClose; }
            set 
            { 
                _AllowClose = value;
                AcceptButton = _AllowClose ? btnOK : null;
                CancelButton = _AllowClose ? btnCancel : null;
            }
        }
        private void OKCancelDatForm_Load(object sender, EventArgs e)
        {
            if (AllowClose)
                AppendChanged(this.Controls);
            btnOK.Enabled = !AllowClose;
        }
        void AppendChanged(Control.ControlCollection controls)
        {
            foreach (Control ctl in controls)
            {
                if (ctl is CtlGrid)
                {
                    ((CtlGrid)ctl).ListChanged += delegate(object snd, ListChangedEventArgs ev) { IsChanged = true; };
                }
                else if (ctl is IDataMember && !(ctl is ISelectable))
                    ((IDataMember)ctl).ValueChanged += delegate(object snd, EventArgs ev) { IsChanged = true; };
                else if (ctl.Controls.Count > 0)
                    AppendChanged(ctl.Controls);
            }
        }
        bool _firstTime = true;
        private void OKCancelDatForm_Activated(object sender, EventArgs e)
        {
            if (_firstTime && _AllowClose)
                IsChanged = false;
            _firstTime = false;
        }
        
        private bool _CanSave = true;
        public bool CanSave
        {
            get { return _CanSave; }
            set { _CanSave = value; }
        }

        protected override void CloseForm(FormClosingEventArgs e)
        {
            if (DialogResult == DialogResult.OK)
            {
                try
                {
                    GetEntity();
                    if (NewValue == null) return;
                    if (_CanSave)
                    {
                        DatErrorList wList = NewValue.ValidateWarnings();
                        DatErrorList eList = NewValue.ValidateErrors();
                        DatErrorProvider.Clear();
                        if (eList.Count > 0)
                        {
                            string ErrorsMessage = "";
                            foreach (DatErrorList.DatError derr in eList)
                            {
                                ErrorsMessage += derr.Description + "\n";
                            }
                            MarkErrorControls(this, eList);
                            MessageBox.Show(ErrorsMessage, "Обнаружены ошибки, сохранение невозможно", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            e.Cancel = true;
                            return;
                        }

                        if (wList.Count > 0)
                        {
                            string ErrorsMessage = "";
                            foreach (DatErrorList.DatError derr in wList)
                            {
                                ErrorsMessage += derr.Description + "\n";
                            }
                            ErrorsMessage += "\n\nВы по прежнему хотите сохранить запись?\n";
                            //ErrorsMessage += "\n" + DialogResult.Yes.ToString() + " - выйти и сохранить.";
                            //ErrorsMessage += "\n" + DialogResult.No.ToString() + " - выйти, не сохраняя.";
                            //ErrorsMessage += "\n" + DialogResult.Cancel.ToString() + " - возврат к редактированию.";
                            //DialogResult r = MessageBox.Show(ErrorsMessage, "Обнаружены замечания", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button3);
                            DialogResult r = MessageBox.Show(ErrorsMessage, "Обнаружены замечания", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2);

                            switch (r)
                            {
                                case DialogResult.Abort:
                                case DialogResult.Cancel:
                                case DialogResult.None:
                                case DialogResult.No:
                                    e.Cancel = true;
                                    return;
                                //case DialogResult.No:
                                //    e.Cancel = false;
                                //    return;
                                case DialogResult.OK:
                                case DialogResult.Retry:
                                case DialogResult.Yes:
                                case DialogResult.Ignore:
                                    break;
                                default:
                                    break;
                            }
                        }

                        if (NewValue is IHDWrapper)
                            ((IHDWrapper)NewValue).Save(OldValue);
                        else if (!NewValue.EqualDat(OldValue))
                        {
                            NewValue.Save();
                            //if (NewValue is IDat)
                            //    NewValue.Load(((IDat)NewValue).ID);
                        }
                    }
                    if (_OldValue != null)
                        NewValue.CopyTo(_OldValue);

                }
                catch (Exception exp)
                {
                    e.Cancel = true;
                    new ExceptionForm("Внимание", "Не удалось сохранить элемент.\nДля более подробной информации раскройте детали сообщения об ошибке.", exp, MessageBoxIcon.Error).ShowDialog();
                }
            }
            else
            {
                //GetEntity();
                //bool changed = (NewValue != null && !NewValue.EqualDat(OldValue));
                if (IsChanged)
                {
                    DialogResult res = MessageBox.Show("Данные были изменены! Сохранить?\nОтмена - продолжить редактирование.", "Внимание", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
                    if (res == DialogResult.Yes)
                    {
                        this.DialogResult = DialogResult.OK;
                        CloseForm(e);
                    }
                    e.Cancel = (res == DialogResult.Cancel);
                }
            }

        }
        //Это закоментарил Кишик так как этот код нах не нужен, он есть в базовой OKCancelForm, при этом он вызывал глюки.
        //private void OKCancelDatForm_FormClosing(object sender, FormClosingEventArgs e)
        //{
        //    CloseForm(e);
        //}
        private void OKCancelDatForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            try
            {
                UnbindControls(this);
            }
            catch (Exception ex)
            {
                MessageBox.Show(Common.ExMessage(ex), "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        public void BindControls(Control ctlEntity)
        {
            foreach (Control ctl in ctlEntity.Controls)
            {
                if (ctl is IDataMember && ctl.DataBindings.Count == 0)
                {
                    ((IDataMember)ctl).AddBinding(NewValue);
                }
                else
                    BindControls(ctl);
            }
            Application.DoEvents();
        }
        public void MarkErrorControls(Control ctlEntity, DatErrorList eList)
        {
            foreach (Control ctl in ctlEntity.Controls)
            {
                if (ctl is IDataMember || ctl is CtlGrid)
                {
                    CtlGrid grd = ctl as CtlGrid;
                    DatErrorList.DatError error = eList.Find(delegate(DatErrorList.DatError de) { return de.PropertyName == (grd == null ? ((IDataMember)ctl).DataMember : grd.DataMember); });
                    if (error != null)
                        SetError(ctl, error.Description);
                }
                else
                    MarkErrorControls(ctl, eList);
            }
            Application.DoEvents();
        }
        public void UnbindControls(Control ctlEntity)
        {
            foreach (Control ctl in ctlEntity.Controls)
            {
                if (ctl is IDataMember)
                {
                    ((IDataMember)ctl).RemoveBinding();
                }
                else
                    UnbindControls(ctl);
            }
        }
        public void WriteBindingValues(Control ctlEntity)
        {
            foreach (Control ctl in ctlEntity.Controls)
            {
                if (ctl is IDataMember)
                {
                    ((IDataMember)ctl).WriteValue();
                }
                else
                    WriteBindingValues(ctl);
            }
        }
        public void WriteBindingValue(Control ctl)
        {
            if (ctl is IDataMember)
            {
                ((IDataMember)ctl).WriteValue();
            }
        }
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (_AllowClose && (keyData == (Keys.Enter | Keys.Control)))
            {
                btnOK.Focus();
                this.DialogResult = DialogResult.OK;
                this.Close();
                return true;
            }
            else if (_AllowClose && (keyData == Keys.Escape))
            {
                btnCancel.Focus();                              
                this.DialogResult = DialogResult.Cancel;
                this.Close();
                return true;
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }

        #region IHelpLabel Members

        public virtual void WriteHelp(string text)
        {
            lblHelp.Text = text;
            toolTip.SetToolTip(lblHelp, text);
        }

        public void SetError(Control ctl, string description)
        {
            DatErrorProvider.SetError(ctl, description);
        }

        #endregion
    }
}