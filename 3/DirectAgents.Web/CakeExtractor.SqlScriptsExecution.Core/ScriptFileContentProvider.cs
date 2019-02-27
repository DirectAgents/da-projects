using System;
using System.IO;

namespace CakeExtractor.SqlScriptsExecution.Core
{
    /// <summary>
    /// Extract sql script content. Replace params values in sql script.
    /// </summary>
    public class ScriptFileContentProvider
    {
        /// <summary>
        /// Gets the content of the SQL script file.
        /// </summary>
        /// <param name="scriptPath">The script path.</param>
        /// <param name="scriptParamsArray">The script parameters array.</param>
        /// <returns>Sql script text file content.</returns>
        public string GetSqlScriptFileContent(string scriptPath, string[] scriptParamsArray)
        {
            var scriptFileContent = File.ReadAllText(scriptPath);
            if (scriptParamsArray.Length > 0)
            {
                scriptFileContent = ReplaceTokensWithParamsValues(scriptFileContent, scriptParamsArray);
            }
            return scriptFileContent;
        }

        /// <summary>
        /// Replaces the tokens with parameters values.
        /// </summary>
        /// <param name="sqlContent">Content of the SQL.</param>
        /// <param name="scriptParamsArray">The script parameters array.</param>
        /// <returns>Script file content with params tokens replaced with params values.</returns>
        private string ReplaceTokensWithParamsValues(string sqlContent, string[] scriptParamsArray)
        {
            const string parameterTokenPrefix = "@@param_";
            for (int i = 0; i <= scriptParamsArray.Length - 1; i++)
            {
                sqlContent = string.Join(scriptParamsArray[i], 
                    sqlContent.Split(new[] { $"{parameterTokenPrefix}{i}" }, StringSplitOptions.None));
            }
            return sqlContent;
        }
    }
}
