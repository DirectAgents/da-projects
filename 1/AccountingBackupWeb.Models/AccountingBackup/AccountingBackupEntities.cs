using System.Linq;

namespace AccountingBackupWeb.Models.AccountingBackup
{
    public partial class AccountingBackupEntities
    {
        public interface IFactory
        {
            AccountingBackupEntities Create();
        }

        public static AccountingBackupEntities Instance
        {
            get { return new AccountingBackupEntities(); }
        }

        partial void OnContextCreated()
        {
            if (TabGroups.Count() == 0)
            {
                CreateTabGroups();
                SaveChanges();
            }
        }

        private void CreateTabGroups()
        {
            //
            // Accounting Backup
            //

            Tab campaignSpendTab = new Tab {
                Name = "Campaign Spend",
                Location = "CampaignSpend.aspx"
            };

            Tab openInvoicesTab = new Tab {
                Name = "Open Invoices",
                Location = "OpenInvoices.aspx"
            };

            Tab allOpenInvoicesTab = new Tab {
                Name = "All Open Invoices",
                Location = "AllOpenInvoices.aspx"
            };

            Tab paymentsTab = new Tab {
                Name = "Payments",
                Location = "Payments.aspx"
            };

            Tab advertiserSummaryTab = new Tab {
                Name = "Advertiser Summary",
                Location = "AdvertiserSummary.aspx"
            };

            Tab budgetReportTab = new Tab {
                Name = "Budget Report",
                Location = "BudgetReport.aspx"
            };

            Tab advertiserMappingTab = new Tab {
                Name = "Advertiser Mapping",
                Location = "AdvertiserMapping.aspx"
            };

            TabGroup accountingBackupTabGroup = new TabGroup {
                Name = "Accounting Backup"
            };

            accountingBackupTabGroup.Tabs.Add(campaignSpendTab);
            accountingBackupTabGroup.Tabs.Add(openInvoicesTab);
            accountingBackupTabGroup.Tabs.Add(allOpenInvoicesTab);
            accountingBackupTabGroup.Tabs.Add(paymentsTab);
            accountingBackupTabGroup.Tabs.Add(advertiserSummaryTab);
            accountingBackupTabGroup.Tabs.Add(budgetReportTab);
            accountingBackupTabGroup.Tabs.Add(advertiserMappingTab);

            TabGroups.AddObject(accountingBackupTabGroup);

            //
            // Direct Track
            //

            Tab directTrackCampaignsTab = new Tab {
                Name = "Campaigns",
                Location = "DirectTrack/DirectTrackCampaigns.aspx"
            };

            TabGroup directTrackTabGroup = new TabGroup {
                Name = "Direct Track"
            };

            directTrackTabGroup.Tabs.Add(directTrackCampaignsTab);

            TabGroups.AddObject(directTrackTabGroup);
        }
    }
}
