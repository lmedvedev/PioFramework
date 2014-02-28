using Microsoft.Reporting.WinForms;
using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Printing;
using System.Drawing.Imaging;
using BO;

namespace BOF
{
    public partial class ReportViewerForm : Form
    {
        List<Stream> _pages = new List<Stream>();
        int _curpage = 0;
        PrintDocument _printdoc = new PrintDocument();

        public bool PreviewOnly
        {
            get
            {
                return !ctlReportViewer.ShowPrintButton; 
            }
            set
            {
                ctlReportViewer.ShowPrintButton = !value;
            }
        }

        public ReportViewerForm()
        {
            InitializeComponent();

            ToolStripButton btnClose = new ToolStripButton("Закрыть", global::BOF.Properties.Resources.CloseForm.ToBitmap());
            btnClose.Click += new EventHandler(delegate { this.Close(); });
            ToolBar.Items.Add(btnClose);
        }

        public ReportViewerForm(Stream report, params ReportDataSource[] datasources)
            : this()
        {
            ctlReportViewer.ProcessingMode = ProcessingMode.Local;
            if (report != null)
                ctlReportViewer.LocalReport.LoadReportDefinition(report);
            if (datasources != null)
                foreach (ReportDataSource var in datasources)
                {
                    ctlReportViewer.LocalReport.DataSources.Add(var);
                }

            ctlReportViewer.RefreshReport();
        }

        public ReportViewerForm(Stream report, List<ReportDataSource> datasources, Dictionary<string, string> para)
            : this()
        {
            ctlReportViewer.ProcessingMode = ProcessingMode.Local;
            if (report != null)
                ctlReportViewer.LocalReport.LoadReportDefinition(report);
            if (datasources != null)
                foreach (ReportDataSource var in datasources)
                {
                    ctlReportViewer.LocalReport.DataSources.Add(var);
                }

            ReportParameter[] parameters = new ReportParameter[para.Count];
            int i = 0;
            foreach (string Key in para.Keys)
            {
                ReportParameter p = new ReportParameter(Key, para[Key], false);
                parameters[i] = p;
                i++;
            }
            ctlReportViewer.LocalReport.SetParameters(parameters);
            ctlReportViewer.RefreshReport();
        }

        public ReportViewerForm(string server, string path)
            : this()
        {
            ctlReportViewer.ProcessingMode = ProcessingMode.Remote;
            ctlReportViewer.ServerReport.ReportServerUrl = new Uri(server);
            ctlReportViewer.ServerReport.ReportPath = path;
        }

        public ReportViewerForm(string server, string path, Dictionary<string, string> para)
            : this()
        {
            ctlReportViewer.ProcessingMode = ProcessingMode.Remote;
            ctlReportViewer.ServerReport.ReportServerUrl = new Uri(server);
            ctlReportViewer.ServerReport.ReportPath = path;

            ReportParameter[] parameters = new ReportParameter[para.Count];
            int i = 0;
            foreach (string Key in para.Keys)
            {
                ReportParameter p = new ReportParameter(Key, para[Key], false);
                parameters[i] = p;
                i++;
            }
            ctlReportViewer.ServerReport.SetParameters(parameters);
        }

        private void ReportingServiceForm_Load(object sender, EventArgs e)
        {
            this.MdiParent = MainFormBaseController.MainController.Frm;
            ctlReportViewer.RefreshReport();
        }

        private Stream CreateStream(string name, string fileNameExtension, Encoding encoding, string mimeType, bool willSeek)
        {
            Stream stream = new MemoryStream();
            _pages.Add(stream);
            return stream;
        }
        
        private void PrintPage(object sender, PrintPageEventArgs ev)
        {
            ev.Graphics.DrawImage(new Metafile(_pages[_curpage]), ev.PageBounds);
            _curpage++;
            ev.HasMorePages = (_curpage < _pages.Count);
        }

