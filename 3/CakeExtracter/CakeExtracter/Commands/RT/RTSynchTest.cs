using System;
using System.ComponentModel.Composition;
using System.Linq;
using CakeExtracter.Common;
using DirectAgents.Domain.Contexts;
using DirectAgents.Domain.Entities.RevTrack;

namespace CakeExtracter.Commands.RT
{
    [Export(typeof(ConsoleCommand))]
    public class RTSynchTest : ConsoleCommand
    {
        public override void ResetProperties()
        {
        }

        public RTSynchTest()
        {
            IsCommand("rtSynchTest", "revtrack synch test");
        }

        public override int Execute(string[] remainingArguments)
        {
            CopySummaries(clientId: 16);
            return 0;
        }

        public void CopyProgClients()
        {
            using (var cpprogContext = new ClientPortalProgContext())
            using (var rtContext = new RevTrackContext())
            {
                var cppAdvertisers = cpprogContext.Advertisers;
                foreach (var cppAdv in cppAdvertisers)
                {
                    var progClient = new ProgClient
                    {
                        Id = cppAdv.Id,
                        Name = cppAdv.Name
                    };
                    rtContext.ProgClients.Add(progClient);
                }
                rtContext.SaveChanges();
            }
        }
        public void CopyCampaigns()
        {
            using (var cpprogContext = new ClientPortalProgContext())
            using (var rtContext = new RevTrackContext())
            {
                var cppCampaigns = cpprogContext.Campaigns;
                foreach (var cppCamp in cppCampaigns)
                {
                    var progCampaign = new ProgCampaign
                    {
                        Id = cppCamp.Id,
                        ProgClientId = cppCamp.AdvertiserId,
                        Name = cppCamp.Name,
                        DefaultBudgetInfo = new DirectAgents.Domain.Entities.CPProg.BudgetInfoVals
                        {
                            MediaSpend = cppCamp.DefaultBudgetInfo.MediaSpend,
                            MgmtFeePct = cppCamp.DefaultBudgetInfo.MgmtFeePct,
                            MarginPct = cppCamp.DefaultBudgetInfo.MarginPct
                        }
                    };
                    rtContext.ProgCampaigns.Add(progCampaign);
                }
                rtContext.SaveChanges();
            }
        }
        public void CopyBudgetInfos()
        {
            using (var cpprogContext = new ClientPortalProgContext())
            using (var rtContext = new RevTrackContext())
            {
                var cppBudgetInfos = cpprogContext.BudgetInfos;
                foreach (var cppBI in cppBudgetInfos)
                {
                    var progBudgetInfo = new ProgBudgetInfo
                    {
                        ProgCampaignId = cppBI.CampaignId,
                        Date = cppBI.Date,
                        MediaSpend = cppBI.MediaSpend,
                        MgmtFeePct = cppBI.MgmtFeePct,
                        MarginPct = cppBI.MarginPct
                    };
                    rtContext.ProgBudgetInfos.Add(progBudgetInfo);
                }
                var cppPBIs = cpprogContext.PlatformBudgetInfos;
                foreach (var cppPBI in cppPBIs)
                {
                    var progVendorBudgetInfo = new ProgVendorBudgetInfo
                    {
                        ProgCampaignId = cppPBI.CampaignId,
                        ProgVendorId = cppPBI.PlatformId,
                        Date = cppPBI.Date,
                        MediaSpend = cppPBI.MediaSpend,
                        MgmtFeePct = cppPBI.MgmtFeePct,
                        MarginPct = cppPBI.MarginPct
                    };
                    rtContext.ProgVendorBudgetInfos.Add(progVendorBudgetInfo);
                }
                rtContext.SaveChanges();
            }
        }

        public void CopyProgVendors()
        {
            using (var cpprogContext = new ClientPortalProgContext())
            using (var rtContext = new RevTrackContext())
            {
                var platforms = cpprogContext.Platforms;
                foreach (var platform in platforms)
                {
                    var progVendor = new ProgVendor
                    {
                        Id = platform.Id,
                        Name = platform.Name,
                        Code = platform.Code
                    };
                    rtContext.ProgVendors.Add(progVendor);
                }
                rtContext.SaveChanges();
            }
        }

        public void CopySummaries(int clientId)
        {
            using (var cpprogContext = new ClientPortalProgContext())
            using (var rtContext = new RevTrackContext())
            {
                var progClient = rtContext.ProgClients.Find(clientId);
                if (progClient != null)
                {
                    var campIds = progClient.ProgCampaigns.Select(c => c.Id).ToList();
                    foreach (var campId in campIds)
                    {
                        var extAccounts = cpprogContext.ExtAccounts.Where(a => a.CampaignId.HasValue && a.CampaignId.Value == campId);
                        var platformGroups = extAccounts.GroupBy(a => a.PlatformId);
                        foreach (var platformGroup in platformGroups)
                        {
                            var acctIds = platformGroup.Select(g => g.Id).ToList();
                            var dSums = cpprogContext.DailySummaries.Where(ds => acctIds.Contains(ds.AccountId));
                            var progSums = dSums.GroupBy(ds => new
                            {
                                Year = ds.Date.Year,
                                Month = ds.Date.Month
                            }).AsEnumerable()
                              .Select(g => new ProgSummary
                              {
                                  Date = new DateTime(g.Key.Year, g.Key.Month, 1),
                                  ProgCampaignId = campId,
                                  ProgVendorId = platformGroup.Key,
                                  Cost = g.Sum(ds => ds.Cost)
                              }).ToList();
                            //TODO: check if exists?
                            rtContext.ProgSummaries.AddRange(progSums);
                        }
                    }
                }
                rtContext.SaveChanges();
            }
        }

        public void CopyTemplate()
        {
            using (var cpprogContext = new ClientPortalProgContext())
            using (var rtContext = new RevTrackContext())
            {
                rtContext.SaveChanges();
            }
        }

    }
}
