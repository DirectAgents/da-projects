using CakeExtracter.Helpers;
using DirectAgents.Domain.Entities.CPProg.Facebook;
using System.Collections.Generic;
using System.Linq;

namespace CakeExtracter.Etl.SocialMarketing.EntitiesLoaders
{
    public class FacebookCreativesLoader : BaseFacebookEntityLoader<FbCreative>
    {
        /// <summary>
        /// Entity id storage of already updated ads.
        /// </summary>
        private static EntityIdStorage<FbCreative> fbAdEntityIdStorage = new EntityIdStorage<FbCreative>(x => x.Id,
            x => $"{x.AccountId} {x.ExternalId}");

        private static object lockObject = new object();

        public void AddUpdateDependentEntities(List<FbCreative> items)
        {
            var uniqueItems = items.GroupBy(item => item.ExternalId).Select(gr => gr.First()).ToList();
            AddUpdateDependentItems(uniqueItems, fbAdEntityIdStorage, lockObject);
            AssignIdToItems(items, fbAdEntityIdStorage);
        }
    }
}
