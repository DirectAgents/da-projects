using System.Collections.Generic;
using System.Linq;
using EomTool.Domain.Abstract;
using EomTool.Domain.Entities;

namespace EomTool.Domain.Concrete
{
    public class DAMain1Repository : IDAMain1Repository
    {
        private List<DADatabase> daDatabases;
        public List<DADatabase> DADatabases
        {
            get
            {
                if (this.daDatabases == null)
                    using (var db = new DAMain1Entities())
                        this.daDatabases = db.DADatabases
                                             .OrderByDescending(c => c.effective_date)
                                             .ToList();
                return this.daDatabases;
            }
        }
    }
}
