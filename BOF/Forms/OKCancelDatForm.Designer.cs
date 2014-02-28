namespace BOF
{
    partial class OKCancelDatForm
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
            this.components = new System.ComponentModel.Container();
            this.pnlMain = new System.Windows.Forms.Panel();
            this.DatErrorProvider = new System.Windows.Forms.ErrorProvider(this.components);
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.DatErrorProvider)).BeginInit();
            this.SuspendLayout();
            // 
            // pnlMain
            // 
            this.pnlMain.BackColor = System.Drawing.SystemColors.ControlLight;
            this.pnlMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlMain.Location = new System.Drawing.Point(0, 0);
            this.pnlMain.Name = "pnlMain";
            this.pnlMain.Size = new System.Drawing.Size(497, 237);
            this.pnlMain.TabIndex = 4;
            // 
            // DatErrorProvider
            // 
            this.DatErrorProvider.ContainerControl = this;
            // 
            // OKCancelDatForm
            // 
            this.AcceptButton = null;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ControlLight;
            this.CancelButton = null;
            this.ClientSize = new System.Drawing.Size(497, 275);
            this.Controls.Add(this.pnlMain);
            this.Name = "OKCancelDatForm";
            this.ShowInTaskbar = false;
            this.Load += new System.EventHandler(this.OKCancelDatForm_Load);
            this.Activated += new System.EventHandler(this.OKCancelDatForm_Activated);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.OKCancelDatForm_FormClosed);
            this.Controls.SetChildIndex(this.pnlMain, 0);
            ((System.ComponentModel.ISupportInitialize)(this.DatErrorProvider)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        public System.Windows.Forms.Panel pnlMain;
        private System.Windows.Forms.ErrorProvider DatErrorProvider;
        private System.Windows.Forms.ToolTip toolTip;
    }
}