using System.Collections.Generic;
using SeleniumDataBrowser.Helpers;
using OpenQA.Selenium;

namespace SeleniumDataBrowser.PDA.Models
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
