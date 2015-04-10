function CreateSummaryDataSource(url, readData, group, sort, pageSize) {
    if (pageSize == 0) pageSize = 100; // default
    if (pageSize == -1) pageSize = 1000; // "unlimited"

    var args = {
        serverAggregates: true,
        serverFiltering: true,
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
                    Impressions: { type: 'number' },
                    Clicks: { type: 'number' },
                    Orders: { type: 'number' },
                    ViewThrus: { type: 'number' },
                    ViewThruRev: { type: 'number' },
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
                    Calls: { type: 'number' },
                    TotalLeads: { type: 'number' },
                    ConvRate: { type: 'number' },
                    CPL: { type: 'number' },
                }
            }
        },
        aggregate: [
            { field: 'Impressions', aggregate: 'sum' },
            { field: 'Clicks', aggregate: 'sum' },
            { field: 'Orders', aggregate: 'sum' },
            { field: 'ViewThrus', aggregate: 'sum' },
            { field: 'ViewThruRev', aggregate: 'sum' },
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
            { field: 'Calls', aggregate: 'sum' },
            { field: 'TotalLeads', aggregate: 'sum' },
            { field: 'ConvRate', aggregate: 'agg' },
            { field: 'CPL', aggregate: 'agg' },
        ]
    };
    // Note: sorting is client-side
    if (group) {
        args.group = {
            field: 'Range'
        };
        args.sort = { field: 'Title', dir: 'asc' };
    } else {
        if (sort == 'revenue')
            args.sort = [{ field: 'Revenue', dir: 'desc' }, { field: 'Cost', dir: 'desc' }, { field: 'Impressions', dir: 'desc' }];
        else if (sort == 'clicks')
            args.sort = [{ field: 'Clicks', dir: 'desc' }, { field: 'Impressions', dir: 'desc' }];
        else if (sort == 'title')
            args.sort = { field: 'Title', dir: 'asc' };
        else
            args.sort = { field: 'Channel', dir: 'asc' }; // default (for weekly & monthly)
    }
    return new kendo.data.DataSource(args);
}

