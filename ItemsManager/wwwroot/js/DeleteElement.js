function OnDeleteClick() {

    var ids_to_delete = [];
    var idSelector = function () { return this.id; };
    var checked_checkboxes = $(":checkbox:checked").map(idSelector).get();

    checked_checkboxes.forEach(function (value, index) {
        var number = value.substr("checkbox".length - value.length);
        ids_to_delete[index] = articles_id[number];
    });

    ids_to_delete.forEach(function (value) {
        $.ajax({
            url: urlAddress + "/" + value,
            type: "DELETE",
            contentType: "application/json",
            success: function (item, textStatus, jqXHR) {
                
            },
            error: function (jqXHR, exception) {
              
            }
        });

    });

    LoadData();
}
