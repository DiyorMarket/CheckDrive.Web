function onEditDebt(id) {
    $('#debtContent').empty();

    $.ajax({
        url: `/debts/edit/${id}`,
        type: 'GET',
        success: function (data) {
            $('#debtContent').html(data);
            $('#debtModal').modal('show');
        },
        error: function (error) {
            console.log(error);
            $('#debtContent').html('<p>Error loading details</p>');
        }
        
    });
    $(this).find('#debtContent').empty();
    $(this).off('hidden.bs.modal');
}

function searchAndFilter() {
    const gridInstance = document.getElementById("debts-list")?.ej2_instances[0];
    const searchBoxInstance = document.getElementById("search-box");
    const comboboxInstance = document.getElementById("statuses-combobox")?.ej2_instances[0];
    const searchText = searchBoxInstance?.value?.trim();
    const selectedPosition = comboboxInstance?.value?.Text;

    if (!gridInstance) {
        return;
    }

    if (!searchText && !selectedPosition) {
        console.log("clearing filter");
        gridInstance.clearFiltering();
        gridInstance.searchSettings.key = "";
    }

    if (searchText) {
        console.log("searching: ", searchText);
        gridInstance.search(searchText);
    }

    if (selectedPosition) {
        console.log("filtering: ", selectedPosition);
        gridInstance.filterByColumn("FormattedPosition", "equal", selectedPosition);
    }

    gridInstance.refresh();
}
