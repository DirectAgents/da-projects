using System;
using System.Collections.Generic;
using System.Data.Objects;
using System.Data.SqlClient;
using System.Linq;
using System.Xml.Linq;
using DAgents.Common;
using Microsoft.Practices.Unity;

namespace EomApp1.Formss.AB2.Model
{
    public class ABModelFactory : IDirectAgentsEntitiesFactory
    {
        public void Init()
        {
            InitUnits();
        }

        public DirectAgentsEntities GetModel()
        {
            return new DirectAgentsEntities(this.EntityConnectionString);
        }

        private void InitUnits()
        {
            using (var model = new DirectAgentsEntities(EntityConnectionString))
            {
                model.DeleteUnits();
                foreach (var item in UnitNameSource.UnitNames)
                {
                    model.Units.AddObject(new Unit { Name = item });
                }
            }
        }

        [Dependency("EntityConnectionString")]
        public string EntityConnectionString { get; set; }

        [Dependency]
        public IUnitNameSource UnitNameSource { get; set; }

        string _connectionString;

        /// <summary>
        /// 
        /// </summary>
        public dynamic Settings;

        #region Constructors
        /// <summary>
        /// 
        /// </summary>
        /// <param name="connectionString"></param>
        /// <param name="settings"></param>
        public ABModelFactory(string connectionString, Utilities.XSettings settings)
        {
            _connectionString = connectionString;
            Settings = settings;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="connectionString"></param>
        /// <param name="settings"></param>
        public ABModelFactory(string connectionString, XDocument settings)
        {
            _connectionString = connectionString;
            Settings = new Utilities.XSettings(settings.Root);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="connectionString"></param>
        public ABModelFactory(string connectionString)
        {
            _connectionString = connectionString;
        } 
        #endregion

        /// <summary>
        /// Create all of the units.
        /// USD, GBP, etc..
        /// </summary>
        public void CreateUnits()
        {
            using (var model = CreateModel())
            {
                foreach (string unitName in Settings.Units)
                {
                    model.Units.AddObject(
                                    new Unit
                                    {
                                        Name = unitName
                                    });
                }

                model.SaveChanges();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="paymentsXML"></param>
        /// <param name="companyFile"></param>
        //public void CreatePayments(string paymentsXML, string companyFile)
        //{
        //    dynamic payments = new Utilities.XSettings(System.IO.File.ReadAllText(paymentsXML));

        //    using (var db = CreateModel())
        //    {
        //        foreach (var payment in payments.ReceivedPayments)
        //        {
        //            string txnID = payment.TxnID;
        //            string txnNumber = payment.TxnNumber;

        //            string memo;
        //            try
        //            {
        //                memo = payment.Memo;
        //            }
        //            catch
        //            {
        //                memo = string.Empty;
        //            }

        //            string customerRefListID;
        //            try
        //            {
        //                customerRefListID = payment.CustomerRefListID;
        //            }
        //            catch (Exception)
        //            {
        //                Console.WriteLine("warning: no customerRefListID for txnID, skipping" + payment.TxnID);
        //                continue;
        //            }

        //            string aRAccountRefFullName = payment.ARAccountRefFullName;
        //            string currency = "USD";
        //            foreach (var name in db.Units.Select(c => c.Name))
        //            {
        //                if (aRAccountRefFullName.Contains(name))
        //                {
        //                    currency = name;
        //                    break;
        //                }
        //            }

        //            decimal totalAmount;
        //            if (!Decimal.TryParse(payment.TotalAmount, out totalAmount))
        //            {
        //                throw new Exception("could not parse " + payment.TotalAmount);
        //            }

        //            DateTime txnDate;
        //            if (!DateTime.TryParse(payment.TxnDate, out txnDate))
        //            {
        //                throw new Exception("could not parse date " + payment.TxnDate);
        //            }

        //            db.ReceivedPayments.AddObject(new ReceivedPayment
        //            {
        //                TxnNumber = txnNumber,
        //                Memo = memo,
        //                Credit = new Credit
        //                {
        //                    Description = "Payment #" + txnNumber + ", " + memo,
        //                    Item = new Item
        //                    {
        //                        DateSpan = db.DateSpans.FirstOrDefault(c => c.FromDate == txnDate) ?? new DateSpan
        //                        {
        //                            FromDate = txnDate
        //                        },
        //                        Amount = new Amount
        //                        {
        //                            Number = totalAmount,
        //                            Unit = db.Units.First(c => c.Name == currency)
        //                        },
        //                        Client = (from c in db.Customers
        //                                  where c.ListId == customerRefListID
        //                                  select c.Client).First()
        //                    }
        //                }
        //            });
        //        }
        //        db.SaveChanges();
        //    }
        //}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="customersXML"></param>
        /// <param name="companyFile"></param>
        //public void CreateCustomers(string customersXML, string companyFile)
        //{
        //    dynamic customers = new DAgents.Common.Utilities.XSettings(System.IO.File.ReadAllText(customersXML));

        //    using (var db = CreateModel())
        //    {
        //        foreach (var customer in customers.Customers)
        //        {
        //            string listID = customer.ListID;
        //            string customerName = customer.FullName;

        //            if (!db.Customers.Any(
        //                c => c.Name == customerName && c.ListId == listID))
        //            {
        //                db.Customers.AddObject(new Customer
        //                {
        //                    Name = customerName,
        //                    ListId = listID,
        //                    Client = new Client
        //                    {
        //                        Name = "(unk. advertiser)/" + customerName
        //                    },
        //                    QuickBooksCompanyFile = db.QuickBooksCompanyFiles.First(c => c.Name == companyFile)
        //                });
        //            }
        //        }
        //        db.SaveChanges();
        //    }
        //}

        /// <summary>
        /// 
        /// </summary>
        //public void CreateStartingBalanceClients()
        //{
        //    foreach (var sbal in Settings.StartingBalances)
        //    {
        //        using (var db = CreateModel())
        //        {
        //            string advertiserName = sbal.Advertiser;
        //            string customerName = sbal.Customer;
        //            string companyFileName = sbal.CompanyFile;
        //            string clientName = advertiserName + "/" + customerName;
        //            string listID = sbal.ListId;

        //            if (db.Clients.FirstOrDefault(c => c.Name == clientName) == null)
        //            {
        //                var client = new Client
        //                {
        //                    Name = clientName
        //                };
        //                db.Clients.AddObject(client);

        //                var advertiser = db.Advertisers.FirstOrDefault(c => c.Name == advertiserName) ?? new Advertiser
        //                {
        //                    Name = advertiserName,
        //                    Client = client
        //                };

        //                var customer = db.Customers.FirstOrDefault(c => c.Name == customerName) ?? new Customer
        //                {
        //                    Name = customerName,
        //                    Client = client,
        //                    QuickBooksCompanyFile = db.QuickBooksCompanyFiles.First(c => c.Name == companyFileName),
        //                    ListId = listID
        //                };
        //            }
        //            db.SaveChanges();
        //        }
        //    }
        //}

        /// <summary>
        /// 
        /// </summary>
        public void CreateStartingBalanceDateSpans()
        {
            using (var db = CreateModel())
            {
                db.DateSpans.AddObject(new DateSpan
                {
                    From = new DateTime(2011, 1, 1)
                });
                db.SaveChanges();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        //public void CreateStartingBalanceCredits()
        //{
        //    using (var db = CreateModel())
        //    {
        //        foreach (var sbal in Settings.StartingBalances)
        //        {
        //            string advertiserName = sbal.Advertiser;
        //            string customerName = sbal.Customer;
        //            string clientName = advertiserName + "/" + customerName;
        //            string currency = sbal.Currency;

        //            decimal amount;
        //            if (!Decimal.TryParse(sbal.Amount, out amount))
        //            {
        //                Console.WriteLine("could not parse " + sbal.Amount);
        //            }

        //            if (db.StartingBalances.FirstOrDefault(
        //                c => c.Credit.Item.Client.Name == clientName) == null)
        //            {
        //                db.StartingBalances.AddObject(new StartingBalance
        //                {
        //                    Credit = new Credit
        //                    {
        //                        Description = "Starting Balance 2011",
        //                        Item = new Item
        //                        {
        //                            Amount = new Amount
        //                            {
        //                                Number = amount,
        //                                Unit = db.Units.First(c => c.Name == currency)
        //                            },
        //                            DateSpan = db.DateSpans.First(c => c.FromDate == new DateTime(2011, 1, 1)),
        //                            Client = db.Clients.First(c => c.Name == clientName)
        //                        }
        //                    }
        //                });
        //            }
        //        }
        //        db.SaveChanges();
        //    }
        //}

        /// <summary>
        /// 
        /// </summary>
        //public void CreatePeriodDateSpansAndDatabaseRecordSources()
        //{
        //    using (var db = CreateModel())
        //    {
        //        foreach (var dadb in Settings.DirectAgentsDatabases)
        //        {
        //            string name = dadb.PeriodName;
        //            DateTime from = dadb.FromDate;
        //            DateTime to = dadb.ToDate;
        //            var period = new Period
        //            {
        //                Name = name,
        //                DateSpan = new DateSpan
        //                {
        //                    FromDate = from,
        //                    ToDate = to
        //                }
        //            };
        //            db.Periods.AddObject(period);
        //            db.SqlServerDatabases.AddObject(new SqlServerDatabase
        //            {
        //                Name = dadb.DatabaseName,
        //                ConnectionString = dadb.ConStr,
        //                RecordSource = new RecordSource
        //                {
        //                    Name = "Record Source for " + dadb.DatabaseName,
        //                    RecordSourceType = db.RecordSourceTypes.First(c => c.Name == "DADatabase"),
        //                    DateSpan = period.DateSpan
        //                }
        //            });
        //        }
        //        db.SaveChanges();
        //    }
        //}

        /// <summary>
        /// 
        /// </summary>
        public void CreateCompanyFiles()
        {
            using (var db = CreateModel())
            {
                foreach (string name in Settings.CompanyFiles)
                    db.QuickBooksCompanyFiles.AddObject(new QuickBooksCompanyFile { Name = name });
                db.SaveChanges();
            }
        }

        #region Advertising Revenue
        //public void CreateAdvertisingRevenue()
        //{
        //    foreach (var database in this.SqlServerDatabaseList)
        //    {
        //        using (var sqlCon = new SqlConnection(database.ConnectionString))
        //        using (var sqlCmd = new SqlCommand(_selectAdvertisingRevenueRecordsFromDADatabaseQuery, sqlCon))
        //        {
        //            sqlCon.Open();

        //            SqlDataReader reader = sqlCmd.ExecuteReader();

        //            while (reader.Read())
        //                AddAdvertisingRevenue(reader, database.RecordSource.DateSpan);

        //            sqlCon.Close();
        //        }
        //    }
        //}

        //void AddAdvertisingRevenue(SqlDataReader rd, DateSpan dts)
        //{
        //    string advertiserName = (string)rd["Advertiser"];
        //    string currency = (string)rd["Currency"];
        //    decimal amount = (decimal)rd["Amount"];
        //    string status = (string)rd["Status"];
        //    int pid = (int)rd["CampaignPid"];
        //    string campaignName = (string)rd["CampaignName"];

        //    using (var db = CreateModel())
        //    {
        //        Advertiser advertiser = db.Advertisers.FirstOrDefault(c => c.Name == advertiserName);
        //        if (advertiser == null)
        //        {
        //            db.Advertisers.AddObject(
        //                new Advertiser
        //                {
        //                    Name = advertiserName,
        //                    Client = new Client
        //                    {
        //                        Name = advertiserName
        //                    }
        //                }
        //            );
        //            db.SaveChanges();
        //            advertiser = db.Advertisers.First(c => c.Name == advertiserName);
        //        }

        //        db.AdvertisingRevenues.AddObject(new AdvertisingRevenue
        //        {
        //            Charge = new Charge
        //            {
        //                Debit = new Debit
        //                {
        //                    Description = dts.FromDate.ToString("y") + " Activity", //Charge to " + client.Name + " for activity in PIDs (" + campaignIDs + ")",
        //                    Item = new Item
        //                    {
        //                        DateSpan = db.DateSpans.First(c => c.Id == dts.Id),
        //                        Amount = new Amount
        //                        {
        //                            Number = amount,
        //                            Unit = db.Units.First(c => c.Name == currency)
        //                        },
        //                        Client = advertiser.Client
        //                    }
        //                },
        //                Status = status
        //            },
        //            Campaign = db.Campaigns.FirstOrDefault(c => c.Pid == pid) ?? new Campaign
        //            {
        //                Pid = pid,
        //                Name = campaignName
        //            }
        //        });

        //        db.SaveChanges();
        //    }
        //}

        static string _selectAdvertisingRevenueRecordsFromDADatabaseQuery =
@"SELECT TOP (100) PERCENT
    dbo.Advertiser.name AS Advertiser, 
    dbo.Currency.name AS Currency, 
    SUM(dbo.Item.total_revenue) AS Amount, 
    dbo.CampaignStatus.name AS Status, 
    dbo.Campaign.pid AS CampaignPid,
    dbo.Campaign.campaign_name AS CampaignName
FROM
    dbo.Item INNER JOIN
    dbo.Campaign ON dbo.Item.pid = dbo.Campaign.pid INNER JOIN
    dbo.Advertiser ON dbo.Campaign.advertiser_id = dbo.Advertiser.id INNER JOIN
    dbo.Currency ON dbo.Item.revenue_currency_id = dbo.Currency.id INNER JOIN
    dbo.CampaignStatus ON dbo.Campaign.campaign_status_id = dbo.CampaignStatus.id
GROUP BY 
    dbo.Advertiser.name, 
    dbo.Currency.name, 
    dbo.CampaignStatus.name,
    dbo.Campaign.pid,
    dbo.Campaign.campaign_name";
        #endregion

        List<SqlServerDatabase> SqlServerDatabaseList
        {
            get
            {
                return CreateModel().SqlServerDatabases.ToList();
            }
        }

        #region Charges
        ///// <summary>
        ///// 
        ///// </summary>
        //public void CreateCharges()
        //{
        //    foreach (var database in this.SqlServerDatabaseList)
        //    {
        //        using (var sqlCon = new SqlConnection(database.ConnectionString))
        //        using (var sqlCmd = new SqlCommand(_selectChargeRecordsFromDADatabaseQuery, sqlCon))
        //        {
        //            sqlCon.Open();

        //            SqlDataReader reader = sqlCmd.ExecuteReader();

        //            while (reader.Read())
        //                AddCharge(reader, database.RecordSource.DateSpan);

        //            sqlCon.Close();
        //        }
        //    }
        //}

        //Charge AddCharge(SqlDataReader rd, DateSpan dts)
        //{
        //    string advertiserName = (string)rd["Advertiser"];
        //    string currency = (string)rd["Currency"];
        //    decimal amount = (decimal)rd["Amount"];
        //    string status = (string)rd["Status"];
        //    //string campaignIDs = (string)rd["CampaignIDs"];

        //    using (var db = this.DirectAgentsEntities)
        //    {
        //        Advertiser advertiser = db.Advertisers.FirstOrDefault(c => c.Name == advertiserName);
        //        if (advertiser == null)
        //        {
        //            db.Advertisers.AddObject(
        //                new Advertiser
        //                {
        //                    Name = advertiserName,
        //                    Client = new Client
        //                    {
        //                        Name = advertiserName
        //                    }
        //                }
        //            );
        //            db.SaveChanges();
        //            advertiser = db.Advertisers.First(c => c.Name == advertiserName);
        //        }

        //        Client client = advertiser.Client;

        //        Charge ch;
        //        ch = new Charge
        //        {
        //            Debit = new Debit
        //            {
        //                Description = dts.FromDate.ToString("y") + " Activity", //Charge to " + client.Name + " for activity in PIDs (" + campaignIDs + ")",
        //                Item = new Item
        //                {
        //                    DateSpan = db.DateSpans.First(c => c.Id == dts.Id),
        //                    Amount = new Amount
        //                    {
        //                        Number = amount,
        //                        Unit = db.Units.First(c => c.Name == currency)
        //                    },
        //                    Client = client
        //                }
        //            },
        //            Status = status
        //        };

        //        db.Charges.AddObject(ch);

        //        db.SaveChanges();

        //        return ch;
        //    }
        //}

        //        /// <summary>
        //        /// 
        //        /// </summary>
        //        static string _selectChargeRecordsFromDADatabaseQuery =
        //@"SELECT TOP (100) PERCENT
        //    dbo.Advertiser.name AS Advertiser, 
        //    dbo.Currency.name AS Currency, 
        //    SUM(dbo.Item.total_revenue) AS Amount, 
        //    dbo.CampaignStatus.name AS Status, 
        //    --dbo.SumKeys(dbo.Campaign.pid) AS CampaignIDs
        //FROM
        //    dbo.Item INNER JOIN
        //    dbo.Campaign ON dbo.Item.pid = dbo.Campaign.pid INNER JOIN
        //    dbo.Advertiser ON dbo.Campaign.advertiser_id = dbo.Advertiser.id INNER JOIN
        //    dbo.Currency ON dbo.Item.revenue_currency_id = dbo.Currency.id INNER JOIN
        //    dbo.CampaignStatus ON dbo.Campaign.campaign_status_id = dbo.CampaignStatus.id
        //GROUP BY 
        //    dbo.Advertiser.name, 
        //    dbo.Currency.name, 
        //    dbo.CampaignStatus.name"; 
        #endregion


        /// <summary>
        /// 
        /// </summary>
        //public void Delete()
        //{
        //    using (var db = CreateModel())
        //    {
        //        db.Periods.DeleteObjects(db.Periods);
        //        db.SaveChanges();
        //    }
        //    using (var db = CreateModel())
        //    {
        //        db.StartingBalances.DeleteObjects(db.StartingBalances);
        //        db.SaveChanges();
        //    }
        //    using (var db = CreateModel())
        //    {
        //        db.AdvertisingRevenues.DeleteObjects(db.AdvertisingRevenues);
        //        db.SaveChanges();
        //    }
        //    using (var db = CreateModel())
        //    {
        //        db.Campaigns.DeleteObjects(db.Campaigns);
        //        db.SaveChanges();
        //    }
        //    using (var db = CreateModel())
        //    {
        //        db.Charges.DeleteObjects(db.Charges);
        //        db.SaveChanges();
        //    }
        //    using (var db = CreateModel())
        //    {
        //        db.ReceivedPayments.DeleteObjects(db.ReceivedPayments);
        //        db.SaveChanges();
        //    }
        //    using (var db = CreateModel())
        //    {
        //        db.Debits.DeleteObjects(db.Debits);
        //        db.SaveChanges();
        //    }
        //    using (var db = CreateModel())
        //    {
        //        db.Credits.DeleteObjects(db.Credits);
        //        db.SaveChanges();
        //    }
        //    using (var db = CreateModel())
        //    {
        //        db.Items.DeleteObjects(db.Items);
        //        db.SaveChanges();
        //    }
        //    using (var db = CreateModel())
        //    {
        //        db.Amounts.DeleteObjects(db.Amounts);
        //        db.SaveChanges();
        //    }
        //    using (var db = CreateModel())
        //    {
        //        db.Units.DeleteObjects(db.Units);
        //        db.SaveChanges();
        //    }
        //    using (var db = CreateModel())
        //    {
        //        db.UnitConversions.DeleteObjects(db.UnitConversions);
        //        db.SaveChanges();
        //    }
        //    using (var db = CreateModel())
        //    {
        //        db.SqlServerDatabases.DeleteObjects(db.SqlServerDatabases);
        //        db.SaveChanges();
        //    }
        //    using (var db = CreateModel())
        //    {
        //        db.Advertisers.DeleteObjects(db.Advertisers);
        //        db.SaveChanges();
        //    }
        //    using (var db = CreateModel())
        //    {
        //        db.Customers.DeleteObjects(db.Customers);
        //        db.SaveChanges();
        //    }
        //    using (var db = CreateModel())
        //    {
        //        db.QuickBooksCompanyFiles.DeleteObjects(db.QuickBooksCompanyFiles);
        //        db.SaveChanges();
        //    }
        //    using (var db = CreateModel())
        //    {
        //        db.Clients.DeleteObjects(db.Clients);
        //        db.SaveChanges();
        //    }
        //    //using (var db = CreateModel())
        //    //{
        //    //    db.RecordSources.DeleteObjects(db.RecordSources);
        //    //    db.SaveChanges();
        //    //}
        //    using (var db = CreateModel())
        //    {
        //        db.DateSpans.DeleteObjects(db.DateSpans);
        //        db.SaveChanges();
        //    }
        //    //using (var db = CreateModel())
        //    //{
        //    //    db.RecordSourceTypes.DeleteObjects(db.RecordSourceTypes);
        //    //    db.SaveChanges();
        //    //}
        //}

        /// <summary>
        /// 
        /// </summary>
        public DirectAgentsEntities CreateModel()
        {
            string entityConnectionFormat = @"metadata=res://*/Formss.AB2.Model.ABModel.csdl|res://*/Formss.AB2.Model.ABModel.ssdl|res://*/Formss.AB2.Model.ABModel.msl;provider=System.Data.SqlClient;provider connection string=""{0};multipleactiveresultsets=True;App=EntityFramework""";
            string entityConnectionString = String.Format(entityConnectionFormat, _connectionString);
            return new DirectAgentsEntities(entityConnectionString);
        }

        #region Old
        //public DirectAgentsEntities Create()
        //{
        //    Delete();
        //    AddPeriods();
        //    AddRecordSourceTypes();
        //    AddRecordSources();
        //    AddDatabases();
        //    AddCompanyFiles();
        //    AddCustomerClients();
        //    AddAdvertiserClients();
        //    AddUnits();
        //    AddUnitConversions();
        //    AddReceivedPaymentCreditItemAmounts();
        //    AddCharges();
        //    var db = this.DirectAgentsEntities;
        //    return db;
        //}

        //private void AddCharges()
        //{
        //    var db = this.DirectAgentsEntities;
        //    Charge ch;

        //    ch = new Charge
        //    {
        //        Debit = new Debit
        //        {
        //            Description = "Debit for Charge 1",
        //            Item = new Item
        //            {
        //                DateSpan = new DateSpan
        //                {
        //                    FromDate = new DateTime(2011, 6, 1),
        //                    ToDate = new DateTime(2011, 6, 30)
        //                },
        //                Amount = new Amount
        //                {
        //                    Number = 5.25m,
        //                    Unit = db.Units.First(c => c.Name == "USD")
        //                },
        //                Client = db.Clients.First(c => c.Name == "Client 1")
        //            }
        //        },
        //        Status = "Active"
        //    };
        //    db.Charges.AddObject(ch);

        //    ch = new Charge
        //    {
        //        Debit = new Debit
        //        {
        //            Description = "Charge 2",
        //            Item = new Item
        //            {
        //                DateSpan = new DateSpan
        //                {
        //                    FromDate = new DateTime(2011, 6, 1),
        //                    ToDate = new DateTime(2011, 6, 30)
        //                },
        //                Amount = new Amount
        //                {
        //                    Number = 500m,
        //                    Unit = db.Units.First(c => c.Name == "USD")
        //                },
        //                Client = db.Clients.First(c => c.Name == "Client 2")
        //            }
        //        },
        //        Status = "Finalized"
        //    };
        //    db.Charges.AddObject(ch);

        //    ch = new Charge
        //    {
        //        Debit = new Debit
        //        {
        //            Description = "Charge 3",
        //            Item = new Item
        //            {
        //                DateSpan = new DateSpan
        //                {
        //                    FromDate = new DateTime(2011, 6, 1),
        //                    ToDate = new DateTime(2011, 6, 30)
        //                },
        //                Amount = new Amount
        //                {
        //                    Number = 1500m,
        //                    Unit = db.Units.First(c => c.Name == "USD")
        //                },
        //                Client = db.Clients.First(c => c.Name == "Client 2")
        //            }
        //        },
        //        Status = "Verified"
        //    };
        //    db.Charges.AddObject(ch);

        //    db.SaveChanges();
        //}

        //private void AddReceivedPaymentCreditItemAmounts()
        //{
        //    var db = this.DirectAgentsEntities;
        //    ReceivedPayment rp;

        //    rp = new ReceivedPayment
        //    {
        //        Credit = new Credit
        //        {
        //            Description = "Credit for Payment 1",
        //            Item = new Item
        //            {
        //                DateSpan = new DateSpan
        //                {
        //                    FromDate = new DateTime(2011, 6, 5)
        //                },
        //                Amount = new Amount
        //                {
        //                    Number = 101,
        //                    Unit = db.Units.First(c => c.Name == "USD")
        //                },
        //                Client = db.Clients.First(c => c.Name == "Client 1")
        //            }
        //        }
        //    };
        //    db.ReceivedPayments.AddObject(rp);

        //    rp = new ReceivedPayment
        //    {
        //        Credit = new Credit
        //        {
        //            Description = "Credit to Client 2",
        //            Item = new Item
        //            {
        //                DateSpan = new DateSpan
        //                {
        //                    FromDate = new DateTime(2011, 6, 2)
        //                },
        //                Amount = new Amount
        //                {
        //                    Number = 5000,
        //                    Unit = db.Units.First(c => c.Name == "USD")
        //                },
        //                Client = db.Clients.First(c => c.Name == "Client 2")
        //            }
        //        }
        //    };
        //    db.ReceivedPayments.AddObject(rp);

        //    db.SaveChanges();
        //}

        //private void AddUnitConversions()
        //{
        //    var db = this.DirectAgentsEntities;
        //    UnitConversion uc;
        //    uc = new UnitConversion
        //    {
        //        Date = new DateTime(2011, 6, 1),
        //        FromUnit = db.Units.First(c => c.Name == "USD"),
        //        ToUnit = db.Units.First(c => c.Name == "USD"),
        //        Coefficient = 1m
        //    };
        //    db.UnitConversions.AddObject(uc);
        //    uc = new UnitConversion
        //    {
        //        Date = new DateTime(2011, 7, 1),
        //        FromUnit = db.Units.First(c => c.Name == "USD"),
        //        ToUnit = db.Units.First(c => c.Name == "USD"),
        //        Coefficient = 1m
        //    };
        //    db.UnitConversions.AddObject(uc);
        //    uc = new UnitConversion
        //    {
        //        Date = new DateTime(2011, 6, 1),
        //        FromUnit = db.Units.First(c => c.Name == "USD"),
        //        ToUnit = db.Units.First(c => c.Name == "GBP"),
        //        Coefficient = .75m
        //    };
        //    db.UnitConversions.AddObject(uc);
        //    uc = new UnitConversion
        //    {
        //        Date = new DateTime(2011, 7, 1),
        //        FromUnit = db.Units.First(c => c.Name == "USD"),
        //        ToUnit = db.Units.First(c => c.Name == "GBP"),
        //        Coefficient = .77m
        //    };
        //    db.UnitConversions.AddObject(uc);
        //    db.SaveChanges();
        //}

        //private void AddUnits()
        //{
        //    var db = this.DirectAgentsEntities;
        //    Unit u;
        //    u = new Unit
        //    {
        //        Name = "USD"
        //    };
        //    db.Units.AddObject(u);
        //    u = new Unit
        //    {
        //        Name = "GBP"
        //    };
        //    db.Units.AddObject(u);
        //    db.SaveChanges();
        //}

        //private void AddAdvertiserClients()
        //{
        //    var db = this.DirectAgentsEntities;
        //    Advertiser a;
        //    a = new Advertiser
        //    {
        //        Name = "Advertiser 1",
        //        Client = db.Clients.First(c => c.Name == "Client 1")
        //    };
        //    db.SaveChanges();
        //}

        //private void AddCustomerClients()
        //{
        //    var db = this.DirectAgentsEntities;
        //    Customer cu;

        //    cu = new Customer
        //    {
        //        Name = "Customer 1 (QBUS)",
        //        Client = new Client
        //        {
        //            Name = "Client 1"
        //        },
        //        CompanyFile = db.CompanyFiles.First(c => c.Name == "US")
        //    };

        //    cu = new Customer
        //    {
        //        Name = "Customer 2 (QBUS)",
        //        Client = new Client
        //        {
        //            Name = "Client 2"
        //        },
        //        CompanyFile = db.CompanyFiles.First(c => c.Name == "US")
        //    };

        //    db.SaveChanges();
        //}

        //private void AddRecordSourceTypes()
        //{
        //    var db = this.DirectAgentsEntities;
        //    RecordSourceType rst;

        //    rst = new RecordSourceType
        //    {
        //        Name = "Accounting Item Records",
        //    };
        //    db.RecordSourceTypes.AddObject(rst);

        //    db.SaveChanges();
        //}

        //private void AddRecordSources()
        //{
        //    var db = this.DirectAgentsEntities;
        //    RecordSource rs;
        //    RecordSourceType rst = db.RecordSourceTypes.First(c => c.Name == "Accounting Item Records");

        //    rs = new RecordSource
        //    {
        //        Name = "Jun11 Records",
        //        RecordSourceType = rst
        //    };
        //    db.RecordSources.AddObject(rs);

        //    rs = new RecordSource
        //    {
        //        Name = "Jul11 Records",
        //        RecordSourceType = rst
        //    };
        //    db.RecordSources.AddObject(rs);

        //    db.SaveChanges();
        //}

        //private void AddDatabases()
        //{
        //    var db = this.DirectAgentsEntities;
        //    SqlServerDatabase ss;
        //    RecordSource rs;

        //    rs = db.RecordSources.First(c => c.Name == "Jun11 Records");
        //    ss = new SqlServerDatabase
        //    {
        //        Name = @"DADatabaseJun11 on Biz2\da",
        //        ConnectionString = @"data source=biz2\da;initial catalog=DADatabaseJun11;integrated security=True;",
        //        RecordSource = rs
        //    };
        //    db.SqlServerDatabases.AddObject(ss);

        //    rs = db.RecordSources.First(c => c.Name == "Jul11 Records");
        //    ss = new SqlServerDatabase
        //    {
        //        Name = @"DADatabaseJul11 on Biz2\da",
        //        ConnectionString = @"data source=biz2\da;initial catalog=DADatabaseJul11;integrated security=True;",
        //        RecordSource = rs
        //    };
        //    db.SqlServerDatabases.AddObject(ss);

        //    db.SaveChanges();
        //}

        //private void AddPeriods()
        //{
        //    var db = this.DirectAgentsEntities;

        //    Period june = new Period
        //    {
        //        Name = "Jun 2011",
        //        DateSpan = new DateSpan
        //        {
        //            FromDate = new DateTime(2011, 6, 1),
        //            ToDate = new DateTime(2011, 6, 30)
        //        }
        //    };
        //    db.Periods.AddObject(june);

        //    Period july = new Period
        //    {
        //        Name = "Jul 2011",
        //        DateSpan = new DateSpan
        //        {
        //            FromDate = new DateTime(2011, 7, 1),
        //            ToDate = new DateTime(2011, 7, 31)
        //        }
        //    };
        //    db.Periods.AddObject(july);

        //    db.SaveChanges();
        //} 
        #endregion
    }

    /// <summary>
    /// 
    /// </summary>
    static class Extensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="set"></param>
        /// <param name="data"></param>
        static public void DeleteObjects<TEntity>(this ObjectSet<TEntity> set, IEnumerable<TEntity> data) where TEntity : class
        {
            if (set.Count() == 0) return;

            foreach (var entity in data)
                set.DeleteObject(entity);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="set"></param>
        /// <param name="data"></param>
        static public void AddObjects<TEntity>(this ObjectSet<TEntity> set, params TEntity[] data) where TEntity : class
        {
            foreach (var entity in data)
                set.AddObject(entity);
        }
    }
}
