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
    public abstract class TreeNFormController<TS, TD> : FormController
        where TS : BaseSet<TD, TS>, new()
        where TD : BaseDat<TD>, ITreeNDat, new()
    {
        public TreeNController<TS, TD> TreeList = new TreeNController<TS, TD>();
        //CtlCaptionPanel PanelCaption;

        public TreeNFormController(Form mdiForm, string name, string caption, Icon icon, CardTreeNForm tree_form)
            : base(mdiForm, name, caption, icon)
        {
            PaintPanels();
            TreeList.EntityForm = tree_form;
            //fltController.Init(FToolBar);
            //ToolBar.Items[2].Enabled = ToolBar.Items[3].Enabled = ToolBar.Items[4].Enabled = false;

            TreeList.Init(SetTree, FForm.ContentPanel, new CardTreeNForm());
            FForm.MdiParent = mdiForm;
            FForm.FormController = this;

            FToolBar.Visible = (FToolBar.Items != null && FToolBar.Items.Count > 0);
                
            Show();
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

        //private Panel _PanelTree = null;
        //public Panel PanelTree
        //{
        //    get { return _PanelTree; }
        //}
        //public ToolStrip FToolBar
        //{
        //    get { return FForm.FToolBar; }
        //}
        public virtual void PaintPanels()
        {
            FForm.SuspendLayout();
            FForm.ContentPanel.Controls.Clear();
            Panel panel = FForm.ContentPanel;

            //ToolStripButton ButtonGrouping = new ToolStripButton("Группировка", BOF.Icons.GroupPanel);
            //ButtonGrouping.Click += new EventHandler(ButtonGrouping_Click);
            //FToolBar.Items.Add(ButtonGrouping);

            //FToolBar.GripStyle = ToolStripGripStyle.Visible;

            FForm.ResumeLayout();
        }
    }
}
