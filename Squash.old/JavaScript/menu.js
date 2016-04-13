$(function () {
    $("a[href=#menuExpand]").click(function (e) {
        showHideMobile();
        e.preventDefault();
    });

    $(".mobileDimmer").click(function () {
        showHideMobile();
    });
});

function showHideMobile() {
    $(".mobileMenu").toggleClass("menuOpen");
    $(".mobileBodyWrapper").toggleClass("menuOpen");
    $(".mobileDimmer").toggle();
}