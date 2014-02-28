namespace BOF
{
    partial class CardTreeForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CardTreeForm));
            this.chkRoot = new BOF.CtlBool();
            this.pnlBase = new System.Windows.Forms.Panel();
            this.txtID = new BOF.CtlIntString();
            this.txtParent_FP = new BOF.CtlString();
            this.txtName = new BOF.CtlString();
            this.lblDelimeter = new System.Windows.Forms.Label();
            this.lblParent = new System.Windows.Forms.Label();
            this.txtCode = new BOF.CtlInt();
            this.lblID = new System.Windows.Forms.Label();
            this.lblName = new System.Windows.Forms.Label();
            this.pnlMain.SuspendLayout();
            this.pnlBase.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtCode)).BeginInit();
            this.SuspendLayout();
            // 
            // pnlMain
            // 
            this.pnlMain.Controls.Add(this.pnlBase);
            this.pnlMain.Size = new System.Drawing.Size(434, 71);
            // 
            // chkRoot
            // 
            this.chkRoot.DataMember = null;
            this.chkRoot.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.chkRoot.Location = new System.Drawing.Point(226, 1);
            this.chkRoot.Name = "chkRoot";
            this.chkRoot.ReadOnly = false;
            this.chkRoot.Size = new System.Drawing.Size(106, 21);
            this.chkRoot.TabIndex = 0;
            this.chkRoot.TabStop = false;
            this.chkRoot.Text = "корневой узел";
            this.chkRoot.UseVisualStyleBackColor = true;
            this.chkRoot.Value = false;
            this.chkRoot.CheckedChanged += new System.EventHandler(this.chkRoot_CheckedChanged);
            // 
            // pnlBase
            // 
            this.pnlBase.Controls.Add(this.txtID);
            this.pnlBase.Controls.Add(this.txtParent_FP);
            this.pnlBase.Controls.Add(this.chkRoot);
            this.pnlBase.Controls.Add(this.txtName);
            this.pnlBase.Controls.Add(this.lblDelimeter);
            this.pnlBase.Controls.Add(this.lblParent);
            this.pnlBase.Controls.Add(this.txtCode);
            this.pnlBase.Controls.Add(this.lblID);
            this.pnlBase.Controls.Add(this.lblName);
            this.pnlBase.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlBase.Location = new System.Drawing.Point(0, 0);
            this.pnlBase.Name = "pnlBase";
            this.pnlBase.Size = new System.Drawing.Size(434, 72);
            this.pnlBase.TabIndex = 30;
            // 
            // txtID
            // 
            this.txtID.DataMember = "";
            this.txtID.Location = new System.Drawing.Point(122, 22);
            this.txtID.Name = "txtID";
            this.txtID.ReadOnly = true;
            this.txtID.Size = new System.Drawing.Size(65, 20);
            this.txtID.TabIndex = 8;
            this.txtID.TabStop = false;
            // 
            // txtParent_FP
            // 
            this.txtParent_FP.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtParent_FP.DataMember = "";
            this.txtParent_FP.ForeColor = System.Drawing.SystemColors.WindowText;
            this.txtParent_FP.Location = new System.Drawing.Point(226, 23);
            this.txtParent_FP.Name = "txtParent_FP";
            this.txtParent_FP.ReadOnly = true;
            this.txtParent_FP.Size = new System.Drawing.Size(114, 20);
            this.txtParent_FP.TabIndex = 3;
            this.txtParent_FP.TabStop = false;
            this.txtParent_FP.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // txtName
            // 
            this.txtName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtName.DataMember = "";
            this.txtName.ForeColor = System.Drawing.SystemColors.WindowText;
            this.txtName.Location = new System.Drawing.Point(122, 50);
            this.txtName.Name = "txtName";
            this.txtName.Size = new System.Drawing.Size(306, 20);
            this.txtName.TabIndex = 7;
            // 
            // lblDelimeter
            // 
            this.lblDelimeter.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblDelimeter.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lblDelimeter.Location = new System.Drawing.Point(341, 24);
            this.lblDelimeter.Name = "lblDelimeter";
            this.lblDelimeter.Size = new System.Drawing.Size(16, 20);
            this.lblDelimeter.TabIndex = 4;
            this.lblDelimeter.Text = ".";
            this.lblDelimeter.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblParent
            // 
            this.lblParent.Location = new System.Drawing.Point(186, 20);
            this.lblParent.Name = "lblParent";
            this.lblParent.Size = new System.Drawing.Size(37, 20);
            this.lblParent.TabIndex = 2;
            this.lblParent.Text = "Путь:";
            this.lblParent.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtCode
            // 
            this.txtCode.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.txtCode.DataMember = "";
            this.txtCode.ForeColor = System.Drawing.SystemColors.WindowText;
            this.txtCode.Location = new System.Drawing.Point(358, 23);
            this.txtCode.Maximum = new decimal(new int[] {
            2147483647,
            0,
            0,
            0});
            this.txtCode.Minimum = new decimal(new int[] {
            -2147483648,
            0,
            0,
            -2147483648});
            this.txtCode.Name = "txtCode";
            this.txtCode.Size = new System.Drawing.Size(70, 20);
            this.txtCode.TabIndex = 5;
            this.txtCode.Value = 0;
            // 
            // lblID
            // 
            this.lblID.Location = new System.Drawing.Point(52, 20);
            this.lblID.Name = "lblID";
            this.lblID.Size = new System.Drawing.Size(67, 20);
            this.lblID.TabIndex = 0;
            this.lblID.Text = "ID:";
            this.lblID.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblName
            // 
            this.lblName.Location = new System.Drawing.Point(53, 49);
            this.lblName.Name = "lblName";
            this.lblName.Size = new System.Drawing.Size(66, 18);
            this.lblName.TabIndex = 6;
            this.lblName.Text = "Название:";
            this.lblName.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // CardTreeForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.ClientSize = new System.Drawing.Size(434, 109);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "CardTreeForm";
            this.Text = "Узел";
            this.Load += new System.EventHandler(this.CardTreeForm_Load);
            this.pnlMain.ResumeLayout(false);
            this.pnlBase.ResumeLayout(false);
            this.pnlBase.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtCode)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        protected CtlBool chkRoot;
        private System.Windows.Forms.Panel pnlBase;
        private CtlString txtParent_FP;
        private CtlString txtName;
        private System.Windows.Forms.Label lblDelimeter;
        private System.Windows.Forms.Label lblParent;
        private CtlInt txtCode;
        private System.Windows.Forms.Label lblID;
        private System.Windows.Forms.Label lblName;
        private CtlIntString txtID;




    }
}
