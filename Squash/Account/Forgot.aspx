<%@ Page Title="Forgot password" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Forgot.aspx.cs" Inherits="Squash.Account.ForgotPassword" Async="true" %>

<asp:Content runat="server" ID="BodyContent" ContentPlaceHolderID="MainContent">
    <div class="pageDiv" id="forgotDiv">
        <h2>Glömt lösenord</h2>

        <asp:PlaceHolder ID="loginForm" runat="server">
            <div class="form-horizontal">
                <h4>Här kan du beställa ett nytt lösenord.</h4>
                <hr />
                <asp:Panel ID="pnlDefaultButton" runat="server" DefaultButton="btnResetPW">
                    <asp:PlaceHolder runat="server" ID="ErrorMessage" Visible="false">
                        <p class="text-danger">
                            <asp:Literal runat="server" ID="FailureText" />
                        </p>
                    </asp:PlaceHolder>
                    <div class="form-group" id="forgotEmailDiv">
                        <asp:Label runat="server" AssociatedControlID="ResetToEmail" CssClass="col-md-2 control-label">Email</asp:Label>
                        <div class="col-md-10">
                            <asp:TextBox runat="server" ID="ResetToEmail" CssClass="form-control" TextMode="Email" />
                            <asp:RequiredFieldValidator runat="server" ControlToValidate="ResetToEmail"
                                CssClass="text-danger" ErrorMessage="Fyll i din registrerade e-postadress." />
                        </div>
                    </div>
                    <div class="form-group">
                        <div class="col-md-offset-2 col-md-10">
                            <asp:Button ID="btnResetPW" runat="server" OnClick="btnResetPW_Click" Text="Skicka lösenord" CssClass="btn btn-default" />
                            <asp:PlaceHolder runat="server" ID="DisplayEmail" Visible="false">
                                <p class="text-info">
                                    Vi har skickat ditt nya lösenord till din mailaddress.
                                </p>
                            </asp:PlaceHolder>
                            <asp:PlaceHolder runat="server" ID="wrongEmail" Visible="false">
                                <p class="text-danger">
                                    Vi känner inte igen din mailadress.
                                </p>
                            </asp:PlaceHolder>
                        </div>
                    </div>
                </asp:Panel>
            </div>
        </asp:PlaceHolder>

    </div>
</asp:Content>
