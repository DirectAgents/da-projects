function setupDatePicker() {
    $("#datetimepicker").datetimepicker({
        minDate: getNowTimeInServerTimeZone()
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
    $("#schedulingWaiting").show();
    var command = getSelectedCommand();
    var commandArguments = getCommandArguments();
    var schedule = $("#scheduledTimeInput")[0].value;
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
            $("#schedulingWaiting").hide();
            $("#errorText").text("Scheduling of the command failed: " + error.statusText);
            console.error(error);
        }
    });
}

function setAbortedStatusToItems() {
    $("#abortingWaiting").show();
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
            error: function (error) {
                $("#abortingWaiting").hide();
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

function getNowTimeInServerTimeZone() {
    var now = new Date();
    var nowServerTime = new Date(
        now.getUTCFullYear(),
        now.getUTCMonth(),
        now.getUTCDate(),
        now.getUTCHours() + getServerTimeUtcOffsetInHours(),
        now.getUTCMinutes(),
        now.getUTCSeconds());
    return nowServerTime;
}

function getServerTimeUtcOffsetInHours() {
    var serverTimeZoneUtcOffsetInHoursAsText = $("#serverTimeZoneUtcOffsetInHours").text();
    return parseInt(serverTimeZoneUtcOffsetInHoursAsText);
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