using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EomAppCommon
{
    public static class EomAppSettings
    {
        public static string ConnStr { get; set; }
        public static string MasterDatabaseListConnectionString { get; set; }
        public static bool DebugEomDatabase { get; set; }
    }
}
