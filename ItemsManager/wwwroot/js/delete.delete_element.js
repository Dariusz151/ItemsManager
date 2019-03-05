function OnDeleteClick() {
    var urlAddress = url + "/api/FoodItems";

    //var ids_to_delete = [];
    var idToDelete = {};
    var idSelector = function () { return this.id; };
    var checked_checkboxes = $(":checkbox:checked").map(idSelector).get();
    if (checked_checkboxes.length > 1) {
        console.log("You can delete only one row");
    }
    else {
        checked_checkboxes.forEach(function (value, index) {
            var number = value.substr("checkbox".length - value.length);
            idToDelete.Id = articlesTable[number].id;
        });
    }
    fetch(urlAddress,
        {
            method: 'DELETE',
            body: JSON.stringify(idToDelete),
            headers: {
                'Content-Type': 'application/json',
                'Authorization': 'Bearer ' + sessionStorage.getItem('token')
            }
        }).then(response => {
            console.log(response);
            //TODO: error handling
            toastr.success('Successfully deleted article. Refreshing!', 'Success');

            setTimeout(function () {
                location.reload();
            }, 500);
        });
}
