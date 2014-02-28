using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using BO;
using System.Drawing;

namespace BOF
{
    public class DropDownGridStatus : Control, IDropDownControl
    {
        DataGridView grid;
        Label lblText;

        public DropDownGridStatus()
        {
            this.DoubleBuffered = true;

            grid = new DataGridView();
            grid.AllowUserToAddRows = false;
            grid.AllowUserToDeleteRows = false;
            grid.AllowUserToResizeRows = false;
            grid.AutoGenerateColumns = true;
            grid.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            grid.MultiSelect = false;
            grid.RowHeadersVisible = false;
            grid.BorderStyle = BorderStyle.None;
            grid.CellDoubleClick += new DataGridViewCellEventHandler(grid_CellDoubleClick);
            grid.Dock = DockStyle.Fill;

            Panel sbar = new Panel();
            sbar.Height = 14;
            sbar.Dock = DockStyle.Bottom;

            PictureBox pSelect = new PictureBox();
            pSelect.SizeMode = PictureBoxSizeMode.CenterImage;
            pSelect.Image = global::BOF.Properties.Resources.Select.ToBitmap();
            pSelect.Size = new Size(sbar.Height, sbar.Height);
            pSelect.Dock = DockStyle.Right;
            pSelect.Cursor = Cursors.Hand;
            pSelect.Click += new EventHandler(pSelect_Click);

            PictureBox pClose = new PictureBox();
            pClose.SizeMode = PictureBoxSizeMode.CenterImage;
            pClose.Image = global::BOF.Properties.Resources.XIcon;
            pClose.Size = new Size(sbar.Height, sbar.Height);
            pClose.Dock = DockStyle.Right;
            pClose.Cursor = Cursors.Hand;
            pClose.Click += new EventHandler(pClose_Click);

            lblText = new Label();
            lblText.Dock = DockStyle.Fill;
            lblText.AutoEllipsis = true;

            sbar.Controls.AddRange(new Control[] { lblText, pSelect, pClose });

            this.Controls.AddRange(new Control[] { grid, sbar });
        }

        void pClose_Click(object sender, EventArgs e)
        {
            if (DropDownClosed != null)
                DropDownClosed(this, EventArgs.Empty);
        }

        void pSelect_Click(object sender, EventArgs e)
        {
            if (RowSelected != null)
                RowSelected(grid, new EventArgs<int>(grid.CurrentRow.Index));
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            switch (keyData)
            {
                case Keys.Enter:
                    if (RowEntered != null)
                    {
                        RowEntered(grid, new EventArgs<int>(grid.CurrentRow.Index));
                        return true;
                    }
                    break;
                case Keys.Enter | Keys.Control:
                    if (RowSelected != null)
                    {
                        RowSelected(grid, new EventArgs<int>(grid.CurrentRow.Index));
                        return true;
                    }
                    break;
                case Keys.Escape:
                    if (DropDownClosed != null)
                    {
                        DropDownClosed(this, EventArgs.Empty);
                        return true;
                    }
                    break;
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }

        void grid_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (RowEntered != null)
                RowEntered(grid, new EventArgs<int>(e.RowIndex));
        }

        #region IDropDownControl Members

        public DataGridView Grid
        {
            get { return grid; }
        }

        public event EventHandler<BO.EventArgs<int>> RowEntered;
        public event EventHandler<BO.EventArgs<int>> RowSelected;
        public event EventHandler DropDownClosed;
        public void WriteText(string text)
        {
            lblText.Text = text;
        }
        #endregion
    }
}
