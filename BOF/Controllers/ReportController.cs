using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Drawing.Printing;
using System.Windows.Forms;
using System.Globalization;
using System.Xml;
using System.IO;
using BO;
using System.Reflection;
using System.Drawing.Drawing2D;
using BO.Reports;
using System.Xml.Serialization;
using System.Drawing.Imaging;

namespace BOF
{
    public class ReportController : ReportDocument
    {
        public void Preview()
        {
            if (!Common.IsNullOrEmpty(errors))
                MessageBox.Show(errors, "Ошибки");
            else if (CountPages > 0)
            {
                PreviewForm preview = new PreviewForm(this);
                preview.WindowState = FormWindowState.Maximized;
                preview.Text = Title;
                preview.ShowDialog();
            }
        }
    }

    public class PreviewForm : Form
    {
        private PrintPreviewControl ctlPreview;
        private CtlPanel pnlLeft;
        private Label lblPrinters;
        private Button btnPrint;
        private Button btnClose;
        private ComboBox cmbPrinters;
        //private CtlInt txtCount;
        private ListView lstDocs;
        //private Label lblCount;
        //private LinkLabel lnkClose;
        //private CtlBool blnForAll;
        private CtlBool blnOne;
        //Cursor с = new Cursor("MyCursor.Cur");  


        Cursor curZoomIn; //= CursorConverter.ConvertFrom(global::BOF.Properties.Resources.ZoomIn);
        Cursor curZoomOut; //= new Cursor(Assembly.GetExecutingAssembly().GetManifestResourceStream("BOF.Images.ZoomOut.cur"));
        private ReportController document = null;
        public PreviewForm(ReportController controller)
        {
            CursorConverter cc = new CursorConverter();
            
            curZoomIn = cc.ConvertFrom(global::BOF.Properties.Resources.ZoomIn) as Cursor;
            curZoomOut = cc.ConvertFrom(global::BOF.Properties.Resources.ZoomOut) as Cursor;

            InitializeComponent();
            ctlPreview.Document = document = controller;
            lstDocs.ShowItemToolTips = true;
            foreach (KeyValuePair<string, string> page in document.GetPages(true))
            {
                ListViewItem item = new ListViewItem(page.Key);
                item.Checked = true;
                item.ToolTipText = page.Value;
                lstDocs.Items.Add(item);
            }
            cmbPrinters.Items.Clear();
            foreach (string printer in PrinterSettings.InstalledPrinters)
            {
                cmbPrinters.Items.Add(printer);
            }
            cmbPrinters.SelectedIndex = cmbPrinters.Items.IndexOf(document.DefaultPrinter);
            ctlPreview.Cursor = curZoomIn;
            this.Load += new EventHandler(PreviewForm_Load);
        }

        public PreviewForm(PrintDocument pdoc)
        {
            CursorConverter cc = new CursorConverter();

            curZoomIn = cc.ConvertFrom(global::BOF.Properties.Resources.ZoomIn) as Cursor;
            curZoomOut = cc.ConvertFrom(global::BOF.Properties.Resources.ZoomOut) as Cursor;

            InitializeComponent();
            ctlPreview.Document = pdoc;
            lstDocs.ShowItemToolTips = true;
            //foreach (string page in document.GetPages(true))
            //{
            //    ListViewItem item = new ListViewItem(page);
            //    item.Checked = true;
            //    item.ToolTipText = page;
            //    lstDocs.Items.Add(item);
            //}
            cmbPrinters.Items.Clear();
            foreach (string printer in PrinterSettings.InstalledPrinters)
            {
                cmbPrinters.Items.Add(printer);
            }
            cmbPrinters.SelectedIndex = cmbPrinters.Items.IndexOf(pdoc.PrinterSettings.PrinterName);
            ctlPreview.Cursor = curZoomIn;
            this.Load += new EventHandler(PreviewForm_Load);
        }

