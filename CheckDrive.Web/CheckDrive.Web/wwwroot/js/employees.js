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

function onPositionChange() {
    const positionSelect = document.getElementById("position-select");
    const assignedCarSelect = document.getElementById("assigned-car-select");

    const selectedPosition = positionSelect.value;

    if (selectedPosition === "Driver") {
        assignedCarSelect.disabled = false;
    } else {
        assignedCarSelect.disabled = true;
        assignedCarSelect.value = "";
    }
}

function onEditClick(id) {
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

function onToolbarClick(args) {
    let grid = document.getElementById('employees-list').ej2_instances[0];
    if (args.item.id === 'employees-list_pdfexport') {
        grid.pdfExport();
    } else if (args.item.id === 'employees-list_excelexport') {
        grid.excelExport();
    }
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

function exportToPdf() {
    let grid = document.getElementById('employees-list').ej2_instances[0];
    grid.pdfExport({
        includeHiddenColumn: true,
        fileName: 'xodimlar.pdf',
        theme: {
            header: {
                fontName: 'Arial',
                fontSize: 14,
                size: 16,
                bold: true,
                italic: false,
                color: '#FFFFFF',
                background: '#4CAF50' // Header background color
            },
            record: {
                fontName: 'Arial',
                fontSize: 12,
                size: 14,
                color: '#000000'
            },
            caption: {
                fontName: 'Arial',
                fontSize: 12,
                size: 14,
                bold: true,
                color: '#333333',
                background: '#f8f8f8'
            }
        },
        header: {
            fromTop: 0,
            height: 130,
            contents: [
                {
                    width: 100,
                    height: 50,
                    type: 'Text',
                    value: 'ATP Garaj xodimlari',
                    position: { x: 250, y: 50 },
                    style: { textBrushColor: '#3d3d3d', fontSize: 24, bold: true }
                },
                {
                    type: 'Text',
                    value: new Date().toLocaleDateString(),
                    position: { x: 600, y: 100 },
                    style: { textBrushColor: '#1f1f1f', fontSize: 16 }
                }
            ]
        },
        footer: {
            fromBottom: 0,
            height: 40,
            contents: [
                {
                    type: 'Text',
                    value: '© 2025 Silk Route Connect',
                    position: { x: 250, y: 20 },
                    style: { textBrushColor: '#333333', fontSize: 12 }
                }
            ]
        },
        columns: grid.getColumns().filter(col => col.field !== null && col.field !== undefined)
    });

    grid.addEventListener('pdfQueryCellInfo', function (args) {
        if (args.cell) {
            args.style = {
                padding: { top: 5, right: 10, bottom: 5, left: 10 }
            };
        }
    });
}

function exportToExcel() {
}