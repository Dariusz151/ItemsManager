var recipes = [];

$('document').ready(fetchRecipes());

function fetchRecipes() {
    asyncGetRecipes().then(function (response) {
        FillRecipesTable(response);
    });
}

function asyncGetRecipes() {

    return fetch(url + '/api/Recipes',
        {
            method: 'GET',
            headers: {
                'Content-Type': 'application/json',
                'Authorization': 'Bearer ' + sessionStorage.getItem('token')
            }
        })
        .then(response => response.json());
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
    //console.log(item.id);


    $("#myTable").append("<tr class='row' id='row" + index + "'></tr>")

    $("#row" + index).append("<td class='col-11 col_recipeName'></td");
    //$("#row" + index + " .col_recipeName").html("<a data-toggle='modal' href='' data-target='#myRecipeModal' id='recipeDetailsLink'>" + item.name + "</a>");
    $("#row" + index + " .col_recipeName").html("<b>" + item.name + "</b>");

    $("#row" + index).append("<td class='col-lg-1 text-center col_functions'></td>");
    $("#row" + index + " .col_functions").html('<button data-toggle="modal" data-target="#myRecipeModal" onclick="GetRecipeDetails(this.value);" value="' + item.id + '">Details</button>');
    
    //$("#row" + index).append("<td class='col-lg-1 text-center col_functions'></td>");
    //$("#row" + index + " .col_functions").html("<label class='customcheck'><input type='checkbox' autocomplete='off' id='checkbox" + index + "' onchange='checkCheckboxes();'><span class='checkmark'></span></label>");
}

function SaveRecipesToTable(json) {
    $.each(json, function (index, item) {
        recipes[index] = item;
    });
}



