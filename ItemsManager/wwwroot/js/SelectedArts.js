var selectedItems_names = [];
var selectedItems = [];

function checkCheckboxes() {
    selectedItems_names = [];
    selectedItems = [];

    var idSelector = function () { return this.id; };
    var checked_checkboxes = $(":checkbox:checked").map(idSelector).get();

    checked_checkboxes.forEach(function (value, index) {
        var number = value.substr("checkbox".length - value.length);
        selectedItems[index] = articlesTable[number].id;
    });

    selectedItems.forEach(function (value, index) {
        selectedItems_names[index] = articlesTable[articlesTable.findIndex(x => x.id === value)].articleName;
    });
    EditList(selectedItems_names);
   
}

function EditList(selectedItems_names) {
    console.log(selectedItems_names);
    $("ul").parent().empty();

    $("#selectedItemsList").append("<ul></ul>");
    selectedItems_names.forEach(function (item) {
        $("ul").append("<h2>" + item + "</h2>")
    });
    
}