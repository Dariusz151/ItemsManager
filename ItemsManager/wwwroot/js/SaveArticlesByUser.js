var articlesTable = [];

function SaveArticlesByUser(userID) {
    
    var urlAddress = "https://localhost:44378/api/SmartFridge";

    $.ajax({
        async: false,
        url: urlAddress + "/" + userID,
        type: "GET",
        dataType: "json",
        contentType: "application/json",
        success: function (fridge) {
            SaveArticles(fridge);
        },
        error: function (request, message, error) {
            console.log("error: " + request.status);
            console.log("error: " + request.statusText);
        }
    });
}

function SaveArticles(fridge) {
    $.each(fridge, function (index, item) {
        articlesTable[index] = item;
    });
}

