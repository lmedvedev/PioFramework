using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Reflection;
//using System.

namespace BOF
{
    public partial class AboutForm : Form
    {
        public AboutForm(Icon AboutIcon)
        {
            InitializeComponent();
            if (AboutIcon != null)
                this.Icon = AboutIcon;
        }
        
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            DateTime dt = System.IO.File.GetLastWriteTime(System.Reflection.Assembly.GetEntryAssembly().Location);

            string AboutText = "";
            AboutText += "Приложение: " + Application.ProductName + "\r\n";
            AboutText += "Версия: " + Application.ProductVersion + "\r\n";
            AboutText += "Дата: " + dt.ToString("dd.MM.yyyy HH:mm") + "\r\n";
            AboutText += "Пользователь: " + System.Security.Principal.WindowsIdentity.GetCurrent().Name + "\r\n";
            labelAbout.Text = AboutText;

            ListViewGroup Assemb = new ListViewGroup("Сборки");
            ListViewGroup Connect = new ListViewGroup("Подключения");
            ListViewGroup Developers = new ListViewGroup("Разработчики");
            ListViewGroup Consultants = new ListViewGroup("Консультанты");
            listViewAbout.Groups.Add(Assemb);
            listViewAbout.Groups.Add(Connect);
            listViewAbout.Groups.Add(Developers);
            listViewAbout.Groups.Add(Consultants);

            foreach (Assembly asm in AppDomain.CurrentDomain.GetAssemblies())
            {
                AssemblyName asName = asm.GetName();
                ListViewItem l = new ListViewItem(new string[] { asName.Name, asName.Version.ToString() });
                l.Group = Assemb;
                listViewAbout.Items.Add(l);
            }

            foreach (string ConnStr in DA.Global.Connections.Keys)
            {
                ListViewItem l = new ListViewItem(ConnStr.Split(':'));
                l.Group = Connect;
                listViewAbout.Items.Add(l);
            }
            {
                ListViewItem l = null;
                //l = new ListViewItem("А. Бугаев");
                //l.Group = Developers;
                //listViewAbout.Items.Add(l);
                
                //l = new ListViewItem("И. Кишик");
                //l.Group = Developers;
                //listViewAbout.Items.Add(l);
                
                l = new ListViewItem("Л. Медведев");
                l.Group = Developers;
                listViewAbout.Items.Add(l);
                
                //l = new ListViewItem("А. Корнейчук");
                //l.Group = Developers;
                //listViewAbout.Items.Add(l);

                //l = new ListViewItem("К. Волынкин");
                //l.Group = Consultants;
                //listViewAbout.Items.Add(l);
            }

            this.Opacity = 0.0;
            this.Activate();
            this.Refresh();
            fadeTimer.Start();
        }

        private void fadeTimer_Tick(object sender, EventArgs e)
        {
            double d = 1000.0 / fadeTimer.Interval / 100.0;
            if (Opacity + d >= 1.0)
            {
                Opacity = 1.0;
                fadeTimer.Stop();
            }
            else
            {
                Opacity = Opacity + d;
            }
        }
    }
}