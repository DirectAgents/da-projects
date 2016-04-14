using System.Linq;
using DirectAgents.Domain.Contexts;
using DirectAgents.Domain.Entities.TD;

namespace DirectAgents.Domain.Seed
{
    public class TDSeeder
    {
        // Caller is responsible for instantiating and disposing of the context(s)
        public DATDContext context { get; set; }

        public TDSeeder(DATDContext context)
        {
            this.context = context;
        }

        public void SeedPlatforms()
        {
            AddPlatformIfNotExist("adr", "AdRoll");
            AddPlatformIfNotExist("dbm", "DBM");
            AddPlatformIfNotExist("fb", "Facebook");
            AddPlatformIfNotExist("tw", "Twitter");
            AddPlatformIfNotExist("insta", "Instagram");
            AddPlatformIfNotExist("datd", "DA Trading Desk");
            AddPlatformIfNotExist("mf", "Management Fee");
            AddPlatformIfNotExist("adel", "Adelphic");
            AddPlatformIfNotExist("adf", "Adform");
            AddPlatformIfNotExist("adt", "Adroit");
            AddPlatformIfNotExist("bc", "Bluecore");
            AddPlatformIfNotExist("crit", "Criteo");
            AddPlatformIfNotExist("li", "LiveIntent");
            AddPlatformIfNotExist("mg", "Magnetic");
            AddPlatformIfNotExist("max", "Maxpoint");
            AddPlatformIfNotExist("tab", "Taboola");
            AddPlatformIfNotExist("tc", "TapCommerce");
            AddPlatformIfNotExist("yam", "YAM");
            AddPlatformIfNotExist("zem", "Zemanta");
        }

        public void AddPlatformIfNotExist(string code, string name)
        {
            if (!context.Platforms.Any(p => p.Code == code))
            {
                var platform = new Platform
                {
                    Code = code,
                    Name = name
                };
                context.Platforms.Add(platform);
                context.SaveChanges();
            }
        }
    }
}
