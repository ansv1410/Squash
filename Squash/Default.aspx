﻿<%@ Page Title="Hem" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="Squash._Default" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <div id="startPageDiv" class="pageDiv">
        <%--<h2><%: Title %></h2>--%>

        <div class="row">

            <%-- MEDDELANDEDIV: Hela bredden överst centrerat. --%>
            <div class="col-md-10" id="messagesDiv" runat="server" visible="false"> 
                <%--<h2>Meddelanden</h2>--%>
                <%--<p>Du har bokat en tid Fredag 2016-04-22 kl 17:00.</p>--%>
            </div>
            <%-- PRESENTATIONSDIV: Ligger till vänster om nyhetsdiv och under meddelandediv. Smal. --%>
            <div class="col-md-4" id="presentationDiv" runat="server" visible="false">
                <h2>Välkommen!</h2>
                <p class="foldedText">Östersunds Squashförening bildades 2004 och driver idag squashhallen på Storsjöstråket med två banor, omklädningsrum, dusch och bastu.</p>
                <p class="foldedText">Banbokning görs enkelt här på hemsidan när du <a class="redirectLinks" href="Account/Register.aspx" title="Till sidan för att bli medlem">registrerat</a> dig och godkänts som medlem, också det via hemsidan.</p>
                <p class="foldedText">Tillsvidare kan du läsa mer om oss och om squash via våra sidor på sociala medier:</p>
                <div class="socialMediaIcon"><a href="https://www.facebook.com/pages/Squashhallen-%C3%96stersund/173142666069296" title="Till Facebooksida"><img runat="server" src="~\Images\FB.svg" /></a></div>
                <div class="socialMediaIcon"><a href="https://plus.google.com/+OstersundssquashSe" title="Till Google+sida"><img runat="server" src="~\Images\Gplus.svg" /></a></div>
            </div>

            <%-- NYHETSDIV: Till Höger om presentationsdiv och under meddelandediv. Bred--%>
            <div class="col-md-7" id="newsDiv" runat="server" visible="false">
                <%--<h2>Nyheter</h2>
                <p>Pga snöstorm nu så är det dags att ta sig till squashhallen och lira lite.</p>
                <p>Det finns kaffe i receptionen!</p>--%>
            </div>
        </div>
    </div>
</asp:Content>
