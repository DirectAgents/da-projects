using System.Data.Objects;

namespace AccountingBackupWeb.Models
{
    public static class ObjectContextExtensions
    {
        public static void ResetDatabase(this ObjectContext objectContext)
        {
            if (objectContext.DatabaseExists())
            {
                objectContext.DeleteDatabase();
            }
            objectContext.CreateDatabase();
        }
    }
}
