using System.Web.Configuration;

namespace LTWeb
{
    /// <summary>
    /// Provide strongly typed access to items from appSettings section of Web.config 
    /// </summary>
    public static class AppSettings
    {
        public static string LendingTreeServiceConfig { get { return GetString("LendingTreeServiceConfig"); } }

        public static string VisitorUrl { get { return GetString("BaseUrl"); } }

        public static int SsnRequiredModeValue { get { return GetInt("Mode_SsnRequired"); } }

        public static int SsnNotRequiredModeValue { get { return GetInt("Mode_SsnNotRequired"); } }

        #region Private Helpers

        static int GetInt(string name)
        {
            return int.Parse(GetAppSetting(name));
        }

        static string GetString(string name)
        {
            return GetAppSetting(name);
        }

        static string GetAppSetting(string name)
        {
            return WebConfigurationManager.AppSettings[name];
        } 

        #endregion
    }
}