using System.Linq;
using System.Data.Objects;

namespace EomApp1.Screens.Synch.Models.Eom
{
    public partial class TrackingSystem
    {
    }

    public static class TrackingSystemExtensions
    {
        public static TrackingSystem Cake(this ObjectSet<TrackingSystem> trackingSystems)
        {
            return trackingSystems.First(c => c.name == "Cake Marketing");
        }
    }
}
