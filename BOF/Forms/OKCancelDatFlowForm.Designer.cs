namespace BOF
{
    partial class OKCancelDatFlowForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.pnlFlow = new System.Windows.Forms.FlowLayoutPanel();
            this.pnlFlowTop = new System.Windows.Forms.FlowLayoutPanel();
            this.pnlMain.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlMain
            // 
            this.pnlMain.Controls.Add(this.pnlFlow);
            this.pnlMain.Controls.Add(this.pnlFlowTop);
            this.pnlMain.Size = new System.Drawing.Size(344, 137);
            // 
            // pnlFlow
            // 
            this.pnlFlow.BackColor = System.Drawing.SystemColors.Control;
            this.pnlFlow.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlFlow.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.pnlFlow.Location = new System.Drawing.Point(0, 18);
            this.pnlFlow.Name = "pnlFlow";
            this.pnlFlow.Size = new System.Drawing.Size(344, 119);
            this.pnlFlow.TabIndex = 31;
            // 
            // pnlFlowTop
            // 
            this.pnlFlowTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlFlowTop.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.pnlFlowTop.Location = new System.Drawing.Point(0, 0);
            this.pnlFlowTop.Name = "pnlFlowTop";
            this.pnlFlowTop.Padding = new System.Windows.Forms.Padding(4, 4, 4, 0);
            this.pnlFlowTop.Size = new System.Drawing.Size(344, 18);
            this.pnlFlowTop.TabIndex = 32;
            // 
            // OKCancelDatFlowForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.ClientSize = new System.Drawing.Size(344, 175);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Sizable;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "OKCancelDatFlowForm";
            this.Load += new System.EventHandler(this.Form_Load);
            this.pnlMain.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        public System.Windows.Forms.FlowLayoutPanel pnlFlow;
        public System.Windows.Forms.FlowLayoutPanel pnlFlowTop;
    }
}
