using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Data;
using System.Windows.Forms;
using System.Drawing.Design;
using System.ComponentModel.Design;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.IO;

namespace CtlTabControl
{
    /// <summary>
    /// Summary description for FlatTabControl.
    /// </summary>
    [ToolboxBitmap(typeof(System.Windows.Forms.TabControl))] //,
    //Designer(typeof(Designers.FlatTabControlDesigner))]

    public class FlatTabControl : System.Windows.Forms.TabControl
    {

        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.Container components = null;
        private SubClass scUpDown = null;
        private bool bUpDown; // true when the button UpDown is required
        private ImageList leftRightImages = null;
        private const int nMargin = 4;
        private Color mBackColor = SystemColors.Control;
        Bitmap new_img;
        public FlatTabControl()
        {
            // This call is required by the Windows.Forms Form Designer.
            InitializeComponent();

            // double buffering
            this.SetStyle(ControlStyles.UserPaint, true);
            this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            this.SetStyle(ControlStyles.DoubleBuffer, true);
            this.SetStyle(ControlStyles.ResizeRedraw, true);
            this.SetStyle(ControlStyles.SupportsTransparentBackColor, true);

            bUpDown = false;

            this.ControlAdded += new ControlEventHandler(FlatTabControl_ControlAdded);
            this.ControlRemoved += new ControlEventHandler(FlatTabControl_ControlRemoved);
            this.SelectedIndexChanged += new EventHandler(FlatTabControl_SelectedIndexChanged);
            this.MouseMove += new MouseEventHandler(FlatTabControl_MouseMove);
            this.MouseClick += new MouseEventHandler(FlatTabControl_MouseClick);
            this.Selecting += new TabControlCancelEventHandler(FlatTabControl_Selecting);
            this.Resize += new EventHandler(FlatTabControl_Resize);
            leftRightImages = new ImageList();
            //leftRightImages.ImageSize = new Size(16, 16); // default

            System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(FlatTabControl));
            Bitmap updownImage = ((System.Drawing.Bitmap)(resources.GetObject("TabIcons.bmp")));
            new_img = ((System.Drawing.Bitmap)(resources.GetObject("NewDocumentHS")));

            if (updownImage != null)
            {
                updownImage.MakeTransparent(Color.White);
                leftRightImages.Images.AddStrip(updownImage);
            }
        }


        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (components != null)
                {
                    components.Dispose();
                }

