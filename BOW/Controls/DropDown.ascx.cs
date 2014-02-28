using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;

namespace BOW
{
    [ParseChildren(true)]
    [Themeable(true)]
    public partial class DropDown : WebControl //System.Web.UI.UserControl, IPostBackDataHandler, INamingContainer
    {
        protected ListItemCollection _items;
        protected Label ClientScriptLabel;

        private bool requiresDataBind = true;

        #region  Public properties
        [PersistenceMode(PersistenceMode.InnerProperty)]
        public ListItemCollection Items
        {
            get
            {
                if (_items == null)
                    _items = new ListItemCollection();
                return _items;
            }
            set
            {
                _items = value;
            }
        }

        public ListItem SelectedItem
        {
            get
            {
                foreach (ListItem item in Items)
                {
                    if (item.Selected)
                        return item;
                }
                if (Items.Count > 0)
                {
                    Items[0].Selected = true;
                    return Items[0];
                }
                return null;
            }
        }
        public string SelectedValue
        {
            get
            {
                return (SelectedItem == null) ? string.Empty : SelectedItem.Value;
            }
            set
            {
                EnsureChildControls();

                bool selected = false;
                foreach (ListItem item in Items)
                {
                    item.Selected = false;
                    if (item.Value == value && !selected)
                    {
                        item.Selected = true;
                        selected = true;
                    }
                }
            }
        }
        public string DataSourceID
        {
            get { return (string)ViewState["DataSourceID"]; }
            set { ViewState["DataSourceID"] = value; }
        }

        public string DataTextField
        {
            get { return (string)ViewState["DataTextField"]; }
            set { ViewState["DataTextField"] = value; }
        }
        public string DataValueField
        {
            get { return (string)ViewState["DataValueField"]; }
            set { ViewState["DataValueField"] = value; }
        }

        public string OnClientChange
        {
            get { return (string)ViewState["OnClientChange"]; }
            set { ViewState["OnClientChange"] = value; }
        }

        #endregion
        protected override void CreateChildControls()
        {
            base.CreateChildControls();

            DataBind();

            string text = "";
            string script = "<script type=\"text/javascript\">";
            foreach (ListItem item in Items)
            {
                WebControl obj = new WebControl(HtmlTextWriterTag.Div);
                if (ItemContainer == null)
                    ItemContainer = new Panel();
                ItemContainer.Controls.Add(obj);

                obj.Attributes.Add("ItemValue", item.Value);
                obj.Attributes.Add("ItemText", item.Text);
                obj.ToolTip = item.Text;

                foreach (string attributeKey in item.Attributes.Keys)
                {
                    obj.Attributes.Add(attributeKey, item.Attributes[attributeKey]);
                }

                obj.Attributes.Add("onclick", string.Format("updateText1('{0}', '{1}', '{2}', '{3}', '{4}');", TextControl.ClientID, obj.ClientID, this.ClientID, ItemContainer.ClientID, OnClientChange));

                //if (BrowserIsIE6()) // TM: IE6 doesn't support :hover selector. Fixing it with javascript
                //{
                //    obj.Attributes.Add("onmouseover", "this.className = 'dropDownItemHover';");
                //    obj.Attributes.Add("onmouseover", "this.className = 'dropDownItem';");
                //}

                obj.CssClass = "dropDownItem";
                obj.Controls.Add(new LiteralControl(item.Text));
            }
            script += string.Format("registerPageEvents1('{0}', '{1}', '{2}', '{3}');", ItemContainer.ClientID, TextControl.ClientID, DropImage.ClientID, this.ClientID);
            script += "</script>";

            ClientScriptLabel.Text = script;

            ItemContainer.Style.Add("display", "none");
            TextControl.Text = text.TrimEnd(", ".ToCharArray());
            ContainerTable.Width = Width.IsEmpty ? Unit.Percentage(100) : Width;

            this.Page.RegisterRequiresPostBack(this);
        }


        public override void DataBind()
        {
            base.DataBind();

            InternalDataBind();
        }

