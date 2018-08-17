using System;
using System.Diagnostics;
using System.IO;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Logging;
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Logging.TraceListeners;

namespace CakeExtracter.Logging.EnterpriseLibrary
{    
    /// <summary>
    /// Account file listener is responsible for updating logs by creating files
    /// with names based on account id and timestamp.
    /// </summary>
    [ConfigurationElementType(typeof(CustomTraceListenerData))]
    public class AccountFileListener : CustomTraceListener
    {
        private const string DateToken = "{Date}";
        private const string AccountIdToken = "{AccountId}";
        private const string FileNameAttribute = "filename";

        /// <summary>
        /// Override of Write. Gets the "fileNameTemplate" attribute and calls WriteToFile
        /// </summary>
        /// <param name="message"></param>
        public override void Write(string message)
        {
            WriteToFile(GetRootedFileName(Attributes[FileNameAttribute], string.Empty), message);
        }

        /// <summary>
        /// Override of WriteLine -> calls Write() 
        /// </summary>
        /// <param name="message"></param>
        public override void WriteLine(string message)
        {
            Write(message);
        }

        /// <summary>
        /// Entry point override of TraceData  -> calls Write()
        /// </summary>
        /// <param name="eventCache"></param>
        /// <param name="source"></param>
        /// <param name="eventType"></param>
        /// <param name="id"></param>
        /// <param name="data"></param>
        public override void TraceData(TraceEventCache eventCache, string source, TraceEventType eventType, int id, object data)
        {
            var logEntry = (LogEntry) data;
            var message = data.ToString();
            if (logEntry.ExtendedProperties.ContainsKey("AccountId"))
            {
                var accountId = logEntry.ExtendedProperties["AccountId"].ToString();
                WriteToFile(GetRootedFileName(Attributes[FileNameAttribute], accountId), message);
            }
            else
            {
                Write(message);
            }
        }

        /// <summary>
        /// Private method to write string to file.
        /// </summary>
        /// <param name="fileName">Template of file name.</param>
        /// <param name="message">Message to write.</param>
        private void WriteToFile(string fileName, string message)
        {
            FileStream logFileStream = new FileStream(fileName, FileMode.Append, FileAccess.Write, FileShare.ReadWrite);
            StreamWriter logWriter = new StreamWriter(logFileStream);
            logWriter.WriteLine(Environment.NewLine + message);
            logWriter.BaseStream.Seek(0, SeekOrigin.End);
            logWriter.Flush();
            logWriter.Close();
        }

        /// <summary>
        /// Private method to format the filename
        /// </summary>
        /// <param name="fileNameTemplate">Filename.</param>
        /// <param name="accountId">Account id if present.</param>
        /// <returns>string name of formatted file</returns>
        private string GetRootedFileName(string fileNameTemplate, string accountId = "")
        {
            string rootedFileName = fileNameTemplate;
            if (!Path.IsPathRooted(rootedFileName))
            {
                rootedFileName = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, rootedFileName);
            }

            DateTime today = DateTime.Today;
            string datePortion = $"{today.Year:0000}{today.Month:00}{today.Day:00}";
            rootedFileName = rootedFileName.Replace(DateToken, datePortion)
                                           .Replace(AccountIdToken, accountId);

            string directory = Path.GetDirectoryName(rootedFileName);
            if (directory.Length != 0 && !Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            return rootedFileName;
        }
    }
}
