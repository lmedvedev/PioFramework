using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace BOF
{
    public partial class CtlFilterDateFromTo : UserControl
    {
        public enum DateTypes { DTD = 0, WTD = 1, MTD = 2, YTD = 3, Custom = 4 }

        public CtlFilterDateFromTo()
        {
            InitializeComponent();
            DateFilters_Init();
        }

        public DateTime DateFrom
        {
            get
            {
                if (this.ctlDateFrom.Checked)
                    return this.ctlDateFrom.Value;
                else
                    return DateTime.MinValue;
            }
            set
            {
                if (value == DateTime.MinValue)
                    this.ctlDateFrom.Checked = false;
                else
                {
                    this.ctlDateFrom.Checked = true;
                    this.ctlDateFrom.Value = value;
                }
            }
        }
        public DateTime DateTo
        {
            get
            {
                if (this.ctlDateTo.Checked)
                    return this.ctlDateTo.Value;
                else
                    return DateTime.MinValue;
            }
            set
            {
                if (value == DateTime.MinValue)
                    this.ctlDateTo.Checked = false;
                else
                {
                    this.ctlDateTo.Checked = true;
                    this.ctlDateTo.Value = value;
                }
            }
        }
        public DateTypes DateType
        {
            get { return (DateTypes)this.cmbPeriods.SelectedIndex; }
            set { this.cmbPeriods.SelectedIndex = (int)value; }
        }


        private void DateFilters_Init()
        {
            this.cmbPeriods.Items.Clear();

            this.cmbPeriods.Items.AddRange(
                new string[]{"Текущий день",
                            "Текущая неделя",
                            "Текущий месяц",
                            "Текущий год",
                            "Интервал дат"
                            }
            );
            btnCopyDate.Click +=new EventHandler(btnCopyDate_Click);
            this.cmbPeriods.SelectedIndexChanged += cmbPeriods_SelectedIndexChanged;
            this.ctlDateFrom.ValueChanged += ctlDate_ValueChanged;
            this.ctlDateTo.ValueChanged += ctlDate_ValueChanged;

            FilterReset();
        }
        public void FilterReset()
        {
            DateType = DateTypes.MTD;
            FireFilterChanged();
        }
        public void FireFilterChanged()
        {
            if (FilterChanged != null)
                FilterChanged(this, EventArgs.Empty);
        }
        private bool date_noEvents = false;
        public event EventHandler FilterChanged;
        public static DateTime CurrentDate = DateTime.Today;

        void ctlDate_ValueChanged(object sender, EventArgs e)
        {
            if (!date_noEvents)
                DateType = DateTypes.Custom;
        }
        void btnCopyDate_Click(object sender, EventArgs e)
        {
            DateTo = DateFrom;

        }
        void cmbPeriods_SelectedIndexChanged(object sender, EventArgs e)
        {
            date_noEvents = true;
            switch (DateType)
            {
                case DateTypes.DTD:
                    setDates(CurrentDate, CurrentDate);
                    break;
                case DateTypes.WTD:
                    setDates(CurrentDate.AddDays(-7), CurrentDate);
                    break;
                case DateTypes.MTD:
                    setDates(CurrentDate.AddMonths(-1), CurrentDate);
                    break;
                case DateTypes.YTD:
                    setDates(CurrentDate.AddYears(-1), CurrentDate);
                    break;
                case DateTypes.Custom:
                    break;
            }
            date_noEvents = false;
        }

        void setDates(DateTime from, DateTime to)
        {
            this.ctlDateFrom.Checked = (from == DateTime.MinValue);
            this.ctlDateTo.Checked = (to == DateTime.MinValue);

            this.ctlDateFrom.Value = from;
            this.ctlDateTo.Value = to;
        }
    }
}
