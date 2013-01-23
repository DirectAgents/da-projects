using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DAgents.Common;

namespace AccountingBackupWeb.Models.AccountingBackup
{
    public partial class Company
    {
        public static Company ByName(string name, AccountingBackupEntities model, bool create)
        {
            return model.Companies.FirstOrDefault(c => c.Name == name) ?? (create ? Create(name) : null);
        }

        public static Company ByName(string name, bool create)
        {
            using (var model = new AccountingBackupEntities())
            {
                var res = model.Companies.FirstOrDefault(c => c.Name == name);
                if (res == null)
                {
                    res = Create(name);
                    if (create)
                    {
                        model.Companies.AddObject(res);
                        model.SaveChanges();
                    }
                }
                return res;
            }
        }

        private static Company Create(string name)
        {
            return new Company { Name = name };
        }

        public static Company ByName(string p, AccountingBackupEntities model)
        {
            return ByName(p, model, false);
        }
    }
}
