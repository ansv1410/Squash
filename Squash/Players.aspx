<%@ Page Title="Spelare & statistik" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Players.aspx.cs" Inherits="Squash.Players" %>


<%@ Register Assembly="System.Web.DataVisualization, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" Namespace="System.Web.UI.DataVisualization.Charting" TagPrefix="asp" %>


<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="pageDiv">


        <div id="statsDiv" runat="server">
            <div id="monthPickerDiv" runat="server">
            </div>
        </div>



        <div id="addressListDiv" runat="server">
            <div id="users">

                <input id="tbSearchUser" class="search" placeholder="Sök" />
                <ul id="userList" class="list" runat="server">
                 
                </ul>

            </div>
        </div>


    </div>


    <script>
        function MonthVisible(chartId, selectorId) {
            var id = "#" + chartId;
            var sId = "#" + selectorId;

            $('.chartDiv').removeClass('chartVisible');
            $(id).addClass('chartVisible')

            $('.monthSelector').removeClass('activeMonth');
            $(sId).addClass('activeMonth')
        };


        var options = {
            valueNames: ['listName', 'listStreet', 'listZip', 'listCity', 'listPhone']
        };

        var userList = new List('users', options);


    </script>



</asp:Content>
