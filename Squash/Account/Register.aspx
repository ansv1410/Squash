<%@ Page Title="Register" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Register.aspx.cs" Inherits="Squash.Account.Register" %>

<asp:Content runat="server" ID="BodyContent" ContentPlaceHolderID="MainContent">
    <h2><%: Title %>.</h2>
<%--    <p class="text-danger">
        <asp:Literal runat="server" ID="ErrorMessage" />
    </p>--%>

    <div class="form-horizontal">
        <h4>Create a new account</h4>
        <hr />
        <%--<asp:ValidationSummary runat="server" CssClass="text-danger" />--%>

        <div class="form-group">
            <asp:Label runat="server" AssociatedControlID="tbFirstName" CssClass="col-md-2 control-label">Förnamn</asp:Label>
            <div class="col-md-10">
                <asp:TextBox runat="server" ID="tbFirstName" CssClass="form-control" TextMode="SingleLine" />
                <asp:RequiredFieldValidator runat="server" ControlToValidate="tbFirstName"
                    CssClass="text-danger" ErrorMessage="Fyll i förnamn." />
            </div>
        </div>
        <div class="form-group">
            <asp:Label runat="server" AssociatedControlID="tbSurName" CssClass="col-md-2 control-label">Efternamn</asp:Label>
            <div class="col-md-10">
                <asp:TextBox runat="server" ID="tbSurName" CssClass="form-control" TextMode="SingleLine" />
                <asp:RequiredFieldValidator runat="server" ControlToValidate="tbSurName"
                    CssClass="text-danger" ErrorMessage="Fyll i efternamn." />
            </div>
        </div>
        <div class="form-group">
            <asp:Label runat="server" AssociatedControlID="tbStreetAddress" CssClass="col-md-2 control-label">Gatuadress</asp:Label>
            <div class="col-md-10">
                <asp:TextBox runat="server" ID="tbStreetAddress" CssClass="form-control" TextMode="SingleLine" />
                <asp:RequiredFieldValidator runat="server" ControlToValidate="tbStreetAddress"
                    CssClass="text-danger" ErrorMessage="Fyll i gatuadress." />
            </div>
        </div>
        <div class="form-group">
            <asp:Label runat="server" AssociatedControlID="tbPostalCode" CssClass="col-md-2 control-label">Postnummer</asp:Label>
            <div class="col-md-10">
                <asp:TextBox runat="server" ID="tbPostalCode" CssClass="form-control" MaxLength="5"/>
                <asp:RequiredFieldValidator runat="server" ControlToValidate="tbPostalCode"
                    CssClass="text-danger" ErrorMessage="Fyll i postnummer." />
                <asp:RegularExpressionValidator ID="RegularExpressionValidatorPostalCode" runat="server" ControlToValidate="tbPostalcode" CssClass="text-danger" ErrorMessage="Endast siffror." ValidationExpression="\d+"></asp:RegularExpressionValidator>
            </div>
        </div>
        <div class="form-group">
            <asp:Label runat="server" AssociatedControlID="tbCity" CssClass="col-md-2 control-label">Postort</asp:Label>
            <div class="col-md-10">
                <asp:TextBox runat="server" ID="tbCity" CssClass="form-control" TextMode="SingleLine" />
                <asp:RequiredFieldValidator runat="server" ControlToValidate="tbCity"
                    CssClass="text-danger" ErrorMessage="Fyll i postort." />
            </div>
        </div>
        <div class="form-group">
            <asp:Label runat="server" AssociatedControlID="tbTelephone" CssClass="col-md-2 control-label">Telefon</asp:Label>
            <div class="col-md-10">
                <asp:TextBox runat="server" ID="tbTelephone" CssClass="form-control" TextMode="SingleLine" />
                <asp:RequiredFieldValidator runat="server" ControlToValidate="tbTelephone"
                    CssClass="text-danger" ErrorMessage="Fyll i telefon." />
            </div>
        </div>
        <%-- Inloggningskontroller --%>
        <div class="form-group">
            <asp:Label runat="server" AssociatedControlID="tbEmail" CssClass="col-md-2 control-label">E-post</asp:Label>
            <div class="col-md-10">
                <asp:TextBox runat="server" ID="tbEmail" CssClass="form-control" TextMode="Email" />
                <asp:RequiredFieldValidator runat="server" ControlToValidate="tbEmail"
                    CssClass="text-danger" ErrorMessage="Fyll i e-post." />
            </div>
        </div>
        <div class="form-group">
            <asp:Label runat="server" AssociatedControlID="tbPassword" CssClass="col-md-2 control-label">Lösenord</asp:Label>
            <div class="col-md-10">
                <asp:TextBox runat="server" ID="tbPassword" TextMode="Password" CssClass="form-control" />
                <asp:RequiredFieldValidator runat="server" ControlToValidate="tbPassword"
                    CssClass="text-danger" ErrorMessage="Fyll i önskat lösenord." />
                <br />
                <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" ControlToValidate="tbPassword" CssClass="text-danger" ErrorMessage="Ska vara mellan 6-12 tecken och minst en siffra." ValidationExpression="^(?=.*\d)(?=.*[a-zA-Z]|å|Å|ä|Ä|ö|Ö).{6,12}$"></asp:RegularExpressionValidator>
            </div>
        </div>
        <div class="form-group">
            <asp:Label runat="server" AssociatedControlID="tbConfirmPassword" CssClass="col-md-2 control-label">Upprepa lösenord</asp:Label>
            <div class="col-md-10">
                <asp:TextBox runat="server" ID="tbConfirmPassword" TextMode="Password" CssClass="form-control" />
                <asp:RequiredFieldValidator runat="server" ControlToValidate="tbConfirmPassword"
                    CssClass="text-danger" Display="Dynamic" ErrorMessage="Fyll i upprepa lösenord." />
                <asp:CompareValidator runat="server" ControlToCompare="tbPassword" ControlToValidate="tbConfirmPassword"
                    CssClass="text-danger" Display="Dynamic" ErrorMessage="Lösenorden du fyllt i stämmer inte överens." />
                <br />
            </div>
        </div>
        <%-- Radiobuttons för att godkänna lagring och eventuell visning i kontaktlista --%>
        <div class="form-group">
            <div class="col-md-offset-2 col-md-10">
                <asp:Label AssociatedControlID="rblAgreement" runat="server" Text="Jag godkänner att personuppgifter lagras och vill:"></asp:Label>
                <asp:RadioButtonList ID="rblAgreement" runat="server"
                    RepeatDirection="Vertical" RepeatLayout="Table">
                    <asp:ListItem Text=" synas i kontaktlista för andra medlemmar." Value="Agree" />
                    <asp:ListItem Text=" <u><em>inte</em></u> synas i kontaktlista för andra medlemmar." Value="Disagree" />
                </asp:RadioButtonList>
                 <asp:RequiredFieldValidator runat="server" ControlToValidate="rblAgreement"
                    CssClass="text-danger" Display="Dynamic" ErrorMessage="Välj ett alternativ ovan." />
            </div>
        </div>
        <div class="form-group">
            <div class="col-md-offset-2 col-md-10">
                <asp:Button runat="server" OnClick="CreateUser_Click" Text="Skapa konto" CssClass="btn btn-default" />
            </div>
        </div>
        
        <asp:Label ID="lblMessage" runat="server" Text="Label"></asp:Label>
    </div>
</asp:Content>
