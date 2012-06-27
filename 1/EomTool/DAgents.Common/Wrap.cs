namespace DAgents.Common
{
    public class Wrap<T> where T : new()
    {
        public Wrap()
        {
            this.Inner = new T();
        }

        public Wrap(T inner)
        {
            this.Inner = inner;
        }

        public T Inner { get; set; }
    }
}
