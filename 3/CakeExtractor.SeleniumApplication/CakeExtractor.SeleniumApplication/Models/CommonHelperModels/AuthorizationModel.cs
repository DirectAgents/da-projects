using System.Collections.Generic;
using OpenQA.Selenium;

namespace CakeExtractor.SeleniumApplication.Models.CommonHelperModels
{
    internal class AuthorizationModel
    {
        public string Login { get; set; }
        public string Password { get; set; }
        public string SignInUrl { get; set; }
        public string CookiesDir { get; set; }
        public IEnumerable<Cookie> Cookies { get; set; }
    }
}
