var recipes = {};

function ChooseRecipe() {
    WhichRecipe(selectedArticles);
}

function WhichRecipe(selectedArticles) {

    asyncGetChosenRecipe().then(function (response) {
        if (response.length > 0) {
            $("#modal_chosenRecipeDescription").html("");
            $("#modal_chosenRecipeIngredients").html("");

            $("#modal_chosenRecipeDescription").html(response[0].description);
            $("#modal_chosenRecipeIngredients").append("<ul></ul>");
            $.each(response[0].ingredients, function (index, item) {
                $("#modal_chosenRecipeIngredients ul").append("<li>" + item.name + " " + item.weight + " g</li>");
            });
        }
        else {
            $("#modal_chosenRecipeDescription").html("");
            $("#modal_chosenRecipeIngredients").html("");
            $("#modal_chosenRecipeDescription").html("No recipes found.");
        }
    });
}

function asyncGetChosenRecipe() {
    return fetch(url + "/api/FindRecipe",
        {
            method: 'POST',
            body: JSON.stringify(selectedArticles),
            headers: {
                'Content-Type': 'application/json',
                'Authorization': 'Bearer ' + sessionStorage.getItem('token')
            }
        }).then(res => res.json())
        .catch(error => {
            console.error('Error:', error)
            toastr.error('Can\'t find any recipes!', 'Error');
        });
}

//function ContainsAll(array1, array2) {
//    return array1.every(elem => array2.indexOf(elem) > -1);
//}