                leftRightImages.Dispose();
            }
            base.Dispose(disposing);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            DrawControl(e.Graphics);
        }

        internal void DrawControl(Graphics g)
        {
            if (!Visible)
                return;

            Rectangle TabControlArea = this.ClientRectangle;
            Rectangle TabArea = this.DisplayRectangle;

            //----------------------------
            // fill client area
            Brush br = new SolidBrush(mBackColor); //(SystemColors.Control); UPDATED
            g.FillRectangle(br, TabControlArea);
            br.Dispose();
            //----------------------------

            //----------------------------
            // draw border
            int nDelta = SystemInformation.Border3DSize.Width;

            Pen border = new Pen(SystemColors.ControlDark);
            TabArea.Inflate(nDelta, nDelta);
            g.DrawRectangle(border, TabArea);
            border.Dispose();
            //----------------------------


            //----------------------------
            // clip region for drawing tabs
            Region rsaved = g.Clip;
            Rectangle rreg;

            int nWidth = TabArea.Width + nMargin;
            if (bUpDown)
            {
                // exclude updown control for painting
                if (Win32.IsWindowVisible(scUpDown.Handle))
                {
                    Rectangle rupdown = new Rectangle();
                    Win32.GetWindowRect(scUpDown.Handle, ref rupdown);
                    Rectangle rupdown2 = this.RectangleToClient(rupdown);

                    nWidth = rupdown2.X + 3;
                }
            }

            rreg = new Rectangle(TabArea.Left, TabControlArea.Top, nWidth - nMargin, TabControlArea.Height);

            g.SetClip(rreg);

            // draw tabs
            for (int i = 0; i < this.TabCount - (NewTabVisible ? 1 : 0); i++)
                DrawTab(g, this.TabPages[i], i);

            _NewTabRectangle = new Rectangle(0, 0, 0, 0);
            if (NewTabVisible)
            {
                Rectangle recBounds = this.GetTabRect(TabPages.Count - 1);
                //recBounds.Offset(recBounds.Width, 0);
                recBounds.Width = 24;


                Point[] pt = new Point[7];
                if (this.Alignment == TabAlignment.Top)
                {
                    pt[0] = new Point(recBounds.Left, recBounds.Bottom);
                    pt[1] = new Point(recBounds.Left, recBounds.Top + 3);
                    pt[2] = new Point(recBounds.Left + 3, recBounds.Top);
                    pt[3] = new Point(recBounds.Right - 3, recBounds.Top);
                    pt[4] = new Point(recBounds.Right, recBounds.Top + 3);
                    pt[5] = new Point(recBounds.Right, recBounds.Bottom);
                    pt[6] = new Point(recBounds.Left, recBounds.Bottom);
                }
                else
                {
                    pt[0] = new Point(recBounds.Left, recBounds.Top);
                    pt[1] = new Point(recBounds.Right, recBounds.Top);
                    pt[2] = new Point(recBounds.Right, recBounds.Bottom - 3);
                    pt[3] = new Point(recBounds.Right - 3, recBounds.Bottom);
                    pt[4] = new Point(recBounds.Left + 3, recBounds.Bottom);
                    pt[5] = new Point(recBounds.Left, recBounds.Bottom - 3);
                    pt[6] = new Point(recBounds.Left, recBounds.Top);
                }

                _NewTabRectangle = recBounds;
                //----------------------------
                // fill this tab with background color
                Brush b = new SolidBrush(this.myBackColor);
                g.FillPolygon(b, pt);
                b.Dispose();
                //----------------------------

                //----------------------------
                // draw border
                //g.DrawRectangle(SystemPens.ControlDark, recBounds);
                g.DrawPolygon(SystemPens.ControlDark, pt);
            }

            g.Clip = rsaved;
            //----------------------------


            //----------------------------
            // draw background to cover flat border areas
            if (this.SelectedTab != null)
            {
                TabPage tabPage = this.SelectedTab;
                Color color = tabPage.BackColor;
                border = new Pen(color);

                TabArea.Offset(1, 1);
                TabArea.Width -= 2;
                TabArea.Height -= 2;

                g.DrawRectangle(border, TabArea);
                TabArea.Width -= 1;
                TabArea.Height -= 1;
                g.DrawRectangle(border, TabArea);

                border.Dispose();
            }
            //----------------------------
        }

        public event EventHandler NewTabClicked = null;
        void FlatTabControl_MouseMove(object sender, MouseEventArgs e)
        {
            if (NewTabVisible)
            {
                Graphics g = CreateGraphics();
                Rectangle rimage = new Rectangle(_NewTabRectangle.X + 3, _NewTabRectangle.Y + 2, new_img.Width, new_img.Height);
                if (_NewTabRectangle.Contains(new Point(e.X, e.Y)))
                    g.DrawImage(new_img, rimage);
                else
                    g.FillRectangle(new SolidBrush(myBackColor), rimage);
            }
        }

        void FlatTabControl_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left && NewTabVisible && NewTabClicked != null && _NewTabRectangle.Contains(new Point(e.X, e.Y)))
                NewTabClicked(this, EventArgs.Empty);
            else if (e.Button == MouseButtons.Right && CanDelete)
            {
                TabPage page = null;
                for (int i = 0; i < this.TabCount - (NewTabVisible ? 1 : 0); i++)
                {
                    Rectangle rect = this.GetTabRect(i);
                    if (rect.Contains(new Point(e.X, e.Y)))
                        page = TabPages[i];
                }
                if (page != null)
                {
                    ContextMenuStrip menu = new ContextMenuStrip();
                    ToolStripMenuItem mnu = new ToolStripMenuItem("Закрыть вкладку '" + page.Text + "'", null, delegate(object snd, EventArgs ev)
                    {
                        if (this.TabCount - (NewTabVisible ? 1 : 0) > 1)
                            TabPages.Remove(page);
                    });
                    mnu.AutoSize = true;
                    mnu.DisplayStyle = ToolStripItemDisplayStyle.Text;
                    menu.Items.Add(mnu);
                    menu.ShowImageMargin = false;
                    menu.Show(this, new Point(e.X, e.Y));
                }
            }
        }

        void FlatTabControl_Selecting(object sender, TabControlCancelEventArgs e)
        {
            if (NewTabVisible && e.TabPageIndex == TabPages.Count - 1)
                e.Cancel = true;
        }

        internal void DrawTab(Graphics g, TabPage tabPage, int nIndex)
        {
            Rectangle recBounds = this.GetTabRect(nIndex);
            RectangleF tabTextArea = (RectangleF)this.GetTabRect(nIndex);

            bool bSelected = (this.SelectedIndex == nIndex);

            Point[] pt = new Point[7];
            if (this.Alignment == TabAlignment.Top)
            {
                pt[0] = new Point(recBounds.Left, recBounds.Bottom);
                pt[1] = new Point(recBounds.Left, recBounds.Top + 3);
                pt[2] = new Point(recBounds.Left + 3, recBounds.Top);
                pt[3] = new Point(recBounds.Right - 3, recBounds.Top);
                pt[4] = new Point(recBounds.Right, recBounds.Top + 3);
                pt[5] = new Point(recBounds.Right, recBounds.Bottom);
                pt[6] = new Point(recBounds.Left, recBounds.Bottom);
            }
            else
            {
                pt[0] = new Point(recBounds.Left, recBounds.Top);
                pt[1] = new Point(recBounds.Right, recBounds.Top);
                pt[2] = new Point(recBounds.Right, recBounds.Bottom - 3);
                pt[3] = new Point(recBounds.Right - 3, recBounds.Bottom);
                pt[4] = new Point(recBounds.Left + 3, recBounds.Bottom);
                pt[5] = new Point(recBounds.Left, recBounds.Bottom - 3);
                pt[6] = new Point(recBounds.Left, recBounds.Top);
            }

            //----------------------------
            // fill this tab with background color
            Brush br = new SolidBrush(tabPage.BackColor);
            g.FillPolygon(br, pt);
            br.Dispose();
            //----------------------------

            //----------------------------
            // draw border
            //g.DrawRectangle(SystemPens.ControlDark, recBounds);
            g.DrawPolygon(SystemPens.ControlDark, pt);

            if (bSelected)
            {
                //----------------------------
                // clear bottom lines
                Pen pen = new Pen(tabPage.BackColor);

                switch (this.Alignment)
                {
                    case TabAlignment.Top:
                        g.DrawLine(pen, recBounds.Left + 1, recBounds.Bottom, recBounds.Right - 1, recBounds.Bottom);
                        g.DrawLine(pen, recBounds.Left + 1, recBounds.Bottom + 1, recBounds.Right - 1, recBounds.Bottom + 1);
                        break;

                    case TabAlignment.Bottom:
                        g.DrawLine(pen, recBounds.Left + 1, recBounds.Top, recBounds.Right - 1, recBounds.Top);
                        g.DrawLine(pen, recBounds.Left + 1, recBounds.Top - 1, recBounds.Right - 1, recBounds.Top - 1);
                        g.DrawLine(pen, recBounds.Left + 1, recBounds.Top - 2, recBounds.Right - 1, recBounds.Top - 2);
                        break;
                }

                pen.Dispose();
                //----------------------------
            }
            //----------------------------

            //----------------------------
            // draw tab's icon

            if ((tabPage.ImageIndex >= 0) && (ImageList != null) && (ImageList.Images[tabPage.ImageIndex] != null))
            {
                int nLeftMargin = 4;
                int nRightMargin = 2;

                Image img = ImageList.Images[tabPage.ImageIndex];

                Rectangle rimage = new Rectangle(recBounds.X + nLeftMargin, recBounds.Y + 2, img.Width, img.Height);

                // adjust rectangles
                float nAdj = (float)(nLeftMargin + img.Width + nRightMargin);

                tabTextArea.Width -= nAdj;

                // draw icon
                g.DrawImage(img, rimage);
                rimage.Y += (recBounds.Height - img.Height) / 2;
                tabTextArea.X += nAdj;
            }
            //----------------------------

            //----------------------------
            // draw string
            StringFormat stringFormat = new StringFormat();
            stringFormat.Alignment = StringAlignment.Center;
            stringFormat.LineAlignment = StringAlignment.Center;

            br = new SolidBrush(tabPage.ForeColor);

            g.DrawString(tabPage.Text, Font, br, tabTextArea, stringFormat);
            //----------------------------
        }

        internal void DrawIcons(Graphics g)
        {
            if ((leftRightImages == null) || (leftRightImages.Images.Count != 4))
                return;

            //----------------------------
            // calc positions
            Rectangle TabControlArea = this.ClientRectangle;

            Rectangle r0 = new Rectangle();
            Win32.GetClientRect(scUpDown.Handle, ref r0);

            //Rectangle r01 = this.GetTabRect(0);
            //r0.Height = r01.Height;
            //r0.Width = 30;
            //r0.X = 5;
            //r0.Y = 4;
            //r0.Offset(0,3);

            Brush br = new SolidBrush(myBackColor);
            g.FillRectangle(br, r0);
            br.Dispose();

            Pen border = new Pen(SystemColors.ControlDark);
            Rectangle rborder = r0;
            rborder.Inflate(-2, -2);
            rborder.Offset(0, 1);
            g.DrawRectangle(border, rborder);
            border.Dispose();

            int nMiddle = (r0.Width / 2);
            int nTop = (r0.Height - 16) / 2;
            int nLeft = (nMiddle - 16) / 2;

            Rectangle r1 = new Rectangle(nLeft, nTop + 2, 16, 16);
            Rectangle r2 = new Rectangle(nMiddle + nLeft, nTop + 2, 16, 16);
            //----------------------------

            //----------------------------
            // draw buttons
            Image img = leftRightImages.Images[1];
            if (img != null)
            {
                if (this.TabCount > 0)
                {
                    Rectangle r3 = this.GetTabRect(0);
                    if (r3.Left < TabControlArea.Left)
                        g.DrawImage(img, r1);
                    else
                    {
                        img = leftRightImages.Images[3];
                        if (img != null)
                            g.DrawImage(img, r1);
                    }
                }
            }

            img = leftRightImages.Images[0];
            if (img != null)
            {
                if (this.TabCount > 0)
                {
                    Rectangle r3 = this.GetTabRect(this.TabCount - 1);
                    if (r3.Right > (TabControlArea.Width - r0.Width))
                        g.DrawImage(img, r2);
                    else
                    {
                        img = leftRightImages.Images[2];
                        if (img != null)
                            g.DrawImage(img, r2);
                    }
                }
            }
            //----------------------------
        }

        protected override void OnCreateControl()
        {
            base.OnCreateControl();

            FindUpDown();
        }

        private void FlatTabControl_ControlAdded(object sender, ControlEventArgs e)
        {
            FindUpDown();
            UpdateUpDown();
        }

        private void FlatTabControl_ControlRemoved(object sender, ControlEventArgs e)
        {
            FindUpDown();
            UpdateUpDown();
        }

        void FlatTabControl_Resize(object sender, EventArgs e)
        {
            FindUpDown();
            UpdateUpDown();
        }

        private void FlatTabControl_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateUpDown();
            Invalidate();	// we need to update border and background colors
        }

        private void FindUpDown()
        {
            bool bFound = false;

            // find the UpDown control
            IntPtr pWnd = Win32.GetWindow(this.Handle, Win32.GW_CHILD);

            while (pWnd != IntPtr.Zero)
            {
                //----------------------------
                // Get the window class name
                char[] className = new char[33];

                int length = Win32.GetClassName(pWnd, className, 32);

                string s = new string(className, 0, length);
                //----------------------------

                if (s == "msctls_updown32")
                {
                    bFound = true;

                    if (!bUpDown)
                    {
                        //----------------------------
                        // Subclass it
                        this.scUpDown = new SubClass(pWnd, true);
                        this.scUpDown.SubClassedWndProc += new SubClass.SubClassWndProcEventHandler(scUpDown_SubClassedWndProc);
                        //----------------------------

                        bUpDown = true;
                    }
                    break;
                }

                pWnd = Win32.GetWindow(pWnd, Win32.GW_HWNDNEXT);
            }

            if ((!bFound) && (bUpDown))
                bUpDown = false;
        }

        private void UpdateUpDown()
        {
            if (bUpDown)
            {
                if (Win32.IsWindowVisible(scUpDown.Handle))
                {
                    Rectangle rect = new Rectangle();

                    Win32.GetClientRect(scUpDown.Handle, ref rect);
                    Win32.InvalidateRect(scUpDown.Handle, ref rect, true);
                }
            }
        }

        #region scUpDown_SubClassedWndProc Event Handler

        private int scUpDown_SubClassedWndProc(ref Message m)
        {
            switch (m.Msg)
            {
                case Win32.WM_PAINT:
                    {
                        //------------------------
                        // redraw
                        IntPtr hDC = Win32.GetWindowDC(scUpDown.Handle);
                        Graphics g = Graphics.FromHdc(hDC);

                        DrawIcons(g);

                        g.Dispose();
                        Win32.ReleaseDC(scUpDown.Handle, hDC);
                        //------------------------

                        // return 0 (processed)
                        m.Result = IntPtr.Zero;

                        //------------------------
                        // validate current rect
                        Rectangle rect = new Rectangle();

                        Win32.GetClientRect(scUpDown.Handle, ref rect);
                        Win32.ValidateRect(scUpDown.Handle, ref rect);
                        //------------------------
                    }
                    return 1;
            }

            return 0;
        }
        #endregion

        #region Component Designer generated code
        /// <summary>
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
        }


        #endregion

        #region Properties

        [Editor(typeof(TabpageExCollectionEditor), typeof(UITypeEditor))]
        public new TabPageCollection TabPages
        {
            get
            {
                return base.TabPages;
            }
        }

        new public TabAlignment Alignment
        {
            get { return base.Alignment; }
            set
            {
                TabAlignment ta = value;
                if ((ta != TabAlignment.Top) && (ta != TabAlignment.Bottom))
                    ta = TabAlignment.Top;

                base.Alignment = ta;
            }
        }

        [Browsable(false)]
        new public bool Multiline
        {
            get { return base.Multiline; }
            set { base.Multiline = false; }
        }

        [Browsable(true)]
        public Color myBackColor
        {
            get { return mBackColor; }
            set { mBackColor = value; this.Invalidate(); }
        }

        bool _NewTabVisible;
        [Browsable(true)]
        public bool NewTabVisible
        {
            get { return _NewTabVisible; }
            set
            {
                if (_NewTabVisible != value)
                {
                    _NewTabVisible = value;
                    if (_NewTabVisible && !TabPages.ContainsKey("NEW_TAB"))
                    {
                        TabPages.Add("NEW_TAB", "");
                        TabPages["NEW_TAB"].ToolTipText = "Добавить вкладку";
                    }
                    else if (TabPages.ContainsKey("NEW_TAB"))
                        TabPages.RemoveByKey("NEW_TAB");
                    this.Invalidate();
                }
            }
        }

        Rectangle _NewTabRectangle = new Rectangle(0, 0, 0, 0);
        bool _CanDelete = false;
        [Browsable(true)]
        public bool CanDelete
        {
            get { return _CanDelete; }
            set { _CanDelete = value; }
        }

        //void FlatTabControl_MouseMove(object sender, MouseEventArgs e)
        //{
        //    Rectangle recBounds = GetTabRect(TabPages.IndexOf(SelectedTab));
        //    ShowIcon = !ShowIconOnHover || recBounds.Contains(SelectedTab.PointToClient(this.PointToScreen(new Point(e.X, e.Y))));
        //}
        #endregion

        #region TabpageExCollectionEditor

        internal class TabpageExCollectionEditor : CollectionEditor
        {
            public TabpageExCollectionEditor(System.Type type)
                : base(type)
            {
            }

            protected override Type CreateCollectionItemType()
            {
                return typeof(TabPage);
            }
        }

        #endregion
    }

    //#endregion
}
internal class Win32
{
    /*
     * GetWindow() Constants
     */
    public const int GW_HWNDFIRST = 0;
    public const int GW_HWNDLAST = 1;
    public const int GW_HWNDNEXT = 2;
    public const int GW_HWNDPREV = 3;
    public const int GW_OWNER = 4;
    public const int GW_CHILD = 5;

