function searchAndFilter() {
    const gridInstance = document.getElementById("employees-list")?.ej2_instances[0];
    const searchBoxInstance = document.getElementById("search-box");
    const comboboxInstance = document.getElementById("positions-combobox")?.ej2_instances[0];
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

function onEditClick(id) {
    console.log(id);
    $('#accountEditModalContent').empty();
    $.ajax({
        url: `/employees/edit/${id}`,
        type: 'GET',
        success: function (data) {
            $('#accountEditModalContent').html(data);
            $('#accountEditModal').modal({
                backdrop: 'static',
                keyboard: false
            }).modal('show');
        },
        error: function (error) {
            alert("Tahrirlash oynasini ochishda muommo bo'ldi \n" + error);
        }
    });
    $(this).find('#accountEditModalContent').empty();
    $(this).off('hidden.bs.modal');
}

function onDeleteClick(id) {
    console.log(id);
    $('#deleteContent').empty();

    $.ajax({
        url: `/employees/delete/${id}`,
        type: 'GET',
        success: function (data) {
            $('#deleteContent').html(data);
            $('#deleteModal').modal('show');
        },
        error: function (error) {
            console.log(error);
            $('#deleteContent').html('<p>Error loading details</p>');
        }
    });
    $(this).find('#deleteContent').empty();
    $(this).off('hidden.bs.modal');
}

function onDetailsClick(id) {
    $('#detailsContent').empty();

    $.ajax({
        url: `/employees/details/${id}`,
        type: 'GET',
        contentType: 'application/x-www-form-urlencoded; charset=UTF-8',
        success: function (data) {
            console.log(data);
            $('#detailsContent').html(data);
            var detailsModal = new bootstrap.Modal(document.getElementById('detailsModal'), {
                backdrop: 'static',
                keyboard: false
            });
            detailsModal.show();
        },
        error: function (error) {
            console.log(error);
            alert("Ma'lumotlarni yuklashda muammo yuz berdi.");
        }
    });
}

$('#detailsModal').on('hidden.bs.modal', function () {
    $(this).find('#detailsContent').empty();
});

$(document).ready(function () {
    $('#openModalButton').on('click', function () {
        $('#accountModalContent').empty();
        $.ajax({
            url: '/employees/create',
            type: 'GET',
            contentType: 'application/x-www-form-urlencoded; charset=UTF-8',
            headers: {
                'RequestVerificationToken': $('input[name="__RequestVerificationToken"]').val()
            },
            success: function (data) {
                $('#accountModalContent').html(data);
                $.getScript('/lib/jquery-validation/dist/jquery.validate.min.js', function () {
                    $.getScript('/lib/jquery-validation-unobtrusive/jquery.validate.unobtrusive.min.js');
                });
                var modal = new bootstrap.Modal(document.getElementById('createModal'), {
                    backdrop: 'static',
                    keyboard: false
                });

                modal.show();

                var phoneTextbox = new ej.inputs.TextBox({
                    value: "+998",
                });
                phoneTextbox.appendTo("#phoneNumberId");

            },
            error: function (error) {
                alert("Yaratish oynasini chiqarishda muommo buldi.");
            }
        });
    });
});

$('#createModal').on('hidden.bs.modal', function () {
    $(this).find('#accountModalContent').empty();
    $(this).off('hidden.bs.modal');
});
