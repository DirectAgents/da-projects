using System;
using System.Collections.Generic;
using System.Linq;

namespace DAgents.Common
{
    public class ObjectMapper<TFromType, TToType>
    {
        private ISet<TFromType> _from;
        private ISet<TToType> _to;

        public ObjectMapper(
            ISet<TFromType> fromSet,
            ISet<TToType> toSet
            )
        {
            _from = fromSet;
            _to = toSet;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="rules"></param>
        /// <param name="factory"></param>
        /// <returns></returns>
        public ISet<TToType> Map(
            List<Func<TFromType, TToType, Tuple<bool, Func<TFromType, TToType, TToType>>>> rules,
            Func<TToType> factory
            )
        {
            var resultSet = new HashSet<TToType>();

            var results = from @from in _from
                          from to in _to
                          from rule in rules
                          let mapper = rule(@from, to)
                          where mapper.Item1 == true
                          select mapper.Item2(@from, to);

            foreach (var result in results)
                resultSet.Add(result);

            return resultSet;
        }
    }
}