        void PreviewForm_Load(object sender, EventArgs e)
        {
            lstDocs.ItemChecked += new ItemCheckedEventHandler(lstDocs_ItemChecked);
            lstDocs_SelectedIndexChanged(this, EventArgs.Empty);
            ctlPreview.MouseClick += new MouseEventHandler(ctlPreview_MouseClick);
        }

        void ctlPreview_MouseClick(object sender, MouseEventArgs e)
        {
            if (ctlPreview.Cursor == curZoomIn)
            {
                ctlPreview.Zoom *= 2;
                ctlPreview.Cursor = curZoomOut;
            }
            else
            {
                ctlPreview.Zoom /= 2;
                ctlPreview.Cursor = curZoomIn;
            }
        }

        void lstDocs_ItemChecked(object sender, ItemCheckedEventArgs e)
        {
            btnPrint.Enabled = (lstDocs.CheckedItems.Count > 0);
            document.SetPagePrint(lstDocs.Items.IndexOf(e.Item), e.Item.Checked);
        }

        //private void lnkClose_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        //{
        //    Hide();
        //}

        private void lstDocs_SelectedIndexChanged(object sender, EventArgs e)
        {
            ctlPreview.StartPage = (lstDocs.SelectedItems.Count == 0) ? 0 : lstDocs.Items.IndexOf(lstDocs.SelectedItems[0]);
            //txtCount.Value = document.GetPageCount(ctlPreview.StartPage);
        }

        //void txtCount_ValueChanged(object sender, EventArgs e)
        //{
        //    //document.SetPageCount(ctlPreview.StartPage, txtCount.Value);
        //}

