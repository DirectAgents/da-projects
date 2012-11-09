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

        protected override void Seed(LTWeb.DataAccess.LTWebDataContext context)
        {
            //  This method will be called after migrating to the latest version.
            context.ServiceConfigs.AddOrUpdate(
                x => x.Name,
                LTWeb.ServiceConfig.CreateFromXml(TestConfig),
                LTWeb.ServiceConfig.CreateFromXml(ProductionConfig)
            );
        }

        static string TestConfig = @"<?xml version=""1.0"" encoding=""utf-8"" ?>
<ServiceConfig>
  <Name>Test</Name>
  <SourceOfRequest>
    <LendingTreeAffiliatePartnerCode>19655</LendingTreeAffiliatePartnerCode>
    <LendingTreeAffiliateUserName>DirectAgentsUser</LendingTreeAffiliateUserName>
    <LendingTreeAffiliatePassword>capeWrAjujujajA5uspa</LendingTreeAffiliatePassword>
    <LendingTreeAffiliateEsourceID>5860430</LendingTreeAffiliateEsourceID>
  </SourceOfRequest>
  <PostUrl>http://qaaffiliates.lendingtree.com/v1/qfpost.aspx</PostUrl>
</ServiceConfig>";

        static string ProductionConfig = @"<?xml version=""1.0"" encoding=""utf-8"" ?>
<ServiceConfig>
  <Name>Production</Name>
  <SourceOfRequest>
    <LendingTreeAffiliatePartnerCode>19655</LendingTreeAffiliatePartnerCode>
    <LendingTreeAffiliateUserName>DirectAgentsUser</LendingTreeAffiliateUserName>
    <LendingTreeAffiliatePassword>racHadRa3uQufuspaste</LendingTreeAffiliatePassword>
    <LendingTreeAffiliateEsourceID>5860430</LendingTreeAffiliateEsourceID>
  </SourceOfRequest>
  <PostUrl>https://affiliates.lendingtree.com/v1/qfpost.aspx</PostUrl>
</ServiceConfig>";
    }
}
