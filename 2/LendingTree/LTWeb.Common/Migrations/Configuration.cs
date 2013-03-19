namespace LTWeb.Common.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<LTWeb.DataAccess.LTWebDataContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(LTWeb.DataAccess.LTWebDataContext db)
        {
            var test = new SourceOfRequestType
            {
                LendingTreeAffiliatePartnerCode = "19655",
                LendingTreeAffiliateUserName = "DirectAgentsUser",
                LendingTreeAffiliatePassword = "capeWrAjujujajA5uspa",
                LendingTreeAffiliateEsourceID = "5860430",
            };
            db.ServiceConfigs.AddOrUpdate(c => c.Name, new ServiceConfig
            {
                SourceOfRequest = test,
                PostUrl = "http://qaaffiliates.lendingtree.com/v1/qfpost.aspx",
                Name = "Test"
            });

            var prod = new SourceOfRequestType
            {
                LendingTreeAffiliatePartnerCode = "19655",
                LendingTreeAffiliateUserName = "DirectAgentsUser",
                LendingTreeAffiliatePassword = "racHadRa3uQufuspaste",
                LendingTreeAffiliateEsourceID = "5860430",
            };
            db.ServiceConfigs.AddOrUpdate(c => c.Name, new ServiceConfig
            {
                SourceOfRequest = prod,
                PostUrl = "https://affiliates.lendingtree.com/v1/qfpost.aspx",
                Name = "Production"
            });

            db.AdminSettings.AddOrUpdate(c => c.Name, new AdminSetting
            {
                Name = "Rate1",
                Value = @"<div class=""rate"">
   2.5%
   <br />
   (2.62% APR)*
</div>"
            });

            var paperLeaf = new SourceOfRequestType
            {
                LendingTreeAffiliatePartnerCode = "19655",
                LendingTreeAffiliateUserName = "DirectAgentsUser",
                LendingTreeAffiliatePassword = "racHadRa3uQufuspaste",
                LendingTreeAffiliateEsourceID = "6103846",
            };
            db.ServiceConfigs.AddOrUpdate(c => c.Name, new ServiceConfig
            {
                SourceOfRequest = paperLeaf,
                PostUrl = "https://affiliates.lendingtree.com/v1/qfpost.aspx",
                Name = "PaperLeaf"
            });

            db.AdminSettings.AddOrUpdate(c => c.Name, new AdminSetting
            {
                Name = "Rate1",
                Value = @"<div class=""rate"">
   2.5%
   <br />
   (2.62% APR)*
</div>"
            });

            db.AdminSettings.AddOrUpdate(c => c.Name, new AdminSetting
            {
                Name = "Rate2",
                Value = @"<h2 class=""green"">$200,000 mortgage for $790/month</h2>
<h2 class=""green"">$300,000 mortgage for $1,185/month</h2>
<h2 class=""green"">$400,000 mortgage for $1,581/month</h2>"
            });

            db.AdminSettings.AddOrUpdate(c => c.Name, new AdminSetting
            {
                Name = "Pixel",
                Value = "default"
            });
        }
    }
}
