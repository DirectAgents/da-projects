using CakeExtractor.SeleniumApplication.PageActions.AmazonVcd;
using CakeExtractor.SeleniumApplication.SeleniumExtractors.VCD.VcdExtractionHelpers.UserInfoExtracting.Models;
using Newtonsoft.Json;

namespace CakeExtractor.SeleniumApplication.SeleniumExtractors.VCD.VcdExtractionHelpers.UserInfoExtracting
{
    internal class UserInfoExtracter
    {
        //in current implementation account inf like msId and vendor group is configurable values.
        //in future this data can be extracted from page usin selenium or from database
        public UserInfo ExtractUserInfo(AmazonVcdPageActions pageActions)
        {
            var userInfoJson = pageActions.GetUserInfoJson();
            return JsonConvert.DeserializeObject<UserInfo>(userInfoJson);
        }
    }
}
