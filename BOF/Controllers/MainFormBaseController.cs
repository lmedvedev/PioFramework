using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using BO;

namespace BOF
{
    public class MainFormBaseController
    {
        public static MainFormBaseController MainController;
        public bool MergeMenu = false;
        public MainFormBase Frm;
        public Dictionary<string, string> StatusLabelText = new Dictionary<string, string>();
        private ToolStripMenuItem mnuWindows;
        public ToolStripMenuItem mnuHelp;
        //private ToolStripLabel tsLabel = new ToolStripLabel();
        private ToolStripSeparator tsSeparator = new ToolStripSeparator();

        private Font ActiveButtonFontB;
        private Font ActiveButtonFontR;

        #region Constructors
        public MainFormBaseController(string caption, Icon icon, bool MDI)
        {
            Frm = new MainFormBase(MDI);
            Frm.SuspendLayout();

            //Frm.Name = name;
            Frm.Text = caption;
            Frm.Icon = icon;
            Frm.WindowState = FormWindowState.Maximized;
            //tsLabel.AutoSize = true;
            ActiveButtonFontB = new Font(Frm.statusStripMF.Font, FontStyle.Bold);
            ActiveButtonFontR = new Font(Frm.statusStripMF.Font, FontStyle.Regular);

            Frm.statusStripMF.ItemClicked += new ToolStripItemClickedEventHandler(statusStripMF_ItemClicked);
            //Frm.Load += new EventHandler(MainFormBase_Load);
            //Frm.ResizeBegin += new EventHandler(Frm_ResizeBegin);
            //Frm.ResizeEnd += new EventHandler(Frm_ResizeEnd);
            Frm.statusStripMF.Resize += new EventHandler(statusStripMF_Resize);
            if (MDI)
            {
                foreach (Control ctl in Frm.Controls)
                {
                    if (ctl is MdiClient)
                    {
                        MdiClient mcli = ctl as MdiClient;
                        mcli.ControlAdded += new ControlEventHandler(mcli_ControlAdded);
                        mcli.ControlRemoved += new ControlEventHandler(mcli_ControlRemoved);
                        Frm.MdiChildActivate += new System.EventHandler(this.MainForm_MdiChildActivate);
                        break;
                    }
                }
            }

            AddMenuItem("Выход", mnuExit_Click);
            mnuHelp = AddMenuItem("Справка");
            mnuWindows = AddMenuItem("Окна");

            AddMenuSubItem(mnuHelp, "О программе", Properties.Resources.information, mnuAbout_Click);
            AddMenuSubItem(mnuWindows, "Закрыть все", mnuCloseAll_Click);
            AddMenuSubItem(mnuWindows, "Расположить горизонтально", Properties.Resources.application_tile_vertical, mnuTileH_Click);
            AddMenuSubItem(mnuWindows, "Расположить вертикально", Properties.Resources.application_tile_horizontal, mnuTileV_Click);
            AddMenuSubItem(mnuWindows, "Каскадом", Properties.Resources.application_cascade, mnuCascade_Click);
            Frm.menuStripMF.GripStyle = ToolStripGripStyle.Hidden;
            Frm.menuStripMF.MdiWindowListItem = mnuWindows;
            Frm.ResumeLayout(false);
            Frm.PerformLayout();
            //if (System.Globalization.CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator != ".")
            //    this.Frm.Text += " | десятичный разделитель - не точка";
        }

        void statusStripMF_Resize(object sender, EventArgs e)
        {
            SyncMdiButtons();
        }

        #endregion

        #region Methods

