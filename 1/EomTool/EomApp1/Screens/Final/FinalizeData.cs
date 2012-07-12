using System;

namespace EomApp1.Screens.Final
{
    public partial class FinalizeDataDataContext
    {
        public FinalizeDataDataContext(bool b) :
            base(EomAppCommon.EomAppSettings.ConnStr, mappingSource)
        {
            OnCreated();
        }

        partial void OnCreated()
        {
            if (Connection.ConnectionString != EomAppCommon.EomAppSettings.ConnStr)
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