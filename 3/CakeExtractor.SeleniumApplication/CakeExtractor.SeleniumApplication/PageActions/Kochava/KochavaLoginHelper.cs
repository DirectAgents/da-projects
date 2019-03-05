using CakeExtractor.SeleniumApplication.Models.CommonHelperModels;
using CakeExtractor.SeleniumApplication.PageActions.Kochava;

namespace CakeExtractor.SeleniumApplication.PageActions.Kopchava
{
    internal class KochavaLoginHelper
    {
        private readonly AuthorizationModel authorizationModel;

        private readonly KochavaPageActions pageManager;

        public KochavaLoginHelper(AuthorizationModel authorizationModel, KochavaPageActions pageManager)
        {
            this.pageManager = pageManager;
            this.authorizationModel = authorizationModel;
        }

        public void LoginToKochavaPortal()
        {
            NavigateToLoginPage();
        }

        private void NavigateToLoginPage()
        {
            pageManager.NavigateToUrl(authorizationModel.SignInUrl);
            SetLogin();
            SetPassword();
            SetStaySignedInCheckBox();
            ClickLoginButton();
        }

        private void SetLogin()
        {
            pageManager.ClickElement(KochavaPageObjects.LoginUserNameInput);
            pageManager.SendKeys(KochavaPageObjects.LoginUserNameInput, authorizationModel.Login);
        }

        private void SetPassword()
        {
            pageManager.ClickElement(KochavaPageObjects.LoginPassInput);
            pageManager.SendKeys(KochavaPageObjects.LoginPassInput, authorizationModel.Password);
        }

        private void SetStaySignedInCheckBox()
        {
            pageManager.ClickElement(KochavaPageObjects.StaySignedInCheckbox);
        }

        private void ClickLoginButton()
        {
            pageManager.ClickElement(KochavaPageObjects.LoginButton);
        }
    }
}
