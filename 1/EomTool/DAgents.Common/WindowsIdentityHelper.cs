using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
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

        public static bool DoesCurrentUserHaveIdentity(string identityName)
        {
            return CurrentUsersGroupsAndIdentity.Contains(identityName.ToUpper());
        }

        public static bool DoesCurrentUserHaveIdentity(IEnumerable<string> identityNames)
        {
            return identityNames.Any(c => CurrentUsersGroupsAndIdentity.Contains(c.ToUpper()));
        }

        public static List<string> CurrentUsersGroupsAndIdentity
        {
            get
            {
                if (_CurrentUsersGroups == null)
                {
                    _CurrentUsersGroups = new List<string>();
                    var groups = WindowsIdentity.GetCurrent().Groups;
                    foreach (var group in groups)
                    {
                        try
                        {
                            string identityRef = ((IdentityReference)group).Translate(typeof(NTAccount)).Value.ToUpper();
                            _CurrentUsersGroups.Add(identityRef);
                        }
                        catch (Exception)
                        {
                        }
                    }
                    _CurrentUsersGroups.Add(GetWindowsIdentityName().ToUpper());
                }
                return _CurrentUsersGroups;
            }
        }
        private static List<string> _CurrentUsersGroups;

        public static string GetIpAddress()
        {
            try
            {
                string responseText = "";
                WebRequest request = WebRequest.Create("http://checkip.dyndns.org/");
                using (WebResponse response = request.GetResponse())
                using (StreamReader stream = new StreamReader(response.GetResponseStream()))
                {
                    responseText = stream.ReadToEnd();
                }
                //Search for the ip in the html
                int first = responseText.IndexOf("Address: ") + 9;
                int last = responseText.LastIndexOf("</body>");
                return responseText.Substring(first, last - first).Trim();
            }
            catch
            {
                return string.Empty;
            }
        }
    }
}
