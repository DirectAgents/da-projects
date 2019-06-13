using Newtonsoft.Json;
using SeleniumDataBrowser.VCD.Helpers.UserInfoExtracting.Models;
using SeleniumDataBrowser.VCD.PageActions;

namespace SeleniumDataBrowser.VCD.Helpers.UserInfoExtracting
{
    public class UserInfoExtracter
    {
        //in current implementation account inf like msId and vendor group is configurable values.
        //in future this data can be extracted from page usin selenium or from database
        public PageUserInfo ExtractUserInfo(AmazonVcdPageActions pageActions)
        {
            var userInfoJson = pageActions.GetUserInfoJson();
            return JsonConvert.DeserializeObject<PageUserInfo>(userInfoJson);
        }
    }
}
