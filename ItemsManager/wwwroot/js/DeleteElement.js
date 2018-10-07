function OnDeleteClick() {
    var urlAddress = "https://localhost:44378/api/SmartFridge";

    var ids_to_delete = [];
    var idSelector = function () { return this.id; };
    var checked_checkboxes = $(":checkbox:checked").map(idSelector).get();

    checked_checkboxes.forEach(function (value, index) {
        var number = value.substr("checkbox".length - value.length);
        ids_to_delete[index] = articlesTable[number];
    });

    console.log(ids_to_delete);
    console.log(ids_to_delete.length);

    for (var i = 0; i < ids_to_delete.length; i++) {
        $.ajax({
            async:false,
            url: urlAddress + "/" + ids_to_delete[i].id,
            type: "DELETE",
            contentType: "application/json",
            success: function (item, textStatus, jqXHR) {
                console.log('deleted');
            },
            error: function (jqXHR, exception) {
                console.log('error in delete');
            }
        });
    }
    window.location.replace("https://localhost:44378/static/index.html");
}
