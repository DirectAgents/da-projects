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

        //  This method will be called after migrating to the latest version.
        protected override void Seed(LTWeb.DataAccess.LTWebDataContext context)
        {
            InsertServiceConfigs(context);
            InsertAdminSettings(context);
        }

        private static void InsertServiceConfigs(LTWeb.DataAccess.LTWebDataContext context)
        {
            context.ServiceConfigs.AddOrUpdate(
                x => x.Name,
                LTWeb.ServiceConfig.CreateFromXml(TestConfig),
                LTWeb.ServiceConfig.CreateFromXml(ProductionConfig)
            );
        }

        private static void InsertAdminSettings(LTWeb.DataAccess.LTWebDataContext context)
        {
            LTWeb.AdminSetting adminSetting;

            adminSetting = context.AdminSettings.SingleOrDefault(c => c.Name == "Rate1");
            if (adminSetting == null)
            {
                context.AdminSettings.Add(new LTWeb.AdminSetting { Name = "Rate1", Value = "2.00%" });
            }

            adminSetting = context.AdminSettings.SingleOrDefault(c => c.Name == "Rate2");
            if (adminSetting == null)
            {
                context.AdminSettings.Add(new LTWeb.AdminSetting { Name = "Rate2", Value = "2.02%" });
            }
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
