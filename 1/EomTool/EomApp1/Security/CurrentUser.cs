using System.Collections.Generic;
using System.Linq;
using DAgents.Common;

namespace EomApp1.Security
{
    public static class CurrentUser
    {
        private static List<string> _RoleNames;
        public static List<string> RoleNames
        {
            get
            {
                if (_RoleNames == null)
                    using (var db = new Security.EomToolSecurityEntities())
                        _RoleNames = (from g in db.Groups.ToList()
                                      where WindowsIdentityHelper.DoesCurrentUserHaveIdentity(g.WindowsIdentity)
                                      from r in g.Roles
                                      select r.Name).ToList();
                return _RoleNames;
            }
        }
        public static bool CanFinalize(string accountManagerName)
        {
            return RoleNames.Contains("AM: " + accountManagerName);
        }
        public static bool CanDoAccountingVerify
        {
            get
            {
                return RoleNames.Contains("Pub Rep: Verify");
            }
        }
        public static bool CanDoAccountingApprove
        {
            get
            {
                return RoleNames.Contains("Pub Rep: Approve");
            }
        }
        public static bool CanDoAccountingPay
        {
            get
            {
                return RoleNames.Contains("Pub Rep: Pay");
            }
        }
    }
}
