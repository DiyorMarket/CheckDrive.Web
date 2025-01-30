function onEditClick(id) {
    $('#carEditModalContent').empty();
    console.log(id);
    $.ajax({
        url: "/cars/edit/" + id,
        type: 'GET',
        contentType: 'application/x-www-form-urlencoded; charset=UTF-8',
        headers: {
            'RequestVerificationToken': $('input[name="__RequestVerificationToken"]').val()
        },
        success: function (data) {
            $('#carEditModalContent').html(data);
            $('#carEditModal').modal({
                backdrop: 'static',
                keyboard: false
            }).modal('show');

        },
        error: function (error) {
            alert("Tahrirlash oynasini ochishda muommo bo'ldi \n" + error);
        }
    });
    $(this).find('#carEditModalContent').empty();
    $(this).off('hidden.bs.modal');

}

function onDeleteClick(id) {
    $('#deleteContent').empty();
    $.ajax({
        url: "/cars/delete/" + id,
        type: 'GET',
        success: function (data) {
            $('#deleteContent').html(data);
            $('#deleteModal').modal('show');
        },
        error: function (error) {
            alert("O'chirish oynasini ochishda muommo bo'ldi \n" + error);
        }
    });
    $(this).find('#deleteContent').empty();
    $(this).off('hidden.bs.modal');
}

function onDetailsClick(id) {
    $('#detailsContent').empty();
    console.log(id);
    $.ajax({
        url: `/cars/details/${id}`,
        type: 'GET',
        contentType: 'application/x-www-form-urlencoded; charset=UTF-8',
        success: function (data) {
            $('#detailsContent').html(data);
            var detailsModal = new bootstrap.Modal(document.getElementById('detailsModal'), {
                backdrop: 'static',
                keyboard: false
            });
            detailsModal.show();
        },
        error: function (error) {
            alert("Ma'lumotlarni yuklashda muammo yuz berdi.");
        }
    });
}

$('#detailsModal').on('hidden.bs.modal', function () {
    $(this).find('#detailsContent').empty();
});

$(document).ready(function () {
    $('#openCarCreateModal').on('click', function () {
        $('#carmodalContent').empty();
        $.ajax({
            url: '/cars/create',
            type: 'GET',
            contentType: 'application/x-www-form-urlencoded; charset=UTF-8',
            headers: {
                'RequestVerificationToken': $('input[name="__RequestVerificationToken"]').val()
            },
            success: function (data) {
                $('#carmodalContent').html(data);
                $('#carModal').modal({
                    backdrop: 'static',
                    keyboard: false
                }).modal('show');
                $.getScript('/lib/jquery-validation/dist/jquery.validate.min.js', function () {
                    $.getScript('/lib/jquery-validation-unobtrusive/jquery.validate.unobtrusive.min.js');
                });

            },
            error: function (error) {
                alert("Error occurred while saving car.");
            }
        });
    });
    $('#carModal').on('hidden.bs.modal', function () {
        $(this).find('#carmodalContent').empty();
        $(this).off('click');
    });
    $('#carModal').on('click', '.btn-close', function () {
        $('#carModal').modal('hide');
    });
});

function validateForm() {
    var fuelTankCapacity = document.getElementById("fuelTankCapacity").value;
    var remainingFuel = document.getElementById("remainingFuel").value;

    if (parseFloat(remainingFuel) > parseFloat(fuelTankCapacity)) {
        alert("Avtomobil yoqilg'i qoldig'i yoqilg'i baki sig'imidan katta bo'lmaydi.");
        return false;
    }
    return true;
}

function validateRemainingFuel() {
    var fuelTankCapacity = document.getElementById("fuelTankCapacity").value;
    var remainingFuel = document.getElementById("remainingFuel").value;

    if (fuelTankCapacity === "") {
        alert("Siz avval 'Yoqilg'i baki sig'imi' ga qiymat kiritishingiz kerak");
        document.getElementById("remainingFuel").value = "";
        return false;
    }

    if (parseFloat(remainingFuel) > parseFloat(fuelTankCapacity)) {
        alert("Avtomobil yoqilg'i qoldig'i yoqilg'i baki sig'imidan katta bo'lmaydi.");
        return false;
    }
    return true;
}

function calculateMonthlyDistance() {
    var oneYearMediumDistance = document.getElementById("oneYearMediumDistance").value;
    var meduimFuelConsumption = document.getElementById("meduimFuelConsumption").value;

    if (oneYearMediumDistance === "" || isNaN(oneYearMediumDistance) || parseFloat(oneYearMediumDistance) <= 0) {
        document.getElementById("monthlyDistance").value = "";
        document.getElementById("yearlyMediumFuel").value = "";
        document.getElementById("monthlyMediumFuel").value = "";
        return;
    }

    if (meduimFuelConsumption === "" || isNaN(meduimFuelConsumption) || parseFloat(meduimFuelConsumption) <= 0) {
        alert("Siz avval '100 km uchun yoqilg'i me'yori [litr]' ga qiymat kiritishingiz kerak");
        document.getElementById("oneYearMediumDistance").value = "";
        return;
    }

    oneYearMediumDistance = parseFloat(oneYearMediumDistance);
    meduimFuelConsumption = parseFloat(meduimFuelConsumption);

    var monthlyDistance = oneYearMediumDistance / 12;
    document.getElementById("monthlyDistance").value = monthlyDistance.toFixed(1);

    var yearlyMediumFuel = oneYearMediumDistance * (meduimFuelConsumption / 100);
    document.getElementById("yearlyMediumFuel").value = yearlyMediumFuel.toFixed(1);

    var monthlyMediumFuel = yearlyMediumFuel / 12;
    document.getElementById("monthlyMediumFuel").value = monthlyMediumFuel.toFixed(1);
}


function onChangeSearch() {
    const searchInput = document.getElementById("search-box").value.trim();
    const gridElement = document.getElementById("cars-list");
    const gridInstance = gridElement?.ej2_instances?.[0];

    if (!gridInstance) {
        console.error("Grid instance not found.");
        return;
    }

    if (searchInput) {
        gridInstance.search(searchInput);
    } else {
        gridInstance.searchSettings.key = "";
    }

    gridInstance.refresh();
}