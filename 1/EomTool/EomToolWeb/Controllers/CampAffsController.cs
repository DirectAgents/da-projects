using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using EomTool.Domain.Abstract;
using EomTool.Domain.Entities;
using EomToolWeb.Models;
using MoreLinq;

namespace EomToolWeb.Controllers
{
    public class CampAffsController : EOMController
    {
        public CampAffsController(IMainRepository mainRepository, ISecurityRepository securityRepository, IDAMain1Repository daMain1Repository, IEomEntitiesConfig eomEntitiesConfig)
        {
            this.mainRepo = mainRepository;
            this.securityRepo = securityRepository;
            this.daMain1Repo = daMain1Repository;
            this.eomEntitiesConfig = eomEntitiesConfig;
        }

        public ActionResult Index()
        {
            if (!securityRepo.IsAccountantOrAdmin(User))
                return Content("unauthorized");

            var campAffs = mainRepo.CampAffs().OrderBy(x => x.pid).ThenBy(x => x.affid);

            SetAccountingPeriodViewData();
            return View(campAffs);
        }

        // Used for selecting an affiliate, then listing advs/camps under that affiliate
        public ActionResult Affiliates()
        {
            if (!securityRepo.IsAccountantOrAdmin(User))
                return Content("unauthorized");

            SetAccountingPeriodViewData();
            var activeAffiliates = mainRepo.Affiliates(true).OrderBy(a => a.name2).ToArray();
            foreach (var aff in activeAffiliates)
            {
                aff.AnalystRolesAssigned = mainRepo.AnalystRoles(affid: aff.affid).Any();
            }
            return View(activeAffiliates);
        }

        public ActionResult ShowRoles(int? affid, int? advid, int? pid)
        {
            var items = mainRepo.GetItems(pid: pid, affId: affid, advertiserId: advid);
            items = items.Where(i => i.total_revenue > 0 || i.total_cost > 0);
            var itemGroups = items.GroupBy(i => i.affid);
            var affGroupList = new List<AffGroup>();
            foreach (var itemGroup in itemGroups)
            {
                var affGroup = new AffGroup
                {
                    AffId = itemGroup.Key,
                    Pids = itemGroup.Select(x => x.pid).Distinct().ToArray()
                };
                affGroupList.Add(affGroup);
            }
            var campaigns = mainRepo.Campaigns();
            var allCampAffs = mainRepo.CampAffs();
            foreach (var affGroup in affGroupList)
            {
                var affiliate = mainRepo.GetAffiliate(affGroup.AffId);
                if (affiliate != null)
                    affGroup.AffName = affiliate.name2;
                var affCampaigns = campaigns.Where(c => affGroup.Pids.Contains(c.pid));
                affGroup.AdvCampGroups = affCampaigns.GroupBy(x => x.Advertiser).OrderBy(x => x.Key.name).ToList();
                var campAffs = allCampAffs.Where(x => x.affid == affGroup.AffId).ToList();

                foreach (var advCampGroup in affGroup.AdvCampGroups)
                {
                    foreach (var campaign in advCampGroup)
                    {
                        campaign.CampAff = campAffs.Where(x => x.pid == campaign.pid && x.affid == affGroup.AffId)
                            .SingleOrFallback(() => {
                                return new CampAff { pid = campaign.pid, affid = affGroup.AffId };
                            });
                        //legacy:
                        campaign.AnalystRoles = mainRepo.AnalystRoles(pid: campaign.pid, affid: affGroup.AffId);
                    }
                }
            }
            SetAccountingPeriodViewData();
            return View(affGroupList.OrderBy(x => x.AffName));
        }

        [HttpGet]
        public ActionResult Edit(int pid, int affid)
        {
            var campAff = mainRepo.GetCampAff(pid, affid);
            if (campAff == null)
            {
                //todo: check if camp and aff exist
                campAff = new CampAff
                {
                    pid = pid,
                    affid = affid
                };
                //NOTE: at some point this entity needs to be created in the database
            }
            mainRepo.FillExtended(campAff);

            var model = new CampAffVM
            {
                CurrentEomDateString = eomEntitiesConfig.CurrentEomDateString,
                CampAff = campAff,
                Analysts = mainRepo.Analysts().OrderBy(x => x.name),
                Strategists = mainRepo.Strategists().OrderBy(x => x.name),
                AnalystRoles = mainRepo.AnalystRoles(pid: campAff.pid, affid: campAff.affid)
            };
            return View(model);
        }
        [HttpPost]
        public ActionResult Edit(CampAff campAff)
        {
            if (ModelState.IsValid)
            {
                bool saved = mainRepo.SaveCampAff(campAff);
                if (saved)
                    return RedirectToAction("ShowRoles", new { affid = campAff.affid });
                ModelState.AddModelError("", "Changes could not be saved");
            }
            return View(campAff);
        }

    }
}