using CakeExtractor.SeleniumApplication.PageActions.Kopchava;

namespace CakeExtractor.SeleniumApplication.SeleniumExtractors.Kochava
{
    internal class KochavaExtractor
    {
        private KochavaPageActions pageActions;

        public KochavaExtractor(KochavaPageActions pageActions)
        {
            this.pageActions = pageActions;
        }

        public void PrepareExtractor()
        {
            pageActions.LoginToKochavaPortal();
        }
    }
}
