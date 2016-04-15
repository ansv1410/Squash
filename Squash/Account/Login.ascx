<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Login.ascx.cs" Inherits="Squash_Template.Account.Login" %>

<%--<%@ Register Src="~/Account/OpenAuthProviders.ascx" TagPrefix="uc" TagName="OpenAuthProviders" %>--%>

<%--<asp:Content runat="server" ID="BodyContent" ContentPlaceHolderID="MainContent">--%>
<h2>Logga in</h2>

<div class="row">
    <div id="col-md-8-loginForm" class="col-md-8">
        <section id="loginForm">
            <div class="form-horizontal">
                <h4>Ange e-post och lösenord.</h4>
                <hr />
                <asp:PlaceHolder runat="server" ID="ErrorMessage" Visible="false">
                    <p class="text-danger">
                        <asp:Literal runat="server" ID="FailureText" />
                    </p>
                </asp:PlaceHolder>
                <div class="form-group">
                    <asp:Label runat="server" AssociatedControlID="tbLogInEmail" CssClass="col-md-2 control-label">Email</asp:Label>
                    <div class="col-md-10">
                        <asp:TextBox runat="server" ID="tbLogInEmail" CssClass="form-control" TextMode="Email" />
                        <asp:RequiredFieldValidator runat="server" ControlToValidate="tbLogInEmail" ValidationGroup="Login"
                            CssClass="text-danger" ErrorMessage="The email field is required." />
                    </div>
                </div>
                <div class="form-group">
                    <asp:Label runat="server" AssociatedControlID="tbLogInPassword" CssClass="col-md-2 control-label">Password</asp:Label>
                    <div class="col-md-10">
                        <asp:TextBox runat="server" ID="tbLogInPassword" TextMode="Password" CssClass="form-control" />
                        <asp:RequiredFieldValidator runat="server" ControlToValidate="tbLogInPassword" ValidationGroup="Login"
                            CssClass="text-danger" ErrorMessage="The password field is required." />
                    </div>
                </div>
                <div class="form-group">
                    <div class="col-md-offset-2 col-md-10">
                        <div class="checkbox">
                            <asp:CheckBox runat="server" ID="RememberMe" />
                            <asp:Label runat="server" AssociatedControlID="RememberMe">Remember me?</asp:Label>
                        </div>
                    </div>
                </div>
                <div class="form-group">
                    <div class="col-md-offset-2 col-md-10">
                        <asp:Button runat="server" OnClick="LogIn" OnClientClick="reopenOverlay()" Text="Log in" CssClass="btn btn-default" ValidationGroup="Login" />
                        <asp:Label ID="LoggedInUser" runat="server" Text="Här visas inloggad användare."></asp:Label>
                        <asp:Label ID="LoggedInMember" runat="server" Text="Inloggad medlem"></asp:Label>
                    </div>
                </div>
            </div>
            <p>
                <asp:HyperLink runat="server" ID="RegisterHyperLink" ViewStateMode="Disabled">Register as a new user</asp:HyperLink>
            </p>
            <p>
                <%-- Enable this once you have account confirmation enabled for password reset functionality
                    <asp:HyperLink runat="server" ID="ForgotPasswordHyperLink" ViewStateMode="Disabled">Forgot your password?</asp:HyperLink>
                --%>
            </p>
        </section>
        <script type="text/javascript">

            function OpenOverlay() {
                $('.overlay-container').fadeIn('slow');
                return false;
            }

            function reopenOverlay() {
                if(!Page_ClientValidate("Login"))
                {
                    OpenOverlay()
                }

            }
        </script>
    </div>

    <%--        <div class="col-md-4">
            <section id="socialLoginForm">
                <uc:OpenAuthProviders runat="server" ID="OpenAuthLogin" />
            </section>
        </div>--%>
    <%--<asp:HiddenField ID="hfShowLogin" runat="server" Value="0" />--%>
</div>
<%--</asp:Content>--%>