using System;
using System.IO;

namespace DAgents.Common
{
    public class SqlBatch
    {
        /// <summary>
        /// Creates a SqlBatch object that uses the specified connection string to execute
        /// SQL statements, looks for files in the current direcotry, and uses the statement delimeter "GO".
        /// </summary>
        /// <param name="connectionString">SQL connection string to use for query commands</param>
        public SqlBatch(string connectionString)
            : this(connectionString, "GO")
        { }


        /// <summary>
        /// Creates a SqlBatch object that uses the specified connection string to execute
        /// SQL statements, looks for files in the current direcotry, and the specified statement 
        /// delimeter to parse separate queries from from the files.
        /// </summary>
        /// <param name="connectionString">SQL connection string to use for query commands</param>
        /// <param name="separator">SQL statement delimiter</param>
        public SqlBatch(string connectionString, string separator)
            : this(connectionString, separator, ".")
        { }

        /// <summary>
        /// Creates a SqlBatch object.
        /// </summary>
        /// <param name="connectionString">SQL connection string to use for query commands</param>
        /// <param name="separator">SQL statement delimiter</param>
        /// <param name="path">Path of directory where files are located</param>
        public SqlBatch(string connectionString, string separator, string path)
        {
            ConnectionString = connectionString;
            BatchSeparator = separator;
            Path = path;
        }

        /// <summary>
        /// Executes the contents of files with .sql extension.
        /// Does not recurse into subdirectories.
        /// </summary>
        /// <param name="path"></param>
        public void ExecuteDirectory(string path)
        {
            ExecuteDirectory(path, "*.sql");
        }

        /// <summary>
        /// Executes the contents of files.
        /// Does not recurse into subdirectories.
        /// </summary>
        /// <param name="path">Path of the directory</param>
        /// <param name="searchPattern">Glob style matching pattern to choose files</param>
        public void ExecuteDirectory(string path, string searchPattern)
        {
            ExecuteDirectory(path, searchPattern, false);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="path"></param>
        /// <param name="pattern"></param>
        /// <param name="recurse"></param>
        /// <returns></returns>
        public bool ExecuteDirectory(string path, string pattern, bool recurse)
        {
            var dir = new DirectoryInfo(FullPath(path));
            return ExecuteDirectory(dir, "*.sql", false);
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="di"></param>
        /// <param name="searchPattern"></param>
        /// <param name="recurse"></param>
        /// <returns></returns>
        public bool ExecuteDirectory(DirectoryInfo di, string searchPattern, bool recurse)
        {
            bool res = true;

            SearchOption searchOption = recurse
                ? SearchOption.AllDirectories
                : SearchOption.TopDirectoryOnly;

            foreach (var fileInfo in di.EnumerateFiles(searchPattern, searchOption))
            {
                try
                {
                    SqlBatchUtil.ExecuteFile(ConnectionString, fileInfo.FullName, BatchSeparator);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    res = false;
                }
            }

            return res;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public bool ExecuteFile(string path)
        {
            bool res = true;

            try
            {
                SqlBatchUtil.ExecuteFile(ConnectionString, path);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                res = false;
            }

            return res;
        }

        /// <summary>
        /// 
        /// </summary>
        public string ConnectionString { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string BatchSeparator { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Path
        {
            get
            {
                string path = _path;
                if (!path.EndsWith("\\")) path += "\\";
                return path;
            }
            set { _path = value; }
        }
        private string _path;

        private string FullPath(string path) { return this.Path + path; }
    }
}
