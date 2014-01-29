﻿using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace ClientPortal.Data.Contexts
{
    public class CampaignDrop_Validation
    {
        [Range(typeof(DateTime), "1/1/2000", "1/1/3000")]
        public DateTime Date { get; set; }

        [DataType(DataType.EmailAddress)]
        public string FromEmail { get; set; }
    }

    [MetadataType(typeof(CampaignDrop_Validation))]
    public partial class CampaignDrop
    {
        [NotMapped]
        public bool IsInCPMReport
        {
            get
            {
                bool isInReport = this.CPMReports.Any();
                isInReport = isInReport || this.CampaignDropCopies.Any(cd => cd.CPMReports.Any());
                return isInReport;
            }
        }

        [NotMapped]
        public string DisplayName
        {
            get { return string.Format("{0} Aff: {1} Vol: {2} Cost: {3}", Date.ToShortDateString(), Campaign.AffiliateId, Volume.HasValue ? Volume.Value.ToString("N0") : "", Cost.HasValue ? Cost.Value.ToString("C2") : ""); }
        }

        [NotMapped]
        public decimal? OpenRate
        {
            get
            {
                if (Opens == null || Volume == null || Volume.Value == 0)
                    return null;
                else
                    return (decimal)Opens.Value / Volume.Value;
            }
        }

        private CreativeStat creativeStatTotals;
        [NotMapped]
        public CreativeStat CreativeStatTotals
        {
            get
            {
                if (creativeStatTotals == null)
                {
                    creativeStatTotals = new CreativeStat
                    {
                        Clicks = CreativeStats.Sum(cs => cs.Clicks ?? 0),
                        Leads = CreativeStats.Sum(cs => cs.Leads ?? 0)
                    };
                }
                return creativeStatTotals;
            }
        }

        [NotMapped]
        public decimal? ClickThroughRate
        {
            get
            {
                if (Opens == null || Opens.Value == 0)
                    return null;
                else
                    return (decimal)CreativeStatTotals.Clicks.Value / Opens.Value;
            }
        }

        [NotMapped]
        public decimal? CostPerLead
        {
            get
            {
                if (Cost == null || CreativeStatTotals.Leads == null || creativeStatTotals.Leads.Value == 0)
                    return null;
                else
                    return Cost.Value / CreativeStatTotals.Leads.Value;
            }
        }

        public void SetFieldsFrom(CampaignDrop inDrop)
        {
            Date = inDrop.Date;
            FromEmail = inDrop.FromEmail;
            Subject = inDrop.Subject;
            Cost = inDrop.Cost;
            Volume = inDrop.Volume;
            Opens = inDrop.Opens;
        }

        public CampaignDrop Duplicate()
        {
            var dropCopy = new CampaignDrop
            {
                CampaignId = this.CampaignId,
                CopyOf = this.CampaignDropId
            };
            dropCopy.SetFieldsFrom(this);

            foreach (var cStat in this.CreativeStats)
            {
                var cStatCopy = new CreativeStat();
                cStatCopy.SetPropertiesFrom(cStat);
                dropCopy.CreativeStats.Add(cStatCopy);
            }
            return dropCopy;
        }

        public bool AnyChanges(CampaignDrop inDrop)
        {
            bool anyChanges = false;

            if (this.Date != inDrop.Date ||
                this.Cost != inDrop.Cost ||
                this.Volume != inDrop.Volume ||
                this.Opens != inDrop.Opens ||
                this.Subject != inDrop.Subject)
                anyChanges = true;

            // Check for changes in CreativeStats ?
//            if (!anyChanges)
//                foreach (var cstat in this.CreativeStats)

            return anyChanges;
        }

    }
}
