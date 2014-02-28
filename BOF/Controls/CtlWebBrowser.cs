using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.IO;
using BO;
using System.Xml;
using System.Xml.Xsl;

namespace BOF
{
    public class CtlWebBrowser : WebBrowser
    {
        //string _xsl = "";
        List<WebBrowserPage> _history = new List<WebBrowserPage>();
        public event EventHandler PageChanged = null;
        public static Dictionary<string, XslCompiledTransform> XslPool_Browser = new Dictionary<string, XslCompiledTransform>();


        string _Title = "";
        public string Title
        {
            get { return _Title; }
            set { _Title = value;}
        }
        
        int _CurrentPage = -1;
        private int CurrentPage
        {
            get { return _CurrentPage; }
            set
            {
                _CurrentPage = value;
                string html = DocumentText;
                if (_CurrentPage >= 0 && _CurrentPage < _history.Count && DocumentText != _history[_CurrentPage].HTMLContent)
                {
                    Title = _history[_CurrentPage].Title;
                    html = _history[_CurrentPage].HTMLContent;
                }
                bool has_changed = DocumentText != html;
                if (has_changed)
                {
                    DocumentText = html;
                    if (PageChanged != null)
                        PageChanged(this, EventArgs.Empty);
                }
            }
        }

        new public bool CanGoBack
        {
            get { return base.CanGoBack || _CurrentPage > 0; }
        }

        new public bool CanGoForward
        {
            get { return base.CanGoForward || _CurrentPage < _history.Count - 1; }
        }

        public CtlWebBrowser()
        {
            Clear();
            DocumentCompleted += new WebBrowserDocumentCompletedEventHandler(CtlWebBrowser_DocumentCompleted);
        }

        public void Clear()
        {
            DocumentText = "<html></html>"; 
            _history.Clear();
            _CurrentPage = -1;
        }

        public void AddPage(IXSLTemplate dat)
        {
            AddPage(new WebBrowserPage(dat));
        }

        public void AddPage(string html)
        {
            AddPage(new WebBrowserPage(html));
        }

        public void AddPage(XmlDocument xml)
        {
            AddPage(Common.TranslateXSL(xml));
        }

        public void AddPage(WebBrowserPage page)
        {
            if (page.HTMLContent != "" && (_history.Count == 0 || page.HTMLContent != _history[CurrentPage].HTMLContent))
            {
                if (CurrentPage >= 0 && CurrentPage < _history.Count - 1)
                    _history.RemoveRange(CurrentPage + 1, _history.Count - CurrentPage - 1);
                _history.Add(page);
                _CurrentPage++;
                Navigate("about:" + CurrentPage.ToString());
            }
        }

        new public void GoBack()
        {
            CurrentPage--;
        }

        new public void GoForward()
        {
            CurrentPage++;
        }
        
        protected override void OnNavigating(WebBrowserNavigatingEventArgs e)
        {
            string url = e.Url.ToString();
            if (url.StartsWith("about:blank"))
            {
            }
            else if (url.StartsWith("about:"))
            {
                try
                {
                    int i = int.Parse(e.Url.LocalPath);
                    CurrentPage = i;
                }
                catch { }
            }
            else
                base.OnNavigating(e);
        }

        void CtlWebBrowser_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            foreach (HtmlElement element in Document.All)
            {
                if (!Common.IsNullOrEmpty(element.GetAttribute("sender")))
                {
                    HtmlElementEventHandler handler = new HtmlElementEventHandler(FireElementClicked);
                    element.Click -= handler;
                    element.Click += handler;
                }
            }
        }

        public override bool PreProcessMessage(ref Message msg)
        {
            int WM_KEYDOWN = 0x100;
            if (msg.Msg == WM_KEYDOWN)
            {
                Keys key = (Keys)msg.WParam;
                if (key == Keys.Back)
                {
                    GoBack();
                }
            }
            return base.PreProcessMessage(ref msg);
        }

