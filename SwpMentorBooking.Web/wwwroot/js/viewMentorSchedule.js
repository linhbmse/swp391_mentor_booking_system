$(document).ready(function () {
    $('a[data-bs-toggle="ajax-modal"]').on('click', function (event) {
        event.preventDefault();
        var url = $(this).attr('href');
        $.get(url).done(function (data) {
            $('#scheduleModal .modal-content').html(data);
            $('#scheduleModal').modal('show');
        });
    });
});