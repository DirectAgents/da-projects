using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EomApp1.Screens.Accounting.Data
{
    public partial class AccountingDataClassesDataContext
    {
        public AccountingDataClassesDataContext(bool b) :
            base(EomAppCommon.EomAppSettings.ConnStr)
        {
            OnCreated();
        }
    }
}
