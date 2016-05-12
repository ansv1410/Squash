<%@ Page Title="Spelschema" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Booking.aspx.cs" Inherits="Squash.Booking" %>


<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <div class="pageDiv" id="bookingPageDivcc" runat="server">

        <div id="myBookingsDiv" runat="server" visible="false">
            <h3 id="myBookingsH3">Mina bokningar</h3>
        </div>




        <h2>Spelschema</h2>
        <div id="divBookMess" runat="server">
            <p id="bookingConfirmationMessage" runat="server" class="successfulBookingMessage" visible="false"></p>
            <p id="bookingErrorMessage" runat="server" class="failedBookingMessage" visible="false"></p>
        </div>


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
        <asp:HiddenField ID="hfNoOfClickedCourts" runat="server" Value="0" />
        <asp:HiddenField ID="hfShowBookingMessage" runat="server" Value="0" />
        <asp:HiddenField ID="hfBookingMessage" runat="server" Value="0" />


        <script type="text/javascript">
            function confirm_clicked(ct, mId, fullDate, bookingDivId) {
                var i = 2;

                if (i < 4) {
                    //alert(ct + " " + mId + " " + fullDate);
                }


            }

            function bookBtnClickable() {

                $('.book-btn').each(function () {
                    this.removeAttribute('disabled');
                });
            }
            function bookBtnUnclickable() {
                $('.book-btn').each(function () {
                    this.setAttribute('disabled', 'disabled');
                });
            }

            function OpenBookingOverlay(hourBookingDiv) {
                var id3 = "#" + hourBookingDiv;
                $('.booking-overlay-container').fadeIn('slow');
                $('.hourBookingDiv').hide();
                $(id3).show();
                return false;
            }
            function CloseBookingOverlay() {
                $('.booking-overlay-container').fadeOut('slow');
                $('.BookingHf').val('0');
                document.getElementById("MainContent_hfNoOfClickedCourts").setAttribute('Value', '0');
                bookBtnUnclickable();
                $('.bookingDiv').removeClass('selectedCourt');
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

                var previousClicks = parseInt(document.getElementById("MainContent_hfNoOfClickedCourts").getAttribute('Value'));

                if (chosenOrNot == 0) {

                    document.getElementById(id).setAttribute('Value', courtId);
                    $("#" + bookingDivID).addClass("selectedCourt");
                    previousClicks += 1;
                    document.getElementById("MainContent_hfNoOfClickedCourts").setAttribute('Value', previousClicks.toString());
                    bookBtnClickable();
                }
                else {

                    document.getElementById(id).setAttribute('Value', "0");
                    $("#" + bookingDivID).removeClass("selectedCourt");
                    previousClicks -= 1;
                    document.getElementById("MainContent_hfNoOfClickedCourts").setAttribute('Value', previousClicks.toString())
                    if (previousClicks < 1) {
                        bookBtnUnclickable();
                    }

                }

            }

            function ShowMobileDayDiv(dayDivId) {
                var id = "#" + dayDivId;
                $('.dayDiv').hide();
                $(id).show();

                $('.daySelector').removeClass('selectedMobileDay');
                $(id + "Selector").addClass('selectedMobileDay')
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

            window.addEventListener('resize', function (event) {
                fixWidth();
            });

            /*Document.ready för att startskärmen skall vara korrekt.*/
            $(document).ready(fixWidth());




        </script>




    </div>
</asp:Content>
