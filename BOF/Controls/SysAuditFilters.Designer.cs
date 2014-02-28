namespace BOF
{
    partial class SysAuditFilters
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
            this.lblFilterTbl = new System.Windows.Forms.Label();
            this.lblFilterDates = new System.Windows.Forms.Label();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnApply = new System.Windows.Forms.Button();
            this.btnDefault = new System.Windows.Forms.Button();
            this.lblFilterUser = new System.Windows.Forms.Label();
            this.lblFilterID = new System.Windows.Forms.Label();
            this.lblFilterHost = new System.Windows.Forms.Label();
            this.lblFilterApplication = new System.Windows.Forms.Label();
            this.lblFilterServer = new System.Windows.Forms.Label();
            this.lblFilterBase = new System.Windows.Forms.Label();
            this.cmbTable = new System.Windows.Forms.ComboBox();
            this.ctlFilterBase = new BOF.CtlString();
            this.ctlFilterServer = new BOF.CtlString();
            this.ctlFilterApplication = new BOF.CtlString();
            this.ctlFilterHost = new BOF.CtlString();
            this.ctlFilterID = new BOF.CtlInt();
            this.ctlFilterUser = new BOF.CtlString();
            this.ctlFilterDate = new BOF.CtlFilterDateFromTo();
            this.btnReset = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.ctlFilterID)).BeginInit();
            this.SuspendLayout();
            // 
            // lblFilterTbl
            // 
            this.lblFilterTbl.AutoSize = true;
            this.lblFilterTbl.Location = new System.Drawing.Point(42, 85);
            this.lblFilterTbl.Name = "lblFilterTbl";
            this.lblFilterTbl.Size = new System.Drawing.Size(50, 13);
            this.lblFilterTbl.TabIndex = 26;
            this.lblFilterTbl.Text = "Таблица";
            // 
            // lblFilterDates
            // 
            this.lblFilterDates.AutoSize = true;
            this.lblFilterDates.Location = new System.Drawing.Point(125, 6);
            this.lblFilterDates.Name = "lblFilterDates";
            this.lblFilterDates.Size = new System.Drawing.Size(96, 13);
            this.lblFilterDates.TabIndex = 24;
            this.lblFilterDates.Text = "Фильтр по датам";
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(226, 255);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 22;
            this.btnCancel.Text = "Закрыть";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // btnApply
            // 
            this.btnApply.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnApply.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnApply.Location = new System.Drawing.Point(149, 255);
            this.btnApply.Name = "btnApply";
            this.btnApply.Size = new System.Drawing.Size(72, 23);
            this.btnApply.TabIndex = 21;
            this.btnApply.Text = "Применить";
            this.btnApply.UseVisualStyleBackColor = true;
            // 
            // btnDefault
            // 
            this.btnDefault.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnDefault.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnDefault.Location = new System.Drawing.Point(70, 255);
            this.btnDefault.Name = "btnDefault";
            this.btnDefault.Size = new System.Drawing.Size(72, 23);
            this.btnDefault.TabIndex = 27;
            this.btnDefault.Text = "Сброс";
            this.btnDefault.UseVisualStyleBackColor = true;
            // 
            // lblFilterUser
            // 
            this.lblFilterUser.AutoSize = true;
            this.lblFilterUser.Location = new System.Drawing.Point(12, 136);
            this.lblFilterUser.Name = "lblFilterUser";
            this.lblFilterUser.Size = new System.Drawing.Size(80, 13);
            this.lblFilterUser.TabIndex = 29;
            this.lblFilterUser.Text = "Пользователь";
            // 
            // lblFilterID
            // 
            this.lblFilterID.AutoSize = true;
            this.lblFilterID.Location = new System.Drawing.Point(74, 111);
            this.lblFilterID.Name = "lblFilterID";
            this.lblFilterID.Size = new System.Drawing.Size(18, 13);
            this.lblFilterID.TabIndex = 31;
            this.lblFilterID.Text = "ID";
            // 
            // lblFilterHost
            // 
            this.lblFilterHost.AutoSize = true;
            this.lblFilterHost.Location = new System.Drawing.Point(61, 159);
            this.lblFilterHost.Name = "lblFilterHost";
            this.lblFilterHost.Size = new System.Drawing.Size(31, 13);
            this.lblFilterHost.TabIndex = 33;
            this.lblFilterHost.Text = "Хост";
            // 
            // lblFilterApplication
            // 
            this.lblFilterApplication.AutoSize = true;
            this.lblFilterApplication.Location = new System.Drawing.Point(21, 182);
            this.lblFilterApplication.Name = "lblFilterApplication";
            this.lblFilterApplication.Size = new System.Drawing.Size(71, 13);
            this.lblFilterApplication.TabIndex = 35;
            this.lblFilterApplication.Text = "Приложение";
            // 
            // lblFilterServer
            // 
            this.lblFilterServer.AutoSize = true;
            this.lblFilterServer.Location = new System.Drawing.Point(48, 205);
            this.lblFilterServer.Name = "lblFilterServer";
            this.lblFilterServer.Size = new System.Drawing.Size(44, 13);
            this.lblFilterServer.TabIndex = 37;
            this.lblFilterServer.Text = "Сервер";
            // 
            // lblFilterBase
            // 
            this.lblFilterBase.AutoSize = true;
            this.lblFilterBase.Location = new System.Drawing.Point(60, 229);
            this.lblFilterBase.Name = "lblFilterBase";
            this.lblFilterBase.Size = new System.Drawing.Size(32, 13);
            this.lblFilterBase.TabIndex = 39;
            this.lblFilterBase.Text = "База";
            // 
            // cmbTable
            // 
            this.cmbTable.FormattingEnabled = true;
            this.cmbTable.Location = new System.Drawing.Point(100, 85);
            this.cmbTable.Name = "cmbTable";
            this.cmbTable.Size = new System.Drawing.Size(156, 21);
            this.cmbTable.TabIndex = 40;
            // 
            // ctlFilterBase
            // 
            this.ctlFilterBase.DataMember = null;
            this.ctlFilterBase.Location = new System.Drawing.Point(98, 226);
            this.ctlFilterBase.Name = "ctlFilterBase";
            this.ctlFilterBase.Size = new System.Drawing.Size(194, 20);
            this.ctlFilterBase.TabIndex = 38;
            // 
            // ctlFilterServer
            // 
            this.ctlFilterServer.DataMember = null;
            this.ctlFilterServer.Location = new System.Drawing.Point(98, 202);
            this.ctlFilterServer.Name = "ctlFilterServer";
            this.ctlFilterServer.Size = new System.Drawing.Size(194, 20);
            this.ctlFilterServer.TabIndex = 36;
            // 
            // ctlFilterApplication
            // 
            this.ctlFilterApplication.DataMember = null;
            this.ctlFilterApplication.Location = new System.Drawing.Point(98, 179);
            this.ctlFilterApplication.Name = "ctlFilterApplication";
            this.ctlFilterApplication.Size = new System.Drawing.Size(194, 20);
            this.ctlFilterApplication.TabIndex = 34;
            // 
            // ctlFilterHost
            // 
            this.ctlFilterHost.DataMember = null;
            this.ctlFilterHost.Location = new System.Drawing.Point(98, 156);
            this.ctlFilterHost.Name = "ctlFilterHost";
            this.ctlFilterHost.Size = new System.Drawing.Size(194, 20);
            this.ctlFilterHost.TabIndex = 32;
            // 
            // ctlFilterID
            // 
            this.ctlFilterID.DataMember = null;
            this.ctlFilterID.Location = new System.Drawing.Point(98, 109);
            this.ctlFilterID.Maximum = new decimal(new int[] {
            2147483647,
            0,
            0,
            0});
            this.ctlFilterID.Minimum = new decimal(new int[] {
            -2147483648,
            0,
            0,
            -2147483648});
            this.ctlFilterID.Name = "ctlFilterID";
            this.ctlFilterID.Size = new System.Drawing.Size(194, 20);
            this.ctlFilterID.TabIndex = 30;
            this.ctlFilterID.Value = 0;
            // 
            // ctlFilterUser
            // 
            this.ctlFilterUser.DataMember = null;
            this.ctlFilterUser.Location = new System.Drawing.Point(98, 133);
            this.ctlFilterUser.Name = "ctlFilterUser";
            this.ctlFilterUser.Size = new System.Drawing.Size(194, 20);
            this.ctlFilterUser.TabIndex = 28;
            // 
            // ctlFilterDate
            // 
            this.ctlFilterDate.BackColor = System.Drawing.Color.Transparent;
            this.ctlFilterDate.DateFrom = new System.DateTime(2007, 7, 16, 0, 0, 0, 0);
            this.ctlFilterDate.DateTo = new System.DateTime(2007, 8, 16, 0, 0, 0, 0);
            this.ctlFilterDate.DateType = BOF.CtlFilterDateFromTo.DateTypes.MTD;
            this.ctlFilterDate.Location = new System.Drawing.Point(64, 22);
            this.ctlFilterDate.Name = "ctlFilterDate";
            this.ctlFilterDate.Size = new System.Drawing.Size(228, 60);
            this.ctlFilterDate.TabIndex = 23;
            // 
            // btnReset
            // 
            this.btnReset.Location = new System.Drawing.Point(262, 83);
            this.btnReset.Name = "btnReset";
            this.btnReset.Size = new System.Drawing.Size(30, 23);
            this.btnReset.TabIndex = 41;
            this.btnReset.Text = "X";
            this.btnReset.UseVisualStyleBackColor = true;
            this.btnReset.Click += new System.EventHandler(this.btnReset_Click);
            // 
            // SysAuditFilters
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(304, 280);
            this.Controls.Add(this.btnReset);
            this.Controls.Add(this.cmbTable);
            this.Controls.Add(this.lblFilterBase);
            this.Controls.Add(this.ctlFilterBase);
            this.Controls.Add(this.lblFilterServer);
            this.Controls.Add(this.ctlFilterServer);
            this.Controls.Add(this.lblFilterApplication);
            this.Controls.Add(this.ctlFilterApplication);
            this.Controls.Add(this.lblFilterHost);
            this.Controls.Add(this.ctlFilterHost);
            this.Controls.Add(this.lblFilterID);
            this.Controls.Add(this.ctlFilterID);
            this.Controls.Add(this.lblFilterUser);
            this.Controls.Add(this.ctlFilterUser);
            this.Controls.Add(this.btnDefault);
            this.Controls.Add(this.lblFilterTbl);
            this.Controls.Add(this.lblFilterDates);
            this.Controls.Add(this.ctlFilterDate);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnApply);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SysAuditFilters";
            this.Text = "Фильтр";
            ((System.ComponentModel.ISupportInitialize)(this.ctlFilterID)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblFilterTbl;
        private System.Windows.Forms.Label lblFilterDates;
        private BOF.CtlFilterDateFromTo ctlFilterDate;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnApply;
        private System.Windows.Forms.Button btnDefault;
        private System.Windows.Forms.Label lblFilterUser;
        private BOF.CtlString ctlFilterUser;
        private BOF.CtlInt ctlFilterID;
        private System.Windows.Forms.Label lblFilterID;
        private System.Windows.Forms.Label lblFilterHost;
        private BOF.CtlString ctlFilterHost;
        private System.Windows.Forms.Label lblFilterApplication;
        private BOF.CtlString ctlFilterApplication;
        private System.Windows.Forms.Label lblFilterServer;
        private BOF.CtlString ctlFilterServer;
        private System.Windows.Forms.Label lblFilterBase;
        private BOF.CtlString ctlFilterBase;
        private System.Windows.Forms.ComboBox cmbTable;
        private System.Windows.Forms.Button btnReset;
    }
}