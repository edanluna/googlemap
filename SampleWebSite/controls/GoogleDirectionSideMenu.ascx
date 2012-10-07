<%@ Control Language="C#" AutoEventWireup="false" CodeFile="GoogleDirectionSideMenu.ascx.cs" Inherits="controls_GoogleDirectionSideMenu" %>
<%@ OutputCache Duration="1800" VaryByParam="none" %>
<div class="sidebar_box">
    <h3 style="margin-bottom: 5px;">
        GoogleDirections</h3>
    <asp:Repeater ID="Repeater2" runat="server" DataSourceID="SiteMapDataSource2" EnableViewState="false">
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
    <asp:SiteMapDataSource ID="SiteMapDataSource2" runat="server" SiteMapProvider="GoogleDirectionSamplesSiteMap"
        ShowStartingNode="false" />
</div>
