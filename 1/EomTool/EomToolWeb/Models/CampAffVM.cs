using System.Collections.Generic;
using EomTool.Domain.Entities;

namespace EomToolWeb.Models
{
    public class CampAffVM
    {
        public string CurrentEomDateString { get; set; }
        public CampAff CampAff { get; set; }
        public IEnumerable<Analyst> Analysts { get; set; }
        public IEnumerable<Strategist> Strategists { get; set; }

        public IEnumerable<AnalystRole> AnalystRoles { get; set; } //for legacy data
    }
}