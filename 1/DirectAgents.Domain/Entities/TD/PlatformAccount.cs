﻿using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DirectAgents.Domain.Entities.TD
{
    public class Platform
    {
        public int Id { get; set; }
        [MaxLength(50), Index("CodeIndex", IsUnique=true)]
        public string Code { get; set; }
        public string Name { get; set; }

        public virtual ICollection<ExtAccount> ExtAccounts { get; set; }
        //public virtual ICollection<PlatformBudgetInfo> PlatformBudgetInfos { get; set; }
        public virtual PlatColMapping PlatColMapping { get; set; }

        public const string Code_DATradingDesk = "datd";
        public const string Code_AdRoll = "adr";
        public const string Code_DBM = "dbm";
        public const string Code_FB = "fb";
    }

    public class PlatColMapping : ColumnMapping
    {
        public int Id { get; set; }
        public virtual Platform Platform { get; set; }
    }
    public class ColumnMapping : ColumnMappingStats
    {
        [NotMapped] //TODO: remove these annotations!
        public string StrategyName { get; set; }
        [NotMapped]
        public string StrategyEid { get; set; }
        [NotMapped]
        public string TDadName { get; set; }
        [NotMapped]
        public string TDadEid { get; set; }
        [NotMapped]
        public string SiteName { get; set; }

        public override void SetDefaults()
        {
            base.SetDefaults();
            StrategyName = "Campaign";
            StrategyEid = "CampaignEid";
            TDadName = "Creative";
            TDadEid = "CreativeEid";
            SiteName = "Website";
        }

        public static ColumnMapping CreateDefault()
        {
            var mapping = new ColumnMapping();
            mapping.SetDefaults();
            return mapping;
        }
    }
    public class ColumnMappingStats
    {
        // the values are the names of the columns these properties are mapped to (in DailySummary)
        [Required]
        public string Date { get; set; }
        public string Cost { get; set; }
        public string Impressions { get; set; }
        public string Clicks { get; set; }
        public string PostClickConv { get; set; }
        public string PostViewConv { get; set; }

        public virtual void SetDefaults()
        {
            Date = "Date";
            Cost = "Cost";
            Impressions = "Impressions";
            Clicks = "Clicks";
            PostClickConv = "PostClickConv";
            PostViewConv = "PostViewConv";
        }
    }

    public class ExtAccount
    {
        public int Id { get; set; }
        public int PlatformId { get; set; }
        public virtual Platform Platform { get; set; }

        public int? CampaignId { get; set; }
        public virtual Campaign Campaign { get; set; }

        public string ExternalId { get; set; }
        public string Name { get; set; }

        [NotMapped]
        public string DisplayName1
        {
            get { return "(" + Platform.Name + ") " + Name + " [" + ExternalId + "]"; }
        }
    }

    public class Strategy
    {
        public int Id { get; set; }
        public int AccountId { get; set; }
        [ForeignKey("AccountId")]
        public virtual ExtAccount ExtAccount { get; set; }

        public string ExternalId { get; set; }
        public string Name { get; set; }
    }

    public class TDad // TD Ad
    {
        public int Id { get; set; }
        public int AccountId { get; set; }
        [ForeignKey("AccountId")]
        public virtual ExtAccount ExtAccount { get; set; }

        public string ExternalId { get; set; }
        public string Name { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }

        // ? nullable StrategyId ?
    }
}
