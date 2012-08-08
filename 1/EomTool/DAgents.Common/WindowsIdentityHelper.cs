using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;

namespace DAgents.Common
{
    public class WindowsIdentityHelper 
    {
        public static string GetWindowsIdentityName()
        {
            var ident = System.Security.Principal.WindowsIdentity.GetCurrent();
            return ident.Name;
        }

        public static string GetWindowsIdentityNameLower()
        {
            return GetWindowsIdentityName().ToLower();
        }

        public static bool IsCurrentUserInGroup(string groupName)
        {
            return CurrentUsersGroups.Contains(groupName.ToUpper());
        }

        public static List<string> CurrentUsersGroups
        {
            get
            {
                if (_CurrentUsersGroups == null)
                    _CurrentUsersGroups = WindowsIdentity.GetCurrent().Groups.Select(g => ((IdentityReference)g).Translate(typeof(NTAccount)).Value.ToUpper()).ToList();
                return _CurrentUsersGroups;
            }
        }
        private static List<string> _CurrentUsersGroups;
    }
}
