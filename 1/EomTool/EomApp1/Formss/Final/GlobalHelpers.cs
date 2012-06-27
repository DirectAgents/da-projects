using System;
using System.Collections.Generic;

namespace EomApp1.Formss.Final
{
    internal static class GlobalHelpers
    {
        private static readonly object _lock = new Object();
        private static readonly Queue<string> _postSubmitQueue = new Queue<string>();

        public static void AddPostSubmitSql(string sql)
        {
            lock (_lock)
            {
                _postSubmitQueue.Enqueue(sql);
            }
        }

        public static void ExecutePostSubmitSql()
        {
            lock (_lock)
            {
                while (_postSubmitQueue.Count > 0)
                {
                    string sqlString = _postSubmitQueue.Dequeue();
                    var db = new FinalizeDataDataContext(true);
                    db.ExecuteCommand(sqlString);
                }
            }
        }
    }
}