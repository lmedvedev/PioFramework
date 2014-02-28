<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DropDown.ascx.cs" Inherits="BOW.DropDown" %>
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

<script type="text/javascript">
    function registerPageEvents1(itemId, textId, imgId, clientId)
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
            toggleItemContainer1(i, t, e);
        });        
        
        window[clientId] = { "elements" : { "text" : document.getElementById(textId), 
                                            "hidden" : document.getElementById(clientId), 
                                            "hiddenId" : clientId,
                                            "container" : container, 
                                            "image" : image }
                            };
    }
    function toggleItemContainer1(itemId, textId, e)
    {   
        var evt = e || window.event;
        evt.cancelBubble = true;
        
        var element = document.getElementById(itemId);
        element.style.display = (element.style.display == "") ? "none" : "";
        var textElement = document.getElementById(textId);
        element.style.width = textElement.offsetWidth + 18 + "px";
    }
    function updateText1(textId, itemId, hiddenId, containerId, onChangeHandler)
    {
        var itemElement = document.getElementById(itemId);
        var textElement = document.getElementById(textId);
        var hiddenElement = document.getElementById(hiddenId);
        
        textElement.value = itemElement.getAttribute("ItemText");
        hiddenElement.value = itemElement.getAttribute("ItemValue");
        
        document.getElementById(containerId).style.display = "none";
        
        if (window[onChangeHandler])
        {
            var arg = { "text" : textElement.value, "value" : hiddenElement.value, "controls" : { "item" : itemElement, "text" : textElement, "hidden" : hiddenElement } };
            window[onChangeHandler](arg);
        }
    }

    function registerCheckItem1(textId, checkId, checkText, checkValue)
    {
        if (!window[textId])
        {
            window[textId] = {};
        }
        window[textId][checkId] = { "text" : checkText, "value" : checkValue };
    }
</script>

<asp:Label ID="ClientScriptLabel" runat="server" />