function SendRecipe() {
    var recipeObj = {};

    var recipeName = $("#RecipesModalName").val();
    var recipeDescription = $("#RecipesModalDescription").val();
    var recipeIngredients = [];

    $(".RecipesModalDynamicInputs .form-group").each(function () {
        var ingrDict = {};
        $('input[type=text]', this).each(function () {
            ingrDict.name = $(this).val();
        });
        $('input[type=number]', this).each(function () {
            ingrDict.weight = $(this).val();
        });
        if (!(ingrDict.name == '' || ingrDict.weight == ''))
            recipeIngredients.push(ingrDict);
    });

    recipeObj.ingredients = recipeIngredients;
    recipeObj.name = recipeName;
    recipeObj.description = recipeDescription;
    
    if (recipeObj.ingredients.length == 0 || recipeObj.description == '' || recipeObj.name == '') {
        toastr.error('Some field is empty. Please try again!', 'Error');
    }
    else {
        fetch(url + "/api/Recipes",
        {
            method: 'POST',
            body: JSON.stringify(recipeObj),
            headers: {
                'Content-Type': 'application/json',
                'Authorization': 'Bearer ' + sessionStorage.getItem('token')
            }
        }).then(res => res.json())
            .then(response => {
                var responseStatus = response.statusDescription;
            if (response.statusCode == 400) {
                switch (responseStatus) {
                    case "ItemNameError":
                        toastr.error('Name field is empty!', 'Name Error');
                        break;
                    case "IngredientsError":
                        toastr.error('Ingredients are empty!', 'Ingredients Error');
                        break;
                    case "DescriptionError":
                        toastr.error('Description field is empty!', 'Description Error');
                        break;
                    default:
                        toastr.error('Can\'t create recipe!', 'Unknown Error');
                        break;
                }
            }
            else if (response.statusCode == 200) {
                toastr.success('Successfully created recipe. Refreshing!', 'Success');

                setTimeout(function () {
                    location.reload();
                }, 1000);
            }
            else {
                toastr.error('Can\'t create recipe!', 'Unknown Error');
                console.log(response);
            }
        })
        .catch(error => {
            console.error('Error:', error)
            toastr.error('Can\'t create recipe!', 'Unknown Error');
        });
    }
}

function RecipesModalAddNewInputs() {
    $(".RecipesModalDynamicInputs").append('<div class="form-group"><div class="col-sm-6"><input type="text" class="form-control" placeholder="Ingredient"></div><div class="col-sm-6"><input type="number" class="form-control" placeholder="Weight"></div></div>');
}

function RecipesModalDeleteNewInputs() {
    $(".RecipesModalDynamicInputs div").last().remove();
    $(".RecipesModalDynamicInputs div").last().remove();
    $(".RecipesModalDynamicInputs div").last().remove();
}