function CreateSummaryGrid(dataSource, el, height, titleHeader, titleWidthPct, decimals, sortable, showChannel, showBreakdown, detailInit, showViewThrus, showCalls) {
//ShowCalls not implemented here
    var args = {
        dataSource: dataSource,
        autoBind: false,
        height: height,
        columns: [
            { field: 'Channel', hidden: !showChannel, filterable: { multi: true, checkAll: false } },
            { field: 'Range', hidden: true, groupHeaderTemplate: 'Timeframe: #= value.substring(9) #' },
            { field: 'Title', title: titleHeader, width: titleWidthPct + '%' },
            { field: 'Network', hidden: !showBreakdown },
            { field: 'Device', hidden: !showBreakdown },
            { field: 'Clicks', format: '{0:n0}', attributes: { style: "text-align: right" }, footerTemplate: "#= kendo.toString(sum, 'n0') #", footerAttributes: { style: "font-weight: bold; text-align: right" } },
            { field: 'Impressions', format: '{0:n0}', attributes: { style: "text-align: right" }, footerTemplate: "#= kendo.toString(sum, 'n0') #", footerAttributes: { style: "font-weight: bold; text-align: right" } },
            { field: 'CTR', format: '{0:n2}%', attributes: { style: "text-align: center" }, footerTemplate: "#= kendo.toString(agg, 'n2') + '%' #", footerAttributes: { style: "font-weight: bold; text-align: center" } },
            { field: 'Cost', title: 'Spend', format: '{0:c' + decimals + '}', attributes: { style: "text-align: right" }, footerTemplate: "#= kendo.toString(sum, 'c" + decimals + "') #", footerAttributes: { style: "font-weight: bold; text-align: right" } },
            { field: 'CPC', format: '{0:c2}', attributes: { style: "text-align: center" }, footerTemplate: "#= kendo.toString(agg, 'c2') #", footerAttributes: { style: "font-weight: bold; text-align: center" } },
            { field: 'Orders', format: '{0:n0}', attributes: { style: "text-align: center" }, footerTemplate: "#= kendo.toString(sum, 'n0') #", footerAttributes: { style: "font-weight: bold; text-align: center" } },
            { field: 'Revenue', format: '{0:c' + decimals + '}', attributes: { style: "text-align: right" }, footerTemplate: "#= kendo.toString(sum, 'c" + decimals + "') #", footerAttributes: { style: "font-weight: bold; text-align: right" } },
            { field: 'Margin', title: 'Net', format: '{0:c' + decimals + '}', attributes: { style: "text-align: right" }, footerTemplate: "#= kendo.toString(agg, 'c" + decimals + "') #", footerAttributes: { style: "font-weight: bold; text-align: right" } },
            { field: 'ViewThrus', hidden: !showViewThrus, format: '{0:n0}', attributes: { style: "text-align: center" }, footerTemplate: "#= kendo.toString(sum, 'n0') #", footerAttributes: { style: "font-weight: bold; text-align: center" } },
            { field: 'ViewThruRev', hidden: !showViewThrus, format: '{0:c' + decimals + '}', attributes: { style: "text-align: right" }, footerTemplate: "#= kendo.toString(sum, 'c" + decimals + "') #", footerAttributes: { style: "font-weight: bold; text-align: right" } },
            { field: 'CPO', format: '{0:c' + decimals + '}', attributes: { style: "text-align: center" }, footerTemplate: "#= kendo.toString(agg, 'c" + decimals + "') #", footerAttributes: { style: "font-weight: bold; text-align: center" } },
            { field: 'OrderRate', format: '{0:n2}%', attributes: { style: "text-align: center" }, footerTemplate: "#= kendo.toString(agg, 'n2') + '%' #", footerAttributes: { style: "font-weight: bold; text-align: center" } },
            { field: 'RevenuePerOrder', title: 'Rev/Order', format: '{0:c' + decimals + '}', attributes: { style: "text-align: center" }, footerTemplate: "#= kendo.toString(agg, 'c" + decimals + "') #", footerAttributes: { style: "font-weight: bold; text-align: center" } },
            { field: 'ROAS', format: '{0:n0}%', attributes: { style: "text-align: center" }, footerTemplate: "#= kendo.toString(agg, 'n0') + '%' #", footerAttributes: { style: "font-weight: bold; text-align: center" } },
            //{ field: 'OrdersPerDay', title: 'Orders/Day', format: '{0:n1}', attributes: { style: "text-align: center" }, footerTemplate: "#= kendo.toString(agg, 'n1') #", footerAttributes: { style: "font-weight: bold; text-align: center" } },
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
function CreateSummaryGridLeadGen(dataSource, el, height, titleHeader, titleWidthPct, decimals, sortable, showChannel, showBreakdown, detailInit, showViewThrus, showCalls) {
    var args = {
        dataSource: dataSource,
        autoBind: false,
        height: height,
        columns: [
            { field: 'Channel', hidden: !showChannel, filterable: { multi: true, checkAll: false } },
            { field: 'Range', hidden: true, groupHeaderTemplate: 'Timeframe: #= value.substring(9) #' },
            { field: 'Title', title: titleHeader, width: titleWidthPct + '%' },
            { field: 'Network', hidden: !showBreakdown },
            { field: 'Device', hidden: !showBreakdown },
            { field: 'Clicks', format: '{0:n0}', attributes: { style: "text-align: right" }, footerTemplate: "#= kendo.toString(sum, 'n0') #", footerAttributes: { style: "font-weight: bold; text-align: right" } },
            { field: 'Impressions', format: '{0:n0}', attributes: { style: "text-align: right" }, footerTemplate: "#= kendo.toString(sum, 'n0') #", footerAttributes: { style: "font-weight: bold; text-align: right" } },
            { field: 'CTR', format: '{0:n2}%', attributes: { style: "text-align: center" }, footerTemplate: "#= kendo.toString(agg, 'n2') + '%' #", footerAttributes: { style: "font-weight: bold; text-align: center" } },
            { field: 'Cost', title: 'Spend', format: '{0:c' + decimals + '}', attributes: { style: "text-align: right" }, footerTemplate: "#= kendo.toString(sum, 'c" + decimals + "') #", footerAttributes: { style: "font-weight: bold; text-align: right" } },
            { field: 'CPC', format: '{0:c2}', attributes: { style: "text-align: center" }, footerTemplate: "#= kendo.toString(agg, 'c2') #", footerAttributes: { style: "font-weight: bold; text-align: center" } },
            { field: 'Orders', title: 'Leads', format: '{0:n0}', attributes: { style: "text-align: center" }, footerTemplate: "#= kendo.toString(sum, 'n0') #", footerAttributes: { style: "font-weight: bold; text-align: center" } },
            { field: 'ViewThrus', hidden: !showViewThrus, format: '{0:n0}', attributes: { style: "text-align: center" }, footerTemplate: "#= kendo.toString(sum, 'n0') #", footerAttributes: { style: "font-weight: bold; text-align: center" } },
            { field: 'Calls', hidden: !showCalls, format: '{0:n0}', attributes: { style: "text-align: center" }, footerTemplate: "#= kendo.toString(sum, 'n0') #", footerAttributes: { style: "font-weight: bold; text-align: center" } },
            { field: 'TotalLeads', hidden: !showCalls, format: '{0:n0}', attributes: { style: "text-align: center" }, footerTemplate: "#= kendo.toString(sum, 'n0') #", footerAttributes: { style: "font-weight: bold; text-align: center" } },
            { field: 'CPL', format: '{0:c' + decimals + '}', attributes: { style: "text-align: center" }, footerTemplate: "#= kendo.toString(agg, 'c" + decimals + "') #", footerAttributes: { style: "font-weight: bold; text-align: center" } },
            { field: 'ConvRate', format: '{0:n2}%', attributes: { style: "text-align: center" }, footerTemplate: "#= kendo.toString(agg, 'n2') + '%' #", footerAttributes: { style: "font-weight: bold; text-align: center" } },
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

function DetailInit(e, url, funcCreateSummaryGrid, showViewThrus, showCalls, sort, titleWidthPct) {
    var readData = function () {
        return {
            channel: e.data.Title,
            startdate: e.data.StartDate.toLocaleDateString(),
            enddate: e.data.EndDate.toLocaleDateString()
        };
    }
    var dataSource = CreateSummaryDataSource(url, readData, false, sort, -1);
    dataSource.read();

    var el = $("<div/>").appendTo(e.detailCell);
    funcCreateSummaryGrid(dataSource, el, null, 'Campaign', titleWidthPct, 2, true, false, false, null, showViewThrus, showCalls);
}

function CreateRevROASChart(dataSource, elId, title) {
    var series = [
        { field: "Revenue", name: 'Revenue', axis: "revenue", tooltip: { template: "Revenue: #= kendo.format('{0:C}',value) #" }, type: "area", color: "#007eff" },
        { field: "ROAS", name: 'ROAS', axis: "roas", tooltip: { template: "ROAS: #= kendo.format('{0:N0}',value) #%" }, type: "line", color: "#ff1c1c" }
    ];
    var valueAxis = [
        { name: "revenue", labels: { format: "C0", step: 2 }, title: { text: "Revenue" } },
        { name: "roas", labels: { format: "{0:N0}%", step: 2 }, title: { text: "ROAS" } }
    ];
    CreateSummaryChart(dataSource, elId, title, series, valueAxis);
}
function CreateOrderCPOChart(dataSource, elId, title) {
    var series = [
        { field: "Orders", name: 'Orders', axis: "orders", tooltip: { template: "Orders: #= kendo.format('{0:N0}',value) #" }, type: "area", color: "#007eff" },
        { field: "CPO", name: 'CPO', axis: "cpo", tooltip: { template: "CPO: #= kendo.format('{0:C}',value) #" }, type: "line", color: "#ff1c1c" }
    ];
    var valueAxis = [
        { name: "orders", labels: { format: "N0", step: 2 }, title: { text: "Orders" } },
        { name: "cpo", labels: { format: "C", step: 2 }, title: { text: "CPO" } }
    ];
    CreateSummaryChart(dataSource, elId, title, series, valueAxis);
}

function CreateCTRCPCChart(dataSource, elId, title) {
    var series = [
        { field: "CTR", name: "CTR", axis: "ctr", tooltip: { template: "CTR: #= kendo.format('{0:N2}%',value) #" }, type: "area", color: "#007eff" },
        { field: "CPC", name: "CPC", axis: "cpc", tooltip: { template: "CPC: #= kendo.format('{0:C}',value) #" }, type: "line", color: "#ff1c1c" }
    ];
    var valueAxis = [
        { name: "ctr", labels: { format: "{0:N2}%", step: 2 }, title: { text: "CTR" } },
        { name: "cpc", labels: { format: "C", step: 2 }, title: { text: "CPC" } }
    ];
    CreateSummaryChart(dataSource, elId, title, series, valueAxis);
}
function CreateLeadsCPLChart(dataSource, elId, title, nameForLeads) {
    var series = [
        { field: "TotalLeads", name: nameForLeads, axis: "totalleads", tooltip: { template: nameForLeads + ": #= kendo.format('{0:N0}',value) #" }, type: "area", color: "#007eff" },
        { field: "CPL", name: "CPL", axis: "cpl", tooltip: { template: "CPL: #= kendo.format('{0:C}',value) #" }, type: "line", color: "#ff1c1c" }
    ];
    var valueAxis = [
        { name: "totalleads", labels: { format: "N0", step: 2 }, title: { text: nameForLeads } },
        { name: "cpl", labels: { format: "C", step: 2 }, title: { text: "CPL" } }
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
            labels: { template: "#= value.replace(/ /g,'') #" },
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
