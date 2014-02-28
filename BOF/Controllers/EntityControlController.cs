using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using BO;

namespace BOF
{
    public class EntityControlController
    {
        public EntityControlController() { }
        public EntityControlController(Control ctl,CardControlBase entCtl) 
        {
            _ParentControl = ctl;
            _EntityControl = entCtl;

            if (_ParentControl!=null &&_EntityControl != null)
            {
                _ParentControl.SuspendLayout();
                _ParentControl.Controls.Add(_EntityControl);
                _EntityControl.Dock = DockStyle.Fill;
                _ParentControl.ResumeLayout();
            }
        }
        public void Init(BaseDat dat)
        {
            if (_EntityControl != null)
            {
                _EntityControl.Value = dat;
            }
        }

        private Control _ParentControl;

        public Control ParentControl
        {
            get { return _ParentControl; }
            set { _ParentControl = value; }
        }
        private CardControlBase _EntityControl;

        public CardControlBase EntityControl
        {
            get { return _EntityControl; }
            set { _EntityControl = value; }
        }
    }
}
