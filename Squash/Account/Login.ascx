<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Login.ascx.cs" Inherits="Squash.Account.Login" %>

<h2>Logga in</h2>

<div class="row">
    <div id="col-md-8-loginForm" class="col-md-8">
        <section id="loginForm">
            <div class="form-horizontal">
                <h4 id="emailPwLogin">Ange e-post och lösenord.</h4>
                <hr />
                <asp:Panel ID="pnlDefaultButton" runat="server" DefaultButton="btnLogin">
                    <asp:PlaceHolder runat="server" ID="ErrorMessage" Visible="false">
                        <p class="text-danger">
                            <asp:Literal runat="server" ID="FailureText" />
                        </p>
                    </asp:PlaceHolder>
                    <div class="form-group">
                        <asp:Label runat="server" AssociatedControlID="tbLogInEmail" CssClass="col-md-2 control-label">E-post</asp:Label>
                        <div class="col-md-10">
                            <asp:TextBox runat="server" ID="tbLogInEmail" CssClass="form-control" TextMode="Email" />
                            <asp:RequiredFieldValidator runat="server" ControlToValidate="tbLogInEmail" ValidationGroup="Login"
                                CssClass="text-danger" ErrorMessage="E-postfältet måste fyllas i." />
                        </div>
                    </div>
                    <div class="form-group">
                        <asp:Label runat="server" AssociatedControlID="tbLogInPassword" CssClass="col-md-2 control-label">Lösenord</asp:Label>
                        <div class="col-md-10">
                            <asp:TextBox runat="server" ID="tbLogInPassword" TextMode="Password" CssClass="form-control" />
                            <asp:RequiredFieldValidator runat="server" ControlToValidate="tbLogInPassword" ValidationGroup="Login"
                                CssClass="text-danger" ErrorMessage="Lösenordsfältet måste fyllas i." />
                        </div>
                    </div>

                    <div class="form-group">
                        <div class="col-md-10">
                            <asp:Button ID="btnLogin" runat="server" OnClick="LogIn" OnClientClick="reopenOverlay()" Text="Logga in" CssClass="btn btn-default lockAndLoad" ValidationGroup="Login" />
                            <asp:Label ID="LoginFail" runat="server"></asp:Label>
                        </div>
                    </div>
                </asp:Panel>
                <hr />
            </div>
            <p>
                <asp:HyperLink runat="server" ID="RegisterHyperLink" ViewStateMode="Disabled">Bli medlem</asp:HyperLink>
            </p>
            <p>
                <asp:HyperLink runat="server" ID="ForgotPasswordHyperLink" ViewStateMode="Disabled" NavigateUrl="~/Account/Forgot.aspx">Glömt lösenordet?</asp:HyperLink>
            </p>
        </section>

        <script type="text/javascript">
            function OpenOverlay() {
                $('.overlay-container').fadeIn('slow');
                return false;
            }

            function reopenOverlay() {
                if (!Page_ClientValidate("Login")) {
                    OpenOverlay()
                }

            }
        </script>
    </div>

</div>
