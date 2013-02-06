using Accounting.Domain.Entities;
using QuickBooks.Services;
using QuickBooks.UI.Models;
using QuickBooks.UI.Views;
using System.Collections.Generic;

namespace QuickBooks.UI.Presenters
{
    public class QuickBooksSynchPresenter : PresenterBase<IQuickBooksSynchView>
    {
        QuickBooksSynchService quickBooksSynchService;

        public QuickBooksSynchPresenter(IQuickBooksSynchView view)
            : base(view)
        {
            this.quickBooksSynchService = new QuickBooksSynchService("Aaron Corp");
            View.GetCompanyFiles += View_GetCompanyFiles;
            View.GetQuickBooksSynchSettings += View_GetQuickBooksSynchSettings;
            View.Go += View_Go;
        }

        void View_Go(object sender, System.EventArgs e)
        {
            foreach (var target in View.SynchTargets)
            {
                switch (target)
                {
                    case QuickBooksSynchTargets.Customer:
                        this.quickBooksSynchService.Load(this.quickBooksSynchService.Extract<Customer>(), true);
                        break;
                    case QuickBooksSynchTargets.Invoice:
                        this.quickBooksSynchService.Load(this.quickBooksSynchService.Extract<Customer>(), true);
                        break;
                    case QuickBooksSynchTargets.Payment:
                        this.quickBooksSynchService.Load(this.quickBooksSynchService.Extract<Customer>(), true);
                        break;
                    case QuickBooksSynchTargets.CreditMemo:
                        this.quickBooksSynchService.Load(this.quickBooksSynchService.Extract<Customer>(), true);
                        break;
                    default:
                        break;
                }
            }
        }

        void View_GetQuickBooksSynchSettings(object sender, QuickBooksSynchSettingsEventArgs e)
        {
            var quickBooksSynchSettings = new QuickBooksSynchSettings
            {
                CompanyFileId = 1
            };
            e.QuickBooksSynchSettings = quickBooksSynchSettings;
        }

        void View_GetCompanyFiles(object sender, GetCompanyFilesEventArgs e)
        {
            var companyFiles = new List<Models.CompanyFile>();
            companyFiles.Add(new Models.CompanyFile { Id = 1, Name = "US Company File" });
            companyFiles.Add(new Models.CompanyFile { Id = 2, Name = "INTL Company File" });
            e.CompanyFiles = companyFiles;
        }
    }
}
