using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Windows.Forms;

namespace EomApp1.Formss.AB.Model
{
    class ABModel2 : IABModel
    {
        public ABModel2()
        {
            // create an empty data table
            _abItemsDataTable = new DataTable();

            // create an instance of the DAMain1 model
            var db = new EntityModel.DAMain1Entities1();

            // loop through the databases
            foreach (var item in db.DADBs)
            {
                // 
                string query = string.Format(_abItemsQuery, item.AccountingPeriod.Name);

                using (var sqlConnection = new SqlConnection(item.DBInstance.ConnectionString))
                using (var sqlCommand = new SqlCommand(query, sqlConnection))
                {
                    try
                    {
                        var dataTable = new DataTable();
                        var dataAdapter = new SqlDataAdapter(sqlCommand);
                        dataAdapter.Fill(dataTable);
                        _abItemsDataTable.Merge(dataTable);
                    }
                    finally
                    {
                        sqlConnection.Close();
                    }
                }
            }
        }

        public List<Data.Advertiser> GetAdvertisers()
        {
            var result = new List<Data.Advertiser>();
            var db = new EntityModel.DAMain1Entities1();
            var advertisers = db.Advertisers.OrderBy(c => c.name);
            foreach (var advertiser in advertisers)
            {
                result.Add((Data.Advertiser)advertiser);
            }
            return result;
        }

        // Fill the result up with items for the given advertiser
        public List<Data.ABItem> GetABItems(string advertiserName, bool convertAmountsToStartingBalanceCurrency)
        {
            var db = new EntityModel.DAMain1Entities1();
            var result = new List<Data.ABItem>();

            // The advertiser
            var advertiser = db.Advertisers.First(c => c.name == advertiserName);

            // The advertiser starting balance
            var advertiserStartingBalance =
                advertiser.AdvertiserStartingBalances.FirstOrDefault(c => c.effective_date.Year == DateTime.Now.Year);

            // Translate the starting balance to an item and it add to the result
            if (advertiserStartingBalance != null)
            {
                result.Add(new Data.ABItem
                {
                    Period = "-",
                    Description = "STARTING BALANCE",
                    Quantity = 1,
                    Amount = advertiserStartingBalance.amount,
                    Total = advertiserStartingBalance.amount,
                    Currency = advertiserStartingBalance.currency,
                });
            }

            // The accounting periods
            var accountingPeriods = db.AccountingPeriods;

            // Iterate over the accounting periods
            foreach (var accountingPeriod in accountingPeriods)
            {
                // The items in this accounting period
                var abItems = from c in _abItemsDataTable.AsEnumerable()
                              where c.Field<string>("Advertiser") == advertiserName
                              && c.Field<string>("Period") == accountingPeriod.Name
                              select c;

                // Iterate over the items in this accounting period
                foreach (var abItem in abItems)
                {
                    // Translate the item and it add to the result
                    result.Add(new Data.ABItem
                    {
                        Period = accountingPeriod.Name,
                        Description = abItem.Field<string>("Description"),
                        Quantity = abItem.Field<decimal>("Quantity"),
                        Amount = abItem.Field<decimal>("Amount"),
                        Total = abItem.Field<decimal>("Total"),
                        Currency = abItem.Field<string>("Currency"),
                    });
                }

                // The quickbooks customer
                var customer = advertiser.Customer;

                // The received payments with transaction date falling within the accounting period
                var receivedPayments = from rp in db.ReceivePayments
                                       where rp.txn_date >= accountingPeriod.DateStart
                                       && rp.txn_date <= accountingPeriod.DateStop
                                       && rp.customer_ref_list_id == customer.quickbooks_list_id
                                       select rp;

                // Iterate over the received payments with transaction date falling within the accounting period
                foreach (var receivedPayment in receivedPayments)
                {
                    // Transalte the received payment to an item and add it to the result
                    result.Add(new Data.ABItem
                    {
                        Period = accountingPeriod.Name,
                        Description = "PAYMENT",
                        Quantity = 1,
                        Amount = receivedPayment.total_amount,
                        Total = receivedPayment.total_amount,
                        Currency = receivedPayment.currency,
                    });
                }
            }

            if (result.Count > 0 && convertAmountsToStartingBalanceCurrency)
            {
                string targetCurrency;
                if (result[0].Description == "STARTING BALANCE")
                {
                    targetCurrency = result[0].Currency;
                }
                else
                {
                    targetCurrency = "USD";
                }

                foreach (var accountingPeriod in accountingPeriods)
                {
                    string cs = accountingPeriod.DADBs.First().DBInstance.ConnectionString;
                    string query = "SELECT name, to_usd_multiplier FROM Currency";
                    using (var sqlConnection = new SqlConnection(cs))
                    using (var sqlCommand = new SqlCommand(query, sqlConnection))
                    {
                        try
                        {
                            var currenciesForAccountingPeriod = new DataTable();
                            (new SqlDataAdapter(sqlCommand)).Fill(currenciesForAccountingPeriod);
                            var targetCurrencyRow = currenciesForAccountingPeriod.AsEnumerable().First(c => (string)c["name"] == targetCurrency);
                            foreach (var item in result.Where(c => c.Period == accountingPeriod.Name))
                            {
                                var sourceCurrencyRow = currenciesForAccountingPeriod.AsEnumerable().First(c => (string)c["name"] == item.Currency);
                                var conversionFactor = (decimal)sourceCurrencyRow["to_usd_multiplier"] / (decimal)targetCurrencyRow["to_usd_multiplier"];
                                item.Amount *= conversionFactor;
                                item.Total *= conversionFactor;
                                item.Currency = targetCurrency;
                            }
                        }
                        finally
                        {
                            sqlConnection.Close();
                        }
                    }                   
                }
            }

            return result;
        }

