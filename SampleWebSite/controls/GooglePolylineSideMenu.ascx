<%@ Control Language="C#" AutoEventWireup="false" CodeFile="GooglePolylineSideMenu.ascx.cs" Inherits="controls_GooglePolylineSideMenu" %>
<%@ OutputCache Duration="1800" VaryByParam="none" %>
<div class="sidebar_box">
    <h3 style="margin-bottom: 5px;">
        GooglePolylines</h3>
    <asp:Repeater ID="Repeater4" runat="server" DataSourceID="SiteMapDataSource4" EnableViewState="false">
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
    <asp:SiteMapDataSource ID="SiteMapDataSource4" runat="server" SiteMapProvider="GooglePolylineSamplesSiteMap"
        ShowStartingNode="false" />
</div>
