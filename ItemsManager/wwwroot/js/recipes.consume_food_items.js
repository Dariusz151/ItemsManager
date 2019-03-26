function asyncConsumeFoodItems(ingredients_to_consume) {
  
    return fetch(url + '/api/Recipes',
        {
            method: 'PUT',
            body: JSON.stringify(ingredients_to_consume), 
            headers: {
                'Content-Type': 'application/json',
                'Authorization': 'Bearer ' + sessionStorage.getItem('token')
            }
        })
        .then(response => response.json());
}


function ConsumeFoodItems() {
    asyncConsumeFoodItems(ingredients_to_consume).then(function (response) {
        if (response.statusDescription == "FoodItemsConsumed") {
            toastr.success('Successfully consumed articles. Refreshing!', 'Success');

            setTimeout(function () {
                location.reload();
            }, 500);
        }
        else {
            toastr.error('Failed to consume articles!', 'Error');
        }
       
    });
}