    public const int WM_NCCALCSIZE = 0x83;
    public const int WM_WINDOWPOSCHANGING = 0x46;
    public const int WM_PAINT = 0xF;
    public const int WM_CREATE = 0x1;
    public const int WM_NCCREATE = 0x81;
    public const int WM_NCPAINT = 0x85;
    public const int WM_PRINT = 0x317;
    public const int WM_DESTROY = 0x2;
    public const int WM_SHOWWINDOW = 0x18;
    public const int WM_SHARED_MENU = 0x1E2;
    public const int HC_ACTION = 0;
    public const int WH_CALLWNDPROC = 4;
    public const int GWL_WNDPROC = -4;

    public Win32() { }

    [DllImport("User32.dll", CharSet = CharSet.Auto)]
    public static extern IntPtr GetWindowDC(IntPtr handle);

    [DllImport("User32.dll", CharSet = CharSet.Auto)]
    public static extern IntPtr ReleaseDC(IntPtr handle, IntPtr hDC);

    [DllImport("Gdi32.dll", CharSet = CharSet.Auto)]
    public static extern IntPtr CreateCompatibleDC(IntPtr hdc);

    [DllImport("User32.dll", CharSet = CharSet.Auto)]
    public static extern int GetClassName(IntPtr hwnd, char[] className, int maxCount);

