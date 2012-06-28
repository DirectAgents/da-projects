using System;

namespace EomApp1.Formss.Final
{
    public partial class FinalizeDataDataContext
    {
        public FinalizeDataDataContext(bool b) :
            base(EomAppCommon.Settings.ConnStr, mappingSource)
        {
            OnCreated();
        }

        partial void OnCreated()
        {
            if (Connection.ConnectionString != EomAppCommon.Settings.ConnStr)
            {
                throw new Exception("Connection String Error");
            }
        }

        public new void SubmitChanges()
        {
            base.SubmitChanges();
            GlobalHelpers.ExecutePostSubmitSql();
        }
    }
}