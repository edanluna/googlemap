<%@ Page Language="C#" MasterPageFile="~/GoogleMap.master" AutoEventWireup="false" CodeFile="ListDataBound.aspx.cs"
    Inherits="examples_ListDataBound" CodeFileBaseClass="Artem.GoogleMap.WebSite.UI.PageBase" Title="GoogleMap DataList DataBound Sample"
    Description="GoogleMap Control - DataList DataBound Sample" MetaKeywords="software, freeware, open source, google maps api, googlemap control, capture click sample, asp.net custom control" %>

<asp:Content ContentPlaceHolderID="phContent" ID="cntContent" runat="server">
    <asp:DataList ID="dlSample" runat="server" OnItemDataBound="HandleItemDataBound" 
        DataSourceID="odsSample">
        <ItemTemplate>
            <artem:GoogleMap ID="crtGooleMap" runat="server" Width="640px" Height="600px" >
            </artem:GoogleMap>
        </ItemTemplate>
    </asp:DataList>
    <asp:ObjectDataSource ID="odsSample" runat="server" OldValuesParameterFormatString="original_{0}" 
        SelectMethod="GetMapData" TypeName="Artem.GoogleMap.WebSite.DataSourceHelper">
    </asp:ObjectDataSource>
</asp:Content>
