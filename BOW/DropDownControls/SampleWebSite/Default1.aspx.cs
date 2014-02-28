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
using TeaHousePeople.Samples.DataObjects;
using System.Collections.Generic;

namespace TeaHousePeople.Samples.Web
{
    public partial class Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            
        }

        protected void DropDown3_NeedDataSource(object sender, TeaHousePeople.Samples.Web.Controls.DataSourceEventArgs e)
        {
            // This event handler is a good place to add Items manually without binding them to objects
            DropDown3.Items.Add(new ListItem("Listitem 1", "L1"));
            DropDown3.Items.Add(new ListItem("Listitem 2", "L2"));

            // Attach IEnumerable to the event argument to bind more objects
            IList<Author> authorsDataSource = new List<Author>();
            authorsDataSource.Add(new Author(0, "Author 1"));
            authorsDataSource.Add(new Author(1, "Author 2"));

            e.DataSource = authorsDataSource;
 
        }

        protected void MultiselectDropDown3_NeedDataSource(object sender, TeaHousePeople.Samples.Web.Controls.DataSourceEventArgs e)
        {
            // This event handler is a good place to add Items manually without binding them to objects
            MultiselectDropDown3.Items.Add(new ListItem("Listitem 1", "L1"));
            MultiselectDropDown3.Items.Add(new ListItem("Listitem 2", "L2"));

            // Attach IEnumerable to the event argument to bind more objects
            IList<Author> authorsDataSource = new List<Author>();
            authorsDataSource.Add(new Author(0, "Author 1"));
            authorsDataSource.Add(new Author(1, "Author 2"));

            e.DataSource = authorsDataSource;

        }
    }
}
