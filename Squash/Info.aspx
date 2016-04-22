<%@ Page Title="Information" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Info.aspx.cs" Inherits="Squash.Info" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div id="infoDiv">
    <h2><%: Title %></h2>
        <div class="form-horizontal">
            <h4>Bokning, kodlås och abonnemang</h4>
            <hr />
            <p class="foldedText" id="shortInfo">Squashhallen i Östersund drivs ideellt av Östersunds squashförening. Det innebär att vi inte har möjlighet att ha någon personal anställd. 
                För att detta ska fungera krävs att samtliga spelare som utnyttjar någon av våra banor tar ansvar och följer de regler som vill tillsammans satt upp. <br /><br />
            Vi vill också påminna dig om att det är den som bokar en tid som är personligt ansvarig. Det innebär att du inte under några omständigheter ska lämna ifrån dig koden eller dina inloggningsuppgifter till någon annan.
            Det är även du som har bokat tiden som är ansvarig för att tiden betalas direkt i anslutning till att du spelar.
            Om vi ser att någon missbrukar detta ansvar, så har vi rätten att stänga av spelare från vidare spel i hallen.
            </p>


            <div id="bookingFold" class="headerDiv" onclick="toggleSection('booking')">
                <p class="infoHeaders">Bokningar</p>
            </div>
            <div id="booking" class="foldDiv">
                <p class="foldedText">
                    För att kunna boka en tid måste du vara registrerad som användare. 
                Det gör du i menyn under "Bli medlem". När du fyllt i dina uppgifter skickas de till en administratör. <br /><br />
                Det tar sedan upp till en vecka innan ditt konto aktiverat och du kan börja använda det. Om du vill komma i kontakt med våra administratörer klicka längst ned på "Kontakt / Hitta hit". <br /><br />
                Innan du går in och bokar en tid kan du klicka i menyn på "Spelschema & Bokningar". Där ser du vilka tider de kommande 8 dagarna som är bokningsbara. 
                För att boka loggar du in på ditt konto och sedan klicka i menyn på "Spelschema & Bokningar". Välj sedan den tid du vill boka. Du kan max ha fem aktiva bokningar samtidigt. <br /><br /> 
                Du kan inte använda ett företagskonto (det konto som finns för fasta företagsabonnemang), för att boka strötider, då detta inte är personligt.
                </p>
            </div>

            <div id="codeLockFold" class="headerDiv" onclick="toggleSection('codeLock')">
                <p class="infoHeaders">Kodlås</p>
            </div>
            <div id="codeLock" class="foldDiv">
                <p class="foldedText">
                    För att komma in i squashhallen behöver du den aktuella koden. Koden är personlig, du får av den anledningen inte under några omständigheter lämna ifrån dig koden till någon annan.<br /><br />
                    Koden byts med jämna mellanrum. Vi har inga fasta tider då vi byter kod. Vi har även gått ifrån systemet med fasta koder för dem som har abonnemang. 
                    Det innebär att du alltid ska kontrollera koden innan du åker iväg och spelar. <br /><br />
                    Du får enbart tillgång till koden om du har en tid bokad den aktuella dagen. 
                    Det innebär att du inte kan gå in dagen före aktuell speltid och kontrollera kod. Om du väljer att gå in och kontrollera koden, så förbrukas din möjlighet att gå in och avboka tiden. 
                    Har du bokat en tid och väljer att titta på koden, så är du skyldig att betala för den bokade tiden.
                </p>
            </div>

            <div id="subscriptionFold" class="headerDiv" onclick="toggleSection('subscription')">
                <p class="infoHeaders">Abonnemang</p>
            </div>
            <div id="subscription" class="foldDiv">
                <p class="foldedText">
                    Ett abonnemang innebär att du spelar på en fast tid varje vecka. Abonnemangen löper under 6 månader och gäller för en bana i en timme.<br />
                    <span class="subscriptionHeaders">Abonnemang passar dig som,</span><br />
                    • Spelar regelbundet minst en gång per vecka
                    <br />
                    • Vill slippa att boka tid varje gång du ska spela
                    <br />
                    • Vill vara garanterad en tid att spela på
                    <br />
                    • Är företagare/representerar ett företag och vill dela en tid med dina kollegor/medarbetare
                    Vi har tre olika typer av abonnemang.
                    <br />

                    <span class="subscriptionHeaders">Privatabonnemang</span><br />

                    Ett privatabonnemang innebär att du har en fast tid bokad varje vecka. Utöver den fasta tiden har du möjlighet att boka och spela på lediga strötider till ett reducerat pris.

                    Detta under förutsättning att de är lediga mindre än sex timmar innan du ska spela. 
                    Vill du vara säker på att spela på en ledig tid mer än sex timmar innan du ska spela, så gäller vanliga regler för bokning.
                    <br />
                    • Abonnemanget gäller sex månader
                    <br />
                    • Du har en fast tid bokad varje vecka
                    <br />
                    • Ett abonnemang gäller en timme på en bana per vecka
                    <br />
                    • Du har möjlighet att boka lediga strötider, under vissa förutsättningar
                    <br />
                    Kostnad för ett Privatabonnemang är 1400 kr per sex månader.
                    <br />

                    <span class="subscriptionHeaders">Företagsabonnemang</span><br />

                    Ett företagsabonnemang innebär att ni på ett företag abonnerar en fast tid på en bana. Det är sedan fritt fram för vem som helst på företaget att utnyttja denna tid.
                    <br />
                    • Abonnemanget gäller sex månader
                    <br />
                    • Du har en fast tid bokad varje vecka
                    <br />
                    • Ett abonnemang gäller en timme på en bana per vecka
                    <br />
                    Kostnad för ett företagsabonnemang är XXXX per sex månader.<br />

                    <span class="subscriptionHeaders">Rörligt företagsabonnemang</span><br />

                    Ett rörligt företagsabonnemang innebär att ni på företaget har möjlighet att spela en timme per vecka. Tiden är dock inte fast, ni kan utnyttja den då det passar er. 
                    Abonnemanget är lämpligt för er som inte har möjlighet att på förhand veta när ni har möjlighet att spela. Tiden då ni har möjlighet att boka är vardagar 06.00 - 16.00.
                    <br />
                    <br />
                    • Abonnemanget gäller sex månader
                    <br />
                    • Ni har möjlighet att boka valfri tid en gång per vecka
                    <br />
                    • Tiden för bokning är vardagar 06.00 - 16.00
                    <br />
                    • Ett abonnemang gäller en timme på en bana per vecka
                    <br />

                    <span class="subscriptionHeaders">Strötider</span><br />

                    Har du inte ett abonnemang eller vill du spela fler gånger i veckan. Då kan du utnyttja de strötider som finns. Mer information om det finns under fliken Bokning och kodlås.
                    <br />
                    Kostnaden för en strötid är: 100 kr per bana och timme.
                </p>
            </div>






            <asp:HiddenField ID="hfbookingFolded" runat="server" />
            <asp:HiddenField ID="hfcodeLockFolded" runat="server" />
            <asp:HiddenField ID="hfsubscriptionFolded" runat="server" />

            <script>
                function toggleSection(section) {
                    if ($("#MainContent_hf" + section + "Folded").val() == "true") {
                        $("#" + section).show();
                        $("#MainContent_hf" + section + "Folded").val("false");
                    }
                    else {
                        $("#" + section).hide();
                        $("#MainContent_hf" + section + "Folded").val("true");
                    }
                }

                //function isPostBack() {
                //    return document.referrer.indexOf(document.location.href) > -1;
                //}

                //if (isPostBack()) {
                //    $("foldDiv").each(function () {
                //        if ($("MainContent_hf" + this.id + "Folded").val("true"))
                //        {
                //        $(this.id).hide();
                //        $("#MainContent_hf" + section + "Folded").val("true");
                //        }
                //        else
                //        {
                //            $(this.id).show();
                //        }
                //    });

                //}
                //else {

                $(".foldDiv").each(function () {
                    $("#" + this.id).hide();
                    $("#MainContent_hf" + this.id + "Folded").val("true");
                });
                //}
            </script>

        </div>
    </div>
</asp:Content>
