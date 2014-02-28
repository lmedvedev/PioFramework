using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.IO;
using BO;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;

namespace BOF
{
    public partial class CtlPicture : UserControl, IDataMember
    {
        public event EventHandler ValueChanged;
        PictureBox picture = new PictureBox();
        public CtlPicture()
        {
            picture.Dock = DockStyle.Fill;
            picture.InitialImage = global::BOF.Properties.Resources.wait;
            picture.BackgroundImageLayout = ImageLayout.Center;
            picture.BackgroundImage = Properties.Resources.ThumbnailLoadngHS;
            picture.MouseDown += new MouseEventHandler(picture_MouseDown);
            this.Controls.Add(picture);
            
            ContextMenuStrip menu = new ContextMenuStrip();
            menu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            new ToolStripMenuItem("Изменить изображение", global::BOF.Properties.Resources.NewDataRecord, delegate(object snd, EventArgs ev){ PictureChange(); }, Keys.Insert),
            new ToolStripMenuItem("Сохранить изображение", global::BOF.Properties.Resources.save, delegate(object snd, EventArgs ev){ PictureSave(); }, Keys.Control | Keys.S),
            new ToolStripMenuItem("Удалить изображение", global::BOF.Properties.Resources.DeleteHS, delegate(object snd, EventArgs ev){ PictureDelete(); }, Keys.Delete) });
            menu.Size = new System.Drawing.Size(181, 48);
            menu.Enabled = (this.Enabled);
            this.SetStyle(ControlStyles.Selectable, true);
            this.TabStop = true;
            base.BorderStyle = BorderStyle.None;

