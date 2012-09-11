using System;
using System.Collections.Generic;

namespace DirectAgents.Common
{
    public class Source<T>
    {
        public Source(string key)
        {
            this.Key = key;
        }

        public Source(Func<IEnumerable<T>> items, string key)
        {
            this.Items = items;
            this.Key = key;
        }

        public Func<IEnumerable<T>> Items { get; set; }

        public string Key { get; set; }
    }
}
