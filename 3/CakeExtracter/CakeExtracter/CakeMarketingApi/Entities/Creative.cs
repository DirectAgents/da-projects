using System;
namespace CakeExtracter.CakeMarketingApi.Entities
{
    public class Creative
    {
        public int CreativeId { get; set; }
        public string CreativeName { get; set; }
        public CreativeType CreativeType { get; set; }
        public DateTime DateCreated { get; set; }
    }
}
