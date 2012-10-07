<%@ Page Title="" Language="C#" MasterPageFile="~/GoogleMap.master" AutoEventWireup="true"
    CodeFile="InfoContent.aspx.cs" Inherits="marker_InfoContent" %>

<asp:Content ID="Content1" ContentPlaceHolderID="phContent" runat="Server">
    <h1>
        GoogleMarker - InfoContent
    </h1>
    <artem:GoogleMap ID="GoogleMap1" runat="server" Latitude="42.1229" Longitude="24.7879"
        EnableScrollWheelZoom="true">
        <Markers>
            <artem:GoogleMarker Address="Plovdiv Bulgaria" Text="This is a <b>real</b> simple info window test.">
                <InfoWindowTemplate>
                    <h1>
                        InfoWindowTemplate</h1>
                    <p>
                        This is an InfoWindowTemplate sample.
                    </p>
                </InfoWindowTemplate>
            </artem:GoogleMarker>
        </Markers>
    </artem:GoogleMap>
    <asp:Button ID="btnPost" runat="server" Text="Post" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="phDescription" runat="Server">
    This is a sample of the InfoContent usage for GoogleMaker InfoWindowContent, where
    you can add any kind of controls to its Controls collection.
</asp:Content>