        public void Rebind()
        {
            requiresDataBind = true;
            //InternalDataBind();
            EnsureChildControls();
        }
        private void InternalDataBind()
        {
            if (!requiresDataBind)
                return;

            object dataSource = string.IsNullOrEmpty(DataSourceID) ? null : SearchControl(this.Page, DataSourceID);

            if (dataSource == null)
            {
                DataSourceEventArgs args = new DataSourceEventArgs(dataSource);
                OnNeedDataSource(args);
                dataSource = args.DataSource;
            }

            if (dataSource != null)
            {
                if (dataSource is System.Web.UI.WebControls.ObjectDataSource)
                {
                    System.Web.UI.WebControls.ObjectDataSource objectDataSource = dataSource as System.Web.UI.WebControls.ObjectDataSource;
                    DataBindToIEnumerable(objectDataSource.Select());
                }
                else if (dataSource is IEnumerable)
                {
                    DataBindToIEnumerable(dataSource as IEnumerable);
                }
            }

            requiresDataBind = false;
        }
        public static Control SearchControl(Control control, string controlId)
        {
            Control control2 = control.FindControl(controlId);

            if (control2 != null)
                return control2;

            foreach (Control control3 in control.Controls)
            {
                Control control4 = SearchControl(control3, controlId);

                if (control4 != null)
                    return control4;
            }

            return null;
        }
        private object ResolvePropertyValue(string propertyName, object obj)
        {
            return obj.GetType().GetProperty(propertyName).GetValue(obj, null);
        }
        private void DataBindToIEnumerable(IEnumerable iEnumerable)
        {
            System.Collections.IEnumerator en = iEnumerable.GetEnumerator();
            while (en.MoveNext())
            {
                object obj = en.Current;
                ListItem item = new ListItem();
                Items.Add(item);

                object propertyText = ResolvePropertyValue(DataTextField, obj);
                object propertyValue = ResolvePropertyValue(DataValueField, obj);

                item.Text = (propertyText == null) ? string.Empty : propertyText.ToString();
                item.Value = (propertyValue == null) ? string.Empty : propertyValue.ToString();

                if (ItemDataBound != null)
                    ItemDataBound(this, new DropDownEventArgs(item, obj));
            }
        }

        private void OnNeedDataSource(DataSourceEventArgs args)
        {
            if (NeedDataSource != null)
                NeedDataSource(this, args);
        }

        protected override void Render(HtmlTextWriter writer)
        {
            TextControl.Text = SelectedItem.Text;
            writer.Write(string.Format("<input type='hidden' id='{0}' name='{1}' value='{2}' />", this.ClientID, this.UniqueID, SelectedItem.Value));
            base.Render(writer);
        }
        #region Public Events

        public event DropDownEventHandler ItemDataBound;
        public event EventHandler<DataSourceEventArgs> NeedDataSource;

        #endregion
        #region IPostBackDataHandler Members
        public bool LoadPostData(string postDataKey, System.Collections.Specialized.NameValueCollection postCollection)
        {
            EnsureChildControls();

            string value = postCollection[this.UniqueID];
            SelectedValue = value;
            return false;
        }
        public void RaisePostDataChangedEvent()
        {

        }

        #endregion
    }

    public class DataSourceEventArgs : EventArgs
    {
        private object _dataSource;

        public DataSourceEventArgs(object dataSource)
        {
            _dataSource = dataSource;
        }

        public object DataSource
        {
            get { return _dataSource; }
            set { _dataSource = value; }
        }
    }

    public delegate void DropDownEventHandler(object sender, DropDownEventArgs e);

    public class DropDownEventArgs : EventArgs
    {
        private object _item;
        private ListItem _cItem;

        public DropDownEventArgs()
        {
        }

        public DropDownEventArgs(ListItem item, object dataItem)
        {
            _cItem = item;
            _item = dataItem;
        }

        public object DataObject
        {
            get { return _item; }
            set { _item = value; }
        }

        public ListItem ListItem
        {
            get { return _cItem; }
            set { _cItem = value; }
        }
    }
}