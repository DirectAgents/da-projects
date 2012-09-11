using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace DirectAgents.Common
{
    /// <summary>
    /// This Mapper can transfer the values bwtween two existing objects, the source and the destination.
    /// 
    /// Property names are matched after being normalized:
    ///    1. Underscores are removed (foo_bar_id becomes foobarid).
    ///    2. Converted to uppercase (foobarid becomes FOOBARID)
    /// </summary>
    /// <typeparam name="S"></typeparam>
    /// <typeparam name="T"></typeparam>
    public class Mapper<S, T>
    {
        List<MemberInfo> targetMembers = new List<MemberInfo>();

        private List<string> ignoreList = new List<string>();
        public List<string> IgnoreList
        {
            get { return ignoreList; }
            set { ignoreList = value; }
        }

        public Mapper()
        {
            this.targetMembers.AddRange(typeof(T).GetProperties());
            this.targetMembers.AddRange(typeof(T).GetFields());
        }

        /// <summary>
        /// Transfer the values bwtween two existing objects, the source and the destination.
        /// </summary>
        /// <param name="source">The object from which property values will be obtained.</param>
        /// <param name="target">The object who's properties recieve the value of their matching property in the <paramref name="source"/></param>
        /// <param name="failIfNotMatched">When a property in the <paramref name="source"/> does not match to a property in the <paramref name="target"/>
        /// and <paramref name="failIfNotMatched"/> is TRUE, a <c>TargetNotMatchedException</c> will be thrown.  Otherwise the unmatched property is ignored.< </param>
        /// <param name="mapInheritedMembers">When <paramref name="mapInheritedMembers"/> is TRUE the set of source properties will include properties which
        /// are inherited.  Otherwise only the properties of the most derived type are mapped.</param>
        public void MapProperties(S source, T target, bool failIfNotMatched = true, bool mapInheritedMembers = false)
        {
            BindingFlags bindingFlags = mapInheritedMembers
                ? BindingFlags.Public | BindingFlags.Instance
                : BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.Instance;

            foreach (PropertyInfo property in source.GetType()
                                                    .GetProperties(bindingFlags)
                                                    .Where(c => !IgnoreList.Contains(c.Name)))
            {
                try
                {
                    var sourceField = Factory.Get<MapperItem>(property, source);
                    var targetField = Factory.Get<MapperItem>(MatchToTarget(property), target);

                    targetField.Assign(sourceField);
                }
                catch (TargetNotMatchedException noMatch)
                {
                    if (failIfNotMatched)
                    {
                        throw noMatch;
                    }
                }
            }
        }

        private MemberInfo MatchToTarget(MemberInfo member)
        {
            var exactMatch = this.targetMembers.Where(c => c.Name == member.Name);
            if (exactMatch.FirstOrDefault() != null)
            {
                return exactMatch.First();
            }

            var sameAlphaChars = this.targetMembers.Where(c => Normalize(c.Name) == Normalize(member.Name));
            if (sameAlphaChars.FirstOrDefault() != null)
            {
                return sameAlphaChars.First();
            }

            throw new TargetNotMatchedException(member, typeof(T));
        }

        private static string Normalize(string input)
        {
            string normalized = input.Replace("_", "").ToUpper();
            return normalized;
        }
    }
}
