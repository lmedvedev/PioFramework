using System;
using System.Collections.Generic;
//using System.Linq;
using System.Text;
using System.Windows.Forms;
using DA;
using BO;
using System.Drawing;

namespace BOF
{
    public static class FilterHelper
    {
        public static bool? GetCheckBoxValue(CheckBox box)
        {
            switch (box.CheckState)
            {
                case CheckState.Checked:
                    return true;
                case CheckState.Unchecked:
                    return false;
            }

            //CheckState.Indeterminate:
            return null;

        }

        public static List<int> GetComboValue(ComboBox box)
        {
            List<int> ret = new List<int>();
            if (box.SelectedItem != null)
            {
                KeyValuePair<int, string> kvp = (KeyValuePair<int, string>)box.SelectedItem;

                if (kvp.Key > 0)
                    ret.Add(kvp.Key);
            }
            return ret;
        }

        public static void AddComboBox(ref ComboBox box, FlowLayoutPanel pnlFlow, string labelText)
        {
            if (box == null)
                box = new ComboBox();

            Label lbl = new Label();
            lbl.AutoSize = true;
            lbl.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            lbl.Text = labelText;
            box.DropDownStyle = ComboBoxStyle.DropDownList;
            box.DisplayMember = "Value";
            box.ValueMember = "Key";
            box.Width = pnlFlow.Width - 12;
            AddLabeledControl(lbl, box, pnlFlow);
        }

        public static void AddTextBox(ref TextBox box, FlowLayoutPanel pnlFlow, string labelText)
        {
            if (box == null)
                box = new TextBox();
            Label lbl = new Label();
            lbl.AutoSize = true;
            lbl.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            lbl.Text = labelText;
            box.Width = pnlFlow.Width - 12;
            AddLabeledControl(lbl, box, pnlFlow);
        }

        public static void AddCheckBox(ref CheckBox box, FlowLayoutPanel pnlFlow, string labelText)
        {
            if (box == null)
                box = new CheckBox();
            box.Text = labelText;
            box.Width = pnlFlow.Width - 10;
            box.ThreeState = true;
            box.CheckState = CheckState.Indeterminate;
            pnlFlow.Controls.Add(box);
        }

        public static void AddCtlDateTime(ref CtlDateTime box, FlowLayoutPanel pnlFlow, string labelText)
        {
            if (box == null)
                box = new CtlDateTime();
            Label lbl = new Label();
            lbl.AutoSize = true;
            lbl.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            lbl.Text = labelText;

            box.Width = pnlFlow.Width - 12;
            box.ShowCheckBox = true;
            AddLabeledControl(lbl, box, pnlFlow);

        }

        public static void SetFormHeight(FlowLayoutPanel pnlFlow, FlowLayoutPanel pnlFlowTop)
        {
            int pnlFlowControlsHeight = 0;
            int pnlFlowTopControlsHeight = 0;

            foreach (Control item in pnlFlowTop.Controls)
            {
                pnlFlowTopControlsHeight += item.Height + pnlFlowTop.Padding.Vertical;
            }

            pnlFlowTop.Height = pnlFlowTopControlsHeight;

            foreach (Control item in pnlFlow.Controls)
            {
                pnlFlowControlsHeight += item.Height + pnlFlow.Padding.Vertical;
            }
            int totalHeight = pnlFlowControlsHeight + pnlFlowTopControlsHeight;
            int maxHeight = 500;

            Form frm = pnlFlow.FindForm();

            int columns = totalHeight / maxHeight + 1;
            if (columns < 2)
                frm.Height = totalHeight + pnlFlowTop.Controls.Count + pnlFlow.Controls.Count + 100;
            else
            {
                frm.Height = maxHeight + 100;
                frm.Width = (frm.Width - pnlFlow.Padding.Horizontal) * columns;
            }
        }

        public static void AddCheckBox(ref CheckBox box, FlowLayoutPanel pnlFlow, bool? val, string labelText)
        {
            box = new CheckBox();
            box.Text = labelText;
            box.Width = pnlFlow.Width - 10;
            box.ThreeState = true;
            if (val.HasValue)
                box.Checked = val.Value;
            else
                box.CheckState = CheckState.Indeterminate;
            pnlFlow.Controls.Add(box);
        }

        public static void SetCheckBoxValue(CheckBox box, bool? val)
        {
            box.ThreeState = true;
            if (val.HasValue)
                box.Checked = val.Value;
            else
                box.CheckState = CheckState.Indeterminate;
        }

        public static void AddLabeledControl(Label lbl, Control box, FlowLayoutPanel pnlFlow)
        {
            FlowLayoutPanel flp = new FlowLayoutPanel();
            flp.Padding = new Padding(0);
            //flp.AutoScroll = true;
            //flp.Padding.Left = 1;
            //flp.WrapContents = false;
            //flp.Dock = DockStyle.Top;
            //flp.SetFlowBreak(lbl, true);
            //flp.BackColor = Color.Azure;
            flp.Controls.Add(lbl);
            flp.Controls.Add(box);
            flp.FlowDirection = FlowDirection.LeftToRight;
            flp.Width = box.Width + 4;
            flp.Height = lbl.Height + box.Height + 4;
            pnlFlow.Controls.Add(flp);
        }

