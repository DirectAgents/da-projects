using System.Collections.Generic;

namespace EomApp1.Formss.AB2.Model
{
    public class PayTermSource : IPayTermSource
    {
        public IEnumerable<PayTerm> PayTerms
        {
            get
            {
                yield return new PayTerm { Name = "Biweekly" };
                yield return new PayTerm { Name = "Retainer" };
                yield return new PayTerm { Name = "Net 7" };
                yield return new PayTerm { Name = "Net 15" };
                yield return new PayTerm { Name = "Net 30" };
            }
        }
    }
}
