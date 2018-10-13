﻿var state = false;

function ChangeSortState(property) {
    state = !state;
    SortTableAlph(state);
}

function SortTableAlph(state) {
    
    switch (state) {
        case false:
            propName = "-articleName";
            break;
        case true:
            propName = "articleName";
            break;
    }
    articlesTable.sort(dynamicSort(propName));
    
    $("#cont_tableBody").children().remove();
    $("#cont_tableBody").append(" <table class='table table-hover'><tbody id='myTable'></tbody></table> ");
    $.each(articlesTable, function (index, item) {
        AddFridgeItemToTable(index, item);
    });
}


function dynamicSort(property) {
    var sortOrder = 1;

    if (property[0] === "-") {
        sortOrder = -1;
        property = property.substr(1);
    }

    return function (a, b) {
        if (sortOrder == -1) {
            return b[property].toString().localeCompare(a[property].toString());
        } else {
            return a[property].toString().localeCompare(b[property].toString());
        }
    }
}