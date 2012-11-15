function Save() {
    var form = $('#questionForm');
    $.ajax({
        type: "POST",
        url: form.attr('action'),
        data: form.serialize(),
        success: function (response) {
            alert('success!');
        }
    });
}
