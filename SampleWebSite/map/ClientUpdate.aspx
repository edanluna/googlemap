<%@ Page Language="C#" MasterPageFile="~/GoogleMap.master" AutoEventWireup="false" CodeFile="ClientUpdate.aspx.cs"
    Inherits="map_ClientUpdate" CodeFileBaseClass="Artem.GoogleMap.WebSite.UI.PageBase" 
    Title="GoogleMap Client Update Sample"
    Description="GoogleMap Control - Client Update Sample" 
    MetaKeywords="software, freeware, open source, google maps api, googlemap control, client-side update, asp.net custom control" %>

<asp:Content ID="Content1" ContentPlaceHolderID="phContent" runat="Server">
    <h1>
        GoogleMap Client Update Sample</h1>
    <p style="padding-bottom: 10px;">
        This is a sample of how to do updates on the client-side of GoogleMap control without refreshing(reloading)
        the Google map.<br />
        The site will be automatically update with new markers from server-side code every 5 seconds.
    </p>
    <artem:GoogleMap ID="GoogleMap1" runat="server" Width="640px" Height="600px" Latitude="42.1229" Longitude="24.7879"
        Zoom="4" BorderStyle="Solid" BorderColor="#999999" BorderWidth="1">
    </artem:GoogleMap>
    <asp:UpdatePanel runat="server">
        <ContentTemplate>
            <asp:Literal ID="_ltrScript" runat="server"></asp:Literal>
            <asp:Literal ID="_ltrCounter" runat="server" Text="Counter"></asp:Literal><br />
            <asp:Timer runat="server" Interval="5000">
            </asp:Timer>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
