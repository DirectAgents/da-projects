function dataLoadedCallback() {
    $(".json-cell").each(function () {
        var cellContent = $(this).text();
        if (cellContent) {
            var contentObj = JSON.parse(cellContent);
            var buitifiedJsonText = JSON.stringify(contentObj, null, 4);
            $(this).html("<code class='json-code'>" + buitifiedJsonText + "</code>");
        }
    });
}

dataLoadedCallback();