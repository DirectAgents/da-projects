namespace EomApp1.Formss.Synch.Models.Eom
{
    public partial class Advertiser
    {
        public Advertiser()
        {
        }

        public Advertiser(EomDatabaseEntities targetContext, string name)
        {
            this.name = name;

            using (var context = EomDatabaseEntities.Create())
            {
                context.Advertisers.AddObject(this);
                context.SaveChanges();
                context.Detach(this);
            }
            targetContext.Attach(this);
        }
    }
}