    [DllImport("User32.dll", CharSet = CharSet.Auto)]
    public static extern IntPtr GetWindow(IntPtr hwnd, int uCmd);

    [DllImport("User32.dll", CharSet = CharSet.Auto)]
    public static extern bool IsWindowVisible(IntPtr hwnd);

    [DllImport("user32", CharSet = CharSet.Auto)]
    public static extern int GetClientRect(IntPtr hwnd, ref RECT lpRect);

    [DllImport("user32", CharSet = CharSet.Auto)]
    public static extern int GetClientRect(IntPtr hwnd, [In, Out] ref Rectangle rect);

    [DllImport("user32", CharSet = CharSet.Auto)]
    public static extern bool MoveWindow(IntPtr hwnd, int X, int Y, int nWidth, int nHeight, bool bRepaint);

    [DllImport("user32", CharSet = CharSet.Auto)]
    public static extern bool UpdateWindow(IntPtr hwnd);

    [DllImport("user32", CharSet = CharSet.Auto)]
    public static extern bool InvalidateRect(IntPtr hwnd, ref Rectangle rect, bool bErase);

    [DllImport("user32", CharSet = CharSet.Auto)]
    public static extern bool ValidateRect(IntPtr hwnd, ref Rectangle rect);

    [DllImport("user32.dll", CharSet = CharSet.Auto)]
    internal static extern bool GetWindowRect(IntPtr hWnd, [In, Out] ref Rectangle rect);

