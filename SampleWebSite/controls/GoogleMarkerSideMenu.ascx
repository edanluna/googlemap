<%@ Control Language="C#" AutoEventWireup="false" CodeFile="GoogleMarkerSideMenu.ascx.cs" Inherits="controls_GoogleMarkerSideMenu" %>
<%@ OutputCache Duration="1800" VaryByParam="none" %>
<div class="sidebar_box">
    <h3 style="margin-bottom: 5px;">
        GoogleMarkers</h3>
    <asp:Repeater ID="Repeater1" runat="server" DataSourceID="SiteMapDataSource1" EnableViewState="false">
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
    <asp:SiteMapDataSource ID="SiteMapDataSource1" runat="server" SiteMapProvider="GoogleMarkerSamplesSiteMap"
        ShowStartingNode="false" />
</div>
