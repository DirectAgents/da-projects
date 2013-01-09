using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Web;

namespace EomToolWeb
{
    public class SessionUtility
    {
        public static WikiSettings WikiSettings
        {
            get
            {
                return GetOrCreate<WikiSettings>("WikiSettings", () => new WikiSettings());
            }
        }

        private static T GetOrCreate<T>(string key, Func<T> create = null) where T : class
        {
            T result = Session[key] as T;
            if (result == null)
            {
                if (create != null)
                {
                    result = create();
                    Session[key] = result;
                }
                else
                    throw new Exception(string.Format("Session does not contain an object of type {0} with key {1} and no create function was supplied.",
                                                      typeof(T).Name, key));
            }
            return result;
        }

        private static System.Web.SessionState.HttpSessionState Session
        {
            get { return HttpContext.Current.Session; }
        }
    }

    public class WikiSettings
    {
        [DisplayName("Exclude \"NOT LIVE YET\"")]
        public bool ExcludeNotLiveYet { get; set; }

        [DisplayName("Exclude \"PAUSED\"")]
        public bool ExcludePaused { get; set; }

        [DisplayName("Exclude Hidden (pink)")]
        public bool ExcludeHidden { get; set; }

        [DisplayName("Exclude Inactive")]
        public bool ExcludeInactive { get; set; }

        [DisplayName("Exclude CPM (light blue)")]
        public bool ExcludeCPM { get; set; }

        public WikiSettings()
        {
            ExcludeNotLiveYet = true;
            ExcludePaused = true;
            ExcludeHidden = false;
            ExcludeInactive = true;
            ExcludeCPM = true;
        }

        public List<string> ExcludeStrings()
        {
            List<String> excludeStrings = new List<string>();
            if (ExcludeNotLiveYet)
                excludeStrings.Add("NOT LIVE YET");
            if (ExcludePaused)
                excludeStrings.Add("PAUSED");
            if (ExcludeCPM)
                excludeStrings.Add("CPM");

            return excludeStrings;
        }
    }
}