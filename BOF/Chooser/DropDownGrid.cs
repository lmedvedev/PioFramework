using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using BO;

namespace BOF
{
    public class DropDownGrid : DataGridView, IDropDownControl
    {
        public DropDownGrid()
        {
            this.AllowUserToAddRows = false;
            this.AllowUserToDeleteRows = false;
            this.AllowUserToResizeRows = false;
            this.AutoGenerateColumns = true;
            this.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            this.MultiSelect = false;
            this.RowHeadersVisible = false;
            this.BorderStyle = BorderStyle.None;
        }
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            switch (keyData)
            {
                case Keys.Enter :
                    if (RowEntered != null)
                        RowEntered(this, new EventArgs<int>(this.CurrentRow.Index));
                    break;
                case Keys.Enter | Keys.Control:
                    if(RowSelected!=null)
                        RowSelected(this, new EventArgs<int>(this.CurrentRow.Index));
                    return true;
                case Keys.Escape:
                    if (DropDownClosed != null)
                        DropDownClosed(this, EventArgs.Empty);
                    return true;
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }
        protected override void OnCellDoubleClick(DataGridViewCellEventArgs e)
        {
            if (RowEntered != null)
                RowEntered(this, new EventArgs<int>(e.RowIndex));
            base.OnCellDoubleClick(e);
        }
        #region IDropDownControl Members

        public DataGridView Grid
        {
            get { return this; }
        }
        public event EventHandler<EventArgs<int>> RowEntered;
        public event EventHandler<EventArgs<int>> RowSelected;
        public event EventHandler DropDownClosed;

        public void WriteText(string text){}

        #endregion
    }
}
