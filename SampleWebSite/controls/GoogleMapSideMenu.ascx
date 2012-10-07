<%@ Control Language="C#" AutoEventWireup="false" CodeFile="GoogleMapSideMenu.ascx.cs" Inherits="controls_GoogleMapSideMenu" %>
<%@ OutputCache Duration="1800" VaryByParam="none" %>
<div class="sidebar_box">
    <h3 style="margin-bottom: 5px;">
        GoogleMaps</h3>
    <asp:Repeater ID="GoogleMapSamplesRepeater" runat="server" DataSourceID="GoogleMapSamplesSiteMapDataSource"
        EnableViewState="false">
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
    <asp:SiteMapDataSource ID="GoogleMapSamplesSiteMapDataSource" runat="server" SiteMapProvider="GoogleMapSamplesSiteMap"
        ShowStartingNode="false" />
</div>
