using FacebookAPI.Entities.AdDataEntities;

namespace FacebookAPI.Entities
{
    public class AdData
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string Status { get; set; }

        public Creative Creative { get; set; }

        public AdSet AdSet { get; set; }

        public Campaign Campaign { get; set; }
    }
}
