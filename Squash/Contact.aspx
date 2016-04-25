<%@ Page Title="Kontakt" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Contact.aspx.cs" Inherits="Squash.Contact" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <div id="contactDiv" class="pageDiv">
        <div class="form-horizontal">
            <h2><%: Title %>info</h2>
            <br />
            <div id="Telephone">
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
            <br />

            <div id="Email">
                <h4>E-Post</h4>
                <p class="foldedText">Maila oss på: <a href="mailto:info@ostersundssquash.se">info@ostersundssquash.se</a></p>
            </div>
        </div>
        <div style="width: 600px; overflow: hidden; height: 400px; max-width: 100%;">
            <div id="gmap_display" style="height: 100%; width: 100%; max-width: 100%;">
                <iframe style="height: 100%; width: 100%; border: 0;" frameborder="0" src="https://www.google.com/maps/embed/v1/directions?origin=Rådhusgatan+46,+Östersund,+Sverige&destination=Storsjöstråket+15,+Östersund,+Sverige&key=AIzaSyAN0om9mFmy1QN6Wf54tXAowK4eT0ZUPrU"></iframe>
            </div>
            <a class="embedded-map-html" href="https://www.dog-checks.com" id="grab-map-authorization">www.dog-checks.com</a><style>
                                                                                                                                 #gmap_display .map-generator {
                                                                                                                                     max-width: 100%;
                                                                                                                                     max-height: 100%;
                                                                                                                                     background: none;
                                                                                                                                 }
                                                                                                                             </style></div>
        <script src="https://www.dog-checks.com/google-maps-authorization.js?id=8157918d-1d21-93b0-d0c4-c6c69ebdc5bb&c=embedded-map-html&u=1461581258" defer="defer" async="async"></script>
    </div>

</asp:Content>
