<%@ Page Title="Register" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Register.aspx.cs" Inherits="Squash.Account.Register" %>

<asp:Content runat="server" ID="BodyContent" ContentPlaceHolderID="MainContent">
    <h2><%: Title %>.</h2>
    <p class="text-danger">
        <asp:Literal runat="server" ID="ErrorMessage" />
    </p>

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
            <asp:Label runat="server" AssociatedControlID="tbLastName" CssClass="col-md-2 control-label">Efternamn</asp:Label>
            <div class="col-md-10">
                <asp:TextBox runat="server" ID="tbLastName" CssClass="form-control" TextMode="SingleLine" />
                <asp:RequiredFieldValidator runat="server" ControlToValidate="tbLastName"
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
            <asp:Label runat="server" AssociatedControlID="tbEmail" CssClass="col-md-2 control-label">Email</asp:Label>
            <div class="col-md-10">
                <asp:TextBox runat="server" ID="tbEmail" CssClass="form-control" TextMode="Email" />
                <asp:RequiredFieldValidator runat="server" ControlToValidate="tbEmail"
                    CssClass="text-danger" ErrorMessage="Fyll i e-post." />
            </div>
        </div>
        <div class="form-group">
            <asp:Label runat="server" AssociatedControlID="tbPassword" CssClass="col-md-2 control-label">Password</asp:Label>
            <div class="col-md-10">
                <asp:TextBox runat="server" ID="tbPassword" TextMode="Password" CssClass="form-control" />
                <asp:RequiredFieldValidator runat="server" ControlToValidate="tbPassword"
                    CssClass="text-danger" ErrorMessage="Fyll i önskat lösenord." />
            </div>
        </div>
        <div class="form-group">
            <asp:Label runat="server" AssociatedControlID="tbConfirmPassword" CssClass="col-md-2 control-label">Confirm password</asp:Label>
            <div class="col-md-10">
                <asp:TextBox runat="server" ID="tbConfirmPassword" TextMode="Password" CssClass="form-control" />
                <asp:RequiredFieldValidator runat="server" ControlToValidate="tbConfirmPassword"
                    CssClass="text-danger" Display="Dynamic" ErrorMessage="Fyll i upprepa lösenord." />
                <asp:CompareValidator runat="server" ControlToCompare="tbPassword" ControlToValidate="tbConfirmPassword"
                    CssClass="text-danger" Display="Dynamic" ErrorMessage="Lösenorden du fyllt i stämmer inte överens." />
            </div>
        </div>
        <div class="form-group">
            <div class="col-md-offset-2 col-md-10">
                <asp:Label AssociatedControlID="rblAgreement" runat="server" Text="Jag godkänner att personuppgifter lagras och vill:"></asp:Label>
                <asp:RadioButtonList ID="rblAgreement" runat="server"
                    RepeatDirection="Vertical" RepeatLayout="Table">
                    <asp:ListItem Text=" synas i kontaktlista för andra medlemmar." Value="Agree" />
                    <asp:ListItem Text=" <u><em>inte</em></u> synas i kontaktlista för andra medlemmar." Value="Disagree" />
                </asp:RadioButtonList>
            </div>
        </div>
        <div class="form-group">
            <div class="col-md-offset-2 col-md-10">
                <asp:Button runat="server" OnClick="CreateUser_Click" Text="Register" CssClass="btn btn-default" />
            </div>
        </div>
    </div>
</asp:Content>
