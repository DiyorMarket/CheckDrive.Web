$(document).ready(function () {
    $('#openOilCreateModal').on('click', function () {
        console.log(346);
        $('#oilModalContent').empty();
        $.ajax({
            url: '/oilmarks/create',
            type: 'GET',
            contentType: 'application/x-www-form-urlencoded; charset=UTF-8',
            headers: {
                'RequestVerificationToken': $('input[name="__RequestVerificationToken"]').val()
            },
            success: function (data) {
                console.log(data);
                $('#oilModalContent').html(data);
                $.getScript('/lib/jquery-validation/dist/jquery.validate.min.js', function () {
                    $.getScript('/lib/jquery-validation-unobtrusive/jquery.validate.unobtrusive.min.js');
                });
                var modal = new bootstrap.Modal(document.getElementById('createOilModal'), {
                    backdrop: 'static',
                    keyboard: false
                });
                modal.show();
            },

            error: function (error) {
                alert("Yaratish oynasini chiqarishda muommo buldi.");
            }
        });
    });
});

$('#createOilModal').on('hidden.bs.modal', function () {
    $(this).find('#oilModalContent').empty();
    $(this).off('hidden.bs.modal');
});

function onEditClick(id) {
    $('#oilEditModalContent').empty();
    $.ajax({
        url: "/oilmarks/edit/" + id,
        type: 'GET',
        success: function (data) {
            $('#oilEditModalContent').html(data);
            $('#oilEditModal').modal({
                backdrop: 'static',
                keyboard: false
            }).modal('show');
        },
        error: function (error) {
            alert("Tahrirlash oynasini ochishda muommo bo'ldi \n" + error);
        }
    });
    $(this).find('#oilEditModalContent').empty();
    $(this).off('hidden.bs.modal');
}

function onDeleteClick(id) {
    $('#deleteOilContent').empty();
    $.ajax({
        url: "/oilmarks/delete/" + id,
        type: 'GET',
        success: function (data) {
            $('#deleteOilContent').html(data);
            $('#deleteOilModal').modal('show');
        },
        error: function () {
            $('#deleteOilContent').html('<p>Error loading details</p>');
        }
    });

    $(this).find('#deleteOilContent').empty();
    $(this).off('hidden.bs.modal');
}
