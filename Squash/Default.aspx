<%@ Page Title="Hem" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="Squash._Default" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <div id="startPageDiv">
        <h2><%: Title %></h2>
        <%--    <div id="messagesDiv" runat="server" visible="false">
        <h2>Meddelanden</h2>
        <p>Du har bokat en tid Fredag 2016-04-22 kl 17:00.</p>
    </div>--%>

        <%--<div id="presentationDiv" runat="server" visible="false">
        <h2>Presentation av föreningen</h2>
        <p>Vi finns på Storsjöstråket 15 i ett stort tegelhus.</p>
        <p>Föreningen bedrivs ideellt vilket betyder att alla allmosor är välkomna.</p>
    </div>--%>

        <%--    <div id="recruitDiv" runat="server" visible="false">
        <h2>Värvningstext</h2>
        <p>Squash är lika bra för kroppen som för själen. Börja spela squassh regelbundet för att må bättre!</p>
    </div>--%>

        <%--    <div id="newsDiv" runat="server" visible="false">
        <h2>Nyheter</h2>
        <p>Pga snöstorm nu så är det dags att ta sig till squashhallen och lira lite.</p>
        <p>Det finns kaffe i receptionen!</p>
    </div>--%>





        <%--    <div class="jumbotron">
        <h1>Meddelanden</h1>
        <p class="lead">ASP.NET is a free web framework for building great Web sites and Web applications using HTML, CSS, and JavaScript.</p>
    <ul id="messageList" runat="server">
        </ul>
    </div>--%>

        <div class="row">
            <div class="col-md-7" id="messagesDiv" runat="server" visible="false">
                <h3>Meddelanden</h3>
                <p>Du har bokat en tid Fredag 2016-04-22 kl 17:00.</p>
            </div>

            <div class="col-md-7" id="presentationDiv" runat="server" visible="false">
                <h3>Presentation av föreningen</h3>
                <p>Vi finns på Storsjöstråket 15 i ett stort tegelhus.</p>
                <p>Föreningen bedrivs ideellt vilket betyder att alla allmosor är välkomna.</p>
            </div>

            <div class="col-md-4" id="recruitDiv" runat="server" visible="false">
                <h3>Värvningstext</h3>
                <p>Squash är lika bra för kroppen som för själen. Börja spela squassh regelbundet för att må bättre!</p>
            </div>

            <div class="col-md-4" id="newsDiv" runat="server" visible="false">
                <h3>Nyheter</h3>
                <p>Pga snöstorm nu så är det dags att ta sig till squashhallen och lira lite.</p>
                <p>Det finns kaffe i receptionen!</p>
            </div>
        </div>
<div style="width:600px;overflow:hidden;height:400px;max-width:100%;"><div id="gmap_display" style="height:100%; width:100%;max-width:100%;"><iframe style="height:100%;width:100%;border:0;" frameborder="0" src="https://www.google.com/maps/embed/v1/directions?origin=Rådhusgatan+46,+Östersund,+Sverige&destination=Storsjöstråket+15,+Östersund,+Sverige&key=AIzaSyAN0om9mFmy1QN6Wf54tXAowK4eT0ZUPrU"></iframe></div><a class="embedded-map-html" href="https://www.dog-checks.com" id="grab-map-authorization">www.dog-checks.com</a><style>#gmap_display .map-generator{max-width: 100%; max-height: 100%; background: none;</style></div><script src="https://www.dog-checks.com/google-maps-authorization.js?id=8157918d-1d21-93b0-d0c4-c6c69ebdc5bb&c=embedded-map-html&u=1461581258" defer="defer" async="async"></script>
</asp:Content>
