using System.Linq;

namespace AccountingBackupWeb.Models.AccountingBackup
{
    public partial class SynchTime
    {
        public static SynchTime ByName(AccountingBackupEntities model, string name, bool create)
        {
            var st = model.SynchTimes.FirstOrDefault(c => c.Name == name);
            if (st == null)
            {
                st = Create(name);
                if (create)
                {
                    model.SynchTimes.AddObject(st);
                    model.SaveChanges();
                }
            }
            return st;
        }

        private static SynchTime Create(string name)
        {
            return new SynchTime {
                Name = name
            };
        }

        public static SynchTime CampaignSpendItems
        {
            get
            {
                using (var model = new AccountingBackupEntities())
                {
                    return model.SynchTimes.FirstOrDefault(c => c.Name == "CampaignSpendItems");
                }
            }
        }
    }
}
