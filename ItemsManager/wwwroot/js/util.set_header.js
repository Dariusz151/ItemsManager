var userName = sessionStorage.getItem("userName");

$(document).ready(function () {
    if (userName != null) {
        var htmlString = '<a class="h2 px-3 fl_right">Hello, ' + userName + '</a>';
        $(".userName").empty();
        $(".userName").html(htmlString);
    }
});

