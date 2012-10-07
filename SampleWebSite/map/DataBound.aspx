<%@ Page Language="C#" MasterPageFile="~/GoogleMap.master" AutoEventWireup="false" CodeFile="DataBound.aspx.cs"
    Inherits="map_DataBound" CodeFileBaseClass="Artem.GoogleMap.WebSite.UI.PageBase" 
    Title="GoogleMap DataBound Sample"
    MetaDescription="GoogleMap Control - DataBound Sample" 
    MetaKeywords="software, freeware, open source, google maps api, googlemap control, data bound, asp.net custom control" %>

<asp:Content ID="Content1" ContentPlaceHolderID="phContent" runat="Server">
    <h1>
        GoogleMap DataBound Sample
    </h1>
    <p style="padding-bottom: 10px;">
        This sample shows a case when GoogleMap control is inside a data bound control and some of its properties
        could be bind to data source properties/fields.
    </p>
    <asp:FormView ID="_fvGooleDataBound" runat="server" DataSourceID="_odsGoogleMap" DefaultMode="ReadOnly">
        <ItemTemplate>
            <artem:GoogleMap ID="GoogleMap1" runat="server" Width="640px" Height="600px" Latitude='<%# Eval("Latitude") %>'
                Longitude='<%# Eval("Longitude") %>' Zoom='<%# Eval("Zoom") %>' BorderStyle="Solid" BorderColor="#999999" BorderWidth="1">
            </artem:GoogleMap>
        </ItemTemplate>
    </asp:FormView>
    <asp:ObjectDataSource ID="_odsGoogleMap" runat="server" OldValuesParameterFormatString="{0}" SelectMethod="GetMapData"
        TypeName="Artem.GoogleMap.WebSite.DataSourceHelper"></asp:ObjectDataSource>
</asp:Content>
