using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DirectAgents.Domain.Abstract
{
    interface IVendorProductDateRange
    {
        DateTime StartDate
        {
            get;
            set;
        }

        DateTime EndDate
        {
            get;
            set;
        }
    }
}