        DataTable _abItemsDataTable;

        string _abItemsQuery =
@"SELECT
    A.name                      Advertiser
	,'{0}'                      Period
    ,CP.campaign_name           Description
    ,SUM(I.num_units)           Quantity
    ,I.revenue_per_unit         Amount
	,SUM(I.total_revenue)*-1    Total
    ,C.name                     Currency
    ,CU.quickbooks_list_id      CustomerListId
FROM
	Item                                I
	INNER JOIN  Currency                C       ON I.revenue_currency_id=C.id
	INNER JOIN  Campaign                CP      ON I.pid=CP.pid
	INNER JOIN  Advertiser              A       ON CP.advertiser_id=A.id
	INNER JOIN  CampaignStatus          CS      ON CP.campaign_status_id=CS.id
    INNER JOIN  DAMain1.dbo.Customer    CU      ON A.customer_id=CU.id
GROUP BY C.name,A.name,A.id,CS.name,A.customer_id,I.revenue_per_unit,CP.campaign_name,CU.name,CU.quickbooks_list_id
HAVING (CS.name='Verified')";
    }
}
//        string _abItemsQuery =
//@"SELECT
//    A.name                                  Advertiser
//    --,A.id                                 AdvertiserId
//	,'{0}'                                  Period
//    ,CP.campaign_name                       Description
//    ,SUM(I.num_units)                       Quantity
//    ,I.revenue_per_unit                     Amount
//	,SUM(I.total_revenue)*-1                Total
//    --,C.name + ' ' + CONVERT(VARCHAR(20),SUM(I.total_revenue)) Total2
//    ,C.name Currency
//    --,A.customer_id CustomerId
//    --,CU.name CustomerName
//    ,CU.quickbooks_list_id CustomerListId
//FROM
//	Item I
//	INNER JOIN Currency C ON I.revenue_currency_id=C.id
//	INNER JOIN Campaign CP ON I.pid=CP.pid
//	INNER JOIN Advertiser A ON CP.advertiser_id=A.id
//	INNER JOIN CampaignStatus CS ON CP.campaign_status_id=CS.id
//    INNER JOIN DAMain1.dbo.Customer CU ON A.customer_id=CU.id
//GROUP BY
//		C.name
//	,A.name
//	,A.id
//	,CS.name
//    ,A.customer_id
//    ,I.revenue_per_unit
//    ,CP.campaign_name
//    ,CU.name
//    ,CU.quickbooks_list_id
//HAVING (
//	CS.name='Verified'
//)";