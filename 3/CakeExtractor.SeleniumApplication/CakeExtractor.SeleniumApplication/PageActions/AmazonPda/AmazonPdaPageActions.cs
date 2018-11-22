using System;
using OpenQA.Selenium;

namespace CakeExtractor.SeleniumApplication.PageActions.AmazonPda
{
    internal class AmazonPdaPageActions : BasePageActions
    {
        private readonly TimeSpan timeout;

        public AmazonPdaPageActions(IWebDriver driver, int timeoutMinuts) : base(driver)
        {
            timeout = TimeSpan.FromMinutes(timeoutMinuts);
        }

        public void NavigateToAmazonAdvertising(string url)
        {
            GoToUrl(url);      
            WaitElement(AmazonPdaPageObjects.ForgotPassLink, timeout);
        }

        public void LoginProcess(string email, string password)
        {            
            ClickElement(AmazonPdaPageObjects.LoginEmailInput);
            SendKeys(AmazonPdaPageObjects.LoginEmailInput, email);
            ClickElement(AmazonPdaPageObjects.LoginPassInput);
            SendKeys(AmazonPdaPageObjects.LoginPassInput, password);
            ClickElement(AmazonPdaPageObjects.LoginButton);            
            WaitElement(AmazonPdaPageObjects.CodeInput, timeout);
            WaitElement(AmazonPdaPageObjects.AccountButton, timeout);
        }

        public void NavigateToCampaigns(string url)
        {
            Driver.Navigate().GoToUrl(url);
            WaitElement(AmazonPdaPageObjects.FilterByButton, timeout);
        }

        public void SetFiltersOnCampaigns()
        {
            ClickElement(AmazonPdaPageObjects.FilterByButton);            
            WaitElement(AmazonPdaPageObjects.FilterTypeButton, timeout);
            ClickElement(AmazonPdaPageObjects.FilterTypeButton);
            WaitElement(AmazonPdaPageObjects.FilterByValues, timeout);
            ClickElement(AmazonPdaPageObjects.FilterByValues);            
            WaitElement(AmazonPdaPageObjects.FilterPdaValues, timeout);
            ClickElement(AmazonPdaPageObjects.FilterPdaValues);
            ClickElement(AmazonPdaPageObjects.SaveSearchAndFilterButton);
            WaitElement(AmazonPdaPageObjects.ChartContainer, timeout);
        }
        
        public void ExportCsv()
        {
            WaitElement(AmazonPdaPageObjects.ExportButton, timeout);
            ClickElement(AmazonPdaPageObjects.ExportButton);
        }
    }
}
