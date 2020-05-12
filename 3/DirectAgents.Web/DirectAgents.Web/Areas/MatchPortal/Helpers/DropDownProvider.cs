using System.Collections.Generic;
using System.Web.Mvc;

namespace DirectAgents.Web.Areas.MatchPortal.Helpers
{
    public class DropDownProvider
    {
        public static IEnumerable<SelectListItem> GetCategories()
        {
            var categoryList = new SelectList(MpTempDataContext.Categories);
            return categoryList;
        }

        public static IEnumerable<SelectListItem> GetBrands()
        {
            var brandList = new SelectList(MpTempDataContext.Brands);
            return brandList;
        }
    }
}