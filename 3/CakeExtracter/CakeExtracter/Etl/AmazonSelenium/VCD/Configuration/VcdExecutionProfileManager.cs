using System;
using System.IO;
using CakeExtracter.Etl.AmazonSelenium.VCD.Configuration.Models;
using Newtonsoft.Json;
using SeleniumDataBrowser.Helpers;

namespace CakeExtracter.Etl.AmazonSelenium.VCD.Configuration
{
    /// <summary>
    /// Profile management singleton object. Encapsulate logic about execution profile management and it's configuration.
    /// </summary>
    public class VcdExecutionProfileManager
    {
        /// <summary>
        /// Access point for singleton object.
        /// </summary>
        public static VcdExecutionProfileManager Current = new VcdExecutionProfileManager();

        private int executionProfileNumber;

        private VcdExecutionProfileConfiguration profileConfiguration;

        /// <summary>
        /// Initializes a new instance of the <see cref="VcdExecutionProfileManager"/> class.
        /// Hidden singleton constructor.
        /// </summary>
        private VcdExecutionProfileManager()
        {
        }

        /// <summary>
        /// Gets a point for accessing profile configuration.
        /// </summary>
        public VcdExecutionProfileConfiguration ProfileConfiguration
        {
            get
            {
                if (profileConfiguration == null)
                {
                    throw new Exception("Execution profile is not configured. SetExecutionProfileNumber should be called first");
                }
                return profileConfiguration;
            }
        }

        /// <summary>
        /// Defines execution profile number. Extracts configuration for execution profile.
        /// </summary>
        /// <param name="profileNumber">Number of profile.</param>
        public void SetExecutionProfileNumber(int? profileNumber)
        {
            if (!profileNumber.HasValue)
            {
                const int defaultExecutionProfileNumber = 1;
                Logger.Warn($"Execution profile number not specified or specified incorrectly. {defaultExecutionProfileNumber} will be used as profile number");
                profileNumber = defaultExecutionProfileNumber;
            }

            executionProfileNumber = (int)profileNumber;
            try
            {
                profileConfiguration = ExtractExecutionProfileConfigurationByNumber(executionProfileNumber);
            }
            catch (Exception ex)
            {
                throw new Exception("Error occured while extracting profile number configuration. Check Execution profiles configs.", ex);
            }
        }

        private VcdExecutionProfileConfiguration ExtractExecutionProfileConfigurationByNumber(int profileNumber)
        {
            var relativePathToProfileFile = $"VcdExecutionProfiles\\Profile_{profileNumber}.json";
            var fullPathToProfileFile = PathToFileDirectoryHelper.GetAssemblyRelativePath(relativePathToProfileFile);
            var profileFileContentJson = File.ReadAllText(fullPathToProfileFile);
            var executionProfileConfiguration = JsonConvert.DeserializeObject<VcdExecutionProfileConfiguration>(profileFileContentJson);
            return executionProfileConfiguration;
        }
    }
}