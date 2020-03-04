using System;
using System.ComponentModel.Composition;
using System.Configuration;

using CakeExtracter.Common;
using SeleniumDataBrowser.Helpers;

namespace CakeExtracter.Commands.DPA
{
    /// <summary>
    /// The class represent command to distribute data form OrganicNewKeywords table.
    /// </summary>
    [Export(typeof(ConsoleCommand))]
    public class DPAProcessOrganicKeywordsData : ConsoleCommand
    {
        private const string DaDataConnectionStringName = "DAData";

        private const string DPAScriptPathConfigurationKey = "DPA_ScriptPath";

        private readonly SqlScriptsExecutor sqlScriptsExecutor;

        private string scriptName = string.Empty;

        /// <inheritdoc cref="ConsoleCommand" />
        /// <summary>
        /// Initializes a new instance of the <see cref="DPAProcessOrganicKeywordsData" /> class.
        /// </summary>
        public DPAProcessOrganicKeywordsData()
        {
            IsCommand("dpaProcessOrganicKeywordsData", "Distribute OrganicKeywords data using sql scripts.");
            HasRequiredOption("s|script=", "Script name to execute", c => scriptName = c);
            sqlScriptsExecutor = new SqlScriptsExecutor(DaDataConnectionStringName);
        }

        /// <summary>
        /// Gets a relative path to sql script to process data.
        /// </summary>
        public string SqlScriptRelativePath
        {
            get
            {
                var path = ConfigurationManager.AppSettings[DPAScriptPathConfigurationKey];
                var name = ConfigurationManager.AppSettings[scriptName];
                return PathToFileDirectoryHelper.GetAssemblyRelativePath(PathToFileDirectoryHelper.CombinePath(path, name));
            }
        }

        /// <inheritdoc/>
        public override int Execute(string[] remainingArguments)
        {
            Logger.Info($"DPAProcessOrganicKeywordsData start execute a {scriptName} sql script at {DateTime.Now}");
            try
            {
                sqlScriptsExecutor.ExecuteScriptWithParams(SqlScriptRelativePath, Array.Empty<string>());
            }
            catch (Exception e)
            {
                Logger.Error(e);
            }

            Logger.Info($"DPAProcessOrganicKeywordsData finished execute a {scriptName} sql script at {DateTime.Now}");
            return 0;
        }
    }
}