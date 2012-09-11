using System.Data.Objects;

namespace DirectAgents.Common
{
    public static class ObjectSetExtensions
    {
        public static void DeleteAll<T>(this ObjectSet<T> objects) where T : class
        {
            foreach (var item in objects)
            {
                objects.DeleteObject(item);
            }
        }
    }
}
