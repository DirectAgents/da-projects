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

        //TODO: don't add cities that are already in database (e.g., "Chicago,Illinois" isn't distinct from just "Chicago")
        private void UpdateDependentCities(List<DataTransferRow> items)
        {
            var cities = items.Select(i => new Tuple<string,string,string,bool>(i.city_name,i.short_name,i.country_name,i.unique_city_name)).Distinct().Where(i => i.Item1 != null);
            var countryGroups = cities.GroupBy(c => c.Item3);

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
                        db.ConvCountries.Add(country);
                        db.SaveChanges();
                        Logger.Info("Added new country {0} with id {1} to database.", countryName, country.Id);
                    }

                    countryIdLookupByName[countryName] = country.Id;
                    int countryId = country.Id;

                    foreach (var city in countryGroup)
                    {
                        if (cityIdLookupByName.ContainsKey(city.Item1))
                            continue;

                        var cpCity = db.ConvCities.FirstOrDefault(c => c.Name == city.Item1 && c.CountryId == countryId);

                        if (cpCity == null && city.Item4 == true)
                            cpCity = db.ConvCities.FirstOrDefault(c => c.Name == city.Item2 && c.CountryId == countryId);

                        if (cpCity == null)
                        {
                            cpCity = new ConvCity
                            {
                                Name = city.Item1,
                                CountryId = countryId
                            };

                            db.ConvCities.Add(cpCity);
                            db.SaveChanges();
                            Logger.Info("Added new city {0} in country {1} with id {2} to database.", city.Item1, countryName, cpCity.Id);
                        }

                        else if (city.Item4 == true && cpCity.Name != city.Item1)
                        {
                            var formerName = cpCity.Name;
                            cpCity.Name = city.Item1;
                            db.SaveChanges();
                            Logger.Info("Updated city name from {0} to {1} in database.", formerName, city.Item1);
                        }
                        cityIdLookupByName[city.Item1] = cpCity.Id;
                    }
                }
            }
        }
    }
}
