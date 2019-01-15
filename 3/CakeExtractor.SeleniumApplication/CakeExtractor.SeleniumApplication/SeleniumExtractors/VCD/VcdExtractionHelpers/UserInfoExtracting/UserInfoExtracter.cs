using CakeExtractor.SeleniumApplication.PageActions.AmazonVcd;
using CakeExtractor.SeleniumApplication.SeleniumExtractors.VCD.VcdExtractionHelpers.UserInfoExtracting.Models;
using Newtonsoft.Json;

namespace CakeExtractor.SeleniumApplication.SeleniumExtractors.VCD.VcdExtractionHelpers.UserInfoExtracting
{
    internal class UserInfoExtracter
    {
        public static UserInfo ExtractUserInfo(AmazonVcdPageActions pageActions)
        {
            var userInfoJson = pageActions.GetUserInfoJson();
            return JsonConvert.DeserializeObject<UserInfo>(userInfoJson);
        }
    }
}
