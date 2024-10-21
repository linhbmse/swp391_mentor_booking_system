$(document).ready(function () {
    $('#confirmModal').on('show.bs.modal', function (event) {
        var button = $(event.relatedTarget);
        var bookingId = button.data('booking-id');
        $('#bookingIdInput').val(bookingId);
    });
});