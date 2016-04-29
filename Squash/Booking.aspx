<%@ Page Title="Spelschema" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Booking.aspx.cs" Inherits="Squash.Booking" %>


<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <div class="pageDiv">
        <h2><%: Title %></h2>
        <%--CONTENT HÄR!--%>


        <div class="booking-overlay-container" runat="server">
            <div class="booking-page-overlay" runat="server" onclick="CloseBookingOverlay()">
                <div id="bookingOverlayMessage" class="booking-overlay-message" runat="server">
                    
                </div>
            </div>
        </div>
        <div id="scheduleDiv" runat="server">
        </div>

        <script type="text/javascript">
            function confirm_clicked(ct, mId, fullDate, bookingDivId) {
                var i = 2;

                if (i < 4) {
                    alert(ct + " " + mId + " " + fullDate);
                }


            }



            function OpenBookingOverlay(bookingDivId) {
                var id = "#" + bookingDivId
                $('.booking-overlay-container').fadeIn('slow');
                $('.bookingDiv').hide();
                $(id).show();
                //document.getElementById('hfShowLogin').setAttribute('Value', '1')
                return false;
            }
            function CloseBookingOverlay() {
                //document.getElementById('hfShowLogin').setAttribute('Value', '0')
                $('.booking-overlay-container').fadeOut('slow');
            }
            //function ReopenBookingOverlay() {
            //    var oneOrZero = document.getElementById('hfShowLogin').getAttribute('Value')
            //    if (oneOrZero == "1")
            //        $('.overlay-container').show();
            //}

            //ReopenOverlay();


        </script>



    </div>
</asp:Content>