    [StructLayout(LayoutKind.Sequential)]
    public struct RECT
    {
        public int Left;
        public int Top;
        public int Right;
        public int Bottom;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct WINDOWPOS
    {
        public IntPtr hwnd;
        public IntPtr hwndAfter;
        public int x;
        public int y;
        public int cx;
        public int cy;
        public uint flags;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct NCCALCSIZE_PARAMS
    {
        public RECT rgc;
        public WINDOWPOS wndpos;
    }
}

#region SubClass Classing Handler Class
internal class SubClass : System.Windows.Forms.NativeWindow
{
    public delegate int SubClassWndProcEventHandler(ref System.Windows.Forms.Message m);
    public event SubClassWndProcEventHandler SubClassedWndProc;
    private bool IsSubClassed = false;

    public SubClass(IntPtr Handle, bool _SubClass)
    {
        base.AssignHandle(Handle);
        this.IsSubClassed = _SubClass;
    }

    public bool SubClassed
    {
        get { return this.IsSubClassed; }
        set { this.IsSubClassed = value; }
    }

    protected override void WndProc(ref Message m)
    {
        if (this.IsSubClassed)
        {
            if (OnSubClassedWndProc(ref m) != 0)
                return;
        }
        base.WndProc(ref m);
    }

    public void CallDefaultWndProc(ref Message m)
    {
        base.WndProc(ref m);
    }

    #region HiWord Message Cracker
    public int HiWord(int Number)
    {
        return ((Number >> 16) & 0xffff);
    }
    #endregion

    #region LoWord Message Cracker
    public int LoWord(int Number)
    {
        return (Number & 0xffff);
    }
    #endregion

    #region MakeLong Message Cracker
    public int MakeLong(int LoWord, int HiWord)
    {
        return (HiWord << 16) | (LoWord & 0xffff);
    }
    #endregion

    #region MakeLParam Message Cracker
    public IntPtr MakeLParam(int LoWord, int HiWord)
    {
        return (IntPtr)((HiWord << 16) | (LoWord & 0xffff));
    }
    #endregion

    private int OnSubClassedWndProc(ref Message m)
    {
        if (SubClassedWndProc != null)
        {
            return this.SubClassedWndProc(ref m);
        }

        return 0;
    }
}
#endregion
