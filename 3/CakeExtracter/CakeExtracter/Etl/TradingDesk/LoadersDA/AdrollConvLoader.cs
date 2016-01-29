using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using CakeExtracter.Etl.TradingDesk.Extracters;
using DirectAgents.Domain.Contexts;
using DirectAgents.Domain.Entities.TD;

namespace CakeExtracter.Etl.TradingDesk.LoadersDA
{
    public class AdrollConvLoader : Loader<AdrollConvRow>
    {
        private readonly int accountId;
        private TDConvLoader convLoader;
        private Dictionary<string, int> strategyIdLookupByCampName = new Dictionary<string, int>();
        private Dictionary<string, int> adIdLookupByName = new Dictionary<string, int>();
        private Dictionary<string, int> cityIdLookupByCountryCity = new Dictionary<string, int>();

        public AdrollConvLoader(int acctId)
        {
            this.convLoader = new TDConvLoader();
            this.accountId = acctId;
        }

        protected override int Load(List<AdrollConvRow> items)
        {
            Logger.Info("Loading {0} Adroll Conversions..", items.Count);
            UpdateStrategyLookup(items);
            UpdateAdLookup(items);
            AddUpdateDependentCities(items);
            var convs = items.Select(c => CreateConv(c)).ToList();
            var count = convLoader.UpsertConvs(convs);
            return count;
        }

        public Conv CreateConv(AdrollConvRow convRow)
        {
            int? stratId = strategyIdLookupByCampName.ContainsKey(convRow.Campaign) ? strategyIdLookupByCampName[convRow.Campaign] : (int?)null;
            int? tdAdId = adIdLookupByName.ContainsKey(convRow.Ad) ? adIdLookupByName[convRow.Ad] : (int?)null;
            var countryCity = convRow.Country + " " + convRow.City;
            int? cityId = cityIdLookupByCountryCity.ContainsKey(countryCity) ? cityIdLookupByCountryCity[countryCity] : (int?)null;
            var conv = new Conv
            {
                AccountId = accountId,
                Time = convRow.ConvTime,
                ConvType = ConvTypeAbbrev(convRow.ConvType),
                StrategyId = stratId,
                TDadId = tdAdId,
                ConvVal = decimal.Parse(convRow.ConvVal, NumberStyles.Currency),
                CityId = cityId,
                //ExtData = convRow.ext_data_user_id
                ExtData = convRow.ext_data_order_id
            };
            return conv;
        }
        public static string ConvTypeAbbrev(string conversionType)
        {
            conversionType = conversionType.Trim();
            if (string.IsNullOrEmpty(conversionType))
                return conversionType;
            else
                return conversionType.Substring(0, 1).ToLower();
        }

        private void AddUpdateDependentCities(List<AdrollConvRow> items)
        {
            using (var db = new DATDContext())
            {
                var tuples = items.Select(c => new Tuple<string, string>(c.Country, c.City)).Distinct();
                var countryGroups = tuples.GroupBy(t => t.Item1);
                foreach (var countryGroup in countryGroups)
                {
                    var country = db.ConvCountries.FirstOrDefault(c => c.Name == countryGroup.Key);
                    // (assuming just one ConvCountries with the specified Name)
                    if (country == null)
                    {
                        country = new ConvCountry
                        {
                            Name = countryGroup.Key
                        };
                        db.ConvCountries.Add(country);
                        db.SaveChanges();
                        Logger.Info("Saved new country: {0} ({1})", country.Name, country.Id);
                    }
                    foreach (var tuple in countryGroup)
                    {
                        var cityName = tuple.Item2;
                        var city = db.ConvCities.Where(c => c.CountryId == country.Id && c.Name == cityName).FirstOrDefault();
                        // (assuming just one)
                        if (city == null)
                        {
                            city = new ConvCity
                            {
                                CountryId = country.Id,
                                Name = cityName
                            };
                            db.ConvCities.Add(city);
                            db.SaveChanges();
                            Logger.Info("Saved new city: {0} ({1}), {2}", city.Name, city.Id, country.Name);
                        }
                        var countryCity = country.Name + " " + city.Name;
                        cityIdLookupByCountryCity[countryCity] = city.Id;
                    }
                }
            }
        }

        private void UpdateStrategyLookup(List<AdrollConvRow> items)
        {
            var campNames = items.Select(i => i.Campaign).Distinct();

            using (var db = new DATDContext())
            {
                foreach (var campName in campNames)
                {
                    var strats = db.Strategies.Where(s => s.AccountId == accountId && s.Name == campName);
                    if (strats.Count() == 1)
                    {
                        var strat = strats.First();
                        strategyIdLookupByCampName[campName] = strat.Id;
                    }
                }
            }
        }

        private void UpdateAdLookup(List<AdrollConvRow> items)
        {
            var adNames = items.Select(i => i.Ad).Distinct();

            using (var db = new DATDContext())
            {
                foreach (var adName in adNames)
                {
                    var tdAds = db.TDads.Where(a => a.AccountId == accountId && a.Name == adName);
                    if (tdAds.Count() == 1)
                    {
                        var tdAd = tdAds.First();
                        adIdLookupByName[adName] = tdAd.Id;
                    }
                }
            }
        }
    }
}
