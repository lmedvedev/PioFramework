using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using BO;

namespace BOF
{
    public class ContextMenuController
    {
        public ContextMenuController()
        {
 
        }
        public ContextMenuController(Control ctl):this()
        {
            Init(ctl);
        }

        protected Control parentControl = null;
        protected ContextMenu contextMenu = null; 

        public void Init(Control ctl)
        {
            parentControl = ctl;

            contextMenu = new ContextMenu();
            parentControl.ContextMenu = contextMenu;

            AddDefaultItems();
        }
        public void AddMenuItem(string text, EventHandler eh)
        {
            MenuItem mi = new MenuItem(text, eh);
            contextMenu.MenuItems.Add(mi);
        }
        public void RemoveMenuItem(string text)
        {
            contextMenu.MenuItems.RemoveByKey(text);
        }
        
        public virtual void AddDefaultItems()
        {
        }
    }
}
