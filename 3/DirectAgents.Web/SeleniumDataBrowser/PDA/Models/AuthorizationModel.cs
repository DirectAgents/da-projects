﻿using System.Collections.Generic;
using SeleniumDataBrowser.Helpers;
using OpenQA.Selenium;

namespace SeleniumDataBrowser.PDA.Models
{
    /// <summary>
    /// Class for settings of authorization on the Amazon Advertising Portal.
    /// </summary>
    public class AuthorizationModel
    {
        private string cookiesDir;

        /// <summary>
        /// Gets or sets the e-mail login.
        /// </summary>
        public string Login { get; set; }

        /// <summary>
        /// Gets or sets the password for the login.
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// Gets or sets browser cookies after successful authorization on the Portal.
        /// </summary>
        public IEnumerable<Cookie> Cookies { get; set; }

        /// <summary>
        /// Gets or sets the name of the directory where cookies are stored.
        /// </summary>
        public string CookiesDir
        {
            get => FileManager.GetAssemblyRelativePath(cookiesDir);
            set
            {
                cookiesDir = FileManager.GetAssemblyRelativePath(value);
                FileManager.CreateDirectoryIfNotExist(cookiesDir);
            }
        }
    }
}
