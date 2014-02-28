using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using BO;
using System.Xml;
using DA;

namespace BOF
{
    public abstract class TreeNListFormController<TS, TD, CS, CD> : ListFormController<CS, CD>
        where TS : BaseSet<TD, TS>, new()
        where CS : BaseSet<CD, CS>, new()
        where TD : BaseDat<TD>, ITreeNDat, new()
        where CD : BaseDat<CD>, ICardNDat, new()
    {
        TreeNController<TS, TD> TreeList = new TreeNController<TS, TD>();
        CtlCaptionPanel PanelCaption;
        RichTextBox PanelRTF;

        public TreeNListFormController(Form mdiForm, string name, string caption, Icon icon, CardForm entity_form)
            : this(mdiForm, name, caption, icon, true, true, null)
        {
            EntityForm = entity_form;
        }

        public TreeNListFormController(Form mdiForm, string name, string caption, Icon icon, CardForm entity_form, CardTreeNForm tree_form)
            : this(mdiForm, name, caption, icon, true, true, null)
        {
            EntityForm = entity_form;
            TreeList.EntityForm = tree_form;
        }

        public TreeNListFormController(Form mdiForm, string name, string caption, Icon icon, bool TreePanel, bool EntityPanel, BaseLoadFilter loadfilter)
            : base(mdiForm, name, caption, icon, TreePanel, EntityPanel)
        {
            PaintPanels((TreePanel) ? 320 : 0, (EntityPanel) ? 140 : 0);
            //PaintPanels(TreePanel, EntityPanel);
            fltController.Init(FToolBar);
            FToolBar.Items[2].Enabled = FToolBar.Items[3].Enabled = FToolBar.Items[4].Enabled = false;

            gridMain.Init(PanelList);
            gridMain.EntityNew += new DatEventDelegate(EntityNew);
            gridMain.EntitySelected += new DatEventDelegate(EntitySelected);
            gridMain.EntityChanged += new DatEventDelegate(EntityChanged);

            //if (EntityPanel)
            //{
                PanelRTF = new RichTextBox();
                PanelRTF.Dock = DockStyle.Fill;
                PanelRTF.BackColor = Color.LightCyan;
                PanelRTF.BorderStyle = BorderStyle.None;
                PanelRTF.ReadOnly = true;
                PanelRTF.ScrollBars = RichTextBoxScrollBars.Vertical;
                PanelEntity.Controls.Add(PanelRTF);

                PanelCaption = new CtlCaptionPanel();
                PanelCaption.Active = false;
                PanelCaption.Dock = DockStyle.Top;
                PanelEntity.Controls.Add(PanelCaption);
            //}

            TreeList.Init(SetTree, PanelTree, new CardTreeNForm());
            TreeList.TreeChanged += new DatEventDelegate(TreeChanged);
            //TreeList.Reload();
            splitTL.Panel1Collapsed = false;

            gridMain.Grid.ValueEventDisabled = true;
            Show();
            gridMain.Grid.ValueEventDisabled = false;
            gridMain.Grid.FireValueChanged();
        }

        private static TS _SetTree;
        public static TS SetTree
        {
            get
            {
                if (_SetTree == null)
                {
                    _SetTree = new TS();
                    _SetTree.Load();
                }
                return _SetTree;
            }
            set
            {
                _SetTree = value;
            }
        }

        private static CardForm _EntityForm = null;
        public static CardForm EntityForm
        {
            get
            {
                return _EntityForm;
            }
            set
            {
                _EntityForm = value;
            }
        }

        void TreeChanged(object sender, DatEventArgs e)
        {
            ITreeNDat tree = (ITreeNDat)e.DatEntity;
            SetList.FilterReset();
            SetList.LoadFilter = new SQLFilter();
            SetList.LoadFilter.AddWhere(new FilterString("Parent_FPn", tree.FPn.ToString()));
            Refresh();
            gridMain.captList.Caption = Caption + " [ветка '" + tree.Name + "']";
        }

        public override void Refresh()
        {
            FForm.Cursor = Cursors.WaitCursor;
            try
            {
                CD dat = gridMain.Grid.Value as CD;
                SetStatusCount(0);
                try
                {
                    Application.DoEvents();
                    if (gridMain != null && gridMain.Grid != null)
                        WaitForm.Start(gridMain.Grid);
                    //TreeList.Reload();
                    SetList.Load();
                }
                finally
                {
                    WaitForm.Stop();
                    Application.DoEvents();
                }
                gridMain.Grid.SetDataSource(SetList);
                SetStatusCount(SetList.CountAll, SetList.Count);
                gridMain.Grid.Value = dat;
            }
            finally
            {
                //TreeChanged(null, null);
                gridMain.Grid.ValueEventDisabled = false;
                FForm.Cursor = Cursors.Default;
            }
        }

        void EntityNew(object sender, DatEventArgs e)
        {
            try
            {
                BaseDat dat = new CD();
                if (TreeList.TreeV.Value == null)
                    MessageBox.Show("Не задана ветка, в которой создается карточка !", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                else
                {
                    ((ICardNDat)dat).Parent_FPn = ((ITreeNDat)TreeList.TreeV.Value).FPn;
                    if (EntityForm == null)
                        throw new Exception("Не определена форма для редактирования карточки");
                    EntityForm.OldValue = dat;
                    EntityForm.BindControls(EntityForm);
                    if (EntityForm.ShowDialog() == DialogResult.OK)
                        Refresh();
                    gridMain.Grid.Value = dat;
                }
            }
            catch (Exception Ex)
            {
                MessageBox.Show(Common.ExMessage(Ex), "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        void EntitySelected(object sender, DatEventArgs e)
        {
            try
            {
                if (EntityForm == null)
                    throw new Exception("Не определена форма для редактирования карточки");

                EntityForm.OldValue = e.DatEntity;
                EntityForm.BindControls(EntityForm);
                if (EntityForm.ShowDialog() == DialogResult.OK)
                    Refresh();
            }
            catch (Exception Ex)
            {
                MessageBox.Show(Common.ExMessage(Ex), "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        void EntityChanged(object sender, DatEventArgs e)
        {
            try
            {
                if (e.DatEntity != null && PanelRTF != null)
                {
                    PanelCaption.Caption = "Детали карточки '" + e.DatEntity.ToString() + "':";
                    PanelRTF.Rtf = e.DatEntity.GetRTFDoc();
                }
            }
            catch (Exception Ex)
            {
                MessageBox.Show(Common.ExMessage(Ex), "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
