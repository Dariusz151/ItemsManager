var articlesTable = [];

function SaveArticlesByUser() {
    
    var urlAddress = url + "/api/FoodItems";

    $.ajax({
        async: false,
        url: urlAddress,
        type: "GET",
        dataType: "json",
        headers: { "Authorization": "Bearer " + sessionStorage.getItem('token') },
        contentType: "application/json",
        success: function (fridge) {
            console.log(fridge);
            SaveArticles(fridge);
        },
        error: function (error) {
            console.log("error: " + error);
        }
    });
}

function SaveArticles(fridge) {
    $.each(fridge, function (index, item) {
        articlesTable[index] = item;
    });
}

