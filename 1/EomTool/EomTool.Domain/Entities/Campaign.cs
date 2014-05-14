using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace EomTool.Domain.Entities
{
    public partial class Campaign
    {
        [NotMapped]
        public string DisplayName
        {
            get { return (String.IsNullOrWhiteSpace(display_name) ? campaign_name : display_name); }
        }
    }
}
