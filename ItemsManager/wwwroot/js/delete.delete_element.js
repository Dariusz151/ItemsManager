function OnDeleteClick() {
    var urlAddress = url + "/api/SmartFridge";

    var ids_to_delete = [];
    var idSelector = function () { return this.id; };
    var checked_checkboxes = $(":checkbox:checked").map(idSelector).get();

    checked_checkboxes.forEach(function (value, index) {
        var number = value.substr("checkbox".length - value.length);
        ids_to_delete[index] = articlesTable[number];
    });

    for (var i = 0; i < ids_to_delete.length; i++) {
        $.ajax({
            async:false,
            url: urlAddress + "/" + ids_to_delete[i].id,
            type: "DELETE",
            contentType: "application/json",
            success: function (item, textStatus, jqXHR) {
                toastr.success('Successfully deleted item! Refreshing', 'Success');
                setTimeout(function () {
                    location.reload();
                }, 500);
            },
            error: function (jqXHR, exception) {
                toastr.error('Error while deleting item!', 'Error');
            }
        });
    }
}
