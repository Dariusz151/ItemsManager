function asyncConsumeFoodItems(id) {
    var recipe = {};
    recipe.RecipeId = id;

    return fetch(url + '/api/Recipes/' + id,
        {
            method: 'PUT',
            body: JSON.stringify(recipe), 
            headers: {
                'Content-Type': 'application/json',
                'Authorization': 'Bearer ' + sessionStorage.getItem('token')
            }
        })
        .then(response => response.json());
}

function ConsumeFoodItems(recipeId) {
    asyncConsumeFoodItems(recipeId).then(function (response) {
        //FillModal(response, id);
        console.log(response);
    });
}


