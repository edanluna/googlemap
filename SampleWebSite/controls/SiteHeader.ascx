<%@ Control Language="C#" AutoEventWireup="false" CodeFile="SiteHeader.ascx.cs" Inherits="controls_SiteHeader" %>
<%@ OutputCache Duration="1800" VaryByParam="none" %>
<div id="header">
    <div id="header_box">
        <div id="header_menu">
            <asp:HyperLink ID="HyperLink1" runat="server" NavigateUrl="http://googlemap.artembg.com/">Home</asp:HyperLink>
            |
            <asp:HyperLink ID="HyperLink2" runat="server" NavigateUrl="http://www.codeplex.com/googlemap/">Project Home</asp:HyperLink>
            |
            <asp:HyperLink ID="HyperLink3" runat="server" NavigateUrl="http://artembg.com/contacts">Contact</asp:HyperLink>
        </div>
    </div>
</div>
