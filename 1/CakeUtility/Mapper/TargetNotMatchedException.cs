using System;
using System.Reflection;

namespace DirectAgents.Common
{
    public class TargetNotMatchedException : Exception
    {
        public TargetNotMatchedException(MemberInfo member, Type type)
            : base("no match for member named " + member.Name + " with type named " + type.Name)
        {
            this.Member = member;
            this.Type = type;
        }

        public MemberInfo Member { get; set; }

        public Type Type { get; set; }
    }
}
