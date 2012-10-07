<%@ Page Language="C#" MasterPageFile="~/GoogleMap.master" AutoEventWireup="true"
    CodeFile="MarkerStyle.aspx.cs" Inherits="marker_MarkerStyle" %>

<asp:Content ID="Content1" ContentPlaceHolderID="phContent" runat="Server">
    <h1>
        GoogleMarker - MarkerStyle</h1>
    <artem:GoogleMap ID="GoogleMap1" runat="server" Latitude="42.1229" Longitude="24.7879"
        EnableScrollWheelZoom="true">
        <Markers>
            <artem:GoogleMarker Address="sofia bulgaria" Text="Sofia Bulgaria">
            </artem:GoogleMarker>
            <artem:GoogleMarker Address="athens greece" Text="Athens Greece">
            </artem:GoogleMarker>
        </Markers>
        <MarkerStyle IconUrl="~/images/arrow24.gif" IconSize="24,24" IconAnchor="12,24" Title="Click on the marker"/>
    </artem:GoogleMap>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="phDescription" runat="Server">
    This is a sample of the MarkerStyle usage GoogleMap Control markers.
</asp:Content>
