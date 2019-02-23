﻿var recipes = [];

$('document').ready(fetchRecipes());

function fetchRecipes() {
    asyncGetRecipes().then(function (response) {
        FillRecipesTable(response);
    });
}

function asyncGetRecipes() {
    return fetch(url + '/api/Recipes').then(response => response.json());
}

function FillRecipesTable(json) {
    SaveRecipesToTable(json);

    $("#cont_tableBody").children().remove();
    $("#cont_tableBody").append(" <table class='table table-hover'><tbody id='myTable'></tbody></table> ");

    $.each(json, function (index, item) {
        AddRecipeToTable(index, item);
    });
}

function AddRecipeToTable(index, item) {

    $("#myTable").append("<tr class='row' id='row" + index + "'></tr>")

    $("#row" + index).append("<td class='col-8 col_recipeName'></td");
    $("#row" + index + " .col_recipeName").html(item.name);

    $("#row" + index).append("<td class='col-3 text-center col_date'></td");
    $("#row" + index + " .col_date").html(item.createdAt);

    $("#row" + index).append("<td class='col-lg-1 text-center col_functions'></td>");
    $("#row" + index + " .col_functions").html("<label class='customcheck'><input type='checkbox' autocomplete='off' id='checkbox" + index + "' onchange='checkCheckboxes();'><span class='checkmark'></span></label>");
}

function SaveRecipesToTable(json) {
    $.each(json, function (index, item) {
        recipes[index] = item;
    });
}



