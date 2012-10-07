﻿<%@ Page Language="C#" MasterPageFile="~/GoogleMap.master" AutoEventWireup="false"
    CodeFile="CaptureClick.aspx.cs" Inherits="map_CaptureClick" CodeFileBaseClass="Artem.GoogleMap.WebSite.UI.PageBase"
    Title="GoogleMap Capture Click Sample" Description="GoogleMap Control - Capture Click Sample"
    MetaKeywords="software, freeware, open source, google maps api, googlemap control, capture click sample, asp.net custom control" %>

<asp:Content ID="Content1" ContentPlaceHolderID="phContent" runat="Server">
    <h1>
        GoogleMap Capture Click Sample
    </h1>
    <p style="padding-bottom: 10px;">
        Find an address and then click on the map to find position of the point.
    </p>
    <asp:UpdatePanel runat="server" ID="upTest">
        <ContentTemplate>
            Find Address
            <asp:TextBox ID="_txtAddress" runat="server"></asp:TextBox>
            <asp:Button ID="_btnShow" runat="server" Text="Show" />
            <span id="__info" style="background-color: #EEE; color: #009; padding: 0px 4px 0px 4px;
                border: solid 1px #999;">Position</span>
            <hr style="visibility: hidden;" />
            <artem:GoogleMap ID="GoogleMap1" runat="server" Width="640px" Height="600px" Latitude="42.1229"
                Longitude="24.7879" Zoom="4" OnClientClick="__showInfo">
            </artem:GoogleMap>
        </ContentTemplate>
    </asp:UpdatePanel>

    <script type="text/javascript">
        function __showInfo(overlay, point) {
            if (point) {
                var info = document.getElementById('__info');
                info.innerHTML = point.lat() + '/' + point.lng();
            }
        }
    </script>

</asp:Content>