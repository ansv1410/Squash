<%@ Page Title="Min sida" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="MyPage.aspx.cs" Inherits="Squash.Account.MyPage" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <h2><%: Title %></h2>
    <%--<p class="text-danger">
        <asp:Literal runat="server" ID="ErrorMessage" />
    </p>--%>

    <div class="form-horizontal">
        <h4>Uppdatera dina information nedan.</h4>
        <h5>Uppgifterna skickas till administratör för godkännande. Efter godkännandet får du ett e-mail och därefter är dina uppgifter uppdaterade.</h5>
        <hr />
        <%--<asp:ValidationSummary runat="server" CssClass="text-danger" />--%>

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
                <asp:TextBox runat="server" ID="tbMPEmail" CssClass="form-control" TextMode="Email" />
                <asp:RequiredFieldValidator runat="server" ControlToValidate="tbMPEmail" ValidationGroup="UpdateInfo"
                    CssClass="text-danger" ErrorMessage="Fyll i e-post." />
            </div>
        </div>
        <div class="form-group">
            <asp:Label runat="server" AssociatedControlID="tbMPPassword" CssClass="col-md-2 control-label">Lösenord</asp:Label>
            <div class="col-md-10">
                <asp:TextBox runat="server" ID="tbMPPassword" TextMode="Password" CssClass="form-control" />
                <asp:RequiredFieldValidator runat="server" ControlToValidate="tbMPPassword" ValidationGroup="UpdateInfo"
                    CssClass="text-danger" ErrorMessage="Fyll i önskat lösenord." />
                <br />
                <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" ControlToValidate="tbMPPassword" CssClass="text-danger" ErrorMessage="Ska vara mellan 6-12 tecken och minst en siffra." ValidationExpression="^(?=.*\d)(?=.*[a-zA-Z]|å|Å|ä|Ä|ö|Ö).{6,12}$"></asp:RegularExpressionValidator>
            </div>
        </div>
        <div class="form-group">
            <asp:Label runat="server" AssociatedControlID="tbMPConfirmPassword" CssClass="col-md-2 control-label">Upprepa lösenord</asp:Label>
            <div class="col-md-10">
                <asp:TextBox runat="server" ID="tbMPConfirmPassword" TextMode="Password" CssClass="form-control" />
                <asp:RequiredFieldValidator runat="server" ControlToValidate="tbMPConfirmPassword" ValidationGroup="UpdateInfo"
                    CssClass="text-danger" Display="Dynamic" ErrorMessage="Fyll i upprepa lösenord." />
                <asp:CompareValidator runat="server" ControlToCompare="tbMPPassword" ControlToValidate="tbMPConfirmPassword"
                    CssClass="text-danger" Display="Dynamic" ErrorMessage="Lösenorden du fyllt i stämmer inte överens." />
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
                <asp:Button runat="server" OnClick="UpdateInfo_Click" Text="Uppdatera konto" CssClass="btn btn-default" ValidationGroup="UpdateInfo" />
                <asp:Button ID="Button1" runat="server" Text="Button" CssClass="btn btn-default" OnClick="Button1_Click"/>
            </div>
        </div>

        <asp:Label ID="lblMPMessage" runat="server" Text="Label"></asp:Label>
    </div>
</asp:Content>
