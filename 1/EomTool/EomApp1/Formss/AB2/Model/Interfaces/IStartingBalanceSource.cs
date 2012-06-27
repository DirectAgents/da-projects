using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EomApp1.Formss.AB2.Model
{
    public interface IStartingBalanceSource
    {
        IEnumerable<StartingBalance> StartingBalances
        {
            get;
        }
    }
}
