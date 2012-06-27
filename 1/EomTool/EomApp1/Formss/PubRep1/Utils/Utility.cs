using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Xml.Linq;

namespace EomApp1.Formss.PubRep1.Utils
{
    static class Utility
    {
        internal static string GeneratePublisherReport(
            Data.PublisherReportDataSet1.VerifiedLineItemsDataTable lis,
            Data.PublisherReportDataSet1.AffiliatesHavingReportsRow cur,
            bool payInterface)
        {
            string path = Path.Combine(Path.GetTempPath(), "pubreport" + Guid.NewGuid().ToString() + ".html");

            int lineItemCount = lis.Count();

            string mb = "n/a";
            if (lineItemCount > 0)
            {
                mb = lis.First().media_buyer_name;
            }

            var docType = new XDocumentType("html",
              "-//W3C//DTD XHTML 1.0 Strict//EN",
              "http://www.w3.org/TR/xhtml1/DTD/xhtml1-strict.dtd", null);
            //  "-//W3C//DTD XHTML 1.0 Transitional//EN",
            //  "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd", null);

            XNamespace ns = "http://www.w3.org/1999/xhtml";

            var root =
                new XElement(ns + "html",
                    new XElement(ns + "head",
                        new XElement(ns + "title", "Publisher Report"),

                        //new XElement(ns + "script", new XAttribute("src", "file:///" + jqPath), new XAttribute("type", "text/javascript")),
                //                        new XElement(ns + "script", new XAttribute("type", "text/javascript"),
                //                            @"
                //                                function AddMyClass(id, name) {
                //                                    document.getElementById(""fooid"").className += "" noshow"";
                //                                    var a = document.getElementById(""fooid"");
                //                                    var b = a.className;
                //                                    alert(a + "":"" + b);
                //                                }
                //                            "),

                        new XElement(ns + "style",
                            new XAttribute("type", "text/css"),
                            GeneratePublisherReportGetStyleSheetText())
                    ),
                new XElement(ns + "body",

                        //payInterface ? new XElement(ns + "div", "HELLO FOO", new XAttribute("id", "fooid"), new XAttribute("class", "lineitem6")) : null,

                        new XElement(ns + "div", new XAttribute("id", "outer"),

                        new XElement(ns + "div", new XAttribute("id", "title"),
                            new XElement(ns + "img", new XAttribute("src",
                                "http://www.directagents.com/publisherreportheader.png"))
                        ),

                        new XElement(ns + "div", new XAttribute("id", "subtitle"),
                            new XElement(ns + "span", "Monthly Performance Report",
                                new XAttribute("id", "subtitle1"))
                        ),

                        new XElement(ns + "div", new XAttribute("id", "info1"),
                            new XElement(ns + "span", cur.publisher_name,
                                new XAttribute("id", "info11"))
                        ),

                        new XElement(ns + "div", new XAttribute("id", "info2"),

                        new XElement(ns + "span", "Media Buyer: " + mb,
                            new XAttribute("id", "info21"))
                        ),

                        new XElement(ns + "div", new XAttribute("id", "info3"),
                            new XElement(ns + "span", "Report For: "
                                            + String.Format("{0:M/d/yyyy}", new DateTime(
                                                Properties.Settings.Default.StatsYear,
                                                Properties.Settings.Default.StatsMonth,
                                                1))
                                            + " to " + String.Format("{0:M/d/yyyy}",
                                            new DateTime(
                                                Properties.Settings.Default.StatsYear,
                                                Properties.Settings.Default.StatsMonth,
                                                Properties.Settings.Default.StatsDaysInMonth)),
                                            new XAttribute("id", "info31"))
                        ),

                        GeneratePublisherReportGetLineItemsXElements(lis, "Verified", 5, "Paid", false, payInterface),

                        GeneratePublisherReportGetLineItemsXElements(lis, "Verified", 1, "To Be Paid", false, payInterface),

                        GeneratePublisherReportGetLineItemsXElements(lis, "Verified", 5, "CPM Paid", true, payInterface),

                        GeneratePublisherReportGetLineItemsXElements(lis, "Verified", 1, "CPM To Be Paid", true, payInterface),

                        GeneratePublisherReportGetUnverifiedCampaignsXElements(lis)
                        )
                    )
                );

            var doc =
                new XDocument(
                    new XDeclaration("1.0", "utf-8", "no"),
                    docType,
                    root
                );

            doc.Save(path);

            return path;
        }

