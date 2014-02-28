using System;
using System.Collections;
using System.ComponentModel;
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
    /// <summary>
    ///	This control act as a Dropdown with multi select option.
    ///	User can select multiple items from the drop down and the selected
    ///	items will be displayed in comma separated format.
    ///	Also it provides the tooltip for the selected items and
    ///	user can easily find the items selected without click on the dropdown.
    ///	
    ///	User can change the width of the Dropdown with 'ListWidth' property
    /// </summary>
    /// 
    [ToolboxData("<{0}:MultiselectDropDown runat=server></{0}:MultiselectDropDown>")]
    [ParseChildren(true)]
    [Themeable(true)]
    public partial class MultiselectDropDown : WebControl //System.Web.UI.UserControl, IPostBackDataHandler, INamingContainer
    {
        protected ListItemCollection _items;
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
                ItemContainer.Controls.Add(obj);

                CheckBox check = new CheckBox();
                obj.Controls.Add(check);

                check.Checked = item.Selected;
                check.Enabled = item.Enabled;
                check.Text = item.Text;
                check.Attributes.Add("ItemValue", item.Value);
                check.Attributes.Add("onclick", string.Format("updateText('{0}', '{1}', '{2}');", TextControl.ClientID, check.ClientID, this.ClientID));

                script += string.Format("registerCheckItem('{0}', '{1}', '{2}', '{3}');", TextControl.ClientID, check.ClientID, item.Text, item.Value);

                if (item.Selected)
                {
                    text += item.Text + ", ";
                }
            }
            script += string.Format("registerPageEvents('{0}', '{1}', '{2}');", ItemContainer.ClientID, TextControl.ClientID, DropImage.ClientID);
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
        private void OnNeedDataSource(DataSourceEventArgs args)
        {
            if (NeedDataSource != null)
                NeedDataSource(this, args);
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
        private object ResolvePropertyValue(string propertyName, object obj)
        {
            return obj.GetType().GetProperty(propertyName).GetValue(obj, null);
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
        #region Public Events

        public event DropDownEventHandler ItemDataBound;
        public event EventHandler<DataSourceEventArgs> NeedDataSource;

        #endregion
        #region Public methods

        #endregion

        protected override void Render(HtmlTextWriter writer)
        {
            string text = "";
            foreach (ListItem item in Items)
            {
                if (item.Selected)
                {
                    text += item.Text + ", ";
                }
            }
            TextControl.Text = text.TrimEnd(", ".ToCharArray());

            base.Render(writer);

            writer.Write(string.Format("<input type='hidden' id='{0}' name='{1}' value='{2}' />", this.ClientID, this.UniqueID, GetHiddenFieldValue()));
        }

        private string GetHiddenFieldValue()
        {
            string s = "";
            foreach (ListItem item in Items)
            {
                if (item.Selected)
                {
                    s += item.Value + "|";
                }
            }
            if (s.Length > 0)
            {
                s = s.Substring(0, s.Length - 1);
            }
            return s;
        }
        #region IPostBackDataHandler Members

        public bool LoadPostData(string postDataKey, System.Collections.Specialized.NameValueCollection postCollection)
        {
            string value = postCollection[this.UniqueID];
            string[] selectedValues = value.Split('|');
            foreach (ListItem item in Items)
            {
                foreach (string selValue in selectedValues)
                {
                    item.Selected = false;
                    if (item.Value == selValue)
                    {
                        item.Selected = true;
                        break;
                    }
                }
            }
            return false;
        }

        public void RaisePostDataChangedEvent()
        {

        }

        #endregion
    }
}