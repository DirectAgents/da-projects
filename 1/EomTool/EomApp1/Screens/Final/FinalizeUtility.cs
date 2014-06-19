using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EomApp1.Screens.Final
{
    public class FinalizeUtility
    {
        public static bool CheckFinalizationMargins(int pid, string revcurr, int[] affIds, string[] costCurrs)
        {
            int[] rejectedAffIds;
            EomApp1.Screens.Final.Models.Eom.CheckFinalizationMargins(pid, revcurr, affIds, costCurrs, out rejectedAffIds);
            if (rejectedAffIds.Length > 0)
            {
                string url = Properties.Settings.Default.EOMWebBase + "/Workflow/MarginApproval?period=" + Properties.Settings.Default.StatsDate.ToShortDateString()
                                + "&pid=" + pid + String.Join("", rejectedAffIds.Select(a => "&affid=" + a));
                System.Diagnostics.Process.Start(url);
                return false;
            }
            return true;
        }

    }
}
