namespace BOF
{
    partial class PassPhraseForm
    {

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.buttonOK = new System.Windows.Forms.Button();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.textOldPhrase = new System.Windows.Forms.TextBox();
            this.textNewPhrase = new System.Windows.Forms.TextBox();
            this.labelOldPhrase = new System.Windows.Forms.Label();
            this.labelNewPhrase = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // buttonOK
            // 
            this.buttonOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.buttonOK.Location = new System.Drawing.Point(84, 203);
            this.buttonOK.Name = "buttonOK";
            this.buttonOK.Size = new System.Drawing.Size(75, 23);
            this.buttonOK.TabIndex = 2;
            this.buttonOK.Text = "OK";
            this.buttonOK.UseVisualStyleBackColor = true;
            this.buttonOK.Click += new System.EventHandler(this.buttonOK_Click);
            // 
            // buttonCancel
            // 
            this.buttonCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonCancel.Location = new System.Drawing.Point(204, 203);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(75, 23);
            this.buttonCancel.TabIndex = 3;
            this.buttonCancel.Text = "Cancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            // 
            // textOldPhrase
            // 
            this.textOldPhrase.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.textOldPhrase.Location = new System.Drawing.Point(12, 21);
            this.textOldPhrase.Multiline = true;
            this.textOldPhrase.Name = "textOldPhrase";
            this.textOldPhrase.Size = new System.Drawing.Size(340, 70);
            this.textOldPhrase.TabIndex = 0;
            // 
            // textNewPhrase
            // 
            this.textNewPhrase.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.textNewPhrase.Location = new System.Drawing.Point(12, 120);
            this.textNewPhrase.Multiline = true;
            this.textNewPhrase.Name = "textNewPhrase";
            this.textNewPhrase.Size = new System.Drawing.Size(340, 70);
            this.textNewPhrase.TabIndex = 1;
            // 
            // labelOldPhrase
            // 
            this.labelOldPhrase.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.labelOldPhrase.Location = new System.Drawing.Point(12, 2);
            this.labelOldPhrase.Name = "labelOldPhrase";
            this.labelOldPhrase.Size = new System.Drawing.Size(340, 16);
            this.labelOldPhrase.TabIndex = 4;
            this.labelOldPhrase.Text = "Превет!";
            // 
            // labelNewPhrase
            // 
            this.labelNewPhrase.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.labelNewPhrase.Location = new System.Drawing.Point(12, 101);
            this.labelNewPhrase.Name = "labelNewPhrase";
            this.labelNewPhrase.Size = new System.Drawing.Size(340, 16);
            this.labelNewPhrase.TabIndex = 5;
            this.labelNewPhrase.Text = "Укажите новую кодовую фразу";
            // 
            // PassPhraseForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.buttonCancel;
            this.ClientSize = new System.Drawing.Size(362, 231);
            this.ControlBox = false;
            this.Controls.Add(this.labelNewPhrase);
            this.Controls.Add(this.labelOldPhrase);
            this.Controls.Add(this.textNewPhrase);
            this.Controls.Add(this.textOldPhrase);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.buttonOK);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximumSize = new System.Drawing.Size(370, 258);
            this.MinimumSize = new System.Drawing.Size(370, 258);
            this.Name = "PassPhraseForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Установка или смена кодовой фразы";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.PassPhraseForm_FormClosing);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button buttonOK;
        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.TextBox textOldPhrase;
        private System.Windows.Forms.TextBox textNewPhrase;
        private System.Windows.Forms.Label labelOldPhrase;
        private System.Windows.Forms.Label labelNewPhrase;
    }
}