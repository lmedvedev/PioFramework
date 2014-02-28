using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using BO;

namespace BOF
{
    public class ContextMenuRtfController : ContextMenuController
    {
        public ContextMenuRtfController()
        {
            Rtb.SelectionChanged += new EventHandler(rtb_SelectionChanged);
        }

        void rtb_SelectionChanged(object sender, EventArgs e)
        {
            Guid guid = new Guid();
            string text = Rtb.SelectedText;
            if (IsGUID(text))
            {
                guid = new Guid(text);
            }
        }
        private RichTextBox Rtb
        {
            get { return parentControl as RichTextBox; }
        }
        public ContextMenuRtfController(Control ctl):base(ctl)
        {
            Rtb.SelectionChanged += new EventHandler(rtb_SelectionChanged);
        }

        public override void AddDefaultItems()
        {
            MenuItem miCopy = new MenuItem("&Copy", Copy);
            
            contextMenu.MenuItems.Add(miCopy);
        }

        private void Copy(object sender, EventArgs e)
        {
            Rtb.Copy();
        }


        private bool IsGUID(string text)
        {
            try
            {
                Guid guid = new Guid(text);
            }
            catch
            {
                return false;
            }
            return true;
        }
    }
}
