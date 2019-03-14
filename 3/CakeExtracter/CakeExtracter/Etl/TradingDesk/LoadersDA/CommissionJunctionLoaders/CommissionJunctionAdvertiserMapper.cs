using CommissionJunction.Entities;
using DirectAgents.Domain.Entities.CPProg.CJ;
using System.Collections.Generic;
using System.Linq;

namespace CakeExtracter.Etl.TradingDesk.LoadersDA.CommissionJunctionLoaders
{
    /// <summary>
    /// Converter for commission junction report to db entities.
    /// </summary>
    public class CommissionJunctionAdvertiserMapper
    {
        /// <summary>
        /// Maps the commission junction information to database entities.
        /// </summary>
        /// <param name="reportCommissions">The report commissions.</param>
        /// <returns></returns>
        public List<CjAdvertiserCommission> MapCommissionJunctionInfoToDbEntities(List<AdvertiserCommission> reportCommissions, int accountId)
        {
            var dbCommissions = reportCommissions.Select(rCommission =>
            {
                var dbCommission = new CjAdvertiserCommission();
                AutoMapper.Mapper.Map(rCommission, dbCommission);
                dbCommission.AccountId = accountId;
                return dbCommission;
            });
            return dbCommissions.ToList();
        }
    }
}
