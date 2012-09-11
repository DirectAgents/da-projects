namespace DirectAgents.Common
{
    public class InstanceFactory<T> : IFactory<T> //where T : new()
    {
        public T Create()
        {
            return System.Activator.CreateInstance<T>();
        }
    }
}
