using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EomTool.Domain.Abstract;
using EomTool.Domain.Entities;

namespace EomTool.Domain.Concrete
{
    public class AffiliateRepository : IAffiliateRepository
    {
        EomEntities context;
        public AffiliateRepository()
        {
            context = EomEntities.Create();
        }

        public IQueryable<Affiliate> Affiliates
        {
            get {
                return context.Affiliates;
            }
        }
    }
}
