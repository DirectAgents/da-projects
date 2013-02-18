using System.Collections.Generic;
using System.Linq;
using LTWeb.Common;
using LTWeb.DataAccess;
using LTWeb.Service;

namespace LTWeb
{
    /// <summary>
    /// This static utility class manages session state.
    /// 
    /// First call <see cref="Reset"/>.
    /// 
    /// Then each of the accessors gets or creates the associated session scoped data value.
    /// 
    /// <see cref="SessionUtility"/> is used to encapsulate low level interation with the session.
    /// </summary>
    public static class Session
    {
        public static void Reset()
        {
            SessionUility.Clear(SessionKeys.SessionVars);
            SessionUility.Clear(SessionKeys.LTModel);
            SessionUility.Clear(SessionKeys.Admin);
        }

        /// <summary>
        /// General abstraction for miscallaneous session variables.
        /// </summary>
        public static SessionVars SessionVars
        {
            get
            {
                return SessionUility.GetOrCreate<SessionVars>(SessionKeys.SessionVars, () => 
                    new SessionVars());
            }
        }

        /// <summary>
        /// All the information which relates to a single loan application (i.e. lead we send to Lending Tree)
        /// </summary>
        public static ILendingTreeModel LTModel
        {
            get
            {
                return SessionUility.GetOrCreate<ILendingTreeModel>(SessionKeys.LTModel, () => 
                    new LendingTreeModel(AppSettings.LendingTreeServiceConfig));
            }
        }

        /// <summary>
        /// Key/Value pairs which are editable in the admin view.
        /// </summary>
        public static Dictionary<string, string> Admin
        {
            get
            {
                return SessionUility.GetOrCreate<Dictionary<string, string>>(SessionKeys.Admin, () =>
                {
                    using (var db = new LTWebDataContext())
                    {
                        return db.AdminSettings.ToDictionary(c => c.Name, c => c.Value);
                    }
                });
            }
        }
    }

    public class SessionVars
    {
        public int HighestQuestionIndexDisplayed = 0;
    }
}