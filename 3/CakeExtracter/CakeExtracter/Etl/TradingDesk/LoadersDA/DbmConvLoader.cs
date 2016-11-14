using System.Collections.Generic;
using System.Linq;
using CakeExtracter.Etl.TradingDesk.Extracters;
using CakeExtracter.Etl.TradingDesk.Loaders;
using DirectAgents.Domain.Contexts;
using DirectAgents.Domain.Entities.CPProg;
using System;

namespace CakeExtracter.Etl.TradingDesk.LoadersDA
{
    public class DbmConvLoader : Loader<DataTransferRow>
    {
        private DbmConvConverter convConverter;
        private Dictionary<int, int> accountIdLookupByExtId = new Dictionary<int, int>();
        private Dictionary<string, int> countryIdLookupByName = new Dictionary<string, int>();
        private Dictionary<string, int> cityIdLookupByName = new Dictionary<string, int>();

        public DbmConvLoader(DbmConvConverter convConverter)
        {
            this.convConverter = convConverter;
        }

        protected override int Load(List<DataTransferRow> items)
        {
            UpdateAccountLookup(items);
            UpdateDependentCities(items);
            var convs = items.Select(i => CreateConv(i)).Where(i => i.AccountId > 0).ToList();
            var count = TDConvLoader.UpsertConvs(convs);
            return count;
        }

        private Conv CreateConv(DataTransferRow dtRow)
        {
            var conv = new Conv
            {
                AccountId = accountIdLookupByExtId[dtRow.insertion_order_id.Value],
                Time = convConverter.EventTime(dtRow),
                ConvType = (dtRow.event_sub_type == "postview") ? "v" : "c",
                CityId = cityIdLookupByName[dtRow.city_name],
                //ConvVal = 0,
                IP = dtRow.ip
            };
            return conv;
        }

        private void UpdateAccountLookup(List<DataTransferRow> items)
        {
            var acctExtIds = items.Select(i => i.insertion_order_id.Value).Distinct();

            using (var db = new ClientPortalProgContext())
            {
                foreach (var acctExtId in acctExtIds)
                {
                    if (accountIdLookupByExtId.ContainsKey(acctExtId))
                        continue; // already encountered

                    var tdAccts = db.ExtAccounts.Where(a => a.ExternalId == acctExtId.ToString());
                    if (tdAccts.Count() == 1)
                    {
                        var tdAcct = tdAccts.First();
                        accountIdLookupByExtId[acctExtId] = tdAcct.Id;
                    }
                    else
                    {
                        var newAccount = new ExtAccount
                        {
                            ExternalId = acctExtId.ToString(),
                            PlatformId = Platform.GetId(db,Platform.Code_DBM),
                            Name = "Unknown"
                        };
                        db.ExtAccounts.Add(newAccount);
                        db.SaveChanges();
                        Logger.Info("Added new ExtAccount: InsertionOrder {0}", acctExtId);
                        accountIdLookupByExtId[acctExtId] = newAccount.Id;
                    }
                }
            }
        }

        private void UpdateDependentCities(List<DataTransferRow> items)
        {
            var cities = items.Select(i => new Tuple<string,string>(i.city_name,i.country_name)).Distinct();
            var countryGroups = cities.GroupBy(c => c.Item2);

            using (var db = new ClientPortalProgContext())
            {
                foreach (var countryGroup in countryGroups)
                {
                    var countryName = countryGroup.Key;
                    if (countryIdLookupByName.ContainsKey(countryName))
                        continue;

                    var country = db.ConvCountries.FirstOrDefault(c => c.Name == countryName);
                    if (country == null)
                    {
                        country = new ConvCountry
                        {
                            Name = countryName
                        };
                        Logger.Info("Adding new country {0} with id {1} to database.", countryName,country.Id);
                        db.ConvCountries.Add(country);
                        db.SaveChanges();
                    }
                    countryIdLookupByName[countryName] = country.Id;
                }

                foreach (var city in cities)
                {
                    if (cityIdLookupByName.ContainsKey(city.Item1))
                        continue;
                    var countryId = countryIdLookupByName[city.Item2];
                    var cpCity = db.ConvCities.FirstOrDefault(c => c.Name == city.Item1 && c.CountryId == countryId);
                    if (cpCity == null)
                    {
                        cpCity = new ConvCity
                        {
                            Name = city.Item1,
                            CountryId = countryId
                        };
                        Logger.Info("Adding new city {0} in country {1} with id {2} to database.", city.Item1, city.Item2,cpCity.Id);
                        db.ConvCities.Add(cpCity);
                        db.SaveChanges();
                    }
                    cityIdLookupByName[city.Item1] = cpCity.Id;
                }
            }
        }
    }
}
