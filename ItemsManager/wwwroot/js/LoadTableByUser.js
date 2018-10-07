var articlesTable = [];
var id = localStorage.getItem("userID");

function LoadTableByUser(userID) {
    
    var urlAddress = "https://localhost:44378/api/SmartFridge";

    $.ajax({
        url: urlAddress + "/" + userID,
        type: "GET",
        dataType: "json",
        contentType: "application/json",
        success: function (fridge) {
            SaveArticles(fridge);
            FillTable(fridge);
        },
        error: function (request, message, error) {
            console.log("error: " + request.status);
            console.log("error: " + request.statusText);
        }
    });
}

function SaveArticles(fridge) {
    $.each(fridge, function (index, item) {
        articlesTable[index] = item;
    });
}

function FillTable(fridge) {
    $("#cont_tableBody").children().remove();

    $.each(fridge, function (index, item) {
        AddFridgeItemToTable(index, item);
    });
}

function AddFridgeItemToTable(index, item) {
    $("#cont_tableBody").append("<div class='row' id='row" + index + "'></div>");

    $("#row" + index).append("<div class='col col_articleName'></div>");
    $("#row" + index + " .col_articleName").html(item.articleName);

    $("#row" + index).append("<div class='col col_quantity'></div>");
    $("#row" + index + " .col_quantity").html(item.quantity);

    $("#row" + index).append("<div class='col col_weight'></div>");
    $("#row" + index + " .col_weight").html(item.weight);

    $("#row" + index).append("<div class='col-lg-1 col_functions'></div>");
    $("#row" + index + " .col_functions").html("<input class='form-check-input position-static' type='checkbox' id='checkbox" + index + "' value='option1' aria-label='aria'>");
}

LoadTableByUser(id);

