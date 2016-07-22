// Resize a chart, optionally refreshing it.
function ResizeChart(chart, refresh) {
    if ($('#' + chart + 'ChartSection').length > 0) {
        var width = $('#' + chart + 'ChartSection').width();
        if (width > 0) {
            $('#' + chart + 'ChartSection .chart-wrapper').css('width', width);
            if (refresh)
                $('#' + chart + 'Chart').data('kendoChart').refresh();
        }
    }
}