        private static object GeneratePublisherReportGetUnverifiedCampaignsXElements(
            Data.PublisherReportDataSet1.VerifiedLineItemsDataTable lis)
        {
            // Need at least 1 line item to produce any result
            if (lis.Count < 1)
            {
                return null;
            }

            // Determine the payout currency from the first line item
            string pubPayoutCurr = lis.First().payment_currency_name;

            XNamespace ns = "http://www.w3.org/1999/xhtml";

            var result = new List<XElement>();

            // We want a set of Unverified campaigns
            // Only look at the unpaid line items
            // (even though in theory a paid line item is always verified)
            var matchingLineItems = lis.Where(
                q =>
                    q.status != "Verified"
                    && q.item_accounting_status_id == 1)
                    .Select(c => c.campaign_name).Distinct();

            // Create a table of campaign names
            if (matchingLineItems.Count() > 0)
            {
                foreach (var item in matchingLineItems)
                {
                    result.Add(
                        new XElement(ns + "div",
                        new XElement(ns + "br"),
                        new XElement(ns + "table",
                            new XAttribute("class", "lineitems"),
                            new XElement(ns + "tr", new XAttribute("class", "lineitems1"),
                                new XElement(ns + "td", item, new XAttribute("class", "lineitems2"))
                            )
                        )
                        )
                    );
                }

                result.Add(new XElement(ns + "div", new XAttribute("id", "info4"),
                    new XElement(ns + "span", "Status: Not Yet Final", new XAttribute("id", "info51"))));

            }
            return result;
        }

        private static object GeneratePublisherReportGetLineItemsXElements(
            Data.PublisherReportDataSet1.VerifiedLineItemsDataTable lis,
            string realStatus,
            int accountingStatusID,
            string status,
            bool isCPM,
            bool showPayInterface)
        {
            // Need at least 1 line item to produce any result
            if (lis.Count < 1)
            {
                return null;
            }

            // Determine the payout currency from the first line item
            string pubPayoutCurr = lis.First().payment_currency_name;

            XNamespace ns = "http://www.w3.org/1999/xhtml";

            var result = new List<XElement>();

            // Filter down to 
            var matchingLineItems = lis.Where(
                q =>
                    q.status == realStatus
                    && q.item_accounting_status_id == accountingStatusID);

            matchingLineItems = isCPM ? matchingLineItems.Where(c => c.unit_type_name == "CPM") : matchingLineItems.Where(c => c.unit_type_name != "CPM");

            // Filter out 0 cost per unit
            matchingLineItems = matchingLineItems.Where(c => c.cost_per_unit != 0);

            if (matchingLineItems.Count() > 0)
            {
                foreach (var item in matchingLineItems)
                {
                    result.Add(
                        new XElement(ns + "div",
                        new XElement(ns + "br"),
                        new XElement(ns + "table",
                            new XAttribute("class", "lineitems"),
                            new XElement(ns + "tr", new XAttribute("class", "lineitems1"),
                                new XElement(ns + "td", "Campaign: " + item.campaign_name, new XAttribute("class", "lineitems2")),
                                new XElement(ns + "td", "Payout", new XAttribute("class", "lineitems3")),
                                new XElement(ns + "td", "Actions", new XAttribute("class", "lineitems4")),
                                new XElement(ns + "td", "Revenue", new XAttribute("class", "lineitems5"))
                            ),
                            new XElement(ns + "tr", new XAttribute("class", "lineitems1"),
                                new XElement(ns + "td", item.campaign_name + " - " + item.add_code, new XAttribute("class", "lineitems2")),
                                new XElement(ns + "td", GeneratePublisherReportFormatMoney(item, item.cost_per_unit, pubPayoutCurr), new XAttribute("class", "lineitems3")),
                                new XElement(ns + "td", Convert.ToInt32(item.num_units), new XAttribute("class", "lineitems4")),
                                new XElement(ns + "td", new XAttribute("class", "lineitems5"),
                                    new XElement(ns + "span", GeneratePublisherReportFormatMoney(item, item.total_cost, pubPayoutCurr)),

                                    showPayInterface && accountingStatusID == 1 ?
                                        new XElement(ns + "button",
                                            new XAttribute("type", "button"),
                                            new XAttribute("class", "lineitems6"),
                                            new XAttribute("onclick", "window.external.PayItems('" + item.item_id + "')"),
                                            "Pay"
                                        ) : null
                                    )
                                )
                            )
                        )
                    );
                }

                result.Add(new XElement(ns + "div", new XAttribute("id", "info4"),
                    new XElement(ns + "span", "Status: " + status, new XAttribute("id", "info51"))));

                result.Add(
                    new XElement(ns + "div", new XAttribute("id", "info5"),
                        new XElement(ns + "a", "Total: "
                            + GeneratePublisherReportFormatMoney(
                                lis.AsEnumerable()
                                .Where(q =>
                                    q.status == realStatus
                                    && q.item_accounting_status_id == accountingStatusID
                                    && (isCPM ? q.unit_type_name == "CPM" : q.unit_type_name != "CPM")
                                )
                                .Sum(c =>
                                    c.num_units
                                    * c.cost_per_unit
                                    * c.cust_to_usd_multiplier),
                                lis.First().payment_currency_name,
                                lis.First().payment_to_usd_multiplier
                        ),
                        new XAttribute("id", "info61"),
                            new XAttribute("onclick",
                                "alert('" +
                                lis.AsEnumerable()
                                    .Where(q =>
                                        q.status == realStatus
                                        && q.item_accounting_status_id == accountingStatusID
                                        && (isCPM ? q.unit_type_name == "CPM" : q.unit_type_name != "CPM"))
                                    .Sum(c =>
                                        c.num_units * c.cost_per_unit * c.cust_to_usd_multiplier).ToString() + "')"
                            )
                        )
                    )
                );
            }

            return result;
        }