        public void AddSeparator(ToolStripMenuItem ParentItem)
        {
            ParentItem.DropDownItems.Add(new ToolStripSeparator());
        }
        public ToolStripMenuItem AddMenuItem(MenuStrip Strip, string ItemText, Image icon, EventHandler onClick)
        {
            ToolStripMenuItem newItem = new ToolStripMenuItem(ItemText, icon, onClick);
            Strip.Items.Insert(0, newItem);
            return newItem;
        }
        public ToolStripMenuItem AddMenuItem(MenuStrip Strip, string ItemText, EventHandler onClick)
        {
            return AddMenuItem(Strip, ItemText, null, onClick);
        }
        public ToolStripMenuItem AddMenuItem(MenuStrip Strip, string ItemText, Image icon)
        {
            return AddMenuItem(Strip, ItemText, icon, null);
        }
        public ToolStripMenuItem AddMenuItem(string ItemText, EventHandler onClick)
        {
            return AddMenuItem(Frm.menuStripMF, ItemText, onClick);
        }
        public ToolStripMenuItem AddMenuItem(string ItemText)
        {
            return AddMenuItem(Frm.menuStripMF, ItemText, null, null);
        }
        public ToolStripMenuItem AddMenuItem<T>(string ItemText) where T : FormController //ListFormController
        {
            return AddMenuItem(ItemText, MenuItem_Click<T>);
        }
        public ToolStripMenuItem AddMenuSubItem(ToolStripMenuItem ParentItem, string ItemText, Image icon, EventHandler onClick)
        {
            ToolStripMenuItem newItem = new ToolStripMenuItem(ItemText, icon, onClick);
            ParentItem.DropDownItems.Add(newItem);
            return newItem;
        }
        public ToolStripMenuItem AddMenuSubItem(ToolStripMenuItem ParentItem, string ItemText, Image icon)
        {
            return AddMenuSubItem(ParentItem, ItemText, icon, null);
        }
        public ToolStripMenuItem AddMenuSubItem(ToolStripMenuItem ParentItem, string ItemText, EventHandler onClick)
        {
            return AddMenuSubItem(ParentItem, ItemText, null, onClick);
        }
        public ToolStripMenuItem AddMenuSubItem(ToolStripMenuItem ParentItem, string ItemText)
        {
            return AddMenuSubItem(ParentItem, ItemText, null, null);
        }
        public ToolStripMenuItem AddMenuSubItem<T>(ToolStripMenuItem ParentItem, string ItemText, Image icon) where T : FormController //ListFormController
        {
            return AddMenuSubItem(ParentItem, ItemText, icon, MenuItem_Click<T>);
        }
        public ToolStripMenuItem AddMenuSubItem<T>(ToolStripMenuItem ParentItem, string ItemText) where T : FormController //ListFormController
        {
            return AddMenuSubItem<T>(ParentItem, ItemText, null);
        }

        public void SyncMdiButtons()
        {
            int labelPrefix = 0;
            int wi = 200;

            Frm.statusStripMF.SuspendLayout();
            Frm.statusStripMF.Items.Clear();
            foreach (string labelText in StatusLabelText.Keys)
            {
                if (StatusLabelText[labelText].Length > 0)
                {
                    ToolStripLabel lsLabel = new ToolStripLabel(StatusLabelText[labelText]);
                    ToolStripSeparator lsSep = new ToolStripSeparator();
                    lsLabel.AutoSize = true;
                    Frm.statusStripMF.Items.Add(lsLabel);
                    Frm.statusStripMF.Items.Add(new ToolStripSeparator());
                    labelPrefix += lsLabel.Width + lsSep.Width;
                }
            }

            if (Frm.MdiChildren.Length > 0)
            {
                wi = (int)Math.Floor(((decimal)(Frm.statusStripMF.Width - labelPrefix - 50)) / ((decimal)Frm.MdiChildren.Length));

                if (wi > 200) wi = 200;
                if (wi < 100) wi = 100;

                foreach (Form childFrm in Frm.MdiChildren)
                {
                    string stripText = (childFrm.Text.Trim().Length > 0) ? childFrm.Text : childFrm.Name;
                    ToolStripButton but = new ToolStripButton(stripText);

                    if (childFrm == Frm.ActiveMdiChild)
                    {
                        but.CheckState = CheckState.Checked;
                        but.Font = ActiveButtonFontB;
                    }
                    else
                    {
                        but.CheckState = CheckState.Unchecked;
                        but.Font = ActiveButtonFontR;
                    }

                    but.Tag = childFrm;

                    but.AutoSize = false;
                    but.Width = wi;
                    but.TextAlign = ContentAlignment.MiddleLeft;
                    but.ImageScaling = ToolStripItemImageScaling.SizeToFit;
                    but.ImageAlign = ContentAlignment.MiddleLeft;
                    Image FormIcon = childFrm.Icon.ToBitmap();
                    but.Image = FormIcon;
                    Frm.statusStripMF.Items.Add(but);
                    //ToolStripSeparator sep = new ToolStripSeparator();
                    //Frm.statusStripMF.Items.Add(tsSeparator);
                    Frm.statusStripMF.Items.Add(new ToolStripSeparator());
                }
            }

            Frm.statusStripMF.ResumeLayout();
        }
        #endregion

        #region Events
        //private void Frm_ResizeEnd(object sender, EventArgs e)
        //{
        //    SyncMdiButtons();
        //    //Frm.ResumeLayout(false);
        //    //Frm.PerformLayout();
        //}
        //private void Frm_ResizeBegin(object sender, EventArgs e)
        //{
        //    //Frm.SuspendLayout();
        //}
        private void MainFormBase_Load(object sender, EventArgs e)
        {
            SyncMdiButtons();
            //foreach (string labelText in StatusLabelText.Keys)
            //{
            //    if (StatusLabelText[labelText].Length > 0)
            //    {
            //        Frm.statusStripMF.Items.Add(new ToolStripLabel(labelText));
            //        Frm.statusStripMF.Items.Add(new ToolStripSeparator());
            //    }
            //}
        }

