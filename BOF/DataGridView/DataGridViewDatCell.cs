using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using BO;

namespace BOF
{
    public class DataGridViewDatCell : DataGridViewTextBoxCell
    {
        public DataGridViewDatCell()
            : base()
        {
            //DataGridViewDatColumn clmn = this.OwningColumn as DataGridViewDatColumn;
            //if (clmn != null)
            //    this.Style.Format = clmn.Format;
        }
        public DataGridViewDatCell(string format)
            : this()
        {
            this.Style.Format = format;
        }


        public override void InitializeEditingControl(int rowIndex, object initialFormattedValue, DataGridViewCellStyle cellStyle)
        {

            // Set the value of the editing control to the current cell value.
            base.InitializeEditingControl(rowIndex, initialFormattedValue, cellStyle);

            DataGridViewChooserControl ctl = DataGridView.EditingControl as DataGridViewChooserControl;

            DataGridViewDatColumn clmn = this.OwningColumn as DataGridViewDatColumn;
            ctl.Format = cellStyle.Format;
            ctl.SetClass = clmn.SetClass;
            ctl.Value = (BaseDat)this.Value;
            clmn.FireFormatDropDownEvent(ctl);
        }
        public override void PositionEditingControl(bool setLocation, bool setSize, System.Drawing.Rectangle cellBounds, System.Drawing.Rectangle cellClip, DataGridViewCellStyle cellStyle, bool singleVerticalBorderAdded, bool singleHorizontalBorderAdded, bool isFirstDisplayedColumn, bool isFirstDisplayedRow)
        {

            base.PositionEditingControl(setLocation, setSize, cellBounds, cellClip, cellStyle, singleVerticalBorderAdded, singleHorizontalBorderAdded, isFirstDisplayedColumn, isFirstDisplayedRow);
            DataGridViewChooserControl ctl = DataGridView.EditingControl as DataGridViewChooserControl;
            //if (ctl.Height < cellBounds.Height)
            //    ctl.Top += cellBounds.Height - ctl.Height;
            int h1 = cellBounds.Height;
            int h2 = 18;
            if (h1 > h2) ctl.Top = (int)((h1 - h2) / 2);
        }
        public override Type EditType
        {
            get
            {
                return typeof(DataGridViewChooserControl);
            }
        }
        public override Type ValueType
        {
            get
            {
                return typeof(BaseDat);
            }
        }
        public override object DefaultNewRowValue
        {
            get
            {
                return null;
            }
        }
        public override object ParseFormattedValue(object formattedValue, DataGridViewCellStyle cellStyle, System.ComponentModel.TypeConverter formattedValueTypeConverter, System.ComponentModel.TypeConverter valueTypeConverter)
        {
            return formattedValue;
            //return base.ParseFormattedValue(formattedValue, cellStyle, formattedValueTypeConverter, valueTypeConverter);
        }

    }

}
