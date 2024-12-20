﻿using DirectAgents.Mvc.KendoGridBinder.Containers;
using System.Collections.Generic;
using System.Web.Mvc;

namespace DirectAgents.Mvc.KendoGridBinder
{
    [ModelBinder(typeof(KendoGridModelBinder))]
    public class KendoGridRequest
    {
        public int Take { get; set; }

        public int Skip { get; set; }

        public int Page { get; set; }

        public int PageSize { get; set; }

        public string Logic { get; set; }

        public FilterObjectWrapper FilterObjectWrapper { get; set; }

        public IEnumerable<SortObject> SortObjects { get; set; }
    }
}