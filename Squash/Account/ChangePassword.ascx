<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ChangePassword.ascx.cs" Inherits="Squash.Account.ChangePassword" %>

<h4>Välj ett nytt lösenord nedan</h4>
<h5>Lösenordet uppdateras genast, har du glömt ditt nuvarande lösenord kan du istället <a href="Forgot.aspx">beställa ett nytt</a>.</h5>
<hr />

<asp:Panel ID="pnlDefaultButton2" runat="server" DefaultButton="btnUpdatePW">
        <div class="form-group">
        <asp:Label runat="server" AssociatedControlID="tbMPOldPassword" CssClass="col-md-2 control-label">Nuvarande lösenord</asp:Label>
        <div class="col-md-10">
            <asp:TextBox runat="server" ID="tbMPOldPassword" TextMode="Password" CssClass="form-control" />
            <asp:RequiredFieldValidator runat="server" ControlToValidate="tbMPOldPassword" ValidationGroup="UpdatePW"
                CssClass="text-danger" ErrorMessage="Fyll i nuvarande lösenord." />
            <br />
            <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ControlToValidate="tbMPPassword" CssClass="text-danger" ErrorMessage="Ska vara mellan 6-12 tecken och minst en siffra." ValidationExpression="^(?=.*\d)(?=.*[a-zA-Z]|å|Å|ä|Ä|ö|Ö).{6,12}$"></asp:RegularExpressionValidator>
        </div>
    </div>
    <div class="form-group">
        <asp:Label runat="server" AssociatedControlID="tbMPPassword" CssClass="col-md-2 control-label">Nytt lösenord</asp:Label>
        <div class="col-md-10">
            <asp:TextBox runat="server" ID="tbMPPassword" TextMode="Password" CssClass="form-control" />
            <asp:RequiredFieldValidator runat="server" ControlToValidate="tbMPPassword" ValidationGroup="UpdatePW"
                CssClass="text-danger" ErrorMessage="Fyll i önskat lösenord." />
            <br />
            <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" ControlToValidate="tbMPPassword" CssClass="text-danger" ErrorMessage="Ska vara mellan 6-12 tecken och minst en siffra." ValidationExpression="^(?=.*\d)(?=.*[a-zA-Z]|å|Å|ä|Ä|ö|Ö).{6,12}$"></asp:RegularExpressionValidator>
        </div>
    </div>
    <div class="form-group">
        <asp:Label runat="server" AssociatedControlID="tbMPConfirmPassword" CssClass="col-md-2 control-label">Upprepa nytt lösenord</asp:Label>
        <div class="col-md-10">
            <asp:TextBox runat="server" ID="tbMPConfirmPassword" TextMode="Password" CssClass="form-control" />
            <asp:RequiredFieldValidator runat="server" ControlToValidate="tbMPConfirmPassword" ValidationGroup="UpdatePW"
                CssClass="text-danger" Display="Dynamic" ErrorMessage="Upprepa önskat lösenord." />
            <asp:CompareValidator runat="server" ControlToCompare="tbMPPassword" ControlToValidate="tbMPConfirmPassword" ValidationGroup="UpdatePW"
                CssClass="text-danger" Display="Dynamic" ErrorMessage="Lösenorden du fyllt i stämmer inte överens." />
            <br />
        </div>
    </div>
    <div class="form-group">
        <div class="col-md-offset-2 col-md-10">
            <asp:Button ID="btnUpdatePW" runat="server" Text="Uppdatera lösenord" CssClass="btn btn-default" ValidationGroup="UpdatePW" OnCommand="btnUpdatePW_Command" CommandArgument='1'/>
        </div>
    </div>
</asp:Panel>