        public void FireElementClicked(object sender, HtmlElementEventArgs e)
        {
            WebBrowserClickEventArgs args = new WebBrowserClickEventArgs(sender as HtmlElement);
            if (_history[CurrentPage].Handler != null)
                _history[CurrentPage].Handler(this, args);
        }
    }

    public delegate void WebBrowserEventDelegate(object sender, WebBrowserClickEventArgs e);
    public class WebBrowserClickEventArgs : EventArgs
    {
        public string Sender = "";
        public string ElementType = "";
        public string Arguments = "";
        public string Target = "";
        public WebBrowserClickEventArgs(HtmlElement element)
        {
            ElementType = element.TagName;

            Sender = element.GetAttribute("sender");
            Arguments = element.GetAttribute("args");
            Target = element.GetAttribute("target");
        }
    }

    public class WebBrowserPage
    {
        public string Title;
        public WebBrowserEventDelegate Handler;
        public string HTMLContent = "";
        private static Dictionary<string, XslCompiledTransform> XslPool_WebPage = new Dictionary<string, XslCompiledTransform>();


        public WebBrowserPage()
        {
            Handler = DoClick;
        }

        public WebBrowserPage(string content)
            : this()
        {
            HTMLContent = content;
        }

        public WebBrowserPage(IHTMLTemplate dat)
            : this()
        {
            HTMLContent = dat.GetHTMLString();
            if (HTMLContent == "")
            {
                string name = dat.GetType().Namespace + ".HTML." + dat.GetHTMLName();
                XmlDocument xml = new XmlDocument();
                using (Stream str = System.Reflection.Assembly.GetAssembly(dat.GetType()).GetManifestResourceStream(name))
                {
                    if (str != null) 
                        xml.Load(str);
                }
                HTMLContent = xml.OuterXml;
            }
        }
        
        public WebBrowserPage(IXSLTemplate dat)
            : this()
        {
            bool useCached = true; // кеширование XSLT
#if DEBUG
            useCached = false;
#endif

            bool verbose = true;

            DateTime timeStart1 = DateTime.Now;
            DateTime timeStart = DateTime.Now;
            DateTime timeStop = DateTime.Now;
            DateTime timeStop1 = DateTime.Now;
            XslCompiledTransform _xslt = null;
            StringBuilder timerString = new StringBuilder("");

            XmlDocument xml = new XmlDocument();
            
            try
            {
                timeStart = DateTime.Now;
                xml = dat.Serialize();
                timeStop = DateTime.Now;
                timerString.AppendFormat("Сериализация: {0} ms;<br/>", (timeStop - timeStart).TotalMilliseconds);

                string filename = dat.GetTemplatePath() + dat.GetTemplateName();

                if (!XslPool_WebPage.ContainsKey(filename) || !useCached || XslPool_WebPage[filename] == null)
                {
                    timeStart = DateTime.Now;
                    try
                    {
                        _xslt = new XslCompiledTransform(true);
                        _xslt.Load(filename);
                    }
                    catch (Exception ex)
                    {
                        _xslt = null;
                        timerString.AppendFormat("Ошибка загрузки XSLT-файла {0}: <font color=\"red\">{1}</font><br/>", filename, Common.ExMessage(ex));
                    }
                    XslPool_WebPage[filename] = _xslt;
                    timeStop = DateTime.Now;
                    timerString.AppendFormat("Загрузка XSLT: {0} ms;<br/>", (timeStop - timeStart).TotalMilliseconds);

                }
                else
                {
                    _xslt = XslPool_WebPage[filename];
                }

                HTMLContent = Common.TranslateXSL(xml, _xslt);

                timeStop = DateTime.Now;
                timerString.AppendFormat("Трансформация: {0} ms;<br/>", (timeStop - timeStart).TotalMilliseconds);

            }
            catch (Exception ex)
            {
                HTMLContent = Common.TranslateXSL(xml);
                timerString.AppendFormat("Ошибка XSLT-трансформации: <font color=\"red\">{0}</font>, используем шаблон по-умолчанию.<br/>", Common.ExMessage(ex));
            }
            timeStop1 = DateTime.Now;
            timerString.AppendFormat("Итого: {0} ms;<br/>", (timeStop1 - timeStart1).TotalMilliseconds);

            if (verbose)
                HTMLContent += "<hr width=\"100%\" size=1 align=left>" + timerString.ToString();

        }

        public WebBrowserPage(XmlDocument xml, XslCompiledTransform _xslt)
            : this()
        {
            bool verbose = true;

            DateTime timeStart1 = DateTime.Now;
            DateTime timeStart = DateTime.Now;
            DateTime timeStop = DateTime.Now;
            DateTime timeStop1 = DateTime.Now;
            StringBuilder timerString = new StringBuilder("");

            try
            {
                timerString.AppendFormat("Сериализация: {0} ms;<br/>", (timeStop - timeStart).TotalMilliseconds);

                HTMLContent = Common.TranslateXSL(xml, _xslt);

                timeStop = DateTime.Now;
                timerString.AppendFormat("Трансформация: {0} ms;<br/>", (timeStop - timeStart).TotalMilliseconds);
            }
            catch (Exception ex)
            {
                HTMLContent = Common.TranslateXSL(xml);
                timerString.AppendFormat("Ошибка XSLT-трансформации: <font color=\"red\">{0}</font>, используем шаблон по-умолчанию.<br/>", Common.ExMessage(ex));
            }

            timeStop1 = DateTime.Now;
            timerString.AppendFormat("Итого: {0} ms;<br/>", (timeStop1 - timeStart1).TotalMilliseconds);

            if (verbose)
                HTMLContent += "<hr width=\"100%\" size=1 align=left>" + timerString.ToString();

        }

        public WebBrowserPage(string content, WebBrowserEventDelegate handler)
            : this(content)
        {
            Handler = handler;
        }

        public virtual void DoClick(object sender, WebBrowserClickEventArgs e)
        {

        }
    }
}
