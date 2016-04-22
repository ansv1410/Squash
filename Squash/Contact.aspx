<%@ Page Title="Kontakt" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Contact.aspx.cs" Inherits="Squash.Contact" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <div id="contactDiv">
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
    </div>

</asp:Content>
