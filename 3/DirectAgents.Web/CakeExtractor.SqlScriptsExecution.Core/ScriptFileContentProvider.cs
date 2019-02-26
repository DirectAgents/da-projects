using CakeExtracter;
using System;
using System.IO;

namespace CakeExtractor.SqlScriptsExecution.Core
{
    public class ScriptFileContentProvider
    {
        public string GetSqlScriptFileContent(string scriptFullPath, string[] scriptParamsArray)
        {
            var scriptFileContent = File.ReadAllText(scriptFullPath);
            Logger.Info($"Script full path: {scriptFullPath} was loaded");
            if (scriptParamsArray.Length > 0)
            {
                Logger.Info($"Started injecting params values to script content. Params count - {scriptParamsArray.Length}.");
                scriptFileContent = ReplaceTokensWithParamsValues(scriptFileContent, scriptParamsArray);
                Logger.Info("Params injected");
            }
            return scriptFileContent;
        }

        private string ReplaceTokensWithParamsValues(string sqlContent, string[] scriptParamsArray)
        {
            const string parameterTokenPrefix = "@@param_";
            for (int i = 0; i < scriptParamsArray.Length - 1; i++)
            {
                sqlContent = string.Join(scriptParamsArray[i], 
                    sqlContent.Split(new[] { $"{parameterTokenPrefix}{i}" }, StringSplitOptions.None));
            }
            return sqlContent;
        }
    }
}
