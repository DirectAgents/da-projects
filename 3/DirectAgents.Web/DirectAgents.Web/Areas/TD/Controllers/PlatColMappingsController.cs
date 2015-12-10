using System.Web.Mvc;
using DirectAgents.Domain.Abstract;
using DirectAgents.Domain.Entities.TD;

namespace DirectAgents.Web.Areas.TD.Controllers
{
    public class PlatColMappingsController : DirectAgents.Web.Controllers.ControllerBase
    {
        public PlatColMappingsController(ITDRepository tdRepository)
        {
            this.tdRepo = tdRepository;
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            var platColMapping = tdRepo.PlatColMapping(id);
            if (platColMapping == null)
            {
                var platform = tdRepo.Platform(id);
                if (platform == null)
                    return HttpNotFound();

                platColMapping = new PlatColMapping
                {
                    Id = id,
                    Platform = platform
                };
                platColMapping.SetDefaults();
            }
            return View(platColMapping);
        }
        [HttpPost]
        public ActionResult Edit(PlatColMapping mapping)
        {
            if (ModelState.IsValid)
            {
                if (tdRepo.AddSavePlatColMapping(mapping))
                    return RedirectToAction("Edit", "Platforms", new { id = mapping.Id });
                ModelState.AddModelError("", "PlatColMapping could not be saved.");
            }
            tdRepo.FillExtended(mapping);
            return View(mapping);
        }
	}
}