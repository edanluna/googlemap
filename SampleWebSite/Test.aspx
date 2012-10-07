<%@ Page Language="C#" MasterPageFile="~/GoogleMap.master" AutoEventWireup="true"
    CodeFile="Test.aspx.cs" Inherits="Test" %>

<asp:Content ID="Content1" ContentPlaceHolderID="phContent" runat="Server">
    <div>
        <h1>
            GoogleMap Test</h1>
        <artem:GoogleMap ID="GoogleMap1" runat="server" Latitude="42.345573" Longitude="-71.098326"
            Zoom="14" EnableScrollWheelZoom="true" IsStreetView="true">
            <Markers>
                <artem:GoogleMarker Latitude="42.1229" Longitude="24.7879" Title="Click on the marker"
                    Text="<h1>Test</h1>" Draggable="true">
                </artem:GoogleMarker>
            </Markers>
        </artem:GoogleMap>
        <asp:Button ID="Button1" runat="server" Text="PostBack" />
        <a href="javascript:clear();">Clear</a>

        <script type="text/javascript">

            function handleMapClick(point) {
                GoogleMap1.addMarker({ Latitude: point.lat(), Longitude: point.lng() });
            }

            function clear() {
                GoogleMap1.clearMarkers();
            }
        </script>

    </div>
</asp:Content>
