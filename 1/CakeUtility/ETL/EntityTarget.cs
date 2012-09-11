using System;

namespace DirectAgents.Common
{
    public class EntityTarget<T, TContainer>
    {
        public EntityTarget(Func<TContainer> getContainer, string set, string key, Action<TContainer> save)
        {
            this.Container = getContainer;
            this.Set = set;
            this.Key = key;
            this.Save = save;
        }

        public Func<TContainer> Container { get; set; }

        public string Set { get; set; }

        public string Key { get; set; }

        public Action<TContainer> Save { get; set; }
    }
}
