namespace BOF
{
    partial class AboutForm
    {
        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AboutForm));
            this.buttonOK = new System.Windows.Forms.Button();
            this.listViewAbout = new System.Windows.Forms.ListView();
            this.columnHeaderAssembly = new System.Windows.Forms.ColumnHeader();
            this.columnHeaderVersion = new System.Windows.Forms.ColumnHeader();
            this.labelAbout = new System.Windows.Forms.Label();
            this.fadeTimer = new System.Windows.Forms.Timer(this.components);
            this.SuspendLayout();
            // 
            // buttonOK
            // 
            this.buttonOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.buttonOK.Location = new System.Drawing.Point(307, 245);
            this.buttonOK.Name = "buttonOK";
            this.buttonOK.Size = new System.Drawing.Size(75, 23);
            this.buttonOK.TabIndex = 0;
            this.buttonOK.Text = "Ok";
            this.buttonOK.UseVisualStyleBackColor = true;
            // 
            // listViewAbout
            // 
            this.listViewAbout.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.listViewAbout.AutoArrange = false;
            this.listViewAbout.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeaderAssembly,
            this.columnHeaderVersion});
            this.listViewAbout.FullRowSelect = true;
            this.listViewAbout.GridLines = true;
            this.listViewAbout.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.listViewAbout.Location = new System.Drawing.Point(12, 72);
            this.listViewAbout.MultiSelect = false;
            this.listViewAbout.Name = "listViewAbout";
            this.listViewAbout.Size = new System.Drawing.Size(370, 167);
            this.listViewAbout.TabIndex = 1;
            this.listViewAbout.UseCompatibleStateImageBehavior = false;
            this.listViewAbout.View = System.Windows.Forms.View.Details;
            // 
            // columnHeaderAssembly
            // 
            this.columnHeaderAssembly.Text = "";
            this.columnHeaderAssembly.Width = 240;
            // 
            // columnHeaderVersion
            // 
            this.columnHeaderVersion.Text = "";
            this.columnHeaderVersion.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.columnHeaderVersion.Width = 100;
            // 
            // labelAbout
            // 
            this.labelAbout.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.labelAbout.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.labelAbout.Location = new System.Drawing.Point(12, 9);
            this.labelAbout.Name = "labelAbout";
            this.labelAbout.Size = new System.Drawing.Size(370, 60);
            this.labelAbout.TabIndex = 2;
            this.labelAbout.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // fadeTimer
            // 
            this.fadeTimer.Interval = 50;
            this.fadeTimer.Tick += new System.EventHandler(this.fadeTimer_Tick);
            // 
            // AboutForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.buttonOK;
            this.ClientSize = new System.Drawing.Size(394, 280);
            this.Controls.Add(this.labelAbout);
            this.Controls.Add(this.listViewAbout);
            this.Controls.Add(this.buttonOK);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(400, 200);
            this.Name = "AboutForm";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "О программе";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button buttonOK;
        private System.Windows.Forms.ListView listViewAbout;
        private System.Windows.Forms.Label labelAbout;
        private System.Windows.Forms.ColumnHeader columnHeaderAssembly;
        private System.Windows.Forms.ColumnHeader columnHeaderVersion;
        private System.Windows.Forms.Timer fadeTimer;
        private System.ComponentModel.IContainer components;
    }
}