using System;
using System.Collections.Generic;
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
            CopyBudgetInfos();
            //CopySummaries(clientId: 16);
            return 0;
        }

        //TODO
        // a way to compare the two sides (for each entity)... deletions, changes, additions
        // then, make the copy methods check what's already there, and only copy the new entities
        // ?what to do about changes and deletions?
        // a generic compare screen (view) - id & name of entity?

        //TODO: ?deletions - how to handle?
        //      ABClientId ?
        public void CopyProgClients()
        {
            using (var cpprogContext = new ClientPortalProgContext())
            using (var rtContext = new RevTrackContext())
            {
                //var existingPCids = rtContext.ProgClients.Select(x => x.Id).ToArray();

                var cppAdvertisers = cpprogContext.Advertisers;
                int numChanged = 0;
                foreach (var cppAdv in cppAdvertisers.OrderBy(x => x.Id))
                {
                    //if (!existingPCids.Contains(cppAdv.Id)) // add
                    var existing = rtContext.ProgClients.Find(cppAdv.Id);
                    if (existing == null) // add
                    {
                        var progClient = new ProgClient
                        {
                            Id = cppAdv.Id,
                            Name = cppAdv.Name
                        };
                        rtContext.ProgClients.Add(progClient);
                    }
                    else // update
                    {
                        if (existing.Name != cppAdv.Name)
                        {
                            existing.Name = cppAdv.Name;
                            numChanged++;
                        }
                    }
                }
                rtContext.SaveChanges();
            }
        }
        //TODO: ?deletions - how to handle?
        public void CopyCampaigns()
        {
            using (var cpprogContext = new ClientPortalProgContext())
            using (var rtContext = new RevTrackContext())
            {
                var cppCampaigns = cpprogContext.Campaigns;
                int numChanged = 0;
                foreach (var cppCamp in cppCampaigns.OrderBy(x => x.Id))
                {
                    var fresh = new ProgCampaign
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
                    var existing = rtContext.ProgCampaigns.Find(fresh.Id);
                    if (existing == null) // add
                    {
                        rtContext.ProgCampaigns.Add(fresh);
                    }
                    else // update
                    {
                        if (!PropertyCompare.Equal(existing, fresh) || !PropertyCompare.Equal(existing.DefaultBudgetInfo, fresh.DefaultBudgetInfo))
                        {
                            existing.ProgClientId = fresh.ProgClientId;
                            existing.Name = fresh.Name;
                            existing.DefaultBudgetInfo.SetFrom(fresh.DefaultBudgetInfo);
                            numChanged++;
                        }
                    }
                }
                rtContext.SaveChanges();
            }
        }

        public void CopyBudgetInfos()
        {
            using (var cpprogContext = new ClientPortalProgContext())
            using (var rtContext = new RevTrackContext())
            {
                var cppBudgetInfos = cpprogContext.BudgetInfos; // fresh BIs
                var rtBudgetInfos = rtContext.ProgBudgetInfos; // existing BIs

                var freshCampIds = cppBudgetInfos.Select(x => x.CampaignId).Distinct().ToArray();
                var rtBIsToDelete = rtBudgetInfos.Where(x => !freshCampIds.Contains(x.ProgCampaignId));
                foreach (var rtBItoDelete in rtBIsToDelete)
                {
                    rtContext.ProgBudgetInfos.Remove(rtBItoDelete);
                }

                // Group "fresh" BIs by campaign
                var cppCampGroups = cppBudgetInfos.GroupBy(x => x.CampaignId);
                foreach (var cppCampGroup in cppCampGroups)
                {
                    var rtCampBIs = rtBudgetInfos.Where(x => x.ProgCampaignId == cppCampGroup.Key);
                    var updatedBIDates = new List<DateTime>();
                    foreach (var rtBI in rtCampBIs.OrderBy(x => x.Date))
                    {
                        var cppBI = cppCampGroup.FirstOrDefault(x => x.Date == rtBI.Date);
                        if (cppBI == null) // delete
                            rtContext.ProgBudgetInfos.Remove(rtBI);
                        else // update
                        {
                            rtBI.SetFrom(cppBI);
                            updatedBIDates.Add(rtBI.Date);
                        }
                    }
                    // Handle additions
                    var newBIs = cppCampGroup.Where(x => !updatedBIDates.Contains(x.Date));
                    foreach (var newBI in newBIs)
                    {
                        var progBudgetInfo = new ProgBudgetInfo
                        {
                            ProgCampaignId = newBI.CampaignId,
                            Date = newBI.Date,
                            MediaSpend = newBI.MediaSpend,
                            MgmtFeePct = newBI.MgmtFeePct,
                            MarginPct = newBI.MarginPct
                        };
                        rtContext.ProgBudgetInfos.Add(progBudgetInfo);
                    }
                }
                int numWrites = rtContext.SaveChanges();
            }
        }

        //TODO: new strategy; load fresh PBIs into memory. loop through and put "additions" in a new List. (add to db later)
        // then, loop through existings. delete or update. then do additions from list.

        public void CopyPlatformBudgetInfos()
        {
            using (var cpprogContext = new ClientPortalProgContext())
            using (var rtContext = new RevTrackContext())
            {
                var cppPBIs = cpprogContext.PlatformBudgetInfos;
                foreach (var cppPBI in cppPBIs.OrderBy(x => x.CampaignId).ThenBy(x => x.Date).ThenBy(x => x.PlatformId))
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
                foreach (var platform in platforms.OrderBy(x => x.Id))
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
                    foreach (var campId in campIds.OrderBy(x => x))
                    {
                        var extAccounts = cpprogContext.ExtAccounts.Where(a => a.CampaignId.HasValue && a.CampaignId.Value == campId);
                        var platformGroups = extAccounts.GroupBy(a => a.PlatformId);
                        foreach (var platformGroup in platformGroups.OrderBy(x => x.Key))
                        {
                            var acctIds = platformGroup.Select(g => g.Id).ToList();
                            var dSums = cpprogContext.DailySummaries.Where(ds => acctIds.Contains(ds.AccountId));
                            var progSums = dSums.GroupBy(ds => new
                            {
                                Year = ds.Date.Year,
                                Month = ds.Date.Month
                            }).OrderBy(g => g.Key.Year)
                              .ThenBy(g => g.Key.Month)
                              .AsEnumerable()
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
