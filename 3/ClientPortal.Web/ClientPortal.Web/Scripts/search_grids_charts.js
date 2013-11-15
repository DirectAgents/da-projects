function CreateSummaryDataSource(url, readData, group, sortByRevenue, pageSize) {
    if (pageSize == 0) pageSize = 100; // default
    if (pageSize == -1) pageSize = 1000; // "unlimited"

    var args = {
        serverAggregates: true,
        pageSize: pageSize,
        transport: {
            read: {
                type: 'post', // hard coding transport to POST
                dataType: 'json', // hard coding transport to JSON
                url: url, // URL sent in
                data: readData // func to read data sent in
            }
        },
        schema: {
            data: 'data',
            total: 'total',
            aggregates: 'aggregates',
            model: {
                id: 'StartDate',
                fields: {
                    StartDate: { type: 'date' },
                    EndDate: { type: 'date' },
                    Range: { type: 'string' },
                    Title: { type: 'string' },
                    Channel: { type: 'string' },
                    Network: { type: 'string' },
                    Device: { type: 'string' },
                    ClickType: { type: 'string' },
                    Impressions: { type: 'number' },
                    Clicks: { type: 'number' },
                    Orders: { type: 'number' },
                    Revenue: { type: 'number' },
                    Cost: { type: 'number' },
                    ROAS: { type: 'number' },
                    Margin: { type: 'number' },
                    CPO: { type: 'number' },
                    OrderRate: { type: 'number' },
                    RevenuePerOrder: { type: 'number' },
                    CPC: { type: 'number' },
                    CTR: { type: 'number' },
                    OrdersPerDay: { type: 'number' },
                }
            }
        },
        aggregate: [
            { field: 'Impressions', aggregate: 'sum' },
            { field: 'Clicks', aggregate: 'sum' },
            { field: 'Orders', aggregate: 'sum' },
            { field: 'Revenue', aggregate: 'sum' },
            { field: 'Cost', aggregate: 'sum' },
            { field: 'ROAS', aggregate: 'agg' },
            { field: 'Margin', aggregate: 'agg' },
            { field: 'CPO', aggregate: 'agg' },
            { field: 'OrderRate', aggregate: 'agg' },
            { field: 'RevenuePerOrder', aggregate: 'agg' },
            { field: 'CPC', aggregate: 'agg' },
            { field: 'CTR', aggregate: 'agg' },
            { field: 'OrdersPerDay', aggregate: 'agg' },
        ]
    };
    if (group) {
        args.group = {
            field: 'Range', aggregates: [
                { field: 'Orders', aggregate: 'sum' }
            ]
        };
        args.sort = { field: 'Title', dir: 'asc' };
    } else {
        if (sortByRevenue)
            args.sort = [{ field: 'Revenue', dir: 'desc' }, { field: 'Cost', dir: 'desc' }, { field: 'Impressions', dir: 'desc' }];
        else
            args.sort = { field: 'Channel', dir: 'asc' };
    }
    return new kendo.data.DataSource(args);
}

