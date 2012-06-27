using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Objects;
using System.Reflection;

namespace EomApp1.Formss.AB2.Model
{
    static public class Extensions2
    {
        static public void ForFirst<T>(this IQueryable<T> source, Action<T> action)
        {
            T firstOrDefault = source.FirstOrDefault();

            if (firstOrDefault != null)
            {
                action(firstOrDefault);
            }
        }

        static public void AssignFirstToProperty<T>(this ObjectSet<T> source, Object target, Func<T, bool> predicate) where T : class
        {
            PropertyInfo targetProperty = target.GetType()
                                                    .GetProperties()
                                                    .FirstOrDefault(c => c.Name == typeof(T).Name);

            if (targetProperty != null)
            {
                targetProperty.SetValue(target, source.First(), null);
            }
            else
            {
                throw new Exception("property " + typeof(T).Name + " not found");
            }
        }

        static public void GetsFirst<T>(this Object target, ObjectSet<T> source, Func<T, bool> predicate) where T : class
        {
            source.AssignFirstToProperty(target, predicate);
        }

        static public void FirstTo<T>(this ObjectSet<T> source, Object target, Func<T, bool> predicate) where T : class
        {
            source.AssignFirstToProperty(target, predicate);
        }
    }
}
