$(document).ready(function () {
    $('#editTopicModal').on('show.bs.modal', function (event) {
        var button = $(event.relatedTarget);
        var topicId = button.data('topic-id');
        var topicName = button.data('topic-name');
        var topicDescription = button.data('topic-description');

        var modal = $(this);
        modal.find('#editTopicId').val(topicId);
        modal.find('#editTopicName').val(topicName);
        modal.find('#editTopicDescription').val(topicDescription);
    });

    $('#deleteTopicModal').on('show.bs.modal', function (event) {
        var button = $(event.relatedTarget);
        var topicId = button.data('topic-id');
        var topicName = button.data('topic-name');

        var modal = $(this);
        modal.find('#deleteTopicId').val(topicId);
        modal.find('#deleteTopicName').text(topicName);
    });
});