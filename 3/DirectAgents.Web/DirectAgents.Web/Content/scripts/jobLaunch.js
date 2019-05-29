function setupDatePicker() {
    $('#datetimepicker').datetimepicker({
        minDate: Date.now()
    });
}

function setupCommandDescription() {
    var selectedCommand = $("#commandSelect").map(function () { return this.value; })[0];
    $.ajax({
        url: "/JobsRequest/GetCommandDescription",
        type: "POST",
        data: JSON.stringify({
            commandName: selectedCommand
        }),
        dataType: "json",
        contentType: 'application/json; charset=utf-8',
        success: function (data) {
            printCommandInfo(data);
        },
        error: function (error) {
            console.error(error);
        }
    });
}

function printCommandInfo(data) {
    $("#commandNameText").text(`${data.Name}: ${data.Description}`);
    $("#argumentInput").attr("placeholder", `Example:${data.ArgumentsExample}`);
    var commandInfoDiv = $("#commandDescriptionTable");
    commandInfoDiv.empty();
    commandInfoDiv.append("<tr><th>Argument</th><th>Description</th></tr>");
    for (let arg of data.Arguments) {
        commandInfoDiv.append(`<tr><td>${arg.Prototype}</td><td>${arg.Description}</td></tr>`);
    }
}

$(function() {
    setupDatePicker();
    setupCommandDescription();
});