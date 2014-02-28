namespace BOF
{
    partial class DictForm
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
            this.txtName = new BOF.CtlString();
            this.txtSCode = new BOF.CtlString();
            this.lblSCode = new System.Windows.Forms.Label();
            this.lblName = new System.Windows.Forms.Label();
            this.pnlMain.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlMain
            // 
            this.pnlMain.Controls.Add(this.txtSCode);
            this.pnlMain.Controls.Add(this.txtName);
            this.pnlMain.Controls.Add(this.lblSCode);
            this.pnlMain.Controls.Add(this.lblName);
            this.pnlMain.Size = new System.Drawing.Size(405, 173);
            this.pnlMain.TabIndex = 0;
            // 
            // txtName
            // 
            this.txtName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txtName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtName.DataMember = null;
            this.txtName.ForeColor = System.Drawing.SystemColors.WindowText;
            this.txtName.Location = new System.Drawing.Point(92, 38);
            this.txtName.Name = "txtName";
            this.txtName.Size = new System.Drawing.Size(309, 20);
            this.txtName.TabIndex = 3;
            // 
            // txtSCode
            // 
            this.txtSCode.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtSCode.DataMember = null;
            this.txtSCode.ForeColor = System.Drawing.SystemColors.WindowText;
            this.txtSCode.Location = new System.Drawing.Point(92, 12);
            this.txtSCode.Name = "txtSCode";
            this.txtSCode.Size = new System.Drawing.Size(95, 20);
            this.txtSCode.TabIndex = 1;
            // 
            // lblSCode
            // 
            this.lblSCode.Location = new System.Drawing.Point(25, 10);
            this.lblSCode.Name = "lblSCode";
            this.lblSCode.Size = new System.Drawing.Size(67, 20);
            this.lblSCode.TabIndex = 0;
            this.lblSCode.Text = "Код:";
            this.lblSCode.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblName
            // 
            this.lblName.Location = new System.Drawing.Point(25, 38);
            this.lblName.Name = "lblName";
            this.lblName.Size = new System.Drawing.Size(67, 18);
            this.lblName.TabIndex = 2;
            this.lblName.Text = "Название:";
            this.lblName.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // DictForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.ClientSize = new System.Drawing.Size(405, 204);
            this.Name = "DictForm";
            this.Load += new System.EventHandler(this.DictForm_Load);
            this.pnlMain.ResumeLayout(false);
            this.pnlMain.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        protected CtlString txtName;
        protected CtlString txtSCode;
        private System.Windows.Forms.Label lblSCode;
        private System.Windows.Forms.Label lblName;





    }
}
