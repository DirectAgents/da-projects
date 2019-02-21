﻿using OpenQA.Selenium;

namespace CakeExtractor.SeleniumApplication.PageActions.AmazonVcd
{
    internal class AmazonVcdPageObjects
    {
        public static By AccountsDropdownButton = By.CssSelector("a.i90-accounts-dropdown-button");
        public static By AccountsDropdownContainer = By.CssSelector("div.i90-accounts-dropdown-content");
        public static By AccountsDropdownItem = By.CssSelector("div.i90-accounts-dropdown-item");
    }
}
