<%@ Page Title="Spelschema" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Booking.aspx.cs" Inherits="Squash.Booking" %>


<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <div class="pageDiv" id="bookingPageDivcc" runat="server">
        <h2>Spelschema</h2>

        <div class="booking-overlay-container" runat="server">
            <div class="booking-page-overlay" runat="server" onclick="CloseBookingOverlay()">
            </div>
            <div id="bookingOverlayMessage" class="booking-overlay-message" runat="server">
            </div>
        </div>
        <div id="selectorDiv" runat="server"></div>
        <div id="scheduleDiv" runat="server"></div>

        <asp:HiddenField ID="hfWidthOfDayDivs" runat="server" />
        <asp:HiddenField ID="hfWidthOfDaySelectors" runat="server" />


        <script type="text/javascript">
            function confirm_clicked(ct, mId, fullDate, bookingDivId) {
                var i = 2;

                if (i < 4) {
                    alert(ct + " " + mId + " " + fullDate);
                }


            }



            function OpenBookingOverlay(hourBookingDiv) {
                //var id1 = "#" + firstDiv
                //var id2 = "#" + secondDiv
                var id3 = "#" + hourBookingDiv;
                $('.booking-overlay-container').fadeIn('slow');
                //$('.bookingDiv').hide();
                $('.hourBookingDiv').hide();
                //$(id1).show();
                //$(id2).show();
                $(id3).show();
                //document.getElementById('hfShowLogin').setAttribute('Value', '1')
                return false;
            }
            function CloseBookingOverlay() {
                //document.getElementById('hfShowLogin').setAttribute('Value', '0')
                $('.booking-overlay-container').fadeOut('slow');
                $('.BookingHf').val('0');
            }
            //function ReopenBookingOverlay() {
            //    var oneOrZero = document.getElementById('hfShowLogin').getAttribute('Value')
            //    if (oneOrZero == "1")
            //        $('.overlay-container').show();
            //}

            //ReopenOverlay();


            function chosenCourt(hfID, courtId, bookingDivID) {
                var id = "MainContent_" + hfID;
                var chosenOrNot = document.getElementById(id).getAttribute('Value');
                //var court = document.getElementById('MainContent_hfChosenCourts').getAttribute('value');
                //var courts = court + chosenCourtId;
                if (chosenOrNot == 0) {

                    document.getElementById(id).setAttribute('Value', courtId);

                    $("#" + bookingDivID).addClass("selectedCourt");
                }
                else {

                    document.getElementById(id).setAttribute('Value', "0");
                    $("#" + bookingDivID).removeClass("selectedCourt");
                }

            }

            function ShowMobileDayDiv(dayDivId) {
                var id = "#" + dayDivId;
                $('.dayDiv').hide();
                $(id).show();
            }

            /*Justerar bredden på dayDivarna från mobilläge och tillbaka.*/
            function fixWidth() {
                var browserWidth = window.innerWidth;
                var id = "MainContent_hfWidthOfDayDivs";
                var id2 = "MainContent_hfWidthOfDaySelectors";
                var widthOfDayDivs = document.getElementById(id).getAttribute('Value');
                var widthOfDaySelectors = document.getElementById(id2).getAttribute('Value');

                $(".daySelector").each(function () {
                    this.style.width = widthOfDaySelectors;
                });

                if (browserWidth < 768) {
                    $(".dayDiv").each(function () {
                        this.style.width = "100%";
                        ShowMobileDayDiv('1_day')
                    });
                }
                else {
                    $(".dayDiv").each(function () {
                        this.style.width = widthOfDayDivs;
                        this.style.display = "block";
                    });
                }

            }

            window.addEventListener('resize', function (event){
                fixWidth();
            });

            /*Document.ready för att startskärmen skall vara korrekt.*/
            $(document).ready(fixWidth());
            
            


        </script>



    
    </div>
</asp:Content>
