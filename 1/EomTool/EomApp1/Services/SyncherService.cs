using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DAgents.Common;
using DirectTrack.Rest;
using System.Xml.Linq;
//using EomApp1.Formss.AB2.Model;
namespace EomApp1.Services
{
    public class SyncherService
    {
        //#region Synch Resource Trees
        //public void SynchAdvertisers(ILogger logger)
        //{
        //    Synch(logger, "https://da-tracking.com/apifleet/rest/1137/1/advertiser/");
        //}
        //public void SynchCampaigns(ILogger logger)
        //{
        //    Synch(logger, "https://da-tracking.com/apifleet/rest/1137/1/campaign/");
        //}
        //public void SynchCampaignGroups(ILogger logger)
        //{
        //    Synch(logger, "https://da-tracking.com/apifleet/rest/1137/campaignGroup/");
        //}
        //public void SynchAffiliates(ILogger logger)
        //{
        //    Synch(logger, "https://da-tracking.com/apifleet/rest/1137/1/affiliate/");
        //}
        //#endregion

        public SyncherService(ILogger logger, string url)
        {
            Logger = logger;
            ResourceGetter = new DirectTrack.Rest.ResourceGetter(Logger, url);
        }

        //public SyncherService(ILogger logger, string url, SunkEventHandler sunkHandler)
        //{
        //    Logger = logger;
        //    ResourceGetter = new DirectTrack.Rest.ResourceGetter(Logger, url);
        //    Sunk += sunkHandler;
        //}

        //public void Synch()
        //{
        //    ResourceGetter.GotResource += new ResourceGetter.GotResourceEventHandler(resourceGetter_GotResource);
        //    ResourceGetter.GetResources();
        //}

        //public void UpdateResources(DateTime since)
        //{
        //    using (var model = new DirectAgentsEntities())
        //    {
        //        // get direct track resources updated since 'since'
        //        var outOfDateDirectTrackResources = from c in model.DirectTrackResources
        //                                            where c.Updated >= since
        //                                            select c;

        //        // if ResourceName is campaignGroup
        //        //      - update DirectTrackCampaignGroup
        //        //      - update the association DirectTrackCampaignGroup**DirectTrackCampaign
        //        var outOfDateCampaignGroupDirectTrackResources = from c in outOfDateDirectTrackResources
        //                                                         where c.ResourceName == "campaignGroup"
        //                                                         select c;

        //        Logger.Log(string.Format("found {0} updated campaign groups since {1}", outOfDateCampaignGroupDirectTrackResources.Count(), since));

        //        foreach (var resource in outOfDateCampaignGroupDirectTrackResources)
        //        {
        //            // does the DirectTrackCampaignGroup exist?

        //            //resource.DirectTrackCampaignGroups.Any(c=>c.Name==resource.

        //            // update the name

        //            // update the campaigns association

        //        }
        //    }
        //}

        //void resourceGetter_GotResource(ResourceGetter sender, string location, XDocument resourceXDoc)
        //{
        //    using (var model = new DirectAgentsEntities())
        //    {
        //        var dtResource = model.DirectTrackResources.FirstOrDefault(c => c.Location == location);

        //        if (dtResource == null)
        //        {
        //            dtResource = DirectTrackResource.Create(resourceXDoc);
        //            model.DirectTrackResources.AddObject(dtResource);
        //        }

        //        // only update if content is different
        //        if (dtResource.XmlDoc != resourceXDoc.ToString())
        //        {
        //            dtResource.Location = location; 
        //            model.SaveChanges();
        //        }

        //        model.Detach(dtResource);
        //        OnSunk(dtResource);
        //    }
        //}

        #region Sunk Event
        //public delegate void SunkEventHandler(SyncherService sender, DirectTrackResource resource);
        //public event SunkEventHandler Sunk;
        //void OnSunk(DirectTrackResource directTrackResource)
        //{
        //    if (Sunk != null)
        //    {
        //        Sunk(this, directTrackResource);
        //    }
        //} 
        #endregion

        public ILogger Logger { get; set; }

        public ResourceGetter ResourceGetter { get; set; }


    }
    //public delegate 
}
