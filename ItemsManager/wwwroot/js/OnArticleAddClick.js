function OnAddClick() {
    var userID = localStorage.getItem("userID");

    var urlAddress = "https://localhost:44378/api/SmartFridge";

    var data = {};
    data.ArticleName = $("#articleNameInput").val();
    data.Quantity = $("#quantityInput").val();
    data.Weight = $("#weightInput").val();
    data.UserID = parseInt(userID);
    data.CreatedAt = '2018-01-01 00:00:00'; //DEFAULT, Controller overrides with DateTime.Now
    data.CategoryID = 1014;
    
    $.ajax({
        async: false,
        url: urlAddress,
        type: "POST",
        contentType: "application/json",
        dataType: "text",
        data: JSON.stringify(data),
        success: function (item, textStatus, jqXHR) {
            console.log('added');
            location.reload();
        },
        error: function (jqXHR, exception) {
            console.log("error: " + jqXHR.status);
        }
    });
}