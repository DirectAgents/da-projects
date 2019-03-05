using CakeExtractor.SeleniumApplication.Drivers;
using CakeExtractor.SeleniumApplication.Models.CommonHelperModels;

namespace CakeExtractor.SeleniumApplication.PageActions.Kopchava
{
    internal class KochavaPageActions : BasePageActions
    {
        private readonly KochavaLoginHelper loginHelper;

        public KochavaPageActions(AuthorizationModel authorizationModel)
            : base(new ChromeWebDriver(string.Empty), Properties.Settings.Default.WaitPageTimeoutInMinuts)
        {
            loginHelper = new KochavaLoginHelper(authorizationModel, this);
        }

        public void LoginToKochavaPortal()
        {
            loginHelper.LoginToKochavaPortal();
        }

        public void NavigateToAccountDataPage(string accountId)
        {
        }
    }
}
