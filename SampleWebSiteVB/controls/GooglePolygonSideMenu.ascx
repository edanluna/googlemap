<%@ Control Language="C#" AutoEventWireup="false" CodeFile="GooglePolygonSideMenu.ascx.cs" Inherits="controls_GooglePolygonSideMenu" %>
<%@ OutputCache Duration="1800" VaryByParam="none" %>
<div class="sidebar_box">
    <h3 style="margin-bottom: 5px;">
        GooglePolygons</h3>
    <asp:Repeater ID="Repeater3" runat="server" DataSourceID="SiteMapDataSource3" EnableViewState="false">
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
    <asp:SiteMapDataSource ID="SiteMapDataSource3" runat="server" SiteMapProvider="GooglePolygonSamplesSiteMap"
        ShowStartingNode="false" />
</div>
