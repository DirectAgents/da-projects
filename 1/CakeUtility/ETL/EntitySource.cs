using System;

namespace DirectAgents.Common
{
    public class EntitySource<T, TContainer> : Source<T>
    {
        public EntitySource(Func<TContainer> container, string set, string key)
            : base(key)
        {
            this.Container = container;
            this.Set = set;
        }

        public EntitySource(Func<TContainer> container, string set, string key, Func<T, bool> filter)
            : this(container, set, key)
        {
            this.Filter = filter;
        }

        public Func<TContainer> Container { get; set; }

        public string Set { get; set; }

        public Func<T, bool> Filter { get; set; }
    }
}
