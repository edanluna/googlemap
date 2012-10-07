<%@ Page Language="C#" MasterPageFile="~/GoogleMap.master" AutoEventWireup="true"
    CodeFile="Test.aspx.cs" Inherits="Test" %>

<asp:Content ID="Content1" ContentPlaceHolderID="phContent" runat="Server">
    <div>
        <h1>
            GoogleMap Test
        </h1>
        <asp:UpdatePanel ID="up" runat="server">
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="Button1"  EventName="Click"/>
            </Triggers>
            <ContentTemplate>
                <artem:GoogleMap ID="GoogleMap1" runat="server" Address="1 bushwick new york" Zoom="14"
                    EnableScrollWheelZoom="true" OnZoomEnd="HandleZoomEnd">
                    <Markers>
                        <artem:GoogleMarker Address="1 bushwick new york" Draggable="true" Text="1 bushwick new york" />
                        <artem:GoogleMarker Address="2 bushwick new york" Draggable="true" Text="2 bushwick new york" />
                    </Markers>
                </artem:GoogleMap>
            </ContentTemplate>
        </asp:UpdatePanel>
        <asp:Button ID="Button1" runat="server" Text="PostBack" />
    </div>
</asp:Content>
