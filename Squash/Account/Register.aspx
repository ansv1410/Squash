<%@ Page Title="Bli medlem" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Register.aspx.cs" Inherits="Squash.Account.Register" %>

<asp:Content runat="server" ID="BodyContent" ContentPlaceHolderID="MainContent">
    <div class="pageDiv">
        <h2><%: Title %></h2>
        
        <div class="form-horizontal" id="registerFormHorizontal">
            <h4>Fyll i formuläret nedan för att bli medlem.</h4>
            <h5>Uppgifterna skickas till administratör för godkännande. Efter godkännandet får du ett e-mail och därefter kan du använda tjänsten.</h5>
            <asp:Label ID="lblMessage" runat="server"></asp:Label>
            <hr />
            <asp:Panel ID="pnlDefaultButton" runat="server" DefaultButton="btnCreateUser">

                <div class="form-group">
                    <asp:Label runat="server" AssociatedControlID="tbFirstName" CssClass="col-md-3 control-label">Förnamn</asp:Label>
                    <div class="col-md-10">
                        <asp:TextBox runat="server" ID="tbFirstName" CssClass="form-control" TextMode="SingleLine" />
                        <asp:RequiredFieldValidator runat="server" ControlToValidate="tbFirstName" ValidationGroup="Register"
                            CssClass="text-danger" ErrorMessage="Fyll i förnamn." />
                    </div>
                </div>
                <div class="form-group">
                    <asp:Label runat="server" AssociatedControlID="tbSurName" CssClass="col-md-3 control-label">Efternamn</asp:Label>
                    <div class="col-md-10">
                        <asp:TextBox runat="server" ID="tbSurName" CssClass="form-control" TextMode="SingleLine" />
                        <asp:RequiredFieldValidator runat="server" ControlToValidate="tbSurName" ValidationGroup="Register"
                            CssClass="text-danger" ErrorMessage="Fyll i efternamn." />
                    </div>
                </div>
                <div class="form-group">
                    <asp:Label runat="server" AssociatedControlID="tbStreetAddress" CssClass="col-md-3 control-label">Gatuadress</asp:Label>
                    <div class="col-md-10">
                        <asp:TextBox runat="server" ID="tbStreetAddress" CssClass="form-control" TextMode="SingleLine" />
                        <asp:RequiredFieldValidator runat="server" ControlToValidate="tbStreetAddress" ValidationGroup="Register"
                            CssClass="text-danger" ErrorMessage="Fyll i gatuadress." />
                    </div>
                </div>
                <div class="form-group">
                    <asp:Label runat="server" AssociatedControlID="tbPostalCode" CssClass="col-md-3 control-label">Postnummer</asp:Label>
                    <div class="col-md-10">
                        <asp:TextBox runat="server" ID="tbPostalCode" CssClass="form-control" MaxLength="5" />
                        <asp:RequiredFieldValidator runat="server" ControlToValidate="tbPostalCode" ValidationGroup="Register"
                            CssClass="text-danger" ErrorMessage="Fyll i postnummer." />
                        <asp:RegularExpressionValidator ID="RegularExpressionValidatorPostalCode" runat="server" ControlToValidate="tbPostalcode" CssClass="text-danger" ErrorMessage="Endast siffror." ValidationExpression="\d+"></asp:RegularExpressionValidator>
                    </div>
                </div>
                <div class="form-group">
                    <asp:Label runat="server" AssociatedControlID="tbCity" CssClass="col-md-3 control-label">Postort</asp:Label>
                    <div class="col-md-10">
                        <asp:TextBox runat="server" ID="tbCity" CssClass="form-control" TextMode="SingleLine" />
                        <asp:RequiredFieldValidator runat="server" ControlToValidate="tbCity" ValidationGroup="Register"
                            CssClass="text-danger" ErrorMessage="Fyll i postort." />
                    </div>
                </div>
                <div class="form-group">
                    <asp:Label runat="server" AssociatedControlID="tbTelephone" CssClass="col-md-3 control-label">Telefon</asp:Label>
                    <div class="col-md-10">
                        <asp:TextBox runat="server" ID="tbTelephone" CssClass="form-control" TextMode="SingleLine" MaxLength="15"/>
                        <asp:RequiredFieldValidator runat="server" ControlToValidate="tbTelephone" ValidationGroup="Register"
                            CssClass="text-danger" ErrorMessage="Fyll i telefon." />
                        <asp:RegularExpressionValidator ID="RegularExpressionValidatorPhone" runat="server" ControlToValidate="tbTelephone" CssClass="text-danger" ErrorMessage="Endast siffror." ValidationExpression="\d+"></asp:RegularExpressionValidator>
                    </div>
                </div>
                <%-- Inloggningskontroller --%>
                <div class="form-group">
                    <asp:Label runat="server" AssociatedControlID="tbEmail" CssClass="col-md-3 control-label">E-post</asp:Label>
                    <div class="col-md-10">
                        <asp:TextBox runat="server" ID="tbEmail" CssClass="form-control" TextMode="Email" />
                        <asp:RequiredFieldValidator runat="server" ControlToValidate="tbEmail" ValidationGroup="Register"
                            CssClass="text-danger" ErrorMessage="Fyll i e-post." />
                    </div>
                </div>
                <hr />
                <div class="form-group">
                    <asp:Label runat="server" AssociatedControlID="tbPassword" CssClass="col-md-3 control-label">Lösenord</asp:Label>
                    <div class="col-md-10">
                        <asp:TextBox runat="server" ID="tbPassword" TextMode="Password" CssClass="form-control" />
                        <asp:RequiredFieldValidator runat="server" ControlToValidate="tbPassword" ValidationGroup="Register"
                            CssClass="text-danger" ErrorMessage="Fyll i önskat lösenord." />
                        <br />
                        <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" ControlToValidate="tbPassword" CssClass="text-danger" ErrorMessage="Ska vara mellan 6-12 tecken och minst en siffra." ValidationExpression="^(?=.*\d)(?=.*[a-zA-Z]|å|Å|ä|Ä|ö|Ö).{6,12}$"></asp:RegularExpressionValidator>
                    </div>
                </div>
                <div class="form-group">
                    <asp:Label runat="server" AssociatedControlID="tbConfirmPassword" CssClass="col-md-3 control-label">Upprepa lösenord</asp:Label>
                    <div class="col-md-10">
                        <asp:TextBox runat="server" ID="tbConfirmPassword" TextMode="Password" CssClass="form-control" />
                        <asp:RequiredFieldValidator runat="server" ControlToValidate="tbConfirmPassword" ValidationGroup="Register"
                            CssClass="text-danger" Display="Dynamic" ErrorMessage="Fyll i upprepa lösenord." />
                        <asp:CompareValidator runat="server" ControlToCompare="tbPassword" ControlToValidate="tbConfirmPassword"
                            CssClass="text-danger" Display="Dynamic" ErrorMessage="Lösenorden du fyllt i stämmer inte överens." />
                        <br />
                    </div>
                </div>
                <hr />
                <%-- Radiobuttons för att godkänna lagring och eventuell visning i kontaktlista --%>
                <div class="form-group">
                    <div class="col-md-offset-2 col-md-10" style="padding-left:15px; width:100%;">
                        <asp:Label AssociatedControlID="rblAgreement" runat="server" Text="Jag godkänner att personuppgifter lagras och vill:" style="padding-left:2%;"></asp:Label>
                        <br />
                        <asp:RadioButtonList ID="rblAgreement" runat="server" CssClass="agreementList"
                            RepeatDirection="Vertical" RepeatLayout="Table">
                            <asp:ListItem Text=" synas i kontaktlista för andra medlemmar." Value="Agree" />
                            <asp:ListItem Text=" <u><em>inte</em></u> synas i kontaktlista för andra medlemmar." Value="Disagree" />
                        </asp:RadioButtonList>
                        <asp:RequiredFieldValidator runat="server" ControlToValidate="rblAgreement" ValidationGroup="Register"
                            CssClass="text-danger" Display="Dynamic" ErrorMessage="Välj ett alternativ ovan." />
                    </div>
                </div>
                <div class="form-group">
                    <div class="col-md-offset-2 col-md-10">
                        <asp:Button ID="btnCreateUser" runat="server" OnClick="CreateUser_Click" Text="Skapa konto" CssClass="btn btn-default" ValidationGroup="Register" />
                        <br />
                    </div>
                </div>

            </asp:Panel>
        </div>
    </div>
</asp:Content>
