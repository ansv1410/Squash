<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="EditUserInfo.ascx.cs" Inherits="Squash.Account.EditUserInfo" %>


<h4>Uppdatera dina information nedan.</h4>
<h5>Uppgifterna skickas till administratör för godkännande. Efter godkännandet får du ett e-mail och därefter är dina uppgifter uppdaterade.</h5>
<hr />

<asp:Panel ID="pnlDefaultButton" runat="server" DefaultButton="btnUpdateInfo">
    <div class="form-group">
        <asp:Label runat="server" AssociatedControlID="tbMPFirstName" CssClass="col-md-2 control-label">Förnamn</asp:Label>
        <div class="col-md-10">
            <asp:TextBox runat="server" ID="tbMPFirstName" CssClass="form-control" TextMode="SingleLine" />
            <asp:RequiredFieldValidator runat="server" ControlToValidate="tbMPFirstName" ValidationGroup="UpdateInfo"
                CssClass="text-danger" ErrorMessage="Fyll i förnamn." />
        </div>
    </div>
    <div class="form-group">
        <asp:Label runat="server" AssociatedControlID="tbMPSurName" CssClass="col-md-2 control-label">Efternamn</asp:Label>
        <div class="col-md-10">
            <asp:TextBox runat="server" ID="tbMPSurName" CssClass="form-control" TextMode="SingleLine" />
            <asp:RequiredFieldValidator runat="server" ControlToValidate="tbMPSurName" ValidationGroup="UpdateInfo"
                CssClass="text-danger" ErrorMessage="Fyll i efternamn." />
        </div>
    </div>
    <div class="form-group">
        <asp:Label runat="server" AssociatedControlID="tbMPStreetAddress" CssClass="col-md-2 control-label">Gatuadress</asp:Label>
        <div class="col-md-10">
            <asp:TextBox runat="server" ID="tbMPStreetAddress" CssClass="form-control" TextMode="SingleLine" />
            <asp:RequiredFieldValidator runat="server" ControlToValidate="tbMPStreetAddress" ValidationGroup="UpdateInfo"
                CssClass="text-danger" ErrorMessage="Fyll i gatuadress." />
        </div>
    </div>
    <div class="form-group">
        <asp:Label runat="server" AssociatedControlID="tbMPPostalCode" CssClass="col-md-2 control-label">Postnummer</asp:Label>
        <div class="col-md-10">
            <asp:TextBox runat="server" ID="tbMPPostalCode" CssClass="form-control" MaxLength="5" />
            <asp:RequiredFieldValidator runat="server" ControlToValidate="tbMPPostalCode" ValidationGroup="UpdateInfo"
                CssClass="text-danger" ErrorMessage="Fyll i postnummer." />
            <asp:RegularExpressionValidator ID="RegularExpressionValidatorPostalCode" runat="server" ControlToValidate="tbMPPostalCode" CssClass="text-danger" ErrorMessage="Endast siffror." ValidationExpression="\d+"></asp:RegularExpressionValidator>
        </div>
    </div>
    <div class="form-group">
        <asp:Label runat="server" AssociatedControlID="tbMPCity" CssClass="col-md-2 control-label">Postort</asp:Label>
        <div class="col-md-10">
            <asp:TextBox runat="server" ID="tbMPCity" CssClass="form-control" TextMode="SingleLine" />
            <asp:RequiredFieldValidator runat="server" ControlToValidate="tbMPCity" ValidationGroup="UpdateInfo"
                CssClass="text-danger" ErrorMessage="Fyll i postort." />
        </div>
    </div>
    <div class="form-group">
        <asp:Label runat="server" AssociatedControlID="tbMPTelephone" CssClass="col-md-2 control-label">Telefon</asp:Label>
        <div class="col-md-10">
            <asp:TextBox runat="server" ID="tbMPTelephone" CssClass="form-control" TextMode="SingleLine" />
            <asp:RequiredFieldValidator runat="server" ControlToValidate="tbMPTelephone" ValidationGroup="UpdateInfo"
                CssClass="text-danger" ErrorMessage="Fyll i telefon." />
        </div>
    </div>
    <%-- Inloggningskontroller --%>
    <div class="form-group">
        <asp:Label runat="server" AssociatedControlID="tbMPEmail" CssClass="col-md-2 control-label">E-post</asp:Label>
        <div class="col-md-10">
            <asp:TextBox runat="server" ID="tbMPEmail" CssClass="form-control" TextMode="Email" CausesValidation="true" />
            <asp:RequiredFieldValidator runat="server" ControlToValidate="tbMPEmail" ValidationGroup="UpdateInfo"
                CssClass="text-danger" ErrorMessage="Fyll i e-post." />
            <br />
            <asp:Label ID="lblMPMessage" runat="server" CssClass="text-danger" Visible="false"></asp:Label>
            <br />
        </div>
    </div>
    <%-- Radiobuttons för att godkänna lagring och eventuell visning i kontaktlista --%>
    <div class="form-group">
        <div class="col-md-offset-2 col-md-10">
            <asp:Label AssociatedControlID="rblMPAgreement" runat="server" Text="Jag godkänner att personuppgifter lagras och vill:"></asp:Label>
            <asp:RadioButtonList ID="rblMPAgreement" runat="server"
                RepeatDirection="Vertical" RepeatLayout="Table">
                <asp:ListItem Text=" synas i kontaktlista för andra medlemmar." Value="Agree" />
                <asp:ListItem Text=" <u><em>inte</em></u> synas i kontaktlista för andra medlemmar." Value="Disagree" />
            </asp:RadioButtonList>
            <asp:RequiredFieldValidator runat="server" ControlToValidate="rblMPAgreement" ValidationGroup="UpdateInfo"
                CssClass="text-danger" Display="Dynamic" ErrorMessage="Välj ett alternativ ovan." />
        </div>
    </div>
    <div class="form-group">
        <div class="col-md-offset-2 col-md-10">
            <asp:Button ID="btnUpdateInfo" runat="server" OnClientClick="CheckEmailExist()" OnClick="UpdateInfo_Click" Text="Uppdatera konto" CssClass="btn btn-default" ValidationGroup="UpdateInfo" />
            <br />
        </div>
    </div>
<%--    
    <script>
        function CheckEmailExist(){
            document.getElementById("MainContent_hfEmailExist").setAttribute("Value", "0");
        }

    </script>--%>


</asp:Panel>
