namespace BOF
{
    partial class FilterFormFlowBase
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
            this.SuspendLayout();
            // 
            // pnlFlow
            // 
            this.pnlFlow.BackColor = System.Drawing.SystemColors.Control;
            this.pnlFlow.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlFlow.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.pnlFlow.Location = new System.Drawing.Point(0, 18);
            this.pnlFlow.Name = "pnlFlow";
            this.pnlFlow.Size = new System.Drawing.Size(223, 65);
            this.pnlFlow.TabIndex = 29;
            // 
            // pnlFlowTop
            // 
            this.pnlFlowTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlFlowTop.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.pnlFlowTop.Location = new System.Drawing.Point(0, 0);
            this.pnlFlowTop.Name = "pnlFlowTop";
            this.pnlFlowTop.Padding = new System.Windows.Forms.Padding(4, 4, 4, 0);
            this.pnlFlowTop.Size = new System.Drawing.Size(223, 18);
            this.pnlFlowTop.TabIndex = 30;
            // 
            // FilterFormFlowBase
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.ClientSize = new System.Drawing.Size(223, 112);
            this.Controls.Add(this.pnlFlow);
            this.Controls.Add(this.pnlFlowTop);
            this.Name = "FilterFormFlowBase";
            this.Controls.SetChildIndex(this.pnlFlowTop, 0);
            this.Controls.SetChildIndex(this.pnlFlow, 0);
            this.ResumeLayout(false);

        }

        #endregion

        public System.Windows.Forms.FlowLayoutPanel pnlFlow;
        public System.Windows.Forms.FlowLayoutPanel pnlFlowTop;

    }
}
