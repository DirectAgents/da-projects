using System.Linq;
using DirectAgents.Domain.Contexts;
using DirectAgents.Domain.Entities.TD;

namespace DirectAgents.Domain.Seed
{
    public class TDSeeder
    {
        // Caller is responsible for instantiating and disposing of the context(s)
        public ClientPortalProgContext context { get; set; }

        public TDSeeder(ClientPortalProgContext context)
        {
            this.context = context;
        }

        public void SeedPlatforms()
        {
            AddPlatformIfNotExist(Platform.Code_AdRoll, "AdRoll");
            AddPlatformIfNotExist(Platform.Code_DBM, "DBM");
            AddPlatformIfNotExist(Platform.Code_FB, "Facebook");
            AddPlatformIfNotExist(Platform.Code_Twitter, "Twitter");
            AddPlatformIfNotExist(Platform.Code_Instagram, "Instagram");
            AddPlatformIfNotExist(Platform.Code_DATradingDesk, "DA Trading Desk");
            AddPlatformIfNotExist("mf", "Management Fee");
            AddPlatformIfNotExist("adel", "Adelphic");
            AddPlatformIfNotExist("adf", "Adform");
            AddPlatformIfNotExist("adt", "Adroit");
            AddPlatformIfNotExist("bc", "Bluecore");
            AddPlatformIfNotExist("crit", "Criteo");
            AddPlatformIfNotExist("kin", "Kinetic");
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
