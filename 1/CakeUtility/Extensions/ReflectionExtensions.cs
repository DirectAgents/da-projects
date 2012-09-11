using System;
using System.Reflection;

using System.Linq;

namespace DirectAgents.Common
{
    public static class ReflectionExtensions
    {
        public static Type UnderlyingType(this MemberInfo member)
        {
            Type type;
            switch (member.MemberType)
            {
                case MemberTypes.Field:
                    type = ((FieldInfo)member).FieldType;
                    break;
                case MemberTypes.Property:
                    type = ((PropertyInfo)member).PropertyType;
                    break;
                case MemberTypes.Event:
                    type = ((EventInfo)member).EventHandlerType;
                    break;
                default:
                    throw new ArgumentException("member must be if type FieldInfo, PropertyInfo or EventInfo", "member");
            }
            return Nullable.GetUnderlyingType(type) ?? type;
        }

        public static object Value(this MemberInfo member, object target)
        {
            if (member is PropertyInfo)
            {
                return (member as PropertyInfo).GetValue(target, null);
            }
            else if (member is FieldInfo)
            {
                return (member as FieldInfo).GetValue(target);
            }
            else
            {
                throw new Exception("member must be either PropertyInfo or FieldInfo");
            }
        }

        public static void Assign(this MemberInfo member, object target, object value)
        {
            if (member is PropertyInfo)
            {
                (member as PropertyInfo).SetValue(target, value, null);
            }
            else if (member is FieldInfo)
            {
                (member as FieldInfo).SetValue(target, value);
            }
            else
            {
                throw new Exception("destinationMember must be either PropertyInfo or FieldInfo");
            }
        }

        public static T MemberValue<T>(this object source, string memberName
            //, bool parseDotOperatorInName = false
            )
        {
            //if (parseDotOperatorInName)
            //{
            //    MemberInfo memberInfo = null;
            //    foreach (var name in memberName.Split('.'))
            //    {
            //        memberInfo = memberInfo.GetType().GetMember(name)[0];
            //    }
            //}
            return (T)source.GetType().GetMember(memberName)[0].Value(source);
        }

        public static bool HasAttribute<T>(this MethodInfo method)
        {
            return (method.GetCustomAttributes(typeof(T), false).Length > 0);
        }

        public static MethodInfo GetMethodWithAttribute<TAttribute>(this Type type) where TAttribute : Attribute
        {
            var methodQuery = from method in type.GetMethods()
                              where method.HasAttribute<TAttribute>()
                              select method;

            int count = methodQuery.Count();
            if (count == 0)
            {
                throw new Exception("No method on type " + typeof(TAttribute).Name + " has atttibute " + typeof(TAttribute).Name + ".");
            }
            else if (count > 1)
            {
                throw new Exception("There are multiple methods with attribute " + typeof(TAttribute).Name + ".");
            }
            else
            {
                return methodQuery.First();
            }
        }
    }
}
