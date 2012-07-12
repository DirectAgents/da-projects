namespace EomApp1.Screens.Synch.Models.Eom
{
    public partial class AdManager
    {
        public AdManager()
        {
        }

        public AdManager(EomDatabaseEntities targetContext, string name)
        {
            this.name = name;

            using (var context = EomDatabaseEntities.Create())
            {
                context.AdManagers.AddObject(this);
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
