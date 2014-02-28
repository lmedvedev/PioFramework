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
    public static class FormHelper
    {
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
                frm.Height = totalHeight + 120;
            else
            {
                frm.Height = maxHeight + 80;
                frm.Width = (frm.Width - pnlFlow.Padding.Horizontal) * columns;
            }
        }

        public static void AddCtlBool(CtlBool box, FlowLayoutPanel pnlFlow, string labelText, string dMember)
        {
            box.Text = labelText;
            //box.BackColor = Color.Red;
            box.Width = pnlFlow.Width - 10;
            box.DataMember = dMember;
            pnlFlow.Controls.Add(box);
        }

        public static void AddCtlInt(CtlInt box, FlowLayoutPanel pnlFlow, string labelText, string dMember)
        {
            Label lbl = new Label();
            lbl.AutoSize = true;
            lbl.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            lbl.Text = labelText;
            box.Width = 100;
            box.TextAlign = HorizontalAlignment.Right;
            box.DataMember = dMember;
            AddLabeledControl(lbl, box, pnlFlow);
        }

        public static void AddCtlDecimal(CtlDecimal box, FlowLayoutPanel pnlFlow, string labelText, string dMember)
        {
            Label lbl = new Label();
            lbl.AutoSize = true;
            lbl.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            lbl.Text = labelText;
            box.Width = 100;
            box.TextAlign = HorizontalAlignment.Right;
            box.DataMember = dMember;
            AddLabeledControl(lbl, box, pnlFlow);
        }

        public static void AddLabeledControl(Label lbl, Control box, FlowLayoutPanel pnlFlow)
        {
            FlowLayoutPanel flp = new FlowLayoutPanel();
            flp.Padding = pnlFlow.Padding; //new Padding(0);
            //flp.BackColor = Color.Red;
            //lbl.BackColor = Color.Yellow;
            //box.BackColor = Color.Green;
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

        public static void AddCtlString(CtlString box, FlowLayoutPanel pnlFlow, string labelText, string dMember)
        {
            Label lbl = new Label();
            lbl.AutoSize = true;
            lbl.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            lbl.Text = labelText;
            box.Width = pnlFlow.Width - 12;
            box.DataMember = dMember;
            AddLabeledControl(lbl, box, pnlFlow);
        }

        public static void AddCtlChooser(CtlChooser box, FlowLayoutPanel pnlFlow, string labelText, string dMember)
        {
            Label lbl = new Label();
            lbl.AutoSize = true;
            lbl.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            lbl.Text = labelText;
            box.Width = pnlFlow.Width - 12;
            box.BorderStyle = BorderStyle.Fixed3D;
            box.DataMember = dMember;
            AddLabeledControl(lbl, box, pnlFlow);
        }

        public static void AddCtlDateTime(CtlDateTime box, FlowLayoutPanel pnlFlow, string labelText, string dMember)
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
            box.DataMember = dMember;
            //pnlFlow.Controls.Add(box);
            AddLabeledControl(lbl, box, pnlFlow);

        }
    }
}
