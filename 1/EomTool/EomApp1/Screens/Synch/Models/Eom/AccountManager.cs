namespace EomApp1.Screens.Synch.Models.Eom
{
    public partial class AccountManager
    {
        public AccountManager()
        {
        }

        public AccountManager(EomDatabaseEntities targetContext, string name)
        {
            this.name = name;

            using (var context = EomDatabaseEntities.Create())
            {
                context.AccountManagers.AddObject(this);
                context.SaveChanges();
                context.Detach(this);
            }
            targetContext.Attach(this);
        }

        public bool NameIsEquivalentTo(string name)
        {
            return (NameHelper.NormalizeName(this.name) == NameHelper.NormalizeName(name));
        }
    }
}
