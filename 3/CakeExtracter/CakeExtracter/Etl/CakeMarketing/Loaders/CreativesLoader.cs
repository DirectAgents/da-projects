using CakeExtracter.CakeMarketingApi.Entities;
using CakeExtracter.Common;
using MoreLinq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CakeExtracter.Etl.CakeMarketing.Loaders
{
    public class CreativesLoader : Loader<Creative>
    {
        private readonly int offerId;

        public CreativesLoader(int offerId)
        {
            this.offerId = offerId;
        }

        protected override int Load(List<Creative> items)
        {
            Logger.Info("Synching {0} creatives...", items.Count);
            using (var db = new ClientPortal.Data.Contexts.ClientPortalContext())
            {
                List<ClientPortal.Data.Contexts.CreativeType> newCreativeTypes = new List<ClientPortal.Data.Contexts.CreativeType>();
                foreach (var item in items)
                {
                    var creativeType = db.CreativeTypes.Where(ct => ct.CreativeTypeId == item.CreativeType.CreativeTypeId)
                                        .SingleOrFallback(() =>
                                            {
                                                var newCreativeType = newCreativeTypes.SingleOrDefault(ct => ct.CreativeTypeId == item.CreativeType.CreativeTypeId);
                                                if (newCreativeType == null)
                                                {
                                                    Logger.Info("Adding new CreativeType: {0} ({1})", item.CreativeType.CreativeTypeName, item.CreativeType.CreativeTypeId);
                                                    newCreativeType = new ClientPortal.Data.Contexts.CreativeType
                                                    {
                                                        CreativeTypeId = item.CreativeType.CreativeTypeId,
                                                        CreativeTypeName = item.CreativeType.CreativeTypeName
                                                    };
                                                    newCreativeTypes.Add(newCreativeType);
                                                }
                                                return newCreativeType;
                                            });

                    var creative = db.Creatives.Where(c => c.CreativeId == item.CreativeId)
                                        .SingleOrFallback(() =>
                                            {
                                                var newCreative = new ClientPortal.Data.Contexts.Creative();
                                                newCreative.CreativeId = item.CreativeId;
                                                db.Creatives.Add(newCreative);
                                                return newCreative;
                                            });

                    creative.OfferId = this.offerId;
                    creative.CreativeName = item.CreativeName;
                    creative.CreativeType = creativeType;
                    creative.DateCreated = item.DateCreated;
                }
                Logger.Info("Creatives/CreativeTypes: " + db.ChangeCountsAsString());
                db.SaveChanges();
            }
            return items.Count;
        }
    }
}
