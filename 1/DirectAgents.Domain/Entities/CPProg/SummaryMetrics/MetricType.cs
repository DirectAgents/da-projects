namespace DirectAgents.Domain.Entities.CPProg
{
    public class MetricType
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int? DaysInterval { get; set; }

        public MetricType()
        {
        }

        public MetricType(string name, int? daysInterval)
        {
            Name = name;
            DaysInterval = daysInterval;
        }

        public bool Equals(MetricType metricType)
        {
            return Name.Equals(metricType.Name) && DaysInterval == metricType.DaysInterval;
        }
    }
}