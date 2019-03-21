function OnAddClick() {
    //var urlAddress = url + "/api/SmartFridge";

    var data = {};
    data.Name = $("#ArticleModalName").val();
    data.Quantity = $("#QuantityModal").val();
    data.Weight = $("#WeightModal").val();
    data.CategoryID = 9;
    
    if (data.Weight == '') {
        data.Weight = 1;
    }

    if (data.Quantity == '') {
        toastr.error('Quantity field is empty!', 'Quantity Error');
    }
    else {
        fetch(url + "/api/FoodItems",
            {
                method: 'POST',
                body: JSON.stringify(data),
                headers: {
                    'Content-Type': 'application/json',
                    'Authorization': 'Bearer ' + sessionStorage.getItem('token')
                }
            }).then(res => res.json())
            .then(response => {

                //TODO: error handling
                toastr.success('Successfully created article. Refreshing!', 'Success');

                setTimeout(function () {
                    location.reload();
                }, 500);

                //var responseStatus = response.statusDescription;
                //if (response.statusCode == 400) {
                    
                //    switch (responseStatus) {
                //        case "ItemQuantityError":
                //            toastr.error('Item quantity field is empty!', 'Item quantity Error');
                //            break;
                //        case "ArticleNameError":
                //            toastr.error('Article name is empty!', 'Article name Error');
                //            break;
                //        case "CategoryError":
                //            toastr.error('Category field is empty!', 'Category Error');
                //            break;
                //        default:
                //            toastr.error('Can\'t create article!', 'Unknown Error');
                //            break;
                //    }
                //}
                //else if (response.statusCode == 200) {
                //    toastr.success('Successfully created article. Refreshing!', 'Success');

                //    setTimeout(function () {
                //        location.reload();
                //    }, 500);
                //}
                //else {
                //    toastr.error('Can\'t create article!', 'Unknown Error');
                //    console.log(response);
                //}
            })
            .catch(error => {
                console.error('Error:', error)
                toastr.error('Can\'t create article!', 'Unknown Error');
            });
    }
    
    //$.ajax({
    //    async: false,
    //    url: urlAddress,
    //    type: "POST",
    //    contentType: "application/json",
    //    dataType: "text",
    //    data: JSON.stringify(data),
    //    success: function (item, textStatus, jqXHR) {
    //        toastr.success('Successfully added ' + data.ArticleName+ '.', 'Success');
    //        location.reload();
    //    },
    //    error: function (jqXHR, exception) {
    //        toastr.error('Can\'t add article!', 'Error');
    //    }
    //});
}