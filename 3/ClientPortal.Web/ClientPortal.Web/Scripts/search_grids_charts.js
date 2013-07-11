function CreateSummaryDataSource(url, readData, group) {
    var args = {
        serverAggregates: true,
        pageSize: 100,
        transport: {
            read: {
                type: 'post',
                dataType: 'json',
                url: url,
                data: readData
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
                    Impressions: { type: 'number' },
                    Clicks: { type: 'number' },
                    Orders: { type: 'number' },
                    Revenue: { type: 'number' },
                    Cost: { type: 'number' },
                    ROAS: { type: 'number' },
                    CPO: { type: 'number' },
                }
            }
        },
        sort: { field: 'StartDate', dir: 'asc' },
        aggregate: [
            { field: 'Impressions', aggregate: 'sum' },
            { field: 'Clicks', aggregate: 'sum' },
            { field: 'Orders', aggregate: 'sum' },
            { field: 'Revenue', aggregate: 'sum' },
            { field: 'Cost', aggregate: 'sum' },
            { field: 'ROAS', aggregate: 'agg' },
            { field: 'CPO', aggregate: 'agg' },
        ]
    };
    if (group)
        args.group = {
            field: 'Range', aggregates: [
                { field: 'Orders', aggregate: 'sum' }
            ]
        };
    return new kendo.data.DataSource(args);
}

function CreateSummaryGrid(dataSource, el, height, titleHeader, titleWidthPct, decimals, detailInit) {
    el.kendoGrid({
        dataSource: dataSource,
        autoBind: false,
        height: height,
        columns: [
            { field: 'Range', hidden: true, groupHeaderTemplate: 'Timeframe: #= value #' },
            { field: 'Title', title: titleHeader, width: titleWidthPct + '%' },
            { field: 'Revenue', format: '{0:c' + decimals + '}', attributes: { style: "text-align: right" }, footerTemplate: "#= kendo.toString(sum, 'c" + decimals + "') #", footerAttributes: { style: "font-weight: bold; text-align: right" } },
            { field: 'Cost', name: 'Costs', format: '{0:c' + decimals + '}', attributes: { style: "text-align: right" }, footerTemplate: "#= kendo.toString(sum, 'c" + decimals + "') #", footerAttributes: { style: "font-weight: bold; text-align: right" } },
            //{ field: null, title: 'ROAS', template: "#= kendo.toString(Revenue / Cost, 'p0') #" },
            { field: 'ROAS', format: '{0:n0}%', attributes: { style: "text-align: center" }, footerTemplate: "#= kendo.toString(agg, 'n0') + '%' #", footerAttributes: { style: "font-weight: bold; text-align: center" } },
            { field: null, title: 'Margin', attributes: { style: "text-align: right" }, template: "#= kendo.toString(Revenue - Cost, 'c" + decimals + "') #" },
            { field: 'Orders', format: '{0:n0}', attributes: { style: "text-align: center" }, footerTemplate: "#= kendo.toString(sum, 'n0') #", footerAttributes: { style: "font-weight: bold; text-align: center" } },
            //{ field: null, title: 'CPO', template: "#= kendo.toString(Cost / Orders, 'c0') #" },
            { field: 'CPO', format: '{0:c' + decimals + '}', attributes: { style: "text-align: center" }, footerTemplate: "#= kendo.toString(agg, 'c" + decimals + "') #", footerAttributes: { style: "font-weight: bold; text-align: center" } },
            { field: null, title: 'Order Rate', attributes: { style: "text-align: center" }, template: "#= kendo.toString(Orders / Clicks, 'p2') #" },
            { field: null, title: 'Rev/Order', attributes: { style: "text-align: center" }, template: "#= kendo.toString(Revenue / Orders, 'c" + decimals + "') #" },
            { field: null, title: 'CPC', attributes: { style: "text-align: center" }, template: "#= kendo.toString(Cost / Clicks, 'c2') #" },
            { field: 'Clicks', format: '{0:n0}', attributes: { style: "text-align: right" }, footerTemplate: "#= kendo.toString(sum, 'n0') #", footerAttributes: { style: "font-weight: bold; text-align: right" } },
            { field: 'Impressions', format: '{0:n0}', attributes: { style: "text-align: right" }, footerTemplate: "#= kendo.toString(sum, 'n0') #", footerAttributes: { style: "font-weight: bold; text-align: right" } },
            { field: null, title: 'CTR', attributes: { style: "text-align: center" }, template: "#= kendo.toString(Clicks / Impressions, 'p2') #" },
            { field: null, title: 'Avg Order/Day', attributes: { style: "text-align: center" }, template: "#= kendo.toString(Orders / Days, 'n" + decimals + "') #" },
        ],
        filterable: true,
        sortable: {
            mode: 'multiple'
        },
        pageable: true,
        detailInit: detailInit
    });
}

function DetailInit(e, url) {
    var readData = function () {
        return {
            channel: e.data.Title,
            startdate: e.data.StartDate.toLocaleDateString(),
            enddate: e.data.EndDate.toLocaleDateString()
        };
    }
    var dataSource = CreateSummaryDataSource(url, readData, false);
    dataSource.read();

    var el = $("<div/>").appendTo(e.detailCell);
    CreateSummaryGrid(dataSource, el, null, 'Campaign', 21, 2, null);
}

function CreateRevROASChart(dataSource, elId, title) {
    var series = [
            { field: "Revenue", axis: "revenue", tooltip: { template: "Revenue: #= kendo.format('{0:C}',value) #" } },
            { field: "ROAS", axis: "roas", tooltip: { template: "ROAS: #= kendo.format('{0:N0}',value) #%" }, type: "line" }
    ];
    var valueAxis = [
            { name: "roas", labels: { format: "{0:N0}%", step: 2 }, title: { text: "ROAS" } },
            { name: "revenue", labels: { format: "C0", step: 2 }, title: { text: "Revenue" } }
    ];
    CreateSummaryChart(dataSource, elId, title, series, valueAxis);
}
function CreateOrderCPOChart(dataSource, elId, title) {
    var series = [
            { field: "Orders", axis: "orders", tooltip: { template: "Orders: #= kendo.format('{0:N0}',value) #" } },
            { field: "CPO", axis: "cpo", tooltip: { template: "CPO: #= kendo.format('{0:C}',value) #" }, type: "line" }
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
            field: "Range",
            labels: { template: "#= value #" },
            axisCrossingValue: [0, 1000]
        },
        valueAxis: valueAxis,
        tooltip: {
            visible: true
        },
        legend: {
            position: "left",
            offsetX: 90,
            offsetY: 0
        }
    });
}
