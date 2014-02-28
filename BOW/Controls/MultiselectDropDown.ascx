<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="MultiselectDropDown.ascx.cs" Inherits="BOW.MultiselectDropDown" %>
<asp:Table ID="ContainerTable" runat="server" BorderColor="#7F9DB9" BackColor="White" BorderWidth="1" CellPadding="0" CellSpacing="0">
    <asp:TableRow Height="18">
        <asp:TableCell Width="100%" CssClass="dropTextCell">
            <asp:TextBox ID="TextControl" runat="server" ReadOnly="true" BorderWidth="0" Width="100%" />
        </asp:TableCell>
        <asp:TableCell CssClass="dropImageCell">
            <asp:Image ID="DropImage" runat="server" SkinID="DropDownImage" />
        </asp:TableCell>
    </asp:TableRow>
    <asp:TableRow>
        <asp:TableCell ColumnSpan="2">
            <asp:Panel ID="ItemContainer" runat="server" BorderColor="InactiveCaption" CssClass="dropContainer"
                BorderStyle="Solid" BorderWidth="1px" BackColor="White" />
        </asp:TableCell>
    </asp:TableRow>
</asp:Table>

<script lang="javascript" type="text/javascript">

    function registerPageEvents(itemId, textId, imgId)
    {
        var container = document.getElementById(itemId);
        var image = document.getElementById(imgId);
        
        Page.attachEvent(document, "click", function()
        {
            container.style.display = "none";
        });
        Page.attachEvent(container, "click", function(e)
        {
            var evt = e || window.event;
            evt.cancelBubble = true;
        });
        var i = itemId;
        var t = textId;
        Page.attachEvent(image, "click", function(e)
        {
            toggleItemContainer(i, t, e);
        });        
    }
    function toggleItemContainer(itemId, textId, e)
    {   
        var evt = e || window.event;
        evt.cancelBubble = true;
        
        var element = document.getElementById(itemId);
        element.style.display = (element.style.display == "") ? "none" : "";
        var textElement = document.getElementById(textId);
        element.style.width = textElement.offsetWidth + 18 + "px";
    }
    function updateText(textId, checkId, hiddenId)
    {
        var checkElement = document.getElementById(checkId);
        var textElement = document.getElementById(textId);
        var hiddenElement = document.getElementById(hiddenId);
        
        textElement.value = "";
        hiddenElement.value = "";
        
        for (var item in window[textId])
        {
            if (typeof window[textId][item] != "function")
            {
                var checkBox = document.getElementById(item);
                if (checkBox.checked)
                {
                    textElement.value += window[textId][item].text + ", ";
                    hiddenElement.value += window[textId][item].value + "|";
                }
            }
        }
        if (textElement.value.length > 0)
        {
            textElement.value = textElement.value.substr(0, textElement.value.length - 2);
        }
        if (hiddenElement.value.length > 0)
        {
            hiddenElement.value = hiddenElement.value.substr(0, hiddenElement.value.length - 1);
        }
    }
    function registerCheckItem(textId, checkId, checkText, checkValue)
    {
        if (!window[textId])
        {
            window[textId] = {};
        }
        window[textId][checkId] = { "text" : checkText, "value" : checkValue };
    }
</script>

<asp:Label ID="ClientScriptLabel" runat="server" />