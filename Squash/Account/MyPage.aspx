﻿<%@ Page Title="Min sida" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="MyPage.aspx.cs" Inherits="Squash.Account.MyPage" %>

<%@ Register TagPrefix="uc" TagName="EditMyInfoControl" Src="~/Account/EditUserInfo.ascx" %>
<%@ Register TagPrefix="uc" TagName="ChangePasswordControl" Src="~/Account/ChangePassword.ascx" %>


<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">

    <div class="pageDiv">
        <h2><%: Title %></h2>
        <div class="form-horizontal">
            <div id="myinfo" runat="server">
                <h4>Här syns din information.</h4>
                <h5>Uppgifterna går att uppdatera genom att trycka på knappen nederst.</h5>
                <hr />
                <asp:Panel ID="pnlDefaultButton" runat="server" DefaultButton="BtnShowEditInfo">
                    <div class="contentPaddingLeft">
                        <div class="my-info-label-div">
                            <asp:Label ID="lblName" runat="server" CssClass="my-info-label"></asp:Label>
                        </div>
                        <div class="my-info-label-div">
                            <asp:Label ID="lblStreetAddress" runat="server" CssClass="my-info-label"></asp:Label>
                        </div>
                        <div class="my-info-label-div">
                            <asp:Label ID="lblPostalCode" runat="server" CssClass="my-info-label"></asp:Label>
                        </div>
                        <div class="my-info-label-div">
                            <asp:Label ID="lblCity" runat="server" CssClass="my-info-label"></asp:Label>
                        </div>
                        <div class="my-info-label-div">
                            <asp:Label ID="lblTelephone" runat="server" CssClass="my-info-label"></asp:Label>
                        </div>
                        <div class="my-info-label-div">
                            <asp:Label ID="lblEmail" runat="server" CssClass="my-info-label"></asp:Label>
                        </div>
                        <div class="my-info-label-div">
                            <asp:Label ID="lblAgreement" runat="server" CssClass="my-info-label"></asp:Label>
                        </div>
                        <div class="my-info-label-div">
                            <div class="btn-update-info">
                                <asp:Button ID="BtnShowEditInfo" runat="server" Text="Redigera uppgifter" CssClass="btn btn-default" OnClick="BtnShowEditInfo_Click" />
                                <asp:Button ID="BtnShowEditPW" runat="server" Text="Byt Lösenord" CssClass="btn btn-default" OnClick="BtnShowEditPW_Click" />
                            </div>
                        </div>
                    </div>
                </asp:Panel>
            </div>

            <div id="editinfo" runat="server">

                <uc:EditMyInfoControl ID="EditMyInfoControll" runat="server" />

            </div>
            <div id="changepw" runat="server">

                <uc:ChangePasswordControl ID="ChangePasswordControl" runat="server" />

            </div>


        </div>
        <asp:HiddenField ID="hfEmailExist" runat="server" Value="0"/>
    </div>
</asp:Content>
