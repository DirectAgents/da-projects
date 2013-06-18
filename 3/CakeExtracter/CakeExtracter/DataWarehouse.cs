using System;
using System.Collections.Generic;
using System.Linq;
using ClientPortal.Data.Contexts;
using ClientPortal.Web.Models.Cake;

namespace CakeExtracter
{
    class DataWarehouse
    {
        public void Update(click[] clicks, conversion[] conversions)
        {
            Update(clicks);

            var conversionsToProcess = Process(conversions);

            if (conversionsToProcess.Count > 0)
            {
                Console.WriteLine("{0} conversions to process..", conversionsToProcess.Count);

                var conversionsToProcessByDate = conversionsToProcess.GroupBy(c => c.click_date.Value.Date)
                    .ToArray();

                Console.WriteLine("need to extract clicks from {0} dates..", conversionsToProcessByDate.Count());

                foreach (var conversionsToProcessSet in conversionsToProcessByDate.OrderBy(c => c.Key))
                {
                    Console.WriteLine("extracting clicks for {0} in order to process {1} conversions", conversionsToProcessSet.Key, conversionsToProcessSet.Count());

                    var clicksToProcess = Syncher.ClicksByConversion(conversionsToProcessSet).SelectMany(c => c.clicks);
                    Update(clicksToProcess);

                    var stillToProcess = Process(conversionsToProcessSet);

                    if (stillToProcess.Count > 0)
                    {
                        throw new Exception(string.Format("Failed to process {0} convresions", conversionsToProcess.Count));
                    }
                }
            }
        }

        public void Update(IEnumerable<click> allClicks)
        {
            var clicks = allClicks.ToArray();

            Console.WriteLine("Update {0} clicks..", clicks.Length);

            Process(clicks.Select(c => c.region));
            Process(clicks.Select(c => c.country));
            Process(clicks);
        }

        private List<conversion> Process(IEnumerable<conversion> conversions)
        {
            var factConversions = new List<FactConversion>();
            var notProcessed = new List<conversion>();
            using (var db = new ClientPortalDWContext())
            {
                foreach (var conversion in conversions)
                {
                    int conversionKey = int.Parse(conversion.conversion_id);
                    if (db.FactConversions.FirstOrDefault(c => c.ConversionKey == conversionKey) == null)
                    {

                        int? clickKey = conversion.click_id;
                        if (clickKey != null)
                        {
                            if (db.FactClicks.FirstOrDefault(c => c.ClickKey == conversion.click_id) != null)
                            {
                                factConversions.Add(new FactConversion
                                    {
                                        ConversionKey = conversionKey,
                                        ClickKey = clickKey.Value
                                    });
                            }
                            else
                            {
                                notProcessed.Add(conversion);
                            }
                        }
                        else
                        {
                            Console.WriteLine("Conversion id {0} has null click id.", conversionKey);
                        }
                    }
                }
            }
            Load(factConversions);
            return notProcessed;
        }

        private void Process(IEnumerable<region> regions)
        {
            var dimRegions = regions.Select(c => new DimRegion
                {
                    RegionCode = c.region_code
                });
            Load(dimRegions);
        }

        private void Process(IEnumerable<country> countries)
        {
            var dimCountries = countries.Select(c => new DimCountry
                {
                    CountryCode = c.country_code
                });
            Load(dimCountries);
        }

        private void Process(IEnumerable<click> allClicks)
        {
            var clicks = allClicks.ToArray();
            var dimCountries = DimCountries();
            var dimRegions = DimRegions();
            int count = 0;
            int total = clicks.Length;
            const int step = 1000;
            foreach (var clickSet in clicks.InSetsOf(step))
            {
                Console.WriteLine("Processing {0}/{1}..", count, total);
                var factClicks = new List<FactClick>();
                using (var db = new ClientPortalDWContext())
                {
                    foreach (var click in clickSet)
                    {
                        if (db.FactClicks.FirstOrDefault(c => c.ClickKey == click.click_id) == null)
                        {
                            factClicks.Add(new FactClick
                            {
                                ClickKey = click.click_id,
                                CountryKey = dimCountries[click.country.country_code].CountryKey,
                                RegionKey = dimRegions[click.region.region_code].RegionKey
                            });
                        }
                    }
                    count += step;
                }
                Load(factClicks);
            }
        }

        private void Load(List<FactConversion> factConversions)
        {
            using (var db = new ClientPortalDWContext())
            {
                Console.WriteLine("Saving {0} conversion facts..", factConversions.Count);
                factConversions.ForEach(c => db.FactConversions.Add(c));
                db.SaveChanges();
            }
        }

        private void Load(IEnumerable<DimRegion> dimRegions)
        {
            using (var db = new ClientPortalDWContext())
            {
                var existing = db.DimRegions.ToList();
                var add = dimRegions.Except(existing, new DimRegionEqualityComparer());
                foreach (var item in add)
                {
                    db.DimRegions.Add(item);
                }
                db.SaveChanges();
            }
        }

        private void Load(IEnumerable<DimCountry> dimCountries)
        {
            using (var db = new ClientPortalDWContext())
            {
                var existing = db.DimCountries.ToList();
                var add = dimCountries.Except(existing, new DimCountryEqualityComparer());
                foreach (var item in add)
                {
                    db.DimCountries.Add(item);
                }
                db.SaveChanges();
            }
        }

        private void Load(List<FactClick> factClicks)
        {
            using (var db = new ClientPortalDWContext())
            {
                Console.WriteLine("Saving {0} click facts..", factClicks.Count);
                factClicks.ForEach(c => db.FactClicks.Add(c));
                db.SaveChanges();
            }
        }

        Dictionary<string, DimCountry> DimCountries()
        {
            using (var db = new ClientPortalDWContext())
            {
                return db.DimCountries.ToDictionary(c => c.CountryCode);
            }
        }

        Dictionary<string, DimRegion> DimRegions()
        {
            using (var db = new ClientPortalDWContext())
            {
                return db.DimRegions.ToDictionary(c => c.RegionCode);
            }
        }

        #region EqualityComparers
        class DimCountryEqualityComparer : IEqualityComparer<DimCountry>
        {
            public bool Equals(DimCountry x, DimCountry y)
            {
                return x.CountryCode == y.CountryCode;
            }

            public int GetHashCode(DimCountry obj)
            {
                return obj.CountryCode.GetHashCode();
            }
        }
        class DimRegionEqualityComparer : IEqualityComparer<DimRegion>
        {
            public bool Equals(DimRegion x, DimRegion y)
            {
                return x.RegionCode == y.RegionCode;
            }

            public int GetHashCode(DimRegion obj)
            {
                return obj.RegionCode.GetHashCode();
            }
        }
        #endregion
    }
}