namespace Huggies.Web.Models
{
    public class Repository : IRepository
    {
        private readonly Context context;

        public Repository(Context context)
        {
            this.context = context;
        }

        public void Save(Lead lead)
        {
            this.context.Leads.Add(lead);
        }
    }
}