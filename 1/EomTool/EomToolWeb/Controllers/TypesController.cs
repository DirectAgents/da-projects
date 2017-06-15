using System.Linq;
using System.Web.Mvc;
using EomTool.Domain.Abstract;
using EomToolWeb.Models;

namespace EomToolWeb.Controllers
{
    public class TypesController : EOMController
    {
        public TypesController(IMainRepository mainRepository, ISecurityRepository securityRepository, IDAMain1Repository daMain1Repository, IEomEntitiesConfig eomEntitiesConfig)
        {
            this.mainRepo = mainRepository;
            this.securityRepo = securityRepository;
            this.daMain1Repo = daMain1Repository;
            this.eomEntitiesConfig = eomEntitiesConfig;
        }

        // Note: this is here because the CampaignsController uses the DirectAgents domain
        public JsonResult CampaignsValueText(bool withActivity = false)
        {
            var campaigns = mainRepo.Campaigns(activeOnly: withActivity).OrderBy(c => c.campaign_name);
            var valueTexts = campaigns.Select(c => new IntValueText() { value = c.pid, text = c.campaign_name });
            return Json(valueTexts, JsonRequestBehavior.AllowGet);
        }

        public JsonResult CurrencyCodesValueText()
        {
            var valueTexts = mainRepo.CurrencyList.Select(c => new StringValueText() { value = c.name, text = c.name });
            return Json(valueTexts, JsonRequestBehavior.AllowGet);
        }

        public JsonResult UnitTypesValueText()
        {
            var valueTexts = mainRepo.UnitTypeList.Select(ut => new IntValueText() { value = ut.id, text = ut.name });
            //var valueTexts = mainRepo.UnitTypes().Select(ut => new IntValueText() { value = ut.id, text = ut.name });
            return Json(valueTexts, JsonRequestBehavior.AllowGet);
        }

        public JsonResult CampaignStatusValueText()
        {
            var valueTexts = mainRepo.CampaignStatusList.Select(cs => new IntValueText() { value = cs.id, text = cs.name });
            return Json(valueTexts, JsonRequestBehavior.AllowGet);
        }

        public JsonResult AccountingStatusValueText()
        {
            var valueTexts = mainRepo.AccountingStatusList.Select(a => new IntValueText() { value = a.id, text = a.name });
            return Json(valueTexts, JsonRequestBehavior.AllowGet);
        }
    }
}