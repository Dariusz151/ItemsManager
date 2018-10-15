var recipes = {};

$(document).ready(function () {
    GetAllRecipes();
});

function ChooseRecipe() {
    var selectedItems = [];
    var idSelector = function () { return this.id; };
    var checked_checkboxes = $(":checkbox:checked").map(idSelector).get();

    checked_checkboxes.forEach(function (value, index) {
        var number = value.substr("checkbox".length - value.length);
        selectedItems[index] = articlesTable[number].id;
    });


    selectedItems.forEach(function (value, index) {
        selectedItems_names[index] = articlesTable[articlesTable.findIndex(x => x.id === value)].articleName;
    });

    var dinnerNumbers = WhichRecipe(selectedItems_names, recipes);
}

function ContainsAll(array1, array2) {
    return array1.every(elem => array2.indexOf(elem) > -1)
}

function GetAllRecipes() {

    $.getJSON("/recipes/dinners/dinners.json", function (json) {
        console.log(json);
        recipes = json;
    });
}

function WhichRecipe(selectedItems_names, recipes) {
    var dinnerNumbers = [];
    var name = "";
    console.log("dlugos " + Object.keys(recipes).length);
    for (var k = 1; k <= Object.keys(recipes).length; k++) {
        name = "dinner_" + k;
        if (ContainsAll(selectedItems_names, recipes[name].components))
            dinnerNumbers.push(name);
        console.log(recipes[name].components);
    }
    return dinnerNumbers;
}