namespace BOF
{
    partial class ReportViewerForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ReportViewerForm));
            this.ctlReportViewer = new Microsoft.Reporting.WinForms.ReportViewer();
            this.ToolBar = new System.Windows.Forms.ToolStrip();
            this.SuspendLayout();
            // 
            // ctlReportViewer
            // 
            this.ctlReportViewer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ctlReportViewer.LocalReport.EnableExternalImages = true;
            this.ctlReportViewer.LocalReport.EnableHyperlinks = true;
            this.ctlReportViewer.Location = new System.Drawing.Point(0, 25);
            this.ctlReportViewer.Name = "ctlReportViewer";
            this.ctlReportViewer.Size = new System.Drawing.Size(768, 463);
            this.ctlReportViewer.TabIndex = 0;
            this.ctlReportViewer.ZoomMode = Microsoft.Reporting.WinForms.ZoomMode.PageWidth;
            // 
            // ToolBar
            // 
            this.ToolBar.Location = new System.Drawing.Point(0, 0);
            this.ToolBar.Name = "ToolBar";
            this.ToolBar.Size = new System.Drawing.Size(768, 25);
            this.ToolBar.TabIndex = 2;
            this.ToolBar.Text = "toolStrip1";
            // 
            // ReportViewerForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(768, 488);
            this.Controls.Add(this.ctlReportViewer);
            this.Controls.Add(this.ToolBar);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "ReportViewerForm";
            this.ShowInTaskbar = false;
            this.Text = "ReportViewer";
            this.Load += new System.EventHandler(this.ReportingServiceForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        public Microsoft.Reporting.WinForms.ReportViewer ctlReportViewer;

        private System.Windows.Forms.ToolStrip ToolBar;
    }
}