        private static string GeneratePublisherReportFormatMoney(
                decimal p,
                string cur,
                decimal tousd)
        {
            var map = new Dictionary<string, string>();
            map.Add("USD", "en-us");
            map.Add("GBP", "en-gb");
            map.Add("EUR", "de-de");
            map.Add("AUD", "en-AU");
            return String.Format(CultureInfo.CreateSpecificCulture(map[cur]), "{0:C}",

                (p / tousd)

            );
        }

        private static string GeneratePublisherReportFormatMoney(
            Data.PublisherReportDataSet1.VerifiedLineItemsRow item,
            decimal p,
            string cur)
        {
            var map = new Dictionary<string, string>();
            map.Add("USD", "en-us");
            map.Add("GBP", "en-gb");
            map.Add("EUR", "de-de");
            map.Add("AUD", "en-AU");
            return String.Format(CultureInfo.CreateSpecificCulture(map[cur]), "{0:C}",

                (p * item.cust_to_usd_multiplier / item.payment_to_usd_multiplier)

            );
        }

        internal static string GeneratePublisherReportGetStyleSheetText()
        {
            return @"
body {font-size: 80%; font-family: arial}
h2 {border-width: thin; border-style: solid; text-align: center;}
#outer {width: 700px; margin: 0px auto;}
#title {font-weight: bold; background-color:#ffffff; border-width: thin; border-style: none; text-align: center}
#title1 {font-size: 200%; font-style: italic; color: #000000}
#title2 {font-size: 200%; color: #000000}
#title3 {font-size: 140%; color: #000000}
#subtitle {text-align: center; padding: 1em;}
#info1 {padding: 0.7em;}
#info2 {padding: 0.7em;}
#info3 {padding: 0.7em;}
.lineitems {font-size: 70%; border: 1px solid black; border-collapse:collapse; clear: both}
.lineitems2 {border: 1px solid black; padding:1px; width: 387px}
.lineitems3 {border: 1px solid black; padding:1px; width: 100px; text-align: center}
.lineitems4 {border: 1px solid black; padding:1px; width: 100px; text-align: center}
.lineitems5 {border: 1px solid black; padding:1px; width: 100px; text-align: center}
.lineitems6 {border: 1px solid black; padding:1px; width: 100px; text-align: center; background-color:#cccccc}
.noshow {display: none;}
#info4 {padding: 0.7em; float: left}
#info5 {padding: 0.7em; float: right}
";
        }
    }
}
