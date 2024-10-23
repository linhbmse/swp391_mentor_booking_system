function formatDate(dateString) {
    const options = { weekday: 'long', year: 'numeric', month: 'long', day: 'numeric' };
    const date = new Date(dateString);
    return date.toLocaleDateString('en-US', options);
}

$(document).ready(function () {
    $('.clickable-cell').click(function () {
        var date = $(this).data('date');
        var startTime = $(this).data('start-time');
        var endTime = $(this).data('end-time');
        var scheduleId = $(this).data('schedule-id');
        var status = $(this).data('status');
        var isPastSlot = $(this).data('is-past-slot')

        $('#selectedSlotDate').text('Date: ' + formatDate(date));
        $('#selectedSlotTime').text('Time: ' + startTime + ' - ' + endTime);
        $('#selectedSlotStatus').text('Status: ' + status.charAt(0).toUpperCase() + status.slice(1));

        if (status === 'available' && isPastSlot !== true) {
            $('#proceedToBooking').show().attr('href', proceedToBookingUrl + '?scheduleId=' + scheduleId);
        } else {
            $('#proceedToBooking').hide();
        }


        $('#selectedSlotPreview').show();
    });
});