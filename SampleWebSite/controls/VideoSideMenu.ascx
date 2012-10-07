<%@ Control Language="C#" AutoEventWireup="false" CodeFile="VideoSideMenu.ascx.cs" Inherits="controls_VideoSideMenu" %>
<%@ OutputCache Duration="1800" VaryByParam="none" %>
<div class="sidebar_box">
    <h3 style="margin-bottom: 5px;">
        Videos</h3>
    <asp:Repeater ID="_rptVideo" runat="server" DataSourceID="VideosSiteMap" EnableViewState="false">
        <HeaderTemplate>
            <ul>
        </HeaderTemplate>
        <FooterTemplate>
            </ul>
        </FooterTemplate>
        <ItemTemplate>
            <li>
                <asp:HyperLink ID="HyperLink1" runat="server" NavigateUrl='<%# Eval("Url") %>' EnableViewState="false">&raquo; <%# Eval("Title") %></asp:HyperLink>
            </li>
        </ItemTemplate>
    </asp:Repeater>
    <asp:SiteMapDataSource ID="VideosSiteMap" runat="server" SiteMapProvider="VideosSiteMap" ShowStartingNode="false" />
</div>
