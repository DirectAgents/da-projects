using System.Collections.Generic;
using System.Linq;
using CakeExtracter.Helpers;
using DirectAgents.Domain.Entities.CPProg.Facebook;

namespace CakeExtracter.Etl.Facebook.Loaders.EntitiesLoaders
{
    /// <summary>
    /// Facebook Creatives loader.
    /// </summary>
    /// <seealso cref="BaseFacebookEntityLoader{T}.Domain.Entities.CPProg.Facebook.FbCreative}" />
    public class FacebookCreativesLoader : BaseFacebookEntityLoader<FbCreative>
    {
        /// <summary>
        /// Entity id storage of already updated ads.
        /// </summary>
        private static EntityIdStorage<FbCreative> fbAdEntityIdStorage = new EntityIdStorage<FbCreative>(x => x.Id,
            x => $"{x.AccountId} {x.ExternalId}");

        private static object lockObject = new object();

        /// <summary>
        /// Adds or updates dependent entities.
        /// </summary>
        /// <param name="items">The items.</param>
        public void AddUpdateDependentEntities(List<FbCreative> items)
        {
            var uniqueItems = items.GroupBy(item => item.ExternalId).Select(gr => gr.First()).ToList();
            AddUpdateDependentItems(uniqueItems, fbAdEntityIdStorage, lockObject);
            AssignIdToItems(items, fbAdEntityIdStorage);
        }

        /// <summary>
        /// Updates the existing database item properties if necessary.
        /// </summary>
        /// <param name="existingDbItem">The existing database item.</param>
        /// <param name="latestItemFromApi">The latest item from API.</param>
        /// <returns></returns>
        protected override bool UpdateExistingDbItemPropertiesIfNecessary(FbCreative existingDbItem, FbCreative latestItemFromApi)
        {
            if (existingDbItem.Name != latestItemFromApi.Name ||
                existingDbItem.Body != latestItemFromApi.Body ||
                existingDbItem.Title != latestItemFromApi.Title ||
                existingDbItem.ImageUrl != latestItemFromApi.ImageUrl ||
                existingDbItem.ThumbnailUrl != latestItemFromApi.ThumbnailUrl)
            {
                existingDbItem.Name = latestItemFromApi.Name;
                existingDbItem.Body = latestItemFromApi.Body;
                existingDbItem.Title = latestItemFromApi.Title;
                existingDbItem.ImageUrl = latestItemFromApi.ImageUrl;
                existingDbItem.ThumbnailUrl = latestItemFromApi.ThumbnailUrl;
                return true;
            }
            return false;
        }
    }
}
