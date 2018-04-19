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
        public bool CopyBudgetInfos { get; set; }
        public bool CreateBaseFees { get; set; }
        public bool ActiveLastMonth { get; set; }
        public bool Overwrite { get; set; }

        public override void ResetProperties()
        {
            this.CopyBudgetInfos = true;
            this.CreateBaseFees = true;
            this.ActiveLastMonth = false;
            this.Overwrite = false;
        }

        public DAMonthlyMaintenance()
        {
            IsCommand("daMonthlyMaintenance", "perform monthly maintenance");
            HasOption<bool>("b|copyBudgetInfos=", "Step1- Copy BudgetInfos (default: true)", c => this.CopyBudgetInfos = c);
            HasOption<bool>("c|createBaseFees=", "Step2- Create BaseFees (default: true)", c => this.CreateBaseFees = c);
            HasOption<bool>("a|activeLastMonth=", "Copy budgetinfos for campaigns that were active last month (as opposed to this month) (default: false)", c => this.ActiveLastMonth = c);
            HasOption<bool>("o|overwrite=", "Overwrite any existing budgetinfos (default: false)", c => this.Overwrite = c);
        }

        public override int Execute(string[] remainingArguments)
        {
            var today = DateTime.Today;
            var month = new DateTime(today.Year, today.Month, 1);

            var db = new ClientPortalProgContext();
            using (var repo = new CPProgRepository(db))
            {
                if (CopyBudgetInfos)
                    repo.CopyBudgetInfosTo(month, activeLastMonth: ActiveLastMonth, overwrite: Overwrite);
                if (CreateBaseFees)
                    repo.CreateBaseFees(month);
            }
            return 0;
        }

    }
}
