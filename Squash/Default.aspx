<%@ Page Title="Hem" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="Squash._Default" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <div id="startPageDiv" class="pageDiv">
        <h2><%: Title %></h2>

        <div class="row">

            <%-- MEDDELANDEDIV: Hela bredden överst centrerat. --%>
            <div class="col-md-10" id="messagesDiv" runat="server" visible="false"> 
                <%--<h3>Meddelanden</h3>
                <p>Du har bokat en tid Fredag 2016-04-22 kl 17:00.</p>--%>
            </div>
            <%-- PRESENTATIONSDIV: Ligger till vänster om nyhetsdiv och under meddelandediv. Smal. --%>
            <div class="col-md-4" id="presentationDiv" runat="server" visible="false">
                <h3>Presentation av föreningen</h3>
                <p>Vi finns på Storsjöstråket 15 i ett stort tegelhus.</p>
                <p>Föreningen bedrivs ideellt vilket betyder att alla allmosor är välkomna.</p>
            </div>

            <%-- NYHETSDIV: Till Höger om presentationsdiv och under meddelandediv. Bred--%>
            <div class="col-md-7" id="newsDiv" runat="server" visible="false">
                <%--<h3>Nyheter</h3>
                <p>Pga snöstorm nu så är det dags att ta sig till squashhallen och lira lite.</p>
                <p>Det finns kaffe i receptionen!</p>--%>
            </div>
        </div>
    </div>
</asp:Content>
