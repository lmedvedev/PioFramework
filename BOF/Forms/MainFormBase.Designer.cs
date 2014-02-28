namespace BOF
{
    partial class MainFormBase
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainFormBase));
            this.statusStripMF = new System.Windows.Forms.StatusStrip();
            this.menuStripMF = new System.Windows.Forms.MenuStrip();
            this.SuspendLayout();
            // 
            // statusStripMF
            // 
            this.statusStripMF.Location = new System.Drawing.Point(0, 320);
            this.statusStripMF.Name = "statusStripMF";
            this.statusStripMF.Size = new System.Drawing.Size(729, 22);
            this.statusStripMF.TabIndex = 1;
            // 
            // menuStripMF
            // 
            this.menuStripMF.Location = new System.Drawing.Point(0, 0);
            this.menuStripMF.Name = "menuStripMF";
            this.menuStripMF.Size = new System.Drawing.Size(729, 24);
            this.menuStripMF.TabIndex = 2;
            this.menuStripMF.Text = "menuStrip1";
            // 
            // MainFormBase
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ClientSize = new System.Drawing.Size(729, 342);
            this.Controls.Add(this.statusStripMF);
            this.Controls.Add(this.menuStripMF);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.IsMdiContainer = true;
            this.MainMenuStrip = this.menuStripMF;
            this.Name = "MainFormBase";
            this.Text = "MainFormBase";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        public System.Windows.Forms.StatusStrip statusStripMF;
        public System.Windows.Forms.MenuStrip menuStripMF;

    }
}