using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace BOF
{
    public static class Icons
    {
        public static readonly Icon Appl = global::BOF.Properties.Resources.Appl; 
        public static readonly Icon BlueArrow = global::BOF.Properties.Resources.BlueArrow;
        public static readonly Icon cab = global::BOF.Properties.Resources.cab;
        public static readonly Icon Calendar = global::BOF.Properties.Resources.Calendar;
        public static readonly Icon Copy = global::BOF.Properties.Resources.copy;
        public static readonly Icon checkedIcon = global::BOF.Properties.Resources.checkedIcon;
        public static readonly Icon checkuncheck = global::BOF.Properties.Resources.checkuncheck;
        public static readonly Icon CloseForm = global::BOF.Properties.Resources.CloseForm;
        public static readonly Icon CONTACTS = global::BOF.Properties.Resources.CONTACTS;
        public static readonly Icon Company = new Icon(global::BOF.Properties.Resources.Company, 16, 16);
        public static readonly Icon Data_Schema = global::BOF.Properties.Resources.Data_Schema;
        public static readonly Icon DeleteItem = global::BOF.Properties.Resources.DeleteItem;
        public static readonly Icon document = global::BOF.Properties.Resources.document;
        public static readonly Icon Dots3 = global::BOF.Properties.Resources.Dots3;
        public static readonly Icon Back = global::BOF.Properties.Resources.back;
        public static readonly Icon DoubleArrow = global::BOF.Properties.Resources.DoubleArrow;
        public static readonly Icon DownArrow1 = global::BOF.Properties.Resources.DownArrow1;
        public static readonly Icon DownGreenArrow = global::BOF.Properties.Resources.down;
        public static readonly Icon EditItem = global::BOF.Properties.Resources.EditItem;
        public static readonly Icon Folder = global::BOF.Properties.Resources.Folder;
        public static readonly Icon folderopen = global::BOF.Properties.Resources.folderopen;
        public static readonly Icon GreenArrow = global::BOF.Properties.Resources.GreenArrow;
        public static readonly Icon groupdates = global::BOF.Properties.Resources.groupdates;
        public static readonly Icon groupfunds = global::BOF.Properties.Resources.groupfunds;
        public static readonly Icon groupoutlet = global::BOF.Properties.Resources.groupoutlet;
        public static readonly Icon LeftBottomCorner = global::BOF.Properties.Resources.LeftBottomCorner;
        public static readonly Icon LeftTopCorner = global::BOF.Properties.Resources.LeftTopCorner;
        public static readonly Icon man = global::BOF.Properties.Resources.man;
        public static readonly Icon NewItem = global::BOF.Properties.Resources.NewItem;
        public static readonly Icon OpenFolder = global::BOF.Properties.Resources.OpenFolder;
        //public static readonly Icon printer = global::BOF.Properties.Resources.printer;
        public static readonly Icon Refresh = global::BOF.Properties.Resources.Refresh;
        public static readonly Icon Select = global::BOF.Properties.Resources.Select;
        public static readonly Icon textdoc = global::BOF.Properties.Resources.textdoc;
        public static readonly Icon uncheckedIcon = global::BOF.Properties.Resources.uncheckedIcon;
        public static readonly Icon user = global::BOF.Properties.Resources.user;
        public static readonly Icon warning = global::BOF.Properties.Resources.warning;
        public static readonly Icon Web_XML = global::BOF.Properties.Resources.Web_XML;
        public static readonly Icon XIcon1 = global::BOF.Properties.Resources.XIcon1;
        public static readonly Icon FilterForm = BOF.Properties.Resources.icoFilterForm;
        public static readonly Icon FilterIcon = BOF.Properties.Resources.icoFilter;
        public static readonly Icon FilterEdit = BOF.Properties.Resources.icoFilerEdit;
        public static readonly Icon Sound = BOF.Properties.Resources.sound;
        public static readonly Icon Mail = BOF.Properties.Resources.mail;
        public static readonly Icon ico1C = BOF.Properties.Resources._1C;
        public static readonly Icon NewDataRecordIcon = Icon.FromHandle(global::BOF.Properties.Resources.NewDataRecord.GetHicon());

        public static readonly Bitmap Save = global::BOF.Properties.Resources.save;
        public static readonly Bitmap Filter = global::BOF.Properties.Resources.Filter;
        public static readonly Bitmap DownArrow = global::BOF.Properties.Resources.DownArrow;
        public static readonly Bitmap panelhide = global::BOF.Properties.Resources.panelhide;
        public static readonly Bitmap panelshow = global::BOF.Properties.Resources.panelshow;
        public static readonly Bitmap preview = global::BOF.Properties.Resources.preview;
        public static readonly Bitmap resize_left = global::BOF.Properties.Resources.resize_left;
        public static readonly Bitmap resize_right = global::BOF.Properties.Resources.resize_right;
        public static readonly Bitmap RolledBack = global::BOF.Properties.Resources.RolledBack;
        public static readonly Bitmap SuccessComplete = global::BOF.Properties.Resources.SuccessComplete;
        public static readonly Bitmap XIcon = global::BOF.Properties.Resources.XIcon;
        public static readonly Bitmap NewDataRecord = global::BOF.Properties.Resources.NewDataRecord;
        public static readonly Bitmap GroupPanel = global::BOF.Properties.Resources.GroupPanel;
        public static readonly Bitmap HandHeld = global::BOF.Properties.Resources.handheld;
        public static readonly Bitmap printer = new Icon(BOF.Properties.Resources.printer, 16, 16).ToBitmap();

        public static readonly Icon iconRed = new Icon(BOF.Properties.Resources.red, 16, 16);
        public static readonly Icon iconExclamation = new Icon(BOF.Properties.Resources.Exclamation, 16, 16);

        public static readonly Icon iconCRed = new Icon(BOF.Properties.Resources.cred, 16, 16);
        public static readonly Icon iconCDel = new Icon(BOF.Properties.Resources.cdel2, 16, 16);
        public static readonly Icon iconCYellow = new Icon(BOF.Properties.Resources.cyellow, 16, 16);
        public static readonly Icon iconCYellowDark = new Icon(BOF.Properties.Resources.cyellow2, 16, 16);
        public static readonly Icon iconCGreen = new Icon(BOF.Properties.Resources.cgreen, 16, 16);
        public static readonly Icon iconCWhite = new Icon(BOF.Properties.Resources.other, 16, 16);
        public static readonly Icon iconCBlue = new Icon(BOF.Properties.Resources.cblue, 16, 16);

        public static readonly Icon pio_docspreview = global::BOF.Properties.Resources.pio_docspreview;
        public static readonly Icon pio_docsprint = global::BOF.Properties.Resources.pio_docsprint;


        public static readonly Icon docs = new Icon(global::BOF.Properties.Resources.copy, 16, 16);
        public static readonly Icon go_back = new Icon(global::BOF.Properties.Resources.Collapse, 16, 16);
        public static readonly Icon go_forward = new Icon(global::BOF.Properties.Resources.Expand, 16, 16);

        public static readonly Icon inbox = new Icon(global::BOF.Properties.Resources.inbox_into, 16, 16);
        public static readonly Icon outbox = new Icon(global::BOF.Properties.Resources.outbox_out, 16, 16);
        public static readonly Icon briefcase = new Icon(global::BOF.Properties.Resources.briefcase, 16, 16);

        //public static readonly Cursor ZoomIn = new Cursor(typeof(byte[]), "ZoomIn");

        //public static readonly Cursor ZoomOut = new Cursor(typeof(byte[]), "ZoomOut");

        public static Icon GetStateIcon(BO.DocStatus state, int count)
        {
            Bitmap ret = iconCWhite.ToBitmap();
            switch (state)
            {
                case BO.DocStatus.PARTIAL:
                    ret = iconCYellow.ToBitmap();
                    break;
                case BO.DocStatus.ALL:
                    ret = iconCGreen.ToBitmap();
                    break;
                case BO.DocStatus.UNKNOWN:
                    ret = iconCRed.ToBitmap();
                    break;
            }
            if (count > 0)
            {
                bool[] pixels = Int2Pixels(ret.Width, ret.Height, count);
                int x = 0;
                int y = 0;
                foreach (bool mark in pixels)
                {
                    if (x >= ret.Width)
                    {
                        x = 0;
                        y++;
                    }
                    if (mark)
                        ret.SetPixel(x, y, Color.Navy);
                    x++;
                }
            }
            Icon ic = null;
            try
            {
                ic = Icon.FromHandle(ret.GetHicon());
            }
            catch { }
            return ic;

        }

        public static bool[] Int2Pixels(int width, int height, int count)
        {
            List<bool> ret = new List<bool>();
            int[, ,] res = new int[,,]{
            {
                {0,1,1,1,1},
                {0,1,0,0,1},
                {0,1,0,0,1},
                {0,1,0,0,1},
                {0,1,1,1,1},
            },
            {
                {0,1,1,0,0},
                {0,0,1,0,0},
                {0,0,1,0,0},
                {0,0,1,0,0},
                {0,1,1,1,0},
            },
            {
                {0,1,1,1,1},
                {0,0,0,0,1},
                {0,1,1,1,1},
                {0,1,0,0,0},
                {0,1,1,1,1},
            },
            {
                {0,1,1,1,1},
                {0,0,0,0,1},
                {0,1,1,1,1},
                {0,0,0,0,1},
                {0,1,1,1,1},
            },
            {
                {0,1,0,0,1},
                {0,1,0,0,1},
                {0,1,1,1,1},
                {0,0,0,0,1},
                {0,0,0,0,1},
            },
            {
                {0,1,1,1,1},
                {0,1,0,0,0},
                {0,1,1,1,1},
                {0,0,0,0,1},
                {0,1,1,1,1},
            },
            {
                {0,1,1,1,1},
                {0,1,0,0,0},
                {0,1,1,1,1},
                {0,1,0,0,1},
                {0,1,1,1,1},
            },
            {
                {0,1,1,1,1},
                {0,0,0,0,1},
                {0,0,0,0,1},
                {0,0,0,0,1},
                {0,0,0,0,1},
            },
            {
                {0,1,1,1,1},
                {0,1,0,0,1},
                {0,1,1,1,1},
                {0,1,0,0,1},
                {0,1,1,1,1},
            },
            {
                {0,1,1,1,1},
                {0,1,0,0,1},
                {0,1,1,1,1},
                {0,0,0,0,1},
                {0,1,1,1,1},
            }};
            char[] chars = count.ToString().ToCharArray();
            bool[,] bits = new bool[chars.Length * res.GetLength(2), res.GetLength(1)];
            for (int y = 0; y < res.GetLength(1); y++)
            {
                int pos = 0;
                foreach (char ch in chars)
                {
                    int num = int.Parse(ch.ToString());
                    for (int x = 0; x < res.GetLength(2); x++)
                    {
                        bits[pos, y] = (res[num, y, x] != 0);
                        pos++;
                    }
                }
            }
            int offset_x = (int)Math.Round((decimal)((width - bits.GetLength(0) + 1) / 2));
            int offset_y = (int)Math.Round((decimal)((height - bits.GetLength(1) + 1) / 2));
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    bool val = false;
                    if (x >= offset_x && y >= offset_y && (x - offset_x < bits.GetLength(0)) && (y - offset_y < bits.GetLength(1)))
                        val = bits[x - offset_x, y - offset_y];
                    ret.Add(val);
                }
            }
            return ret.ToArray();
        }
    }
}
