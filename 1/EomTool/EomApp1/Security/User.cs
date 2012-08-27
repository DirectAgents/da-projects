using System.Linq;
using System;

namespace EomApp1.Security
{
    public class User : UserBase
    {
        private static User Instance;
        public static User Current
        {
            get
            {
                if (Instance == null)
                    Instance = new User();
                return Instance;
            }
        }

        public bool CanDoWorkflowFinalize(string accountManagerName)
        {
            return HasPermission("Workflow.Finalize." + accountManagerName.Trim());
        }

        public bool CanDoWorkflowVerify
        {
            get
            {
                return HasPermission("Workflow.Verify");
            }
        }

        public bool CanDoAccountingVerify
        {
            get
            {
                return HasPermission("PublisherReports.Verify");
            }
        }

        public bool CanDoAccountingApprove
        {
            get
            {
                return HasPermission("PublisherReports.Approve");
            }
        }

        public bool CanDoAccountingPay
        {
            get
            {
                return HasPermission("PublisherReports.Pay");
            }
        }

        public bool CanDoMenuItem(string tag)
        {
            return HasPermission(tag);
        }

        public static string GetEmailAddress(string name)
        {
            using (var db = new Security.EomToolSecurityEntities())
            {
                var group = db.Groups.Where(c => c.Name == name).FirstOrDefault();
                if (group != null)
                {
                    return group.EmailAddress;
                }
                else
                {
                    throw new Exception("There is no group named " + name);
                }
            }
        }
    }
}
