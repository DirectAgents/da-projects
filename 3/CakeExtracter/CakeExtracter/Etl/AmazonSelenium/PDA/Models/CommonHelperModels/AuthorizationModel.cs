using System.Collections.Generic;
using CakeExtracter.Etl.AmazonSelenium.Helpers;
using OpenQA.Selenium;

namespace CakeExtracter.Etl.AmazonSelenium.PDA.Models.CommonHelperModels
{
    public class AuthorizationModel
    {
        private string _cookiesDir;

        public string Login { get; set; }

        public string Password { get; set; }

        public string CookiesDir
        {
            get => FileManager.GetAssemblyRelativePath(_cookiesDir);
            set
            {
                _cookiesDir = FileManager.GetAssemblyRelativePath(value);
                FileManager.CreateDirectoryIfNotExist(_cookiesDir);
            }
        }

        public IEnumerable<Cookie> Cookies { get; set; }
    }
}
