<%@ Page Title="Spelschema" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Booking.aspx.cs" Inherits="Squash.Booking" %>


<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <div class="pageDiv" id="bookingPageDivcc" runat="server">

        <div id="myBookingsDiv" runat="server" visible="false">
            <h3 id="myBookingsH3">Mina bokningar</h3>
        </div>
        <%--<input id="btnCancelRes" type="button" onclick="OpenCancelReservationOverlay()" class="btn" value="Avboka" disabled="disabled" />--%>




        <h2 id="scheduleH2">Spelschema</h2>
        <h4 id="scheduleH4">- Boka nedan</h4>
        <div id="divBookMess" runat="server">
            <p id="bookingConfirmationMessage" runat="server" class="successfulBookingMessage" visible="false"></p>
            <p id="bookingErrorMessage" runat="server" class="failedBookingMessage" visible="false"></p>
            <p id="preBookingInfo" runat="server" class="bookingInfoText" visible="false"></p>
        </div>


        <div class="booking-overlay-container" runat="server">
            <div class="booking-page-overlay" runat="server" onclick="CloseBookingOverlay()">
            </div>
            <div id="bookingOverlayMessage" class="booking-overlay-message" runat="server">
                <div id="cancelReservationDiv" runat="server">
                    <h4>Avboka följande?</h4>
                    <div id="toCancelDiv">
                    </div>
                    <div id="cancelButtons" class="promptButtons">
                        <asp:Button ID="btnCancelOK" runat="server" OnClientClick="sendCbIDs(this, false, null);" OnClick="btnCancelOK_Click" Text="OK" CssClass="btn btn-default promptOK" />
                        <input id="btnNoCancel" type="button" onclick="CloseBookingOverlay()" class="btn btn-default promptCancel" value="Avbryt" />
                    </div>
                </div>
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
                $('.booking-overlay-message').css('max-width', '500px');
                $('.booking-overlay-container').fadeIn('slow');
                $('.hourBookingDiv').hide();
                $('#MainContent_cancelReservationDiv').hide();
                $(id3).show();
                return false;
            }


            function CloseBookingOverlay() {
                $('.booking-overlay-container').fadeOut('slow');
                $('.BookingHf').val('0');
                document.getElementById("MainContent_hfNoOfClickedCourts").setAttribute('Value', '0');
                bookBtnUnclickable();
                $('.bookingDiv').removeClass('selectedCourt');
                $('#MainContent_cancelReservationDiv').hide();
            }


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
                    document.getElementById("MainContent_hfNoOfClickedCourts").setAttribute('Value', previousClicks.toString());
                    if (previousClicks < 1) {
                        bookBtnUnclickable();
                    }

                }

            }


            function OpenCancelReservationOverlay(isSingleResId) {

                if (isSingleResId == "false") {
                    loopForm();
                    document.getElementById("MainContent_btnCancelOK").setAttribute("OnClick", "sendCbIDs(this, false, null)");
                }

                $('.booking-overlay-message').css('max-width', '300px');
                $('.booking-overlay-container').fadeIn('slow');
                $('.hourBookingDiv').hide();
                $('#MainContent_cancelReservationDiv').show();
                return false;
                
            }

            function loopForm() {

                var table = document.getElementById("MainContent_myBookingsDiv");
                
                var cbResults = '';

                $('.cbCancelReservation').each(function () {
                    if (this.getAttribute("checked") == "checked") {
                        cbResults += this.getAttribute("value") + "<br />";
                        
                    }
                });

                document.getElementById("toCancelDiv").innerHTML = cbResults;
                
            }

            function checkOrUncheck(id) {
                var cb = document.getElementById(id);

                if (cb.getAttribute("checked") != null && cb.getAttribute("checked") == "checked") {
                    document.getElementById(id).removeAttribute("checked");

                    var counter = 0;
                    $('.cbCancelReservation').each(function () {
                        if (this.getAttribute("checked") == "checked") {
                            counter++;
                        }
                    });
                    if(counter == 0)
                    {
                        document.getElementById("btnCancelRes").setAttribute("disabled", "disabled");
                    }

                }
                else {
                    document.getElementById(id).setAttribute("checked", "checked");
                    document.getElementById("btnCancelRes").removeAttribute("disabled");
                }



            }

            function sendCbIDs(obj, isSingleId, singleResID) {

                var cbCancelReservationIDs = "";
                if (!isSingleId) {
                    $('.cbCancelReservation').each(function () {
                        if (this.getAttribute("checked") == "checked") {
                            cbCancelReservationIDs += this.getAttribute("id") + ",";

                        }
                    });

                }

                else {
                    cbCancelReservationIDs = singleResID;
                }
                    
                alert(cbCancelReservationIDs);
                 __doPostBack(obj.id, cbCancelReservationIDs);


            }

            function CancelThisRes(singelResId, singleResValue) {

                document.getElementById("toCancelDiv").innerHTML = singleResValue;
                OpenCancelReservationOverlay(true);
                document.getElementById("MainContent_btnCancelOK").setAttribute("onclick", "sendCbIDs(this, true, '" + singelResId + "')");
            }



                function ShowMobileDayDiv(dayDivId) {
                    var id = "#" + dayDivId;
                    $('.dayDiv').hide();
                    $(id).show();

                    $('.daySelector').removeClass('selectedMobileDay');
                    $(id + "Selector").addClass('selectedMobileDay');
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

                    if (browserWidth <= 768) {
                        $(".dayDiv").each(function () {
                            this.style.width = "100%";
                            //ShowMobileDayDiv('1_day')
                        });
                    }
                    else {
                        $(".dayDiv").each(function () {
                            this.style.width = widthOfDayDivs;
                            this.style.display = "block";
                        });
                    }

                }

                function loadFirstDay() {
                    var browserWidth = window.innerWidth;
                    if (browserWidth <= 768) {
                        ShowMobileDayDiv('1_day');
                    };
                };

                window.addEventListener('resize', function (event) {
                    fixWidth();
                });

                /*Document.ready för att startskärmen skall vara korrekt.*/
                $(document).ready(fixWidth());
                
                loadFirstDay();



        </script>




    </div>
</asp:Content>
