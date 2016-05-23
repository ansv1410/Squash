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

                <input class="search" placeholder="Search" />
                <button class="sort" data-sort="name">
                    Sort
                </button>

                <ul class="list">
                    <li>
                        <h3 class="name">Jonny Stromberg</h3>
                        <p class="born">1986</p>
                    </li>
                    <li>
                        <h3 class="name">Jonas Arnklint</h3>
                        <p class="born">1985</p>
                    </li>
                    <li>
                        <h3 class="name">Martina Elm</h3>
                        <p class="born">1986</p>
                    </li>
                    <li>
                        <h3 class="name">Gustaf Lindqvist</h3>
                        <p class="born">1983</p>
                    </li>
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
            valueNames: ['name', 'born']
        };

        var userList = new List('users', options);


    </script>



</asp:Content>
