using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using BO;
namespace BOF
{
    public class frmProgress : System.Windows.Forms.Form
    {
        public event EventHandler ProgressCanceled; 
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Label lblCaption;
        public System.Windows.Forms.ProgressBar progressBar;
        #region Windows Form Designer generated code
        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.progressBar = new System.Windows.Forms.ProgressBar();
            this.btnCancel = new System.Windows.Forms.Button();
            this.lblCaption = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // progressBar
            // 
            this.progressBar.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                | System.Windows.Forms.AnchorStyles.Right)));
            this.progressBar.Location = new System.Drawing.Point(3, 62);
            this.progressBar.Name = "progressBar";
            this.progressBar.Size = new System.Drawing.Size(397, 16);
            this.progressBar.TabIndex = 7;
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(163, 84);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(80, 20);
            this.btnCancel.TabIndex = 8;
            this.btnCancel.Text = "Отмена";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // lblCaption
            // 
            this.lblCaption.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                | System.Windows.Forms.AnchorStyles.Left)
                | System.Windows.Forms.AnchorStyles.Right)));
            this.lblCaption.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(204)));
            this.lblCaption.Location = new System.Drawing.Point(1, 1);
            this.lblCaption.Name = "lblCaption";
            this.lblCaption.Size = new System.Drawing.Size(400, 55);
            this.lblCaption.TabIndex = 9;
            this.lblCaption.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // frmProgress
            // 
            this.AutoScaleMode = AutoScaleMode.None;
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.CausesValidation = false;
            this.ClientSize = new System.Drawing.Size(402, 106);
            this.Controls.Add(this.lblCaption);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.progressBar);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "frmProgress";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.TopMost = true;
            this.Closed += new System.EventHandler(this.frmProgress_Closed);
            this.ResumeLayout(false);

        }
        #endregion

        #region Constructors
        public frmProgress()
        {
            InitializeComponent();
            datProgressBar.Current = new datProgressBar();
            datProgressBar.Current.StartPos = 0;
            progressBar.Maximum = 100;
            datProgressBar.Current.EndPos = progressBar.Maximum;
        }

        public frmProgress(string text)
            : this()
        {
            this.Text = text;
        }

        public frmProgress(string text, bool allowcancel)
            : this(text)
        {
            AllowCancel = allowcancel;
        }

        new void Close()
        {
            DialogResult = DialogResult.OK;
            base.Close();
        }
        #endregion

        #region Members
        public bool AllowCancel
        {
            get { return btnCancel.Visible; }
            set { btnCancel.Visible = value; }
        }

        public int Position
        {
            get { return progressBar.Value; }
            set
            {
                if (value > progressBar.Maximum) value = progressBar.Maximum;
                progressBar.Value = value;
            }
        }

        public string Caption
        {
            get { return lblCaption.Text; }
            set { lblCaption.Text = value; }
        }
        #endregion

        private void btnCancel_Click(object sender, System.EventArgs e)
        {
            if (ProgressCanceled == null)
                throw new Exception("Операция отменена пользователем.");
            ProgressCanceled(this, EventArgs.Empty);
        }

        private void frmProgress_Closed(object sender, System.EventArgs e)
        {
            datProgressBar.Current = null;
            //if (DialogResult != DialogResult.OK)
            //    btnCancel_Click(this, System.EventArgs.Empty);
        }
    }
}
