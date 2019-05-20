using System;

namespace Adform.Entities.RequestEntities
{
    internal class PollingOperationResponse
    {
        public string Id { get; set; }
        public string Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime LastActionAt { get; set; }
        public string Location { get; set; }
    }
}
