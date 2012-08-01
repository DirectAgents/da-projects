namespace EomApp1.Screens.Final.Models
{
    using UI;

    public class CampaignsBase
    {
        private Data data;
        protected Data Data { get { return this.data; } }

        protected CampaignsBase(Data data)
        {
            this.data = data;
        }
    }
}
