﻿using System.ComponentModel.DataAnnotations.Schema;

namespace ClientPortal.Data.Contexts
{
    public partial class SearchAccount
    {
        public const string GoogleChannel = "Google";
        public const string BingChannel = "Bing";
        public const string CriteoChannel = "Criteo";

        [NotMapped]
        public bool UseConvertedClicks { get; set; }
    }
}
