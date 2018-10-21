var recipes = {};

$(document).ready(function () {
    GetAllRecipes();
});

function ChooseRecipe() {
    var dinnerNumbers = WhichRecipe(selectedItems_names, recipes);

    if (dinnerNumbers.length != 0) {
        $('#recipeModal').modal('show');

        $("#modalHeader").text(dinnerNumbers[0]);
        $("#modalContent").text(recipes[dinnerNumbers[0]].recipe);

        // TODO:

        //$(".recipeImg").html("<img src='/recipes/img/kurczak_slodko_kwasny.jpg' height='280px' width='400px' />");
    }
    else {
        toastr.error('There are no recipes with this articles', 'Error');
    }
}

function WhichRecipe(selectedItems_names, recipes) {
    var dinnerNumbers = [];
    var name = "";
    
    for (var k = 1; k <= Object.keys(recipes).length; k++) {
        name = "dinner_" + k;
        if (ContainsAll(selectedItems_names, recipes[name].components))
            dinnerNumbers.push(name);
    }
    return dinnerNumbers;
}

function ContainsAll(array1, array2) {
    return array1.every(elem => array2.indexOf(elem) > -1)
}

function GetAllRecipes() {
    $.getJSON("/recipes/dinners/dinners.json", function (json) {
        //console.log("Recipes " + json);
        recipes = json;
    });
    setTimeout(function () { $("#numberOfRecipes").text(Object.keys(recipes).length); }, 200);
}