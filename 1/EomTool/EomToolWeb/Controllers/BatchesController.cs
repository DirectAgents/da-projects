using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EomTool.Domain.Abstract;
using System.Text;

namespace EomToolWeb.Controllers
{
    public class BatchesController : Controller
    {
        private IBatchRepository batchRepository;
        public BatchesController(IBatchRepository batchRepository)
        {
            this.batchRepository = batchRepository;
        }

        public ActionResult ShowNotes(string batchids)
        {
            int[] batchIdsArray = batchids.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries).Select(id => Convert.ToInt32(id)).ToArray();
            var batches = batchRepository.BatchesByBatchIds(batchIdsArray, true);
            var batchUpdates = batches.SelectMany(b => b.BatchUpdates).Distinct().OrderByDescending(n => n.date_created);
            return PartialView(batchUpdates);
        }
    }
}
