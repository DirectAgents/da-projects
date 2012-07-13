using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
            var ident = System.Security.Principal.WindowsIdentity.GetCurrent();
            return ident.Name.ToLower();
        }
    }
}
