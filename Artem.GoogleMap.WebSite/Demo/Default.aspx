<%@ Page Language="C#" AutoEventWireup="false" CodeBehind="Default.aspx.cs" Inherits="Artem.Google.Web.Demo.DefaultPage"
    MasterPageFile="~/Demo/Site.Master" %>

<asp:Content ID="MainContent" ContentPlaceHolderID="main" runat="server">
    <h1>
        BASI</h1>
    <asp:Literal ID="ltrDate" runat="server"></asp:Literal>
    <asp:Button runat="server" Text="Test" OnClick="HandleTestClick" />
</asp:Content>
