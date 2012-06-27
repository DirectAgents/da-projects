using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.Xml.Linq;
using System.Data;

namespace DirectTrack.Rest.Models
{
    /// <summary>
    /// DirectTrack Entities
    /// </summary>
    public partial class DirectTrackContainer
    {
        /// <summary>
        /// DirectTrack Advertisers from advertiser resources in database
        /// </summary>
        public IEnumerable<DirectTrack.Rest.Schemas.Advertiser> Advertisers
        {
            get
            {
                foreach (var i in this.AdvertiserResources)
                    yield return new DirectTrack.Rest.Schemas.Advertiser(i);
            }
        }

        /// <summary>
        /// DirectTrack advertiser resources in database.
        /// </summary>
        public IEnumerable<Resource> AdvertiserResources
        {
            get
            {
                foreach (var i in from c in Resources
                                  where c.ResourceTypeId == (int)ResourceType.EType.Advertiser
                                  select c)
                    yield return i;
            }
        }

        /// <summary>
        /// Gets all the direct track advertiser resources and saves them in the database.
        /// </summary>
        public void LoadDirectTrackAdvertisers()
        {
            var url = "https://da-tracking.com/apifleet/rest/1137/1/advertiser/";

            using (var model = new DirectTrackContainer())
            {
                // Delete all
                foreach (var i in this.AdvertiserResources)
                    model.Resources.DeleteObject(i);

                // Add all
                new ResourceGetter(url, (sender, resourceUrl, resourceDoc) =>
                      {
                          model.Resources.AddObject(new Resource {
                              Location = resourceUrl,
                              Content = resourceDoc.ToString(),
                              Got = DateTime.Now,
                              Posted = null,
                              ResourceTypeId = (int)ResourceType.EType.Advertiser
                          });
                      });

                // Save
                model.SaveChanges();
            }
        }

        partial void OnContextCreated()
        {
            InitEvents();
            InitResourceTypes();
        }

        private void InitEvents()
        {
            /// When changes are saved, make sure the Id of any new ResourceTypes match the
            /// corresponding EType
            this.SavingChanges += (s, e) =>
            {
                List<ResourceType> ResourceTypes =
                    this.ObjectStateManager
                        .GetObjectStateEntries(EntityState.Added)
                        .Select(entry => entry.Entity)
                        .OfType<ResourceType>().ToList();

                foreach (ResourceType item in ResourceTypes)
                    item.Id = (int)item.Type;
            };
        }

        private void InitResourceTypes()
        {
            string[] advertiserTypeNames = Enum.GetNames(typeof(ResourceType.EType));

            if (ResourceTypes.Count() != advertiserTypeNames.Length)
            {
                foreach (string name in advertiserTypeNames)
                    if (ResourceTypes.Count(c => c.Name == name) == 0)
                        ResourceTypes.AddObject(new ResourceType { Name = name });

                this.SaveChanges();
            }
        }
    }
}
