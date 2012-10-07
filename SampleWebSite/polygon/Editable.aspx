<%@ Page Title="" Language="C#" MasterPageFile="~/GoogleMap.master" AutoEventWireup="false"
    CodeFile="Editable.aspx.cs" Inherits="polygon_Editable" %>

<asp:Content ID="cntHead" ContentPlaceHolderID="phHead" runat="server">
    <title>GooglePolygon - Editing and Drawing</title>
    <meta name="description" content="GoogleMap Control - GooglePolygon editing end drawing sample." />
    <meta name="keywords" content="asp.net artem googlemap control polygon editing drawing" />
</asp:Content>
<asp:Content ID="cntContent" ContentPlaceHolderID="phContent" runat="server">
    <h1>
        Editing and Drawing
    </h1>
    <p>
    </p>
    <artem:GoogleMap ID="GoogleMap1" runat="server" Width="640px" Height="600px" Latitude="42.1229"
        Longitude="24.7879" Zoom="4" EnableScrollWheelZoom="true">
        <Polygons>
            <artem:GooglePolygon FillColor="Red" FillOpacity=".8" StrokeColor="Blue" StrokeWeight="2"
                EnableDrawing="true" EnableEditing="true">
                <artem:GoogleLocation Latitude="37.97918" Longitude="23.716647" />
                <artem:GoogleLocation Latitude="41.036501" Longitude="28.984895" />
                <artem:GoogleLocation Latitude="44.447924" Longitude="26.097879" />
                <artem:GoogleLocation Latitude="44.802416" Longitude="20.465601" />
                <artem:GoogleLocation Latitude="42.002411" Longitude="21.436097" />
                <artem:GoogleLocation Latitude="37.97918" Longitude="23.716647" />
            </artem:GooglePolygon>
        </Polygons>
    </artem:GoogleMap>
</asp:Content>
