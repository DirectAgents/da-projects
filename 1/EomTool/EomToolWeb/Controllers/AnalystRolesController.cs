using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using EomTool.Domain.Abstract;
using EomTool.Domain.Entities;
using EomToolWeb.Models;

namespace EomToolWeb.Controllers
{
    public class AnalystRolesController : EOMController
    {
        public AnalystRolesController(IMainRepository mainRepository, ISecurityRepository securityRepository, IDAMain1Repository daMain1Repository, IEomEntitiesConfig eomEntitiesConfig)
        {
            this.mainRepo = mainRepository;
            this.securityRepo = securityRepository;
            this.daMain1Repo = daMain1Repository;
            this.eomEntitiesConfig = eomEntitiesConfig;
        }

        public ActionResult Affiliates()
        {
            SetAccountingPeriodViewData();
            var activeAffiliates = mainRepo.Affiliates(true).OrderBy(a => a.name2);
            return View(activeAffiliates);
        }

        public ActionResult PubAdvCampList(int? affid, int? advid, int? pid)
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
            foreach (var affGroup in affGroupList)
            {
                var affiliate = mainRepo.GetAffiliate(affGroup.AffId);
                if (affiliate != null)
                    affGroup.AffName = affiliate.name2;
                var affCampaigns = campaigns.Where(c => affGroup.Pids.Contains(c.pid));
                affGroup.AdvCampGroups = affCampaigns.GroupBy(x => x.Advertiser).OrderBy(x => x.Key.name).ToList();
                foreach (var advCampGroup in affGroup.AdvCampGroups)
                {
                    foreach (var campaign in advCampGroup)
                    {
                        campaign.AnalystRoles = mainRepo.AnalystRoles(pid: campaign.pid, affid: affGroup.AffId);
                    }
                }
            }
            SetAccountingPeriodViewData();
            return View(affGroupList.OrderBy(x => x.AffName));
        }

        public ActionResult Edit(int pid, int affid)
        {
            var camp = mainRepo.GetCampaign(pid);
            var aff = mainRepo.GetAffiliate(affid);
            if (camp == null || aff == null)
                return HttpNotFound();

            var model = new AnalystRolesVM
            {
                CurrentEomDateString = eomEntitiesConfig.CurrentEomDateString,
                Campaign = camp,
                Affiliate = aff,
                AnalystRoles = mainRepo.AnalystRoles(pid: pid, affid: affid).OrderBy(ar => ar.Person.first_name).ThenBy(ar => ar.Person.last_name),
                AvailablePeople = mainRepo.AvailableAnalystPeople(pid, affid).OrderBy(p => p.first_name).ThenBy(p => p.last_name)
            };
            return View(model);
        }

        public ActionResult Add(int pid, int affid, int personId)
        {
            var aRole = new AnalystRole
            {
                person_id = personId,
                pid = pid,
                affid = affid
            };
            mainRepo.AddAnalystRole(aRole);
            return RedirectToAction("Edit", new { pid = pid, affid = affid });
        }

        public ActionResult Delete(int pid, int affid, int personId)
        {
            mainRepo.DeleteAnalystRole(pid, affid, personId);
            return RedirectToAction("Edit", new { pid = pid, affid = affid });
        }

    }
    public class AffGroup
    {
        public int AffId { get; set; }
        public string AffName { get; set; }
        public int[] Pids { get; set; }
        public IEnumerable<IGrouping<Advertiser, Campaign>> AdvCampGroups { get; set; }
    }
}