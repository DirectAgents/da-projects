using CakeExtractor.SeleniumApplication.Models.CommonHelperModels;
using CakeExtractor.SeleniumApplication.PageActions.Kopchava;
using CakeExtractor.SeleniumApplication.Properties;
using CakeExtractor.SeleniumApplication.SeleniumExtractors.Kochava;

namespace CakeExtractor.SeleniumApplication.Commands
{
    /// <summary>
    /// Command for cochava job.
    /// </summary>
    /// <seealso cref="CakeExtractor.SeleniumApplication.Commands.BaseSeleniumCommand" />
    internal class SyncKochavaDataCommand : BaseSeleniumCommand
    {
        private  AuthorizationModel authorizationModel;

        private  KochavaPageActions pageActions;

        private  KochavaExtractor extractor;

        /// <summary>
        /// Gets the name of the command.
        /// </summary>
        /// <value>
        /// The name of the command.
        /// </value>
        public override string CommandName => "SyncKochavaDataCommand";

        /// <summary>
        /// Do comamnds environment preparation steps.
        /// </summary>
        /// <param name="executionProfileNumber">The execution profile number.</param>
        public override void PrepareCommandEnvironment(int executionProfileNumber)
        {
            authorizationModel = InitializeAuthorizationModel();
            pageActions = new KochavaPageActions(authorizationModel);
            extractor = new KochavaExtractor(pageActions);
            extractor.PrepareExtractor();
        }

        /// <summary>
        /// Starts the command execution.
        /// </summary>
        public override void Run()
        {
        }

        private AuthorizationModel InitializeAuthorizationModel()
        {
            return new AuthorizationModel
            {
                Login = KochavaSettings.Default.UserLoginName,
                Password = KochavaSettings.Default.UserPassword,
                SignInUrl = KochavaSettings.Default.SignInUrl,
                CookiesDir = KochavaSettings.Default.CookiesDirectory
            };
        }
    }
}
