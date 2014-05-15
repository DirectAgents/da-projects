using EomTool.Domain.DTOs;
using EomTool.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EomToolWeb.Models
{
    public class WorkflowModel
    {
        public string CurrentEomDateString { get; set; }
        public IEnumerable<CampaignAmount> CampaignAmounts { get; set; }
        public int? CampaignStatusId { get; set; }
    }
}