            picture.ContextMenuStrip = menu;
            picture.DoubleClick += delegate(object snd, EventArgs ev) { PictureChange(); };
        }

        private string _defaultDirectory;
        [Browsable(true)]
        public string DefaultDirectory
        {
            get { return _defaultDirectory; }
            set { _defaultDirectory = value; }
        }

        private string _DataMember;
        public string DataMember
        {
            get { return _DataMember; }
            set { _DataMember = value; }
        }

        public void AddBinding(object datasource)
        {
            if (!string.IsNullOrEmpty(DataMember))
                this.DataBindings.Add("Value", datasource, DataMember, true, DataSourceUpdateMode.OnPropertyChanged);
        }

        public void AddBinding()
        {
            OKCancelDatForm frm = this.FindForm() as OKCancelDatForm;
            if (frm != null)
                AddBinding(frm.NewValue);
        }

        public void RemoveBinding()
        {
            if (!string.IsNullOrEmpty(DataMember) && this.DataBindings["Value"] != null)
                this.DataBindings.Remove(this.DataBindings["Value"]);
        }

        public void WriteValue()
        {
            if (!string.IsNullOrEmpty(DataMember) && this.DataBindings["Value"] != null)
                this.DataBindings["Value"].WriteValue();
            FireValueChanged();
        }

        new public BorderStyle BorderStyle
        {
            get { return picture.BorderStyle; }
            set { picture.BorderStyle = value; }
        }

        const string _helplabeltext = "Двойной клик, Insert - вставить изображение, Delete - удалить, Ctrl+S - сохранить. Enter, вниз - следующее поле, вверх - предыдущее поле. Правая кнопка мыши - контекстное меню.";
        private string _HelpLabelText = _helplabeltext;

        [Category("BOF")]
        [DefaultValue(_helplabeltext)]
        [Description("Текст, который будет показываться при активации контрола")]
        public string HelpLabelText
        {
            get { return _HelpLabelText; }
            set { _HelpLabelText = value; }
        }
        protected override void OnEnter(EventArgs e)
        {
            if (Enabled)
            {
                IHelpLabel frm = FindForm() as IHelpLabel;
                if (frm != null) frm.WriteHelp(HelpLabelText);
            }
            base.BorderStyle = BorderStyle.FixedSingle;
            base.OnEnter(e);
        }
        protected override void OnLeave(EventArgs e)
        {
            IHelpLabel frm = FindForm() as IHelpLabel;
            if (frm != null) frm.WriteHelp("");
            base.BorderStyle = BorderStyle.None;
            base.OnLeave(e);
        }
        void picture_MouseDown(object sender, MouseEventArgs e)
        {
            this.Focus();
        }

        public PictureBoxSizeMode SizeMode
        {
            get { return picture.SizeMode; }
            set { picture.SizeMode = value; }
        }

        bool _IsMonochrome = false;
        [Category("BOF")]
        public bool IsMonochrome 
        {
            get { return _IsMonochrome; }
            set { _IsMonochrome = value; }
        }

        Size _ItemSize = Size.Empty;
        [Category("BOF")]
        public Size ItemSize
        {
            get { return _ItemSize; }
            set { _ItemSize = value; }
        }

        [Category("BOF")]
        [Bindable(true)]
        public Image Value
        {
            get 
            {
                return (picture.Image == null || picture.Image == BOF.Icons.NewItem.ToBitmap()) ? null : picture.Image; 
            }
            set 
            {
                this.BackgroundImage = (value == null) ? Properties.Resources.ThumbnailLoadngHS : null;
                Image img = value;
                if (img != null)
                {
                    if (ItemSize != Size.Empty)
                    {
                        Bitmap bmp = new Bitmap(ItemSize.Width, ItemSize.Height);
                        Graphics g = Graphics.FromImage(bmp as Image);
                        g.InterpolationMode = InterpolationMode.Bicubic;
                        g.DrawImage(img, 0, 0, bmp.Width, bmp.Height);
                        img = bmp as Image;
                        g.Dispose();
                    }
                    if (IsMonochrome)
                    {
                        Bitmap bmp = new Bitmap(img.Width, img.Height);
                        Graphics g = Graphics.FromImage(bmp as Image);
                        ImageAttributes imgAttributes = new ImageAttributes();
                        ColorMatrix gray = new ColorMatrix(
                        new float[][] {
                              new float[] { 0.299f, 0.299f, 0.299f, 0, 0 },
                              new float[] { 0.588f, 0.588f, 0.588f, 0, 0}, 
                              new float[] { 0.111f, 0.111f, 0.111f, 0, 0 }, 
                              new float[] { 0, 0, 0, 1, 0 }, 
                              new float[] { 0, 0, 0, 0, 1}, 
                            }
                        );
                        imgAttributes.SetColorMatrix(gray);
                        g.DrawImage(img, new Rectangle(0, 0, img.Width, img.Height), 0, 0, img.Width, img.Height, GraphicsUnit.Pixel, imgAttributes);
                        img = bmp as Image;
                        g.Dispose();
                    }
                    //this.Image = null;
                    //if (this.BackgroundImage.Size.Height > this.Size.Height || this.BackgroundImage.Size.Width > this.Size.Width)
                    //{
                    //    this.BackgroundImageLayout = ImageLayout.Zoom;
                    //}
                    //else
                    //{
                    //    this.BackgroundImageLayout = ImageLayout.Center;
                    //}
                }
                picture.Image = img;
            }
        }
        public void FireValueChanged()
        {
            IHelpLabel frm = FindForm() as IHelpLabel;
            if (frm != null)
                frm.SetError(this, "");

            if (ValueChanged != null) ValueChanged(this, new ValueEventArgs(Value));
        }
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == Keys.Return || (keyData == Keys.Down))
            {
                Form frm = this.FindForm();
                if (frm != null)
                    frm.SelectNextControl(this, true, true, true, true);
                return true;
            }
            else if (keyData == Keys.Up)
            {
                Form frm = this.FindForm();
                if (frm != null)
                    frm.SelectNextControl(this, false, true, true, true);
            }
            else if (keyData == Keys.Insert)
                PictureChange();
            else if (keyData == Keys.Delete)
                PictureDelete();
            else if (keyData == (Keys.Control | Keys.S))
                PictureSave();
            return base.ProcessCmdKey(ref msg, keyData);
        }

        public void PictureChange()
        {
            if (!Enabled)
                return;
            try
            {
                OpenFileDialog file_dlg = new OpenFileDialog();
                file_dlg.InitialDirectory = DefaultDirectory;
                file_dlg.RestoreDirectory = true;
                if (Common.IsNullOrEmpty(file_dlg.InitialDirectory))
                {
                    if (Common.IsNullOrEmpty(DefaultDirectory))
                        file_dlg.InitialDirectory = DefaultDirectory;
                    else
                        file_dlg.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
                }
                file_dlg.Title = "Выберите файл с изображением";
                file_dlg.Filter = "Графические файлы (*.bmp;*.jpeg;*.jpg;*.gif;*.png)| *.bmp;*.jpeg;*.jpg;*.gif;*.png|Все файлы (*.*)| *.*";
                if (file_dlg.ShowDialog() == DialogResult.OK)
                {
                    this.Value = Image.FromFile(file_dlg.FileName);
                    WriteValue();
                }

            }
            catch (Exception exp)
            {
                new ExceptionForm("Внимание", "Не удалось сохранить изображение.\nДля более подробной информации раскройте детали сообщения об ошибке.", exp, MessageBoxIcon.Error).ShowDialog();
            }
        }

        public void PictureSave()
        {
            if (!Enabled || this.Value == null)
                return;
            try
            {
                OpenFileDialog file_dlg = new OpenFileDialog();
                file_dlg.InitialDirectory = DefaultDirectory;
                file_dlg.RestoreDirectory = true;
                if (Common.IsNullOrEmpty(file_dlg.InitialDirectory))
                {
                    if (Common.IsNullOrEmpty(DefaultDirectory))
                        file_dlg.InitialDirectory = DefaultDirectory;
                    else
                        file_dlg.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
                }
                file_dlg.FileName = "image.bmp";
                file_dlg.Title = "Выберите имя файла для сохранения";
                file_dlg.CheckFileExists = false;
                file_dlg.Filter = "Графические файлы (*.bmp;*.jpeg;*.jpg;*.gif;*.png)| *.bmp;*.jpeg;*.jpg;*.gif;*.png|Все файлы (*.*)| *.*";
                if (file_dlg.ShowDialog() == DialogResult.OK)
                {
                    FileInfo file = new FileInfo(file_dlg.FileName);
                    ImageFormat frmt = ImageFormat.Bmp;
                    switch (file.Extension)
                    {
                        case "jpg":
                        case "jpeg":
                            frmt = ImageFormat.Jpeg;
                            break;
                        case "gif":
                            frmt = ImageFormat.Gif;
                            break;
                        case "png":
                            frmt = ImageFormat.Png;
                            break;
                    }
                    this.Value.Save(file_dlg.FileName, frmt);
                    MessageBox.Show("Изображение сохранено в файле '" + file_dlg.FileName + "'.", "Успешное завершение", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

            }
            catch (Exception exp)
            {
                new ExceptionForm("Внимание", "Не удалось сохранить изображение.\nДля более подробной информации раскройте детали сообщения об ошибке.", exp, MessageBoxIcon.Error).ShowDialog();
            }
        }
        
        public void PictureDelete()
        {
            if (!Enabled)
                return;
            try
            {
                if (this.Value != null)
                {
                    if (MessageBox.Show("Вы уверены, что хотите удалить изображение?", "Удаление изображения", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
                    {
                        this.Value = null;
                        WriteValue();
                    }
                }
            }
            catch (Exception exp)
            {
                new ExceptionForm("Внимание", "Не удалось удалить изображение.\nДля более подробной информации раскройте детали сообщения об ошибке.", exp, MessageBoxIcon.Error).ShowDialog();
            }
        }
    }
}
