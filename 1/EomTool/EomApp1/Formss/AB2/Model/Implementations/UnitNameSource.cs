using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.Unity;

namespace EomApp1.Formss.AB2.Model
{
    public class UnitNameSource : IUnitNameSource
    {
        public IEnumerable<string> UnitNames
        {
            get
            {
                yield return "USD";
                yield return "GBP";
                yield return "EUR";
                yield return "CAD";
                yield return "AUD";
            }
        }
    }
}
