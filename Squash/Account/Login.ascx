<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Login.ascx.cs" Inherits="Squash.Account.Login" %>

<%--<%@ Register Src="~/Account/OpenAuthProviders.ascx" TagPrefix="uc" TagName="OpenAuthProviders" %>--%>

<%--<asp:Content runat="server" ID="BodyContent" ContentPlaceHolderID="MainContent">--%>
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
                    <%--<div class="form-group">
                    <div class="col-md-offset-2 col-md-10">
                        <div class="checkbox">
                            <asp:CheckBox runat="server" ID="RememberMe" />
                            <asp:Label runat="server" AssociatedControlID="RememberMe">Remember me?</asp:Label>
                        </div>
                    </div>
                </div>--%>
                    <div class="form-group">
                        <div class="col-md-10">
                            <asp:Button ID="btnLogin" runat="server" OnClick="LogIn" OnClientClick="reopenOverlay()" Text="Logga in" CssClass="btn btn-default" ValidationGroup="Login" />
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

    <%--        <div class="col-md-4">
            <section id="socialLoginForm">
                <uc:OpenAuthProviders runat="server" ID="OpenAuthLogin" />
            </section>
        </div>--%>
    <%--<asp:HiddenField ID="hfShowLogin" runat="server" Value="0" />--%>
</div>
<%--</asp:Content>--%>