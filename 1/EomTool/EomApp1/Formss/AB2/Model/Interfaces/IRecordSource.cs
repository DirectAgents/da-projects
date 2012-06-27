using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EomApp1.Formss.AB2.Model.Interfaces
{
    public interface IRecordSource : 
                        ISqlServerDatabaseSource,
                        IUnitNameSource,
                        IUnitConversionSource,
                        IAdvertiserSource,
                        IPayTermSource,
                        IStartingBalanceSource
    {
    }
}
