<%@ Page Language="C#" MasterPageFile="~/GoogleMap.master" AutoEventWireup="false"
    CodeFile="CustomIcons.aspx.cs" Inherits="marker_CustomIcons" %>

<asp:Content ContentPlaceHolderID="phHead" ID="cntHead" runat="server">
    <title>GoogleMarker - Custom Icons</title>
    <meta name="description" content="GoogleMap Control markers custom icons." />
    <meta name="keywords" content="asp.net artem googlemap control marker custom icons" />
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="phContent" runat="Server">
    <h1>
        Custom Icons Sample
    </h1>
    <artem:GoogleMap ID="GoogleMap1" runat="server" Width="634px" Height="600px" Latitude="42.1229"
        Longitude="24.7879" Zoom="5" BorderStyle="Solid">
        <Markers>
            <artem:GoogleMarker Address="sofia bulgaria" IconUrl="~/images/arrow24.gif" IconSize="24,24"
                IconAnchor="12,24" Text="Sofia Bulgaria">
            </artem:GoogleMarker>
            <artem:GoogleMarker Address="athens greece" IconUrl="~/images/rabbit.jpg" IconSize="60,75"
                IconAnchor="30,75" InfoWindowAnchor="30,75" Text="Athens Greece">
            </artem:GoogleMarker>
        </Markers>
    </artem:GoogleMap>
</asp:Content>
<asp:Content ID="cntDescription" runat="server" ContentPlaceHolderID="phDescription">
</asp:Content>
