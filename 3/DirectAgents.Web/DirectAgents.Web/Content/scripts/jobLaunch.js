function setupDatePicker() {
    $('#datetimepicker').datetimepicker({
        minDate: Date.now()
    });
}

function setupCommandDescription() {
    var selectedCommand = getSelectedCommand();
    $.ajax({
        url: "/JobsRequest/GetCommandDescription",
        type: "POST",
        data: JSON.stringify({
            commandName: selectedCommand
        }),
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function (data) {
            printCommandInfo(data);
        },
        error: function (error) {
            console.error(error);
        }
    });
}

function scheduleRequest() {
    var command = getSelectedCommand();
    var commandArguments = getCommandArguments();
    var schedule = getScheduledTime();
    $.ajax({
        url: "/JobsRequest/ScheduleJobRequest",
        type: "POST",
        data: JSON.stringify({
            CommandName: command,
            CommandExecutionArguments: commandArguments,
            ScheduledTime: schedule
        }),
        dataType: "json",
        contentType: 'application/json; charset=utf-8',
        success: function () {
            location.reload();
        },
        error: function (error) {
            $("#errorText").text("Scheduling of the command failed: " + error.statusText);
            console.error(error);
        }
    });
}

function setAbortedStatusToItems() {
    var ids = $("input[type=checkbox]:checked").map(function () { return this.value; }).get();
    var result = confirm("Are you sure?");
    if (result) {
        $.ajax({
            url: "/JobsRequest/SetAbortedStatusToItems",
            type: "POST",
            data: JSON.stringify(ids),
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            success: function() {
                location.reload();
            },
            error: function(error) {
                console.error(error);
            }
        });
    }
}

function getSelectedCommand() {
    return $("#commandSelect").map(function () { return this.value; })[0];
}

function getCommandArguments() {
    return $("#argumentInput")[0].value;
}

function getScheduledTime() {
    return $("#scheduledTimeInput")[0].value;
}

function printCommandInfo(data) {
    $("#commandNameText").text(data.Name + ": " + data.Description);
    $("#argumentInput").attr("placeholder", "Example: " + data.ArgumentsExample);
    var commandInfoDiv = $("#commandDescriptionTable");
    commandInfoDiv.empty();
    commandInfoDiv.append("<tr><th>Argument</th><th>Description</th></tr>");
    for (var i in data.Arguments) {
        var htmlMarkup = "<tr><td>" + data.Arguments[i].Prototype + "</td><td>" + data.Arguments[i].Description + "</td></tr>";
        commandInfoDiv.append(htmlMarkup);
    }
}

$(function() {
    setupDatePicker();
    setupCommandDescription();
});