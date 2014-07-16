using ClientPortal.Web.Controllers;
using System.Web.Configuration;

namespace ClientPortal.Web.Areas.Admin
{
    public class Helpers
    {
        public static void SendUserProfileEmail(string username, string password, string email)
        {
            string from = WebConfigurationManager.AppSettings["EmailFromDefault"];
            string subject = "Direct Agents Client Portal account created";
            string url = WebConfigurationManager.AppSettings["PortalUrl"];
            string body = (@"Congratulations! Your Direct Agents Client Portal account has been created:
<p>
Username: [[Username]]<br/>
Password: [[Password]]<br/>
<br/>
Portal URL: [[Url]]
</p>")
               .Replace("[[Username]]", username)
               .Replace("[[Password]]", password)
               .Replace("[[Url]]", url);

            ControllerHelpers.SendEmail(from, email, null, subject, body, true);
        }
    }
}