        private void FillPages(bool landscape)
        {
            //PaperSize paper_size = ctlReportViewer.LocalReport.GetDefaultPageSettings().PaperSize;
            //string deviceInfo =
            //  "<DeviceInfo>" +
            //  "<OutputFormat>EMF</OutputFormat>" +
            //  ((landscape) ? "<PageWidth>297mm</PageWidth><PageHeight>210mm</PageHeight>" : "<PageWidth>210mm</PageWidth><PageHeight>297mm</PageHeight>") + 
            //  "<MarginTop>0mm</MarginTop>" +
            //  "<MarginLeft>0mm</MarginLeft>" +
            //  "<MarginRight>0mm</MarginRight>" +
            //  "<MarginBottom>10mm</MarginBottom>" +
            //  "</DeviceInfo>";

            
            ReportPageSettings set = ctlReportViewer.LocalReport.GetDefaultPageSettings();
            //MarginsConverter conv = new MarginsConverter();
            //string temp = conv.ConvertToInvariantString(set.Margins);
            string deviceInfo =
              "<DeviceInfo>" +
              "<OutputFormat>EMF</OutputFormat>" +
               ((landscape) ? "<PageWidth>297mm</PageWidth><PageHeight>210mm</PageHeight>" : "<PageWidth>210mm</PageWidth><PageHeight>297mm</PageHeight>") +
              "<MarginTop>" + Common.PropValueDecimal2String(set.Margins.Top / 3.937) + "mm</MarginTop>" +
              "<MarginLeft>" + Common.PropValueDecimal2String(set.Margins.Left / 3.937) + "mm</MarginLeft>" +
              "<MarginRight>" + Common.PropValueDecimal2String(set.Margins.Right / 3.937) + "mm</MarginRight>" +
              "<MarginBottom>" + Common.PropValueDecimal2String(set.Margins.Bottom / 3.937) + "mm</MarginBottom>" +
              "</DeviceInfo>";

            Warning[] warnings;
            _pages = new List<Stream>();
            if (ctlReportViewer.ProcessingMode == ProcessingMode.Local)
                ctlReportViewer.LocalReport.Render("Image", deviceInfo, CreateStream, out warnings);
            foreach (Stream stream in _pages)
                stream.Position = 0;
            _printdoc.PrintPage += new PrintPageEventHandler(PrintPage);
            //_printdoc.DefaultPageSettings.Landscape = (paper_size.Width > 850);
        }

        public void Print(bool landscape)
        {
            //if (_pages.Count == 0)
            //    FillPages(landscape);
            //_curpage = 0;
            //_printdoc.DefaultPageSettings.Landscape = landscape;
            //_printdoc.Print();
            Print(landscape, null);
        }

        public void Print(bool landscape, string documentName)
        {
            if (_pages.Count == 0)
                FillPages(landscape);
            _curpage = 0;
            _printdoc.DefaultPageSettings.Landscape = landscape;
            _printdoc.DocumentName = documentName;
            _printdoc.Print();
        }

        public byte[] Render(bool landscape, int DpiX, int DpiY, ColorDepth cd, out string mimeType, out string encoding, out string fileNameExtension, out string[] streams, out Warning[] warnings)
        {
              string deviceInfo =
              "<DeviceInfo>" +
              "<OutputFormat>TIFF</OutputFormat>" +
               ((landscape) ? "<PageWidth>297mm</PageWidth><PageHeight>210mm</PageHeight>" : "<PageWidth>210mm</PageWidth><PageHeight>297mm</PageHeight>") +
              "<ColorDepth>" + (int)cd + "</ColorDepth>" +
              "<DpiX>" + DpiX + "</DpiX>" +
              "<DpiY>" + DpiY + "</DpiY>" +
              "</DeviceInfo>";

              return ctlReportViewer.LocalReport.Render("Image", deviceInfo, out mimeType, out encoding, out fileNameExtension, out streams, out warnings);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing); 
            if (_pages != null)
            {
                foreach (Stream stream in _pages)
                    stream.Close();
                _pages = null;
                GC.Collect();
            }
        }
    }
}