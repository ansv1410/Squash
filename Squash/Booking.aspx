<%@ Page Title="Spelschema" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Booking.aspx.cs" Inherits="Squash.Booking" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="pageDiv">
        <h2><%: Title %></h2>
        <%--CONTENT HÄR!--%>
        <div id="scheduleDiv" runat="server">
            <div class="toolTip">Klicka för att boka tid</div>
        </div>
    <script>
        function confirm_clicked(ct, mId, fullDate) {
            var i = 2;
            
            if(i < 4)
            {
                alert(ct + " " + mId + " " + fullDate);
            }
        }



        var mouseX;
        var mouseY;
        $(document).mousemove(function (e) {
            mouseX = e.pageX;
            mousey = e.pageY;
        });
        $(".freeCourt").mouseover(function () {
            $('.toolTip').offset({top: mousey, left: mouseX }).fadeIn('slow');
        });
        $(".freeCourt").mouseout(function () {
            $('.toolTip').fadeout('slow');
        });

    </script>



    <script>
        
    </script>




    </div>
</asp:Content>
