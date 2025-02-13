function onDetailsClick(id) {
    $('#detailsContent').empty();

    $.ajax({
        url: `/checkPoint/details/${id}`,
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