        void blnOne_ValueChanged(object sender, EventArgs e)
        {
            pnlLeft.Invalidate();
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            try
            {
                if (blnOne.Value)
                    document.Print(1, cmbPrinters.Text);
                else
                    document.Print(0, cmbPrinters.Text);

            }
            catch (Exception Ex)
            {
                MessageBox.Show(Common.ExMessage(Ex), "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Hide();
        }

        void ctlPreview_Resize(object sender, EventArgs e)
        {
            if (ctlPreview.Cursor == curZoomOut) return;
            double zoom = (double)ctlPreview.Width / 860;
            if (zoom > 0) ctlPreview.Zoom = (zoom > 0.89) ? 0.89 : zoom;
        }

        void pnlLeft_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.DrawRectangle(new Pen(btnClose.BackColor), 11, pnlLeft.Height - 124, 218, 74);
            e.Graphics.DrawString(((blnOne.Value) ? "Все документы, помеченные в списке, выводятся на выбранный принтер в одном экземпляре" : "Количество экземпляров для каждого документа берётся из конфигурации"), blnOne.Font, new SolidBrush(Color.FromArgb(40, 40, 40)), new RectangleF(14, pnlLeft.Height - 96, 216, 48));
        }

        private void InitializeComponent()
        {
            this.pnlLeft = new BOF.CtlPanel();
            this.lstDocs = new System.Windows.Forms.ListView();
            //this.lblCount = new System.Windows.Forms.Label();
            this.lblPrinters = new System.Windows.Forms.Label();
            this.btnPrint = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.cmbPrinters = new System.Windows.Forms.ComboBox();
            //this.txtCount = new BOF.CtlInt();
            //this.blnForAll = new BOF.CtlBool();
            //this.lnkClose = new System.Windows.Forms.LinkLabel();
            this.blnOne = new BOF.CtlBool();
            this.pnlLeft.SuspendLayout();
            //((System.ComponentModel.ISupportInitialize)(this.txtCount)).BeginInit();
            this.SuspendLayout();
            // 
            // ctlPreview
            // 
            this.ctlPreview = new System.Windows.Forms.PrintPreviewControl();
            this.ctlPreview.BackColor = System.Drawing.Color.SlateGray;
            this.ctlPreview.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ctlPreview.Location = new System.Drawing.Point(236, 0);
            this.ctlPreview.Name = "ctlPreview";
            this.ctlPreview.AutoZoom = false;
            this.ctlPreview.Size = new System.Drawing.Size(377, 368);
            this.ctlPreview.TabIndex = 0;
            this.ctlPreview.Resize += new EventHandler(ctlPreview_Resize);
            // 
            // pnlLeft
            // 
            //this.pnlLeft.Controls.Add(this.lnkClose);
            this.pnlLeft.Controls.Add(this.lstDocs);
            //this.pnlLeft.Controls.Add(this.lblCount);
            this.pnlLeft.Controls.Add(this.lblPrinters);
            this.pnlLeft.Controls.Add(this.cmbPrinters);
            this.pnlLeft.Controls.Add(this.btnPrint);
            this.pnlLeft.Controls.Add(this.btnClose);
            //this.pnlLeft.Controls.Add(this.txtCount);
            //this.pnlLeft.Controls.Add(this.blnForAll);
            this.pnlLeft.Controls.Add(this.blnOne);
            this.pnlLeft.Dock = System.Windows.Forms.DockStyle.Left;
            this.pnlLeft.InactiveGradientHighColor = System.Drawing.Color.FromArgb(((int)(((byte)(210)))), ((int)(((byte)(226)))), ((int)(((byte)(237)))));
            this.pnlLeft.InactiveGradientLowColor = System.Drawing.Color.FromArgb(((int)(((byte)(161)))), ((int)(((byte)(191)))), ((int)(((byte)(217)))));
            this.pnlLeft.Location = new System.Drawing.Point(0, 0);
            this.pnlLeft.Name = "pnlLeft";
            this.pnlLeft.Size = new System.Drawing.Size(236, 368);
            this.pnlLeft.TabIndex = 0;
            this.pnlLeft.Paint += new PaintEventHandler(pnlLeft_Paint);
            // 
            // lstDocs
            // 
            this.lstDocs.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.lstDocs.CheckBoxes = true;
            this.lstDocs.FullRowSelect = true;
            this.lstDocs.Location = new System.Drawing.Point(10, 23);
            this.lstDocs.MultiSelect = false;
            this.lstDocs.Name = "lstDocs";
            this.lstDocs.Size = new System.Drawing.Size(220, 130);
            this.lstDocs.TabIndex = 0;
            this.lstDocs.UseCompatibleStateImageBehavior = false;
            this.lstDocs.View = System.Windows.Forms.View.List;
            this.lstDocs.SelectedIndexChanged += new System.EventHandler(this.lstDocs_SelectedIndexChanged);
            // 
            // btnPrint
            // 
            this.btnPrint.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnPrint.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(81)))), ((int)(((byte)(101)))), ((int)(((byte)(125)))));
            this.btnPrint.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btnPrint.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.btnPrint.Location = new System.Drawing.Point(10, 160);
            this.btnPrint.Name = "btnPrint";
            this.btnPrint.Size = new System.Drawing.Size(220, 37);
            this.btnPrint.TabIndex = 1;
            this.btnPrint.Text = "Печатать отмеченные";
            this.btnPrint.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.btnPrint.UseVisualStyleBackColor = false;
            this.btnPrint.Click += new System.EventHandler(this.btnPrint_Click);
            //// 
            //// lblCount
            //// 
            //this.lblCount.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            //this.lblCount.AutoSize = true;
            //this.lblCount.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            //this.lblCount.Location = new System.Drawing.Point(119, 316);
            //this.lblCount.Name = "lblCount";
            //this.lblCount.Size = new System.Drawing.Size(90, 14);
            //this.lblCount.TabIndex = 14;
            //this.lblCount.Text = "Экземпляров";
            // 
            // lblPrinters
            // 
            this.lblPrinters.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblPrinters.AutoSize = false;
            this.lblPrinters.Font = new System.Drawing.Font("Verdana", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lblPrinters.Location = new System.Drawing.Point(8, 202);
            this.lblPrinters.Name = "lblPrinters";
            this.lblPrinters.Size = new System.Drawing.Size(220, 14);
            this.lblPrinters.TabIndex = 66;
            this.lblPrinters.Text = "Выводить на принтер";
            // 
            // cmbPrinters
            // 
            this.cmbPrinters.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.cmbPrinters.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbPrinters.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.cmbPrinters.FormattingEnabled = true;
            this.cmbPrinters.Location = new System.Drawing.Point(10, 218);
            this.cmbPrinters.Name = "cmbPrinters";
            this.cmbPrinters.Size = new System.Drawing.Size(220, 21);
            this.cmbPrinters.TabIndex = 2;
            // 
            // blnOne
            // 
            this.blnOne.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.blnOne.DataMember = null;
            this.blnOne.Value = false;
            this.blnOne.Font = new System.Drawing.Font("Verdana", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.blnOne.Location = new System.Drawing.Point(14, 247);
            this.blnOne.Name = "blnOne";
            this.blnOne.Text = "В одном экземпляре";
            this.blnOne.Size = new System.Drawing.Size(160, 18);
            this.blnOne.TabIndex = 4;
            this.blnOne.ValueChanged += new System.EventHandler(this.blnOne_ValueChanged);
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClose.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(81)))), ((int)(((byte)(101)))), ((int)(((byte)(125)))));
            this.btnClose.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btnClose.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.btnClose.Location = new System.Drawing.Point(10, 324);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(220, 37);
            this.btnClose.TabIndex = 6;
            this.btnClose.Text = "Закрыть форму просмотра";
            this.btnClose.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.btnClose.UseVisualStyleBackColor = false;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            //// 
            //// txtCount
            //// 
            //this.txtCount.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
            //            | System.Windows.Forms.AnchorStyles.Right)));
            //this.txtCount.DataMember = null;
            //this.txtCount.Location = new System.Drawing.Point(118, 333);
            //this.txtCount.Maximum = 100m;
            //this.txtCount.Minimum = 0m;
            //this.txtCount.Name = "txtCount";
            //this.txtCount.Size = new System.Drawing.Size(91, 20);
            //this.txtCount.TabIndex = 3;
            //this.txtCount.Value = 3;
            //// 
            //// blnForAll
            //// 
            //this.blnForAll.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
            //            | System.Windows.Forms.AnchorStyles.Right)));
            //this.blnForAll.DataMember = null;
            //this.blnForAll.Location = new System.Drawing.Point(118, 353);
            //this.blnForAll.Name = "blnForAll";
            //this.blnForAll.Text = "для всех";
            //this.blnForAll.Size = new System.Drawing.Size(91, 20);
            //this.blnForAll.TabIndex = 4;
            //// 
            //// lnkClose
            //// 
            //this.lnkClose.ActiveLinkColor = System.Drawing.Color.Maroon;
            //this.lnkClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            //this.lnkClose.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            //this.lnkClose.LinkColor = System.Drawing.Color.DarkBlue;
            //this.lnkClose.Location = new System.Drawing.Point(146, 1);
            //this.lnkClose.Name = "lnkClose";
            //this.lnkClose.Padding = new System.Windows.Forms.Padding(3);
            //this.lnkClose.Size = new System.Drawing.Size(63, 21);
            //this.lnkClose.TabIndex = 0;
            //this.lnkClose.TabStop = true;
            //this.lnkClose.Text = "Закрыть";
            //this.lnkClose.VisitedLinkColor = System.Drawing.Color.Navy;
            //this.lnkClose.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkClose_LinkClicked);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(613, 368);
            this.MinimumSize = new System.Drawing.Size(600, 340);
            this.Controls.Add(this.pnlLeft);
            this.Controls.Add(this.ctlPreview);
            this.ctlPreview.BringToFront();
            this.StartPosition = FormStartPosition.CenterParent;
            this.Icon = global::BOF.Properties.Resources.printer;
            this.ShowInTaskbar = false;
            //this.CancelButton = lnkClose;
            this.CancelButton = btnClose;
            this.Name = "Preview";
            this.Text = "Предварительный просмотр";
            this.pnlLeft.ResumeLayout(false);
            this.pnlLeft.PerformLayout();
            //((System.ComponentModel.ISupportInitialize)(this.txtCount)).EndInit();
            this.ResumeLayout(false);
        }
    }
}
