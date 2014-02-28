<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="BOW.Default"
    Theme="Default" %>

<%--<%@ Register Src="../Controls/DropDown.ascx" TagName="DropDown" TagPrefix="thp" %>
<%@ Register Src="Controls/MultiselectDropDown.ascx" TagName="MultiselectDropDown"--%>
    TagPrefix="thp" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Untitled Page</title>
</head>
<body>
    <form id="form1" runat="server">

    <script type="text/javascript">
        var Page = {};
        
        Page.attachEvent = function(element, name, handler)
        {
            if(element.attachEvent)
            {
                element.attachEvent("on" + name, handler);
            }
            else if(element.addEventListener)
            {
                element.addEventListener(name, handler, false);
            }
        };

    </script>

    <asp:ObjectDataSource ID="DataSource1" runat="server" DataObjectTypeName="TeaHousePeople.Samples.DataObjects.Author"
        TypeName="TeaHousePeople.Samples.DataObjects.DataService" SelectMethod="GetAuthors" />
    <p>
        <b>Example 1</b><br />
        Poplated using ListItem in .aspx file.
        <thp:DropDown ID="DropDown1" runat="server" Width="200">
            <Items>
                <asp:ListItem Text="Some text" Value="0" Selected="True" />
                <asp:ListItem Text="Another option" Value="1" />
            </Items>
        </thp:DropDown>
    </p>
    <p>
        <b>Example 2</b><br />
        Using ObjectDataSource and binding the control to an object array.
        <thp:DropDown ID="DropDown2" runat="server" Width="200" DataSourceID="DataSource1"
            DataTextField="Name" DataValueField="Id" />
    </p>
    <p>
        <b>Example 3</b><br />
        Attaching custom DataSource and adding ListItems in the NeedDataSource event.<br />
        Client event: <span id="div1" style="color: Red"></span>
        <thp:DropDown ID="DropDown3" runat="server" Width="200" OnNeedDataSource="DropDown3_NeedDataSource"
            DataTextField="Name" DataValueField="Id" OnClientChange="onDropDownChange" />
    </p>

    <script type="text/javascript">
        function onDropDownChange(arg)
        {
            document.getElementById("div1").innerHTML = arg.text + " (" + arg.value + ") selected.";
        }
    </script>

    <br />
    <br />
    <br />
    <p>
        <b>Multiselect Example 1</b><br />
        Poplated using ListItem in .aspx file.
        <thp:MultiselectDropDown ID="MultiselectDropDown1" runat="server" Width="200">
            <Items>
                <asp:ListItem Text="Item 1" Value="0" Selected="True" />
                <asp:ListItem Text="Item 2" Value="1" />
                <asp:ListItem Text="Item 3" Value="2" Selected="True" />
                <asp:ListItem Text="Item 4" Value="3" />
            </Items>
        </thp:MultiselectDropDown>
    </p>
    <p>
        <b>Multiselect Example 2</b><br />
        Using DataSource to bind to objects.
        <thp:MultiselectDropDown ID="MultiselectDropDown2" runat="server" Width="300" DataSourceID="DataSource1"
            DataTextField="Name" DataValueField="Id" />
    </p>
    <p>
        <b>Example 3</b><br />
        Attaching custom DataSource and adding ListItems in the NeedDataSource event.<br />
        Client event: <span id="Span1" style="color: Red"></span>
        <thp:MultiselectDropDown ID="MultiselectDropDown3" runat="server" Width="200" OnNeedDataSource="MultiselectDropDown3_NeedDataSource"
            DataTextField="Name" DataValueField="Id" />
    </p>
    </form>
</body>
</html>
