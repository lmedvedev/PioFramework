namespace BOF
{
    partial class CtlFilterDateFromTo
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.cmbPeriods = new System.Windows.Forms.ComboBox();
            this.btnCopyDate = new System.Windows.Forms.Button();
            this.ctlDateFrom = new BOF.CtlDateTime();
            this.ctlDateTo = new BOF.CtlDateTime();
            this.SuspendLayout();
            // 
            // cmbPeriods
            // 
            this.cmbPeriods.Dock = System.Windows.Forms.DockStyle.Top;
            this.cmbPeriods.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbPeriods.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.cmbPeriods.FormattingEnabled = true;
            this.cmbPeriods.Location = new System.Drawing.Point(0, 0);
            this.cmbPeriods.Name = "cmbPeriods";
            this.cmbPeriods.Size = new System.Drawing.Size(225, 21);
            this.cmbPeriods.TabIndex = 0;
            // 
            // btnCopyDate
            // 
            this.btnCopyDate.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.btnCopyDate.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(81)))), ((int)(((byte)(101)))), ((int)(((byte)(125)))));
            this.btnCopyDate.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCopyDate.Font = new System.Drawing.Font("Arial", 5.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btnCopyDate.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.btnCopyDate.Location = new System.Drawing.Point(102, 24);
            this.btnCopyDate.Name = "btnCopyDate";
            this.btnCopyDate.Size = new System.Drawing.Size(21, 20);
            this.btnCopyDate.TabIndex = 2;
            this.btnCopyDate.Text = "»";
            this.btnCopyDate.UseVisualStyleBackColor = false;
            // 
            // ctlDateFrom
            // 
            this.ctlDateFrom.Location = new System.Drawing.Point(0, 24);
            this.ctlDateFrom.Name = "ctlDateFrom";
            this.ctlDateFrom.ShowCheckBox = true;
            this.ctlDateFrom.Size = new System.Drawing.Size(94, 20);
            this.ctlDateFrom.TabIndex = 1;
            this.ctlDateFrom.Value = new System.DateTime(2006, 7, 31, 0, 0, 0, 0);
            // 
            // ctlDateTo
            // 
            this.ctlDateTo.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.ctlDateTo.Location = new System.Drawing.Point(132, 24);
            this.ctlDateTo.Name = "ctlDateTo";
            this.ctlDateTo.ShowCheckBox = true;
            this.ctlDateTo.Size = new System.Drawing.Size(93, 20);
            this.ctlDateTo.TabIndex = 3;
            this.ctlDateTo.Value = new System.DateTime(2006, 7, 31, 0, 0, 0, 0);
            // 
            // CtlFilterDateFromTo
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.ctlDateFrom);
            this.Controls.Add(this.ctlDateTo);
            this.Controls.Add(this.cmbPeriods);
            this.Controls.Add(this.btnCopyDate);
            this.Name = "CtlFilterDateFromTo";
            this.Size = new System.Drawing.Size(225, 49);
            this.ResumeLayout(false);

        }

        #endregion

        private CtlDateTime ctlDateFrom;
        private CtlDateTime ctlDateTo;

        private System.Windows.Forms.ComboBox cmbPeriods;
        private System.Windows.Forms.Button btnCopyDate;
    }
}
