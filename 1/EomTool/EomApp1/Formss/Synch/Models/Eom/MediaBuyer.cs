namespace EomApp1.Formss.Synch.Models.Eom
{
    public partial class MediaBuyer
    {
        public MediaBuyer()
        {
        }

        public MediaBuyer(EomDatabaseEntities targetContext, string name)
        {
            this.name = name;

            using (var context = EomDatabaseEntities.Create())
            {
                context.MediaBuyers.AddObject(this);
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
