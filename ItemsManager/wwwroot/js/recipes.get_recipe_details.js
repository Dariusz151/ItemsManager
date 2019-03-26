function GetRecipeDetails(id) {
    asyncGetRecipeDetails(id).then(function (response) {
        FillModal(response);
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

function FillModal(response) {
    $("#recipeModalIngredients").html("");
    $("#recipeModalDescription").html("");

    $("#recipeModalIngredients").html(response.description);

    $("#recipeModalDescription").append("<ul></ul>");
    $.each(response.ingredients, function (index, item) {
        $("#recipeModalDescription ul").append("<li>" + item.name + " " + item.weight +" g</li>");
    });
}



