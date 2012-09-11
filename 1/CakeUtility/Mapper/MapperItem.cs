using System;
using System.Collections.Generic;
using System.Reflection;

namespace DirectAgents.Common
{
    /// <summary>
    /// Encapsulates an item to be mapped and supports conversion from the souce type to the destination type.
    /// </summary>
    public class MapperItem
    {
        private MemberInfo memberInfo;
        private object target;
        private Type type;
        private static Dictionary<Tuple<Type, Type>, Func<object, object>> Conversions = new Dictionary<Tuple<Type, Type>, Func<object, object>>();

        /// <summary>
        /// Constructor. TODO: improve comment
        /// </summary>
        /// <param name="member"></param>
        /// <param name="target"></param>
        public MapperItem(MemberInfo member, object target)
        {
            this.memberInfo = member;
            this.target = target;
            this.type = this.memberInfo.UnderlyingType();
        }

        /// <summary>
        /// Transfers the value from one mapper item to the other while applying type conversion.
        /// </summary>
        /// <param name="source"></param>
        public void Assign(MapperItem source)
        {
            this.memberInfo.Assign(this.target, source.Convert(this.type));
        }

        /// <summary>
        /// Allows arbitrary conversions.
        /// </summary>
        /// <typeparam name="S"></typeparam>
        /// <typeparam name="T"></typeparam>
        /// <param name="converter"></param>
        public static void AddConversion<S, T>(Func<object, object> converter)
        {
            Conversions.Add(Tuple.Create(typeof(S), typeof(T)), converter);
        }

        private object Value
        {
            get
            {
                return this.memberInfo.Value(this.target);
            }
        }

        private object Convert(Type convertToType)
        {
            object converted = null;

            if (this.Value == null)
            {
                return converted;
            }
            else if (convertToType.IsAssignableFrom(this.type))
            {
                converted = this.Value;
            }
            else
            {
                var conversionKey = Tuple.Create(this.type, convertToType);

                if (Conversions.ContainsKey(conversionKey))
                {
                    converted = Conversions[conversionKey](this.Value);
                }
                else
                {
                    throw new Exception(convertToType.Name + " is not assignable from " + this.type.Name);
                }
            }

            return converted;
        }
    }
}
