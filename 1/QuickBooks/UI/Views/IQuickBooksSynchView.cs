using QuickBooks.UI.Models;
using System;
using System.Collections.Generic;

namespace QuickBooks.UI.Views
{
    public interface IQuickBooksSynchView : IView
    {
        event EventHandler<GetCompanyFilesEventArgs> GetCompanyFiles;
        event EventHandler<QuickBooksSynchSettingsEventArgs> GetQuickBooksSynchSettings;
        event EventHandler Go;
        QuickBooksSynchTargets[] SynchTargets { get; }
    }

    public class GetCompanyFilesEventArgs : EventArgs
    {
        public IEnumerable<CompanyFile> CompanyFiles { get; set; }
    }

    public class QuickBooksSynchSettingsEventArgs : EventArgs
    {
        public QuickBooksSynchSettings QuickBooksSynchSettings { get; set; }
    }
}
