$(document).ready(function () {
    $('form').on('submit', function (e) {
        var allQuestionsAnswered = true;
        $('.question-radio-group').each(function () {
            var hasCheckedRadio = $(this).find('.question-radio:checked').length > 0;
            if (!hasCheckedRadio) {
                allQuestionsAnswered = false;
            }
        });

        if (!allQuestionsAnswered) {
            e.preventDefault();
            alert('Please answer all questions before submitting the test.');
        }
    });
});