        public static void AddTextBox(ref TextBox box, FlowLayoutPanel pnlFlow, string val, string labelText)
        {
            Label lbl = new Label();
            lbl.AutoSize = true;
            lbl.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            //lbl.Width = pnlFlow.Width - 10;
            lbl.Text = labelText;
            //pnlFlow.Controls.Add(lbl);

            box = new TextBox();
            box.Width = pnlFlow.Width - 12;
            box.Text = val;
            //pnlFlow.Controls.Add(box);
            AddLabeledControl(lbl, box, pnlFlow);
        }

        public static void AddCtlDateTime(ref CtlDateTime box, FlowLayoutPanel pnlFlow, DateTime val, string labelText)
        {
            Label lbl = new Label();
            lbl.AutoSize = true;
            lbl.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            //lbl.Width = pnlFlow.Width - 10;
            lbl.Text = labelText;
            //pnlFlow.Controls.Add(lbl);

            box = new CtlDateTime();
            box.Width = pnlFlow.Width - 12;
            box.ShowCheckBox = true;
            box.Value = val;
            //pnlFlow.Controls.Add(box);
            AddLabeledControl(lbl, box, pnlFlow);

        }

        public static void SetCtlDateTimeValue(CtlDateTime box, DateTime val)
        {
            box.ShowCheckBox = true;
            box.Value = val;
        }

        public static void SetTextBoxValue(TextBox box, string val)
        {
            box.Text = val;
        }

        public static void SetCtlDateTimeValue(CtlFilterDateFromTo box, DateTime valFrom, DateTime valTo)
        {
            box.DateFrom = valFrom;
            box.DateTo = valTo;
        }

        public static void AddCtlFilterDateFromTo(ref CtlFilterDateFromTo box, FlowLayoutPanel pnlFlow, string labelText)
        {
            Label lbl = new Label();
            lbl.AutoSize = true;
            lbl.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            //lbl.Width = pnlFlow.Width - 10;
            lbl.Text = labelText;
            //pnlFlow.Controls.Add(lbl);

            box = new CtlFilterDateFromTo();
            box.Width = pnlFlow.Width - 12;
            //pnlFlow.Controls.Add(box);
            AddLabeledControl(lbl, box, pnlFlow);
        }

    }

    public static class FilterHelper<CS, CD>
        where CS : BaseSet<CD, CS>, new()
        where CD : BaseDat<CD>, new()
    {


        public static void AddFilledCombo(ref ComboBox box, FlowLayoutPanel pnlFlow, string labelText)
        {
            Label lbl = new Label();
            lbl.AutoSize = true;
            lbl.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            //lbl.Width = pnlFlow.Width - 10;
            lbl.Text = labelText;
            //pnlFlow.Controls.Add(lbl);

            box = new ComboBox();
            box.DropDownStyle = ComboBoxStyle.DropDownList;
            box.DisplayMember = "Value";
            box.ValueMember = "Key";
            box.Width = pnlFlow.Width - 10;
            FillCombo(box, labelText);
            //pnlFlow.Controls.Add(box);
            FilterHelper.AddLabeledControl(lbl, box, pnlFlow);

        }

        public static void SetComboValue(ComboBox box, CS list, FilterID val, string labelText)
        {
            if (box.Items.Count == 0)
            {
                if (list == null)
                    list = new CS();

                if (list.Count == 0)
                {
                    list.Load();

                    box.Items.Add(new KeyValuePair<int, string>(0, string.Format("- {0} -", labelText)));
                    box.Sorted = true;
                    foreach (KeyValuePair<int, string> item in list.Convert2KeyValuePairs())
                        box.Items.Add(item);
                }
            }

            //box.SelectedItem = null;
            box.SelectedIndex = 0;
            foreach (KeyValuePair<int, string> item in box.Items)
            {
                if (val.IDList.Contains(item.Key))
                    box.SelectedItem = item;
            }

            //if (box.SelectedItem == null)
            //    box.SelectedIndex = 0;

        }


        public static void FillCombo(ref ComboBox box, CS l, string labelText)
        {
            l.Load();
            box.Items.Add(new KeyValuePair<int, string>(0, string.Format("- {0} -", labelText)));
            box.Sorted = true;
            foreach (KeyValuePair<int, string> item in l.Convert2KeyValuePairs())
                box.Items.Add(item);

            box.SelectedIndex = 0;

        }

        public static void FillCombo(ref ComboBox box, BaseLoadFilter f, string labelText)
        {
            CS l = new CS();
            l.LoadFilter = f.GetFilter();
            FillCombo(ref box, l, labelText);
        }

        public static void FillCombo(ComboBox box, string labelText)
        {
            box.Items.Clear();
            CS l = new CS();
            FillCombo(ref box, l, labelText);
        }
    }
}
