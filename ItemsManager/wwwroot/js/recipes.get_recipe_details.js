function GetRecipeDetails(id) {
    asyncGetRecipeDetails(id).then(function (response) {
        FillModal(response, id);
    });
}

function asyncGetRecipeDetails(id) {
    return fetch(url + '/api/Recipes/' + id,
        {
            method: 'GET',
            headers: {
                'Content-Type': 'application/json',
                'Authorization': 'Bearer ' + sessionStorage.getItem('token')
            }
        })
        .then(response => response.json());
}

function FillModal(response, id) {
    $("#recipeModalIngredients").html("");
    $("#recipeModalDescription").html("");

    $("#recipeModalIngredients").html(response.description);

    $("#recipeModalDescription").append("<ul></ul>");
    $.each(response.ingredients, function (index, item) {
        $("#recipeModalDescription ul").append("<li>" + item.name + " " + item.weight +" g</li>");
    });

    $("#testest").html("teststest");


    $("#consumeDiv").append('<button onclick="ConsumeFoodItems(this.value);" value="' + id + '">Consume</button>');
}



