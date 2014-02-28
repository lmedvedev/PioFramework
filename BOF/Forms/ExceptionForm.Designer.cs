namespace BOF
{
    partial class ExceptionForm
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
            this.btnDetails = new System.Windows.Forms.Button();
            this.btnOK = new System.Windows.Forms.Button();
            this.lblText = new System.Windows.Forms.Label();
            this.imgBox = new System.Windows.Forms.PictureBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.tabControlError = new System.Windows.Forms.TabControl();
            this.tabPageText = new System.Windows.Forms.TabPage();
            this.tabPageHtml = new System.Windows.Forms.TabPage();
            this.errorPlain = new BOF.CtlString();
            this.errorHTML = new BOF.CtlWebBrowser();
            ((System.ComponentModel.ISupportInitialize)(this.imgBox)).BeginInit();
            this.panel1.SuspendLayout();
            this.tabControlError.SuspendLayout();
            this.tabPageText.SuspendLayout();
            this.tabPageHtml.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnDetails
            // 
            this.btnDetails.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnDetails.ImageAlign = System.Drawing.ContentAlignment.BottomRight;
            this.btnDetails.Location = new System.Drawing.Point(625, 53);
            this.btnDetails.Name = "btnDetails";
            this.btnDetails.Size = new System.Drawing.Size(116, 22);
            this.btnDetails.TabIndex = 1;
            this.btnDetails.TabStop = false;
            this.btnDetails.Text = "Показать детали";
            this.btnDetails.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnDetails.UseVisualStyleBackColor = true;
            this.btnDetails.Click += new System.EventHandler(this.btnDetails_Click);
            // 
            // btnOK
            // 
            this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOK.Location = new System.Drawing.Point(539, 53);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(80, 22);
            this.btnOK.TabIndex = 1;
            this.btnOK.Text = "Продолжить";
            this.btnOK.UseVisualStyleBackColor = true;
            // 
            // lblText
            // 
            this.lblText.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblText.BackColor = System.Drawing.SystemColors.Control;
            this.lblText.Location = new System.Drawing.Point(57, 3);
            this.lblText.Name = "lblText";
            this.lblText.Size = new System.Drawing.Size(685, 48);
            this.lblText.TabIndex = 2;
            this.lblText.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // imgBox
            // 
            this.imgBox.Image = global::BOF.Properties.Resources.inf;
            this.imgBox.Location = new System.Drawing.Point(3, 3);
            this.imgBox.Name = "imgBox";
            this.imgBox.Size = new System.Drawing.Size(48, 48);
            this.imgBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.imgBox.TabIndex = 3;
            this.imgBox.TabStop = false;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.imgBox);
            this.panel1.Controls.Add(this.btnOK);
            this.panel1.Controls.Add(this.lblText);
            this.panel1.Controls.Add(this.btnDetails);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(745, 81);
            this.panel1.TabIndex = 4;
            // 
            // tabControlError
            // 
            this.tabControlError.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControlError.Controls.Add(this.tabPageText);
            this.tabControlError.Controls.Add(this.tabPageHtml);
            this.tabControlError.Location = new System.Drawing.Point(0, 87);
            this.tabControlError.Name = "tabControlError";
            this.tabControlError.SelectedIndex = 0;
            this.tabControlError.Size = new System.Drawing.Size(745, 283);
            this.tabControlError.TabIndex = 5;
            // 
            // tabPageText
            // 
            this.tabPageText.Controls.Add(this.errorPlain);
            this.tabPageText.Location = new System.Drawing.Point(4, 22);
            this.tabPageText.Name = "tabPageText";
            this.tabPageText.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageText.Size = new System.Drawing.Size(737, 257);
            this.tabPageText.TabIndex = 0;
            this.tabPageText.Text = "Ошибка";
            this.tabPageText.UseVisualStyleBackColor = true;
            // 
            // tabPageHtml
            // 
            this.tabPageHtml.Controls.Add(this.errorHTML);
            this.tabPageHtml.Location = new System.Drawing.Point(4, 22);
            this.tabPageHtml.Name = "tabPageHtml";
            this.tabPageHtml.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageHtml.Size = new System.Drawing.Size(737, 257);
            this.tabPageHtml.TabIndex = 1;
            this.tabPageHtml.Text = "Ошибка (HTML)";
            this.tabPageHtml.UseVisualStyleBackColor = true;
            // 
            // errorPlain
            // 
            this.errorPlain.DataMember = null;
            this.errorPlain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.errorPlain.Location = new System.Drawing.Point(3, 3);
            this.errorPlain.Multiline = true;
            this.errorPlain.Name = "errorPlain";
            this.errorPlain.Size = new System.Drawing.Size(731, 251);
            this.errorPlain.TabIndex = 0;
            // 
            // errorHTML
            // 
            this.errorHTML.Dock = System.Windows.Forms.DockStyle.Fill;
            this.errorHTML.Location = new System.Drawing.Point(3, 3);
            this.errorHTML.MinimumSize = new System.Drawing.Size(20, 20);
            this.errorHTML.Name = "errorHTML";
            this.errorHTML.Size = new System.Drawing.Size(731, 251);
            this.errorHTML.TabIndex = 0;
            this.errorHTML.Title = "";
            // 
            // ExceptionForm
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(745, 370);
            this.Controls.Add(this.tabControlError);
            this.Controls.Add(this.panel1);
            this.Name = "ExceptionForm";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Сообщение об ошибке";
            this.TopMost = true;
            this.Load += new System.EventHandler(this.ExceptionForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.imgBox)).EndInit();
            this.panel1.ResumeLayout(false);
            this.tabControlError.ResumeLayout(false);
            this.tabPageText.ResumeLayout(false);
            this.tabPageText.PerformLayout();
            this.tabPageHtml.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnDetails;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Label lblText;
        private System.Windows.Forms.PictureBox imgBox;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.TabControl tabControlError;
        private System.Windows.Forms.TabPage tabPageText;
        private CtlString errorPlain;
        private System.Windows.Forms.TabPage tabPageHtml;
        private CtlWebBrowser errorHTML;
    }
}