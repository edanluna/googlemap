<%@ Page Language="C#" MasterPageFile="~/GoogleMap.master" AutoEventWireup="true"
    CodeFile="ServerGeocoding.aspx.cs" Inherits="marker_ServerGeocoding" %>

<asp:Content ID="Content1" ContentPlaceHolderID="phContent" runat="Server">
    <h1>
        GoogleMarker - Server Geocoding
    </h1>
    <artem:GoogleMap ID="GoogleMap1" runat="server" Latitude="42.1229" Longitude="24.7879"
        EnableScrollWheelZoom="true" OnClick="HandleAny">
        <MarkerEvents OnGeoLocationLoaded="HandleAny" />
    </artem:GoogleMap>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="phDescription" runat="Server">
</asp:Content>
