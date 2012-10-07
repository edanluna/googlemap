<%@ Page Language="C#" MasterPageFile="~/GoogleMap.master"
    AutoEventWireup="false" CodeFile="MarkerStyle.aspx.cs" Inherits="marker_MarkerStyle" %>

<asp:Content ContentPlaceHolderID="phHead" ID="cntHead" runat="server">
    <title>GoogleMarker - Style</title>
    <meta name="description" content="GoogleMap Control - GoogleMaprker style sample." />
    <meta name="keywords" content="asp.net artem googlemap control marker style" />
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="phContent" runat="Server">
    <h1>
        Marker Style
    </h1>
    <artem:GoogleMap ID="GoogleMap1" runat="server" Latitude="42.1229" Longitude="24.7879"
        EnableScrollWheelZoom="true">
        <markers>
            <artem:GoogleMarker Address="sofia bulgaria" Text="Sofia Bulgaria">
            </artem:GoogleMarker>
            <artem:GoogleMarker Address="athens greece" Text="Athens Greece">
            </artem:GoogleMarker>
        </markers>
        <markerstyle iconurl="~/images/arrow24.gif" iconsize="24,24" iconanchor="12,24" title="Click on the marker" />
    </artem:GoogleMap>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="phDescription" runat="Server">
    This is a sample of the MarkerStyle usage GoogleMap Control markers.
</asp:Content>
