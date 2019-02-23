var selectedArticles = [];

function checkCheckboxes() {
    var selectedItems_names = [];
    var selectedItems = [];
    var selectedItems_weight = [];
    var article = {};
    selectedArticles = [];
    
    var idSelector = function () { return this.id; };
    var checked_checkboxes = $(":checkbox:checked").map(idSelector).get();

    checked_checkboxes.forEach(function (value, index) {
        var number = value.substr("checkbox".length - value.length);
        selectedItems[index] = articlesTable[number].id;
    });

    selectedItems.forEach(function (value, index) {
        article.Name = articlesTable[articlesTable.findIndex(x => x.id === value)].articleName;
        article.Weight = articlesTable[articlesTable.findIndex(x => x.id === value)].quantity
            * articlesTable[articlesTable.findIndex(x => x.id === value)].weight;

        selectedArticles[index] = ReturnSingleArticle(article.Name, article.Weight);
    });

    EditList(selectedArticles);
}

function ReturnSingleArticle(name, weight) {
    var art = {};
    art.Name = name;
    art.Weight = weight;
    return art;
}

function EditList(selectedArticles) {
    $("ol").parent().empty();

    $("#selectedItemsList").append("<ol></ol>");
    selectedArticles.forEach(function (item) {
        $("ol").append("<li><h2>" + item.Name + "</li></h2>")
    });
}