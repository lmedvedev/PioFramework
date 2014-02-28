using System;
using System.Windows.Forms;
using System.Drawing;
using System.Collections.Generic;
using System.Windows.Forms.Design;
using System.Text;
using BO;

namespace BOF
{
    public class DropDownService : IWindowsFormsEditorService
    {
        private ChooserDropDown DropDownCtl;
        private CtlChooser editor;
        private DropDownForm dropDownForm;
        private bool closingDropDown;

        public DropDownService(CtlChooser ctl)
        {
            editor = ctl;
        }

        #region IWindowsFormsEditorService Members

        public virtual void CloseDropDown()
        {
            if (closingDropDown || DropDownCtl.DontClose)
                return;
            try
            {
                closingDropDown = true;
                if (dropDownForm != null && dropDownForm.Visible)
                {
                    dropDownForm.Component = null;
                    dropDownForm.Visible = false;

                    if (editor.Visible)
                        editor.Focus();
                }
            }
            finally
            {
                closingDropDown = false;
            }
        }

        public void DropDownControl(Control ctl)
        {
            DropDownCtl = (ChooserDropDown)ctl;
            if (editor.CanCall) DropDownCtl.ValueOpenForm += new ValueEventHandler(DropDownService_ValueOpenForm);
            if (dropDownForm == null)
                dropDownForm = new DropDownForm(this);
            dropDownForm.TopMost = true;
            dropDownForm.Visible = false;
            dropDownForm.Component = DropDownCtl;

            Size size = new Size(editor.DropDownWidth, editor.DropDownHeight);

            Point location  = new Point(editor.Left, editor.Bottom);
            if (editor.DropDownAlign == CtlChooser.DropDownAlignment.Right) location.X = editor.Right - size.Width - 1;

            // location in screen coordinate
            location = editor.Parent.PointToScreen(location);

            // check the form is in the screen working area
            Rectangle screenWorkingArea = Screen.FromControl(editor).WorkingArea;

            location.X = Math.Min(screenWorkingArea.Right - size.Width,
                                  Math.Max(screenWorkingArea.X, location.X));

            if (size.Height + location.Y + editor.Height > screenWorkingArea.Bottom)
                location.Y = location.Y - size.Height - editor.Height - 1;

            dropDownForm.FormBorderStyle = FormBorderStyle.FixedSingle;
            dropDownForm.SetBounds(location.X, location.Y, size.Width, size.Height);
            dropDownForm.MinimumSize = size;
            dropDownForm.Size = size;
            dropDownForm.Visible = true;
            DropDownCtl.Focus();
            DropDownCtl.ValueSelected -= new ValueEventHandler(ctl_ValueSelected);
            DropDownCtl.ValueSelected += new ValueEventHandler(ctl_ValueSelected);
            DropDownCtl.ValueEscaped -= new EventHandler(DropDownService_ValueEscaped);
            DropDownCtl.ValueEscaped += new EventHandler(DropDownService_ValueEscaped);

            //editor.SelectTextBox();
            // wait for the end of the editing

            //while (dropDownForm.Visible)
            //{
            //    Application.DoEvents();
            //    MsgWaitForMultipleObjects(0, 0, true, 250, 255);
            //}

            // editing is done or aborted

        }

        void DropDownService_ValueOpenForm(object sender, ValueEventArgs e)
        {
            editor.FireValueOpenForm(e);
        }

        void DropDownService_ValueEscaped(object sender, EventArgs e)
        {
            CloseDropDown();
        }

        void ctl_ValueSelected(object sender, ValueEventArgs e)
        {
            editor.Value = (BaseDat)e.Value;
            CloseDropDown();
        }

        public DialogResult ShowDialog(Form dialog)
        {
            return DialogResult.Cancel;
        }

        #endregion

        //[System.Runtime.InteropServices.DllImport("user32.dll")]
        //public static extern int MsgWaitForMultipleObjects(
        //    int nCount,		// number of handles in array
        //    int pHandles,	// object-handle array
        //    bool bWaitAll,	// wait option
        //    int dwMilliseconds,	// time-out interval
        //    int dwWakeMask	// input-event type
        //    );
    }

    class DropDownForm : Form
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
        /// <param name="service">The service that drops this form.</param>
        public DropDownForm(IWindowsFormsEditorService service)
        {
            StartPosition = FormStartPosition.Manual;
            currentControl = null;
            ShowInTaskbar = false;
            KeyPreview = true;
            ControlBox = false;
            MinimizeBox = false;
            MaximizeBox = false;
            Text = "";
            FormBorderStyle = FormBorderStyle.SizableToolWindow;
            Visible = false;
            this.service = service;
        }

        internal void SystemColorChanged()
        {
            OnSystemColorsChanged(EventArgs.Empty);
        }

        /// <summary>
        /// This member overrides <see cref="Control.OnMouseDown">Control.OnMouseDown</see>.
        /// </summary>
        /// <remarks>
        /// Closes the form when the left button is clicked.
        /// </remarks>
        protected override void OnMouseDown(MouseEventArgs me)
        {
            if (me.Button == MouseButtons.Left)
                service.CloseDropDown();
            base.OnMouseDown(me);
        }

        /// <summary>
        /// This member overrides <see cref="Form.OnClosed">Form.OnClosed</see>.
        /// </summary>
        protected override void OnClosed(EventArgs args)
        {
            if (Visible)
                service.CloseDropDown();
            base.OnClosed(args);
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

        /// <summary>
        /// Gets or sets the control displayed by the form.
        /// </summary>
        /// <value>A <see cref="Control"/> instance.</value>
        public Control Component
        {
            get
            {
                return currentControl;
            }
            set
            {
                if (currentControl != null)
                {
                    Controls.Remove(currentControl);
                    currentControl = null;
                }
                if (value != null)
                {
                    currentControl = value;
                    Controls.Add(currentControl);
                    Size = new Size(currentControl.Width, currentControl.Height);
                    currentControl.Location = new Point(0, 0);
                    currentControl.Visible = true;
                    //currentControl.Resize += new EventHandler(OnCurrentControlResize);
                    currentControl.Dock = DockStyle.Fill;
                }
                Enabled = currentControl != null;
            }
        }

        /// <summary>
        /// Invoked when the dropped control is resized.
        /// This resizes the form and realigns it.
        /// </summary>
        private void OnCurrentControlResize(object o, EventArgs e)
        {
            int width;
            if (currentControl != null)
            {
                width = Width + 2;
                Size = new Size(2 + currentControl.Width, 2 + currentControl.Height);
                Left -= Width - width;
            }
        }

        /// <summary>
        /// Invoked when the form is resized.
        /// </summary>
        protected override void SetBoundsCore(int x, int y, int width, int height, BoundsSpecified specified)
        {
            //if (currentControl != null)
            //{
            //    currentControl.SetBounds(0, 0, width - 2, height - 2);
            //    width = currentControl.Width;
            //    height = currentControl.Height;
            //    //if (height == 0 && currentControl is ListBox)
            //    //{
            //    //    height = ((ListBox)currentControl).ItemHeight;
            //    //    currentControl.Height = height;
            //    //}
            //    width = width +2;
            //    height = height +2;
            //}
            base.SetBoundsCore(x, y, width, height, specified);
        }
    }
}
