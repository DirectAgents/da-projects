using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DirectAgents.Domain.Entities.CPProg;

//TODO: rename file to ProgClientVendor ?

namespace DirectAgents.Domain.Entities.RevTrack
{
    public class ProgClient
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<ProgCampaign> ProgCampaigns { get; set; }
    }

    public class ProgCampaign
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }
        public int ProgClientId { get; set; }
        public virtual ProgClient ProgClient { get; set; }

        public string Name { get; set; }
        public BudgetInfoVals DefaultBudgetInfo { get; set; }

        public virtual ICollection<ProgBudgetInfo> ProgBudgetInfos { get; set; }
        public virtual ICollection<ProgVendorBudgetInfo> ProgVendorBudgetInfos { get; set; }
        public virtual ICollection<ProgSummary> ProgSummaries { get; set; }
        public virtual ICollection<ProgExtraItem> ProgExtraItems { get; set; }
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
        public int ProgVendorId { get; set; }
        public DateTime Date { get; set; }

        public virtual ProgCampaign ProgCampaign { get; set; }
        public virtual ProgVendor ProgVendor { get; set; }
    }

    // --- Vendor ---

    public class ProgVendor
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }
        public string Name { get; set; }
        [MaxLength(50)]
        public string Code { get; set; }
    }

}
