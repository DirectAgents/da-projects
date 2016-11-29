using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using DirectAgents.Domain.Entities.CPProg;

namespace DirectAgents.Domain.Entities.RevTrack
{
    public class Client
    {
        public int Id { get; set; }
        public string Name { get; set; }
        [MaxLength(50)]
        public string Code { get; set; }

        public virtual ICollection<ProgCampaign> ProgCampaigns { get; set; }
    }

    public class ProgCampaign
    {
        public int Id { get; set; }
        public int ClientId { get; set; }
        public virtual Client Client { get; set; }

        public string Name { get; set; }
        [MaxLength(50)]
        public string Code { get; set; }
        public BudgetInfoVals DefaultBudgetInfo { get; set; }

        public virtual ICollection<ProgBudgetInfo> BudgetInfos { get; set; }
        public virtual ICollection<ProgVendorBudgetInfo> VendorBudgetInfos { get; set; }
        public virtual ICollection<ProgSummary> Summaries { get; set; }
        public virtual ICollection<ProgExtraItem> ExtraItems { get; set; }
    }

    // --- BudgetInfos ---

    public class ProgBudgetInfo : BudgetInfoVals
    {
        public int ProgCampaignId { get; set; }
        public DateTime Date { get; set; }

        public virtual ProgCampaign ProgCampaign { get; set; }
    }
    public class ProgVendorBudgetInfo : BudgetInfoVals
    {
        public int ProgCampaignId { get; set; }
        public int VendorId { get; set; }
        public DateTime Date { get; set; }

        public virtual ProgCampaign ProgCampaign { get; set; }
        public virtual Vendor Vendor { get; set; }
    }

    // --- Vendor ---

    public class Vendor
    {
        public int Id { get; set; }
        public string Name { get; set; }
        [MaxLength(50)]
        public string Code { get; set; }
    }

}
