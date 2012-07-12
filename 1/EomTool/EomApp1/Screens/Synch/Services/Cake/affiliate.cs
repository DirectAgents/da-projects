namespace EomApp1.Cake.WebServices._4.Export
{
    public partial class affiliate
    {
        public string AccountManagerName
        {
            get
            {
                return this.account_managers[0].contact_name;
            }
        }

        public string Currency
        {
            get
            {
                return this.currency_settings.currency.currency_abbr;
            }
        }
    }
}