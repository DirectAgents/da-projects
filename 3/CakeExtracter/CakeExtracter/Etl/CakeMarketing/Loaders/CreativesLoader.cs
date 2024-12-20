﻿using System.Collections.Generic;
using System.Linq;
using CakeExtracter.CakeMarketingApi.Entities;
using CakeExtracter.Common;

namespace CakeExtracter.Etl.CakeMarketing.Loaders
{
    public class CreativesLoader : Loader<Creative>
    {
        private readonly int offerId;
        private readonly bool overwriteNames;

        public CreativesLoader(int offerId, bool overwriteNames)
        {
            this.offerId = offerId;
            this.overwriteNames = overwriteNames;
        }

        protected override int Load(List<Creative> items)
        {
            Logger.Info("Synching {0} Creatives...", items.Count);
            using (var db = new ClientPortal.Data.Contexts.ClientPortalContext())
            {
                var newCreativeTypes = new List<ClientPortal.Data.Contexts.CreativeType>();
                foreach (var item in items)
                {
                    var creativeType = db.CreativeTypes.Where(ct => ct.CreativeTypeId == item.CreativeType.CreativeTypeId).SingleOrDefault();
                    if (creativeType == null)
                        creativeType = newCreativeTypes.SingleOrDefault(ct => ct.CreativeTypeId == item.CreativeType.CreativeTypeId);
                    if (creativeType == null)
                    {
                        Logger.Info("Adding new CreativeType: {0} ({1})", item.CreativeType.CreativeTypeName, item.CreativeType.CreativeTypeId);
                        creativeType = new ClientPortal.Data.Contexts.CreativeType
                        {
                            CreativeTypeId = item.CreativeType.CreativeTypeId,
                            CreativeTypeName = item.CreativeType.CreativeTypeName
                        };
                        newCreativeTypes.Add(creativeType);
                    }

                    var creative = db.Creatives.Where(c => c.CreativeId == item.CreativeId).SingleOrDefault();
                    if (creative == null)
                    {
                        creative = new ClientPortal.Data.Contexts.Creative();
                        creative.CreativeId = item.CreativeId;
                        creative.CreativeName = item.CreativeName; // always set the name for a new creative
                        db.Creatives.Add(creative);
                    }
                    if (overwriteNames)
                        creative.CreativeName = item.CreativeName;

                    creative.OfferId = this.offerId;
                    creative.CreativeType = creativeType;
                    creative.DateCreated = item.DateCreated;
                    creative.CreativeStatusId = item.CreativeStatusId;
                    creative.OfferLinkOverride = item.OfferLinkOverride;
                    creative.Width = item.Width;
                    creative.Height = item.Height;

                    if (item.CreativeFiles.Count > 0)
                        Logger.Info("Synching {0} CreativeFiles for Creative {1}...", item.CreativeFiles.Count, item.CreativeId);
                    foreach (var file in item.CreativeFiles)
                    {
                        var creativeFile = db.CreativeFiles.Where(cf => cf.CreativeFileId == file.CreativeFileId).SingleOrDefault();
                        if (creativeFile == null)
                        {
                            creativeFile = new ClientPortal.Data.Contexts.CreativeFile();
                            creativeFile.CreativeFileId = file.CreativeFileId;
                            db.CreativeFiles.Add(creativeFile);
                        }
                        creativeFile.CreativeFileName = file.CreativeFileName;
                        creativeFile.CreativeFileLink = file.CreativeFileLink;
                        creativeFile.Preview = file.Preview;
                        creativeFile.DateCreated = file.DateCreated;

                        creativeFile.CreativeId = creative.CreativeId;
                    }
                }
                Logger.Info("Creatives/CreativeTypes/CreativeFiles: " + db.ChangeCountsAsString());
                db.SaveChanges();
            }
            return items.Count;
        }
    }
}
