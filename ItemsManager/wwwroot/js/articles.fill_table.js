function FillTable(articlesTable) {

    SaveArticlesByUser();
    
    $("#cont_tableBody").children().remove();
    $("#cont_tableBody").append(" <table class='table table-hover'><tbody id='myTable'></tbody></table> ");
   
    $.each(articlesTable, function (index, item) {
        AddFridgeItemToTable(index, item);
    });
}

function AddFridgeItemToTable(index, item) {
   
    $("#myTable").append("<tr class='row' id='row" + index + "'></tr>")

    $("#row" + index).append("<td class='col-7 col_articleName'></td");
    $("#row" + index + " .col_articleName").html(item.name);

    $("#row" + index).append("<td class='col-2 text-center col_quantity'></td");
    $("#row" + index + " .col_quantity").html(item.quantity);

    $("#row" + index).append("<td class='col-2 text-center col_weight'></td");
    $("#row" + index + " .col_weight").html(item.weight);
    
    $("#row" + index).append("<td class='col-lg-1 text-center col_functions'></td>");
    $("#row" + index + " .col_functions").html("<label class='customcheck'><input type='checkbox' autocomplete='off' id='checkbox" + index + "' onchange='checkCheckboxes();'><span class='checkmark'></span></label>");
}

$('document').ready(FillTable(articlesTable));