        private void MainForm_MdiChildActivate(object sender, EventArgs e)
        {
            Frm.statusStripMF.SuspendLayout();
            foreach (ToolStripItem but in Frm.statusStripMF.Items)
            {
                if (but is ToolStripButton && Frm.ActiveMdiChild != null)
                {
                    but.Text = ((Form)but.Tag).Text;
                    if (but.Tag == Frm.ActiveMdiChild)
                    {
                        ((ToolStripButton)but).CheckState = CheckState.Checked;
                        but.Font = ActiveButtonFontB;
                        ListForm frm = Frm.ActiveMdiChild as ListForm;
                        if (frm != null && MergeMenu)
                            frm.FormController.MergeMenu();
                    }
                    else
                    {
                        ((ToolStripButton)but).CheckState = CheckState.Unchecked;
                        but.Font = ActiveButtonFontR;
                    }
                }
            }
            Frm.statusStripMF.ResumeLayout();
        }
        private void mcli_ControlRemoved(object sender, EventArgs e)
        {
            SyncMdiButtons();
        }
        private void mcli_ControlAdded(object sender, EventArgs e)
        {
            SyncMdiButtons();
        }
        private void statusStripMF_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            if (e.ClickedItem is ToolStripButton)
            {
                Frm.SuspendLayout();
                Form clickedForm = (Form)e.ClickedItem.Tag;
                if (clickedForm.WindowState == FormWindowState.Minimized)
                    clickedForm.WindowState = FormWindowState.Normal;
                clickedForm.SuspendLayout();
                clickedForm.Activate();
                clickedForm.ResumeLayout();
                Frm.ResumeLayout();
            }
        }
        private void mnuAbout_Click(object sender, EventArgs e)
        {
            AboutForm frm = new AboutForm(Icon.FromHandle(Properties.Resources.information.GetHicon()));
            frm.ShowDialog();
        }
        private void mnuExit_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Вы действительно хотите завершить приложение?", "Завершение приложения", MessageBoxButtons.OKCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.OK)
                Frm.Close();
        }
        private void mnuCloseAll_Click(object sender, EventArgs e)
        {
            foreach (Form fr in Frm.MdiChildren)
            {
                fr.Close();
            }
        }
        private void mnuCascade_Click(object sender, EventArgs e)
        {
            Frm.LayoutMdi(System.Windows.Forms.MdiLayout.Cascade);
        }
        private void mnuTileH_Click(object sender, EventArgs e)
        {
            Frm.LayoutMdi(System.Windows.Forms.MdiLayout.TileHorizontal);
        }
        private void mnuTileV_Click(object sender, EventArgs e)
        {
            Frm.LayoutMdi(System.Windows.Forms.MdiLayout.TileVertical);
        }

        private void MenuItem_Click<T>(object sender, EventArgs e) where T : FormController //ListFormController
        {
            ActivateController<T>(null);
            //try
            //{
            //    ToolStripMenuItem tsmi = sender as ToolStripMenuItem;
            //    if (tsmi != null)
            //    {
            //        if (!string.IsNullOrEmpty(tsmi.Name))
            //        {
            //            foreach (Form form in Frm.MdiChildren)
            //            {
            //                if (form.Name == tsmi.Name)
            //                {
            //                    form.Activate();
            //                    return;
            //                }
            //            }
            //        }
            //        ListFormController controller = Activator.CreateInstance(typeof(T), Frm) as ListFormController;
            //        if (controller != null)
            //        {
            //            tsmi.Name = controller.Form.Name;
            //        }

            //    }
            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show(Common.ExMessage(ex), "Ошибка");
            //}
        }

        public void ActivateController<C>(BaseLoadFilter filter)
            where C : FormController //ListFormController
        {
            //ListFormController controller = null;
            FormController controller = null;
            try
            {
                foreach (Form form in Frm.MdiChildren)
                {
                    ListForm lf = form as ListForm;
                    if (lf != null && lf.FormController.GetType() == typeof(C))
                    {
                        controller = lf.FormController;
                        lf.Activate();
                        break;
                    }
                }
                
                if (controller == null && filter == null)
                    //controller = (ListFormController)Activator.CreateInstance(typeof(C), Frm);
                    controller = (FormController)Activator.CreateInstance(typeof(C), Frm);
                else if (controller == null)
                    //controller = (ListFormController)Activator.CreateInstance(typeof(C), Frm, filter);
                    controller = (FormController)Activator.CreateInstance(typeof(C), Frm, filter);
                else if (filter != null)
                {
                    controller.InitFilter(filter);
                    controller.Refresh();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(Common.ExMessage(ex), "Ошибка");
            }
        }
        #endregion


    }
}
