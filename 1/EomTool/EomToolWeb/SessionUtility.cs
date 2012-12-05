using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Web;

namespace EomToolWeb
{
    public class SessionUtility
    {
        // TODO: Reset

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

        private static void Clear(string key)
        {
            Session[key] = null;
        }

        private static System.Web.SessionState.HttpSessionState Session
        {
            get { return HttpContext.Current.Session; }
        }
    }

    public class WikiSettings
    {
        [DisplayName("Show \"NOT LIVE YET\"")]
        public bool ShowNotLiveYet { get; set; }

        [DisplayName("Show \"PAUSED\"")]
        public bool ShowPaused { get; set; }

        //[DisplayName("Show Hidden")]
        //public bool ShowHidden { get; set; }

        //[DisplayName("Show Inactive")]
        //public bool ShowInactive;

        public List<string> ExcludeStrings()
        {
            List<String> excludeStrings = new List<string>();
            if (!ShowNotLiveYet)
                excludeStrings.Add("NOT LIVE YET");
            if (!ShowPaused)
                excludeStrings.Add("PAUSED");

            return excludeStrings;
        }
    }
}