function CreateSummaryGrid(dataSource, el, height, titleHeader, titleWidthPct, decimals, sortable, showChannel, showBreakdown, detailInit) {
    var args = {
        dataSource: dataSource,
        autoBind: false,
        height: height,
        columns: [
            { field: 'Channel', hidden: !showChannel },
            { field: 'Range', hidden: true, groupHeaderTemplate: 'Timeframe: #= value.substring(9) #' },
            { field: 'Title', title: titleHeader, width: titleWidthPct + '%' },
            { field: 'Network', hidden: !showBreakdown },
            { field: 'Device', hidden: !showBreakdown },
            { field: 'ClickType', hidden: !showBreakdown },
            { field: 'Revenue', format: '{0:c' + decimals + '}', attributes: { style: "text-align: right" }, footerTemplate: "#= kendo.toString(sum, 'c" + decimals + "') #", footerAttributes: { style: "font-weight: bold; text-align: right" } },
            { field: 'Cost', name: 'Costs', format: '{0:c' + decimals + '}', attributes: { style: "text-align: right" }, footerTemplate: "#= kendo.toString(sum, 'c" + decimals + "') #", footerAttributes: { style: "font-weight: bold; text-align: right" } },
            { field: 'ROAS', format: '{0:n0}%', attributes: { style: "text-align: center" }, footerTemplate: "#= kendo.toString(agg, 'n0') + '%' #", footerAttributes: { style: "font-weight: bold; text-align: center" } },
            { field: 'Margin', format: '{0:c' + decimals + '}', attributes: { style: "text-align: right" }, footerTemplate: "#= kendo.toString(agg, 'c" + decimals + "') #", footerAttributes: { style: "font-weight: bold; text-align: right" } },
            { field: 'Orders', format: '{0:n0}', attributes: { style: "text-align: center" }, footerTemplate: "#= kendo.toString(sum, 'n0') #", footerAttributes: { style: "font-weight: bold; text-align: center" } },
            { field: 'CPO', format: '{0:c' + decimals + '}', attributes: { style: "text-align: center" }, footerTemplate: "#= kendo.toString(agg, 'c" + decimals + "') #", footerAttributes: { style: "font-weight: bold; text-align: center" } },
            { field: 'OrderRate', title: 'Order Rate', format: '{0:n2}%', attributes: { style: "text-align: center" }, footerTemplate: "#= kendo.toString(agg, 'n2') + '%' #", footerAttributes: { style: "font-weight: bold; text-align: center" } },
            { field: 'RevenuePerOrder', title: 'Rev/Order', format: '{0:c' + decimals + '}', attributes: { style: "text-align: center" }, footerTemplate: "#= kendo.toString(agg, 'c" + decimals + "') #", footerAttributes: { style: "font-weight: bold; text-align: center" } },
            { field: 'CPC', format: '{0:c2}', attributes: { style: "text-align: center" }, footerTemplate: "#= kendo.toString(agg, 'c2') #", footerAttributes: { style: "font-weight: bold; text-align: center" } },
            { field: 'Clicks', format: '{0:n0}', attributes: { style: "text-align: right" }, footerTemplate: "#= kendo.toString(sum, 'n0') #", footerAttributes: { style: "font-weight: bold; text-align: right" } },
            { field: 'Impressions', format: '{0:n0}', attributes: { style: "text-align: right" }, footerTemplate: "#= kendo.toString(sum, 'n0') #", footerAttributes: { style: "font-weight: bold; text-align: right" } },
            { field: 'CTR', format: '{0:n2}%', attributes: { style: "text-align: center" }, footerTemplate: "#= kendo.toString(agg, 'n2') + '%' #", footerAttributes: { style: "font-weight: bold; text-align: center" } },
            { field: 'OrdersPerDay', title: 'Orders/Day', format: '{0:n' + decimals + '}', attributes: { style: "text-align: center" }, footerTemplate: "#= kendo.toString(agg, 'n" + decimals + "') #", footerAttributes: { style: "font-weight: bold; text-align: center" } },
        ],
        filterable: true,
        pageable: true,
        detailInit: detailInit
    };
    if (sortable) {
        args.sortable = {
            mode: 'single',
            allowUnsort: false
        };
    } else {
        args.sortable = false;
    }
    el.kendoGrid(args);
}

function DetailInit(e, url) {
    var readData = function () {
        return {
            channel: e.data.Title,
            startdate: e.data.StartDate.toLocaleDateString(),
            enddate: e.data.EndDate.toLocaleDateString()
        };
    }
    var dataSource = CreateSummaryDataSource(url, readData, false, true, -1);
    dataSource.read();

    var el = $("<div/>").appendTo(e.detailCell);
    CreateSummaryGrid(dataSource, el, null, 'Campaign', 18, 2, true, false, false, null);
}

function CreateRevROASChart(dataSource, elId, title) {
    var series = [
            { field: "Revenue", axis: "revenue", tooltip: { template: "Revenue: #= kendo.format('{0:C}',value) #" }, markers: { type: "square" }, name: 'Revenue' },
            { field: "ROAS", axis: "roas", tooltip: { template: "ROAS: #= kendo.format('{0:N0}',value) #%" }, type: "line", name: 'ROAS' }
    ];
    var valueAxis = [
            { name: "roas", labels: { format: "{0:N0}%", step: 2 }, title: { text: "ROAS" }},
            { name: "revenue", labels: { format: "C0", step: 2 }, title: { text: "Revenue" } }
    ];
    CreateSummaryChart(dataSource, elId, title, series, valueAxis);
}
function CreateOrderCPOChart(dataSource, elId, title) {
    var series = [
            { field: "Orders", axis: "orders", tooltip: { template: "Orders: #= kendo.format('{0:N0}',value) #" }, markers: { type: 'square' }, name: 'Orders'},
            { field: "CPO", axis: "cpo", tooltip: { template: "CPO: #= kendo.format('{0:C}',value) #" }, type: "line", name: 'CPO'}
    ];
    var valueAxis = [
            { name: "cpo", labels: { format: "C0", step: 2 }, title: { text: "CPO" } },
            { name: "orders", labels: { format: "N0", step: 2 }, title: { text: "Orders" } }
    ];
    CreateSummaryChart(dataSource, elId, title, series, valueAxis);
}

function CreateSummaryChart(dataSource, elId, title, series, valueAxis) {
    $('#' + elId).kendoChart({
        title: title,
        dataSource: dataSource,
        autoBind: false,
        chartArea: {
            height: 200
        },
        theme: $(document).data("kendoSkin") || "default",
        series: series,
        categoryAxis: {
            field: "Title",
            labels: { template: "#= value #" },
            axisCrossingValue: [0, 1000]
        },
        valueAxis: valueAxis,
        tooltip: {
            visible: true
        },
        legend: {
            position: "bottom",
            //offsetX: 90,
            //offsetY: 0
        }
    });
}
