<%@ Page Language="C#" MasterPageFile="~/GoogleMap.master" AutoEventWireup="false" CodeFile="ClientEvents.aspx.cs"
    Inherits="polygon_ClientEvents" CodeFileBaseClass="Artem.GoogleMap.WebSite.UI.PageBase" Title="GooglePolygon ClientEvents Sample"
    MetaDescription="GoogleMap Control - GooglePolygon ClientEvents Sample" MetaKeywords="software, freeware, open source, google maps api, googlemap control, polygon events, asp.net custom control" %>

<asp:Content runat="server" ContentPlaceHolderID="phContent">

    <script type="text/javascript">
    
        function __setLocation(overlay, point) {
            var latField = document.getElementById('<%= _lat.ClientID %>');
            var lngField = document.getElementById('<%= _lng.ClientID %>');
            latField.value = point.lat();
            lngField.value = point.lng();
            <%= this.ClientScript.GetPostBackEventReference(_btnSubmit, "") %>;
        }
    </script>

    <h1>
        Polygons and Events Sample
    </h1>
    <p>
        Click on the map bellow.<br />
        After second click you will start drawing rectangles.</p>
    <hr style="visibility: hidden;" />
    <asp:UpdatePanel runat="server" ID="upTest">
        <ContentTemplate>
            <artem:GoogleMap ID="GoogleMap1" runat="server" Width="640px" Height="600px" Latitude="42.1229" Longitude="24.7879"
                Zoom="4" OnClientClick="__setLocation" BorderStyle="Solid" BorderColor="#999999" BorderWidth="1">
            </artem:GoogleMap>
            <asp:HiddenField ID="_lat" runat="server" />
            <asp:HiddenField ID="_lng" runat="server" />
            <asp:LinkButton ID="_btnSubmit" runat="server" Text=""></asp:LinkButton>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
