using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DAgents.Common;

namespace AccountingBackupWeb.Models.AccountingBackup
{
    public partial class Terms
    {
        public static Terms ByName(string name, AccountingBackupEntities model, bool create)
        {
            return 
                model.Terms.FirstOrDefault(c => c.Name == name) 
                ?? (create ? Create(name) : null);
        }

        static Terms Create(string name)
        {
            return new Terms {
                Name = name,
                Days = 30
            };
        }
    }
}
