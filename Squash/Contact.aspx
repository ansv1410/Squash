<%@ Page Title="Kontakt" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Contact.aspx.cs" Inherits="Squash.Contact" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <div id="contactDiv" class="pageDiv">
        <div class="form-horizontal">
            <h2><%: Title %>info</h2>
            <br />
            <div id="Telephone" class="contactInfoDiv">
                <h4>Telefon</h4>
                <p class="foldedText">
                    <u>Alexander Berggren</u>
                    <br />
                    070-695 22 70<br />
                    <br />

                    <u>Hans Ling</u><br />
                    070-691 43 86
                </p>
            </div>

            <div id="Email" class="contactInfoDiv">
                <h4>E-Post</h4>
                <p class="foldedText">Maila oss på: <a href="mailto:info@ostersundssquash.se">info@ostersundssquash.se</a></p>
            </div>

            <div id="Address" class="contactInfoDiv">
                <h4>Adress</h4>
                <p class="foldedText">Storsjöstråket 15 <br /> 831 34 Östersund</p>
            </div>
            <br />
            <p class="directionsText"><i>Vägbeskrivning från Rådhuset. Stanna vid parkeringen, ingången ligger i det inre hörnet av byggnaden.</i></p>
        </div>
        <div class="mapDiv">
            <div id="gmap_display">
                <iframe id="iframeMap" frameborder="0" src="https://www.google.com/maps/embed/v1/directions?origin=Rådhusgatan+46,+Östersund,+Sverige&destination=Storsjöstråket+15,+Östersund,+Sverige&key=AIzaSyAN0om9mFmy1QN6Wf54tXAowK4eT0ZUPrU"></iframe>
            </div>
        </div>
    </div>

</asp:Content>
