<%@ Control Language="C#" AutoEventWireup="false" CodeFile="GoogleAnalytics.ascx.cs" Inherits="controls_GoogleAnalytics" %>
<%@ OutputCache Duration="1800" VaryByParam="none" %>
<%-- >> Google Analytics --%>
<script type="text/javascript">
        var gaJsHost = (("https:" == document.location.protocol) ? "https://ssl." : "http://www.");
        document.write(unescape("%3Cscript src='" + gaJsHost + "google-analytics.com/ga.js' type='text/javascript'%3E%3C/script%3E"));
</script>
<%-- << Google Analytics --%>
