using CakeExtractor.SeleniumApplication.Configuration.Models;
using Newtonsoft.Json;
using System;
using System.IO;

namespace CakeExtractor.SeleniumApplication.Configuration.Vcd
{
    /// <summary>
    /// Profile management singleton object. Encapsulate logic about execution profile management and it's configuration.
    /// </summary>
    public class VcdExecutionProfileManger
    {
        private int executionProfileNumber;

        private VcdExecutionProfileConfiguration profileConfiguration;

        /// <summary>
        /// Hidden singleton constructor.
        /// </summary>
        private VcdExecutionProfileManger()
        {
        }

        /// <summary>
        /// Access point for singleton object.
        /// </summary>
        public static VcdExecutionProfileManger Current = new VcdExecutionProfileManger();

        /// <summary>
        /// Point for accessing profile configuration.
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
        /// <param name="profileNumber"></param>
        public void SetExecutionProfileNumber(int? profileNumber)
        {
            if (!profileNumber.HasValue)
            {
                const int defaultExecutionProfileNumber = 1;
                CakeExtracter.Logger.Warn(
                    $"Execution profile number not specified or specified incorrectly. {defaultExecutionProfileNumber} will be used as profile number");
                profileNumber = defaultExecutionProfileNumber;
            }

            executionProfileNumber = (int) profileNumber;
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
            var profileConfigFilePath = $"./VcdExecutionProfiles/Profile_{profileNumber}.json";
            var configFileContentJson = File.ReadAllText(profileConfigFilePath);
            var executionProfileConfiguration = JsonConvert.DeserializeObject<VcdExecutionProfileConfiguration>(configFileContentJson);
            return executionProfileConfiguration;
        }
    }
}
