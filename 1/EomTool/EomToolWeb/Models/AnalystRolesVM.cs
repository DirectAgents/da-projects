using System.Collections.Generic;
using EomTool.Domain.Entities;

namespace EomToolWeb.Models
{
    public class AnalystRolesVM
    {
        public string CurrentEomDateString { get; set; }
        public Campaign Campaign { get; set; }
        public Affiliate Affiliate { get; set; }
        public IEnumerable<AnalystRole> AnalystRoles { get; set; }
        public IEnumerable<Person> AvailablePeople { get; set; }
    }
}