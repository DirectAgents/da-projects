using CakeExtractor.SeleniumApplication.Configuration.Models;
using Newtonsoft.Json;
using System;
using System.IO;

namespace CakeExtractor.SeleniumApplication.Configuration.Vcd
{
    /// <summary>
    /// Profile management singletom object. Incapsulate logic about execution profile management and it's configuration.
    /// </summary>
    public class VcdExecutionProfileManger
    {
        private int? executionProfileNumber;

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
        /// Point for accesssing profile configuration.
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
        /// Define execution profile number. Extract configuration for execution profile.
        /// </summary>
        /// <param name="profileNumber"></param>
        public void SetExecutionProfileNumber(int profileNumber)
        {
            if (executionProfileNumber.HasValue)
            {
                throw new Exception("Execution profile number can be defined only one time");
            }
            else
            {
                executionProfileNumber = profileNumber;
                try
                {
                    profileConfiguration = ExtractExecutionProfileConfigureationByNumber(profileNumber);
                }
                catch (Exception ex)
                {
                    throw new Exception("Error occured while extracting profile number configuration. Check Execution profiles configs.", ex);
                }
                
            }
        }

        private VcdExecutionProfileConfiguration ExtractExecutionProfileConfigureationByNumber(int profileNumber)
        {
            var profileConfigFilePath = $"./VcdExecutionProfiles/Profile_{profileNumber}.json";
            var configFileContentJson = File.ReadAllText(profileConfigFilePath);
            var executionProfileConfiguration = JsonConvert.DeserializeObject<VcdExecutionProfileConfiguration>(configFileContentJson);
            return executionProfileConfiguration;
        }
    }
}
