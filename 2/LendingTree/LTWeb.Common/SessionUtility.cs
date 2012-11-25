using System;
using System.Web;

namespace LTWeb.Common
{
    public static class SessionUility
    {
        public static T GetOrCreate<T>(string key, Func<T> create = null) where T : class
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

        public static void Clear(string key)
        {
            Session[key] = null;
        }

        static System.Web.SessionState.HttpSessionState Session
        {
            get { return HttpContext.Current.Session; }
        }
    }
}
