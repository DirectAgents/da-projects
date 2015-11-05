using System;
using System.Collections.Generic;
using DirectAgents.Domain.Entities.TD;

namespace DirectAgents.Web.Areas.TD.Models
{
    public class ExtraItemsVM
    {
        public DateTime? Month { get; set; }
        public IEnumerable<ExtraItem> Items { get; set; }
    }
}