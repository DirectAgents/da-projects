namespace EomApp1.Cake.WebServices._4.Export
{
    public partial class advertiser1
    {
        public string AccountManagerName
        {
            get
            {
                return this.account_managers[0].contact_name;
            }
        }

        public string AdManagerName
        {
            get
            {
                if (this.tags.Length > 0)
                {
                    return this.tags[0].tag_name;
                }
                else
                {
                    return "default";
                }
            }
        }
    }
}
