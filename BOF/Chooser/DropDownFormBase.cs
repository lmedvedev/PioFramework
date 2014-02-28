using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace BOF
{
    public class DropDownFormBase : Form
    {
        /// <summary>
        /// Currently dropped control.
        /// </summary>
        private Control currentControl;

        /// <summary>
        /// The service that dropped this form.
        /// </summary>
        private IWindowsFormsEditorService service;

        /// <summary>
        /// Creates a <strong>DropDownForm</strong>.
        /// </summary>
        /// <param name="service">The service that drops1 this form.</param>
        public DropDownFormBase(IWindowsFormsEditorService service, IDropDownControl ctl)
        {
            StartPosition = FormStartPosition.Manual;
            ShowInTaskbar = false;
            KeyPreview = true;
            ControlBox = false;
            MinimizeBox = false;
            MaximizeBox = false;
            Text = "";
            Visible = false;
            TopMost = true;
            FormBorderStyle = FormBorderStyle.Sizable;

            currentControl = (Control)ctl;
            Controls.Add(currentControl);
            currentControl.Dock = DockStyle.Fill;
            currentControl.Visible = true;
            //this.Padding = 
            this.service = service;
        }

        internal void SystemColorChanged()
        {
            OnSystemColorsChanged(EventArgs.Empty);
        }

        /// <summary>
        /// This member overrides <see cref="Form.OnDeactivate">Form.OnDeactivate</see>.
        /// </summary>
        protected override void OnDeactivate(EventArgs args)
        {
            if (Visible)
                service.CloseDropDown();
            base.OnDeactivate(args);
        }
    }
}