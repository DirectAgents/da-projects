using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace DirectAgents.Domain.Entities.CPProg
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
        public const string Code_Twitter = "tw";
        public const string Code_Instagram = "insta";

        public static IEnumerable<string> Codes_Social()
        {
            return new string[] { Code_FB, Code_Twitter, Code_Instagram };
        }

        public static int GetId(DirectAgents.Domain.Contexts.ClientPortalProgContext db, string platCode)
        {
            return db.Platforms.Where(p => p.Code == platCode).First().Id;
        }

    }

    public class PlatColMapping : ColumnMapping
    {
        public int Id { get; set; }
        public virtual Platform Platform { get; set; }
    }
    public class ColumnMapping : ColumnMappingStats
    {
        public string StrategyName { get; set; }
        public string StrategyEid { get; set; }
        public string TDadName { get; set; }
        public string TDadEid { get; set; }
        public string SiteName { get; set; }
        public string Month { get; set; }

        public override void SetDefaults()
        {
            base.SetDefaults();
            StrategyName = "Campaign";
            StrategyEid = "";
            TDadName = "Creative";
            TDadEid = "";
            SiteName = "Website";
            Month = "Month";
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
        public string PostClickRev { get; set; }
        public string PostViewRev { get; set; }

        public virtual void SetDefaults()
        {
            Date = "Date";
            Cost = "Cost";
            Impressions = "Impressions";
            Clicks = "Clicks";
            PostClickConv = "PostClickConv";
            PostViewConv = "PostViewConv";
            PostClickRev = "";
            PostViewRev = "";
        }
    }

    public class Network
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public class ExtAccount
    {
        public int Id { get; set; }
        public int PlatformId { get; set; }
        public virtual Platform Platform { get; set; }
        public int? NetworkId { get; set; }
        public virtual Network Network { get; set; }
        public int? CampaignId { get; set; }
        public virtual Campaign Campaign { get; set; }

        public string ExternalId { get; set; }
        public string Name { get; set; }

        public string CreativeURLFormat { get; set; }

        [NotMapped]
        public string DisplayName1
        {
            get { return "(" + Platform.Name + ") " + Name + " [" + ExternalId + "]"; }
        }
        [NotMapped]
        public string DisplayName2
        {
            get { return Id + ". " + Name + " [" + ExternalId + "]"; }
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

    public class AdSet
    {
        public int Id { get; set; }
        public int AccountId { get; set; }
        [ForeignKey("AccountId")]
        public virtual ExtAccount ExtAccount { get; set; }
        public int? StrategyId { get; set; }
        public virtual Strategy Strategy { get; set; }

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
        public string Url { get; set; }

        // AdRoll Facebook
        public string Body { get; set; }
        public string Headline { get; set; }
        public string Message { get; set; }
        public string DestinationUrl { get; set; }

        [NotMapped]
        const int URLMAX = 100;
        [NotMapped]
        public bool IsUrlShortened { get { return Url == null ? false : Url.Length > URLMAX; } }
        [NotMapped]
        public string UrlShortened
        {
            get
            {
                if (IsUrlShortened) return Url.Substring(0, URLMAX - 3) + "...";
                return Url;
            }
        }


        // ? nullable StrategyId ?
    }
}
