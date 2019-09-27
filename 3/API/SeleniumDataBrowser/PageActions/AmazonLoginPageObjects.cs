using OpenQA.Selenium;

namespace SeleniumDataBrowser.PageActions
{
    /// <summary>
    /// Class for elements of Amazon portal login pages (two-factor authentication).
    /// </summary>
    internal class AmazonLoginPageObjects
    {
        /// <summary>
        /// Field for login e-mail.
        /// </summary>
        public static By LoginEmailInput = By.Id("ap_email");

        /// <summary>
        /// Field for login password.
        /// </summary>
        public static By LoginPassInput = By.Id("ap_password");

        /// <summary>
        /// Field for OTP-code.
        /// </summary>
        public static By CodeInput = By.Id("auth-mfa-otpcode");

        /// <summary>
        /// Check box "Remember me".
        /// </summary>
        public static By RememberMeCheckBox = By.XPath(".//input[@name='rememberMe']");

        /// <summary>
        /// Link "Forgot your password?".
        /// </summary>
        public static By ForgotPassLink = By.Id("auth-fpp-link-bottom");

        /// <summary>
        /// Button for submit login.
        /// </summary>
        public static By LoginButton = By.Id("signInSubmit");

        /// <summary>
        /// Link "Your password is incorrect".
        /// </summary>
        public static By IncorrectPasswordSpan = By.XPath(".//span[contains(text(),'Your password is incorrect')]");

        /// <summary>
        /// Check box "Don't ask OTP-code".
        /// </summary>
        public static By DontAskCodeCheckBox = By.Id("auth-mfa-remember-device");

        /// <summary>
        /// Message box for warnings.
        /// </summary>
        public static By AuthWarningMessageBox = By.Id("auth-warning-message-box");
    }
}
