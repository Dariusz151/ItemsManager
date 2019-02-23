var articlesTable = [];

function SaveArticlesByUser(userID) {
    
    var urlAddress = url + "/api/SmartFridge";

    $.ajax({
        async: false,
        url: urlAddress + "/" + userID,
        type: "GET",
        dataType: "json",
        contentType: "application/json",
        success: function (fridge) {
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

