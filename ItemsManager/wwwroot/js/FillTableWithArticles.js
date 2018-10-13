function FillTable(articlesTable) {
    var userID = localStorage.getItem("userID");
    SaveArticlesByUser(userID);

    console.log(articlesTable);

    $("#cont_tableBody").children().remove();
    $("#cont_tableBody").append(" <table class='table table-hover'><tbody id='myTable'></tbody></table> ");

    $.each(articlesTable, function (index, item) {
        AddFridgeItemToTable(index, item);
    });
}

function AddFridgeItemToTable(index, item) {
   
    $("#myTable").append("<tr class='row' id='row" + index + "'></tr>")

    $("#row" + index).append("<td class='col col_articleName'></td");
    $("#row" + index + " .col_articleName").html(item.articleName);

    $("#row" + index).append("<td class='col col_quantity'></td");
    $("#row" + index + " .col_quantity").html(item.quantity);

    $("#row" + index).append("<td class='col col_weight'></td");
    $("#row" + index + " .col_weight").html(item.weight);

    $("#row" + index).append("<td class='col-lg-1 col_functions'></td>");
    $("#row" + index + " .col_functions").html("<input class='form-check-input position-static' type='checkbox' id='checkbox" + index + "' value='option1' aria-label='aria'>");
    
}

$('document').ready(FillTable(articlesTable));

