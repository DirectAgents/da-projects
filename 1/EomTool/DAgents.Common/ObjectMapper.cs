using System;
using System.Collections.Generic;
using System.Linq;

namespace DAgents.Common
{
    public class ObjectMapper<TFromType, TToType>
    {
        private ISet<TFromType> fromSet;
        private ISet<TToType> toSet;

        public ObjectMapper(ISet<TFromType> from, ISet<TToType> to)
        {
            this.fromSet = from;
            this.toSet = to;
        }

        public ISet<TToType> Map(List<Func<TFromType, TToType, Tuple<bool, Func<TFromType, TToType, TToType>>>> rules, Func<TToType> factory)
        {
            var resultSet = new HashSet<TToType>();

            var results = from f in this.fromSet
                          from t in this.toSet
                          from rule in rules
                          let mapper = rule(f, t)
                          where mapper.Item1 == true
                          select mapper.Item2(f, t);

            foreach (var result in results)
                resultSet.Add(result);

            return resultSet;
        }
    }
}
