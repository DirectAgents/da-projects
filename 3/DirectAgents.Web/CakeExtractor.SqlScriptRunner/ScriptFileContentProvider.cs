using CakeExtracter;
using Microsoft.Practices.EnterpriseLibrary.Common.Utility;
using System;
using System.Configuration;
using System.IO;

namespace CakeExtractor.SqlScriptsExecutor
{
    public class ScriptFileContentProvider
    {
        private const string DefaultSqlFolderPath = ".SQL/";

        public string GetSqlScriptFileContent(string scriptRelativePath, string[] scriptParamsArray)
        {
            var scriptFullPath = $"{GetSqlScriptsFolderPath()}{scriptRelativePath}";
            var scriptFileContent = File.ReadAllText(scriptFullPath);
            Logger.Info($"Script full path: {scriptFullPath} was loaded");
            scriptFileContent = ReplaceTokensWithParamsValues(scriptFileContent, scriptParamsArray);
            return scriptFileContent;
        }

        private string GetSqlScriptsFolderPath()
        {
            try
            {
                return string.IsNullOrEmpty(ConfigurationManager.AppSettings[DefaultSqlFolderPath])
                ? ConfigurationManager.AppSettings[DefaultSqlFolderPath]
                : DefaultSqlFolderPath;
            }
            catch
            {
                return DefaultSqlFolderPath;
            }
        }

        private string ReplaceTokensWithParamsValues(string sqlContent, string[] scriptParamsArray)
        {
            const string parameterTokenPrefix = "@@param";
            for (int i = 0; i < scriptParamsArray.Length - 1; i++)
            {
                sqlContent = string.Join(scriptParamsArray[i], 
                    sqlContent.Split(new[] { $"{parameterTokenPrefix}{i}" }, StringSplitOptions.None));
            }
            return sqlContent;
        }
    }
}
