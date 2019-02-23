var recipes = {};

function ChooseRecipe() {
    WhichRecipe(selectedArticles);
}

function WhichRecipe(selectedArticles) {

    fetch(url + "/api/CheckRecipe",
        {
            method: 'POST',
            body: JSON.stringify(selectedArticles),
            headers: {
                'Content-Type': 'application/json'
            }
        }).then(res => res.json())
        .then(response => {
           
            $('#recipeModal').modal('show');
            try {
                $("#modalHeader1").text(response[0].name);
                $("#modalContent1").text(response[0].description);
            }
            catch{
                console.log('Cant add first recipe to modal.');
            }
            try {
                $("#modalHeader2").text(response[1].name);
                $("#modalContent2").text(response[1].description);
            }
            catch{
                console.log('Cant add second recipe to modal.');
            }
            try {
                $("#modalHeader3").text(response[2].name);
                $("#modalContent3").text(response[2].description);
            }
            catch{
                console.log('Cant add third recipe to modal.');
            }
        })
        .catch(error => {
            console.error('Error:', error)
            toastr.error('Can\'t find any recipes!', 'Error');
        });
}

//function ContainsAll(array1, array2) {
//    return array1.every(elem => array2.indexOf(elem) > -1);
//}