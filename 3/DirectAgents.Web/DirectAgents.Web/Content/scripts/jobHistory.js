function dataLoadedCallback() {
    $(".json-cell").each(function () {
        var cellContent = $(this).text();
        if (cellContent) {
            var contentObj = JSON.parse(cellContent);
            var buitifiedJsonText = JSON.stringify(contentObj, null, 4);
            $(this).html("<div class='json-block'><code class='json-code'>" + buitifiedJsonText + "</code></div>");
        }
    });
}

function setupDatePicker() {
    $('#datetimepicker').datetimepicker({
        format: 'L'
    });
}

function setAbortedStatusToItems() {
    var ids = $("input[type=checkbox]:checked").map(function () { return this.value; }).get();
    var result = confirm("Are you sure?");
    if (result) {
        $.ajax({
            url: "/JobsExecution/SetAbortedStatusToItems",
            type: "POST",
            data: JSON.stringify(ids),
            dataType: "json",
            contentType: 'application/json; charset=utf-8',
            success: function () { location.reload(); },
            error: function (error) { console.error(error); }
        });
    }
}

setupDatePicker();
dataLoadedCallback();
