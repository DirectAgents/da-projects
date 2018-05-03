using System;
using System.ComponentModel.Composition;
using CakeExtracter.Common;
using DirectAgents.Domain.Concrete;
using DirectAgents.Domain.Contexts;

namespace CakeExtracter.Commands
{
    [Export(typeof(ConsoleCommand))]
    public class DAMonthlyMaintenance : ConsoleCommand
    {
        public bool ActiveLastMonth { get; set; }
        public bool Overwrite { get; set; }

        private bool? _copyBudgetInfos;
        private bool? _createBaseFees;
        public bool CopyBudgetInfos
        {
            get { return (!_copyBudgetInfos.HasValue || _copyBudgetInfos.Value); }
        }   // default: true
        public bool CreateBaseFees
        {
            get { return (!_createBaseFees.HasValue || _createBaseFees.Value); }
        }   // default: true

        public override void ResetProperties()
        {
            _copyBudgetInfos = null;
            _createBaseFees = null;
            ActiveLastMonth = false;
            Overwrite = false;
        }

        public DAMonthlyMaintenance()
        {
            IsCommand("daMonthlyMaintenance", "perform monthly maintenance");
            HasOption<bool>("b|copyBudgetInfos=", "Step1- Copy BudgetInfos (default: true)", c => _copyBudgetInfos = c);
            HasOption<bool>("c|createBaseFees=", "Step2- Create BaseFees (default: true)", c => _createBaseFees = c);
            HasOption<bool>("a|activeLastMonth=", "Copy budgetinfos for campaigns that were active last month (as opposed to this month) (default: false)", c => ActiveLastMonth = c);
            HasOption<bool>("o|overwrite=", "Overwrite any existing budgetinfos (default: false)", c => Overwrite = c);
        }

        public override int Execute(string[] remainingArguments)
        {
            var today = DateTime.Today;
            var month = new DateTime(today.Year, today.Month, 1);

            var db = new ClientPortalProgContext();
            using (var repo = new CPProgRepository(db))
            {
                if (CopyBudgetInfos)
                {
                    Logger.Info("Copying BudgetInfos...");
                    repo.CopyBudgetInfosTo(month, activeLastMonth: ActiveLastMonth, overwrite: Overwrite);
                }
                if (CreateBaseFees)
                {
                    Logger.Info("Creating Base Fees...");
                    repo.CreateBaseFees(month); // for advertisers that haven't ended
                }
            }
            return 0;
        }

    }
}
