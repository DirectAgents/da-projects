using System;
using System.Reflection;

namespace DirectAgents.Common
{
    public static class ObjectExtensions
    {
        public static void Dump(this object obj)
        {
            if (obj == null)
            {
                Console.WriteLine("Not dumping null object.");
                return;
            }

            foreach (var property in obj.GetType().GetProperties())
            {
                object value = property.GetValue(obj, null);
                if (value != null)
                {
                    string output = string.Format("{0}={1}", property.Name, value.ToString());

                    if (!string.IsNullOrWhiteSpace(output))
                    {
                        Console.WriteLine(output);
                    }

                    if(value is System.Collections.ICollection)
                    {
                        Console.WriteLine("[");

                        foreach (var item in (System.Collections.ICollection)value)
                        {
                            if (item is String)
                            {
                                Console.WriteLine(item);
                            }
                            else
                            {
                                item.Dump();
                            }
                        }

                        Console.WriteLine("]"); 

                    }
                }
            }

            Console.WriteLine();
        }

        private static bool ShouldRecurse(Type type)
        {
            bool result = type.FullName.Contains("System.Collections.Generic.List");
            return result;
        }

        public static object InvokeMethod(this object obj, string methodName, bool errorIfMethodDoesNotExist = true, bool accessNonPublic = false)
        {
            BindingFlags bindingFlags = accessNonPublic
                ? BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance
                : BindingFlags.Public | BindingFlags.Instance;

            var method = obj.GetType().GetMethod(methodName, bindingFlags);

            if (method == null && errorIfMethodDoesNotExist)
            {
                throw new Exception("Method " + methodName + " not found on object " + obj.GetType().FullName);
            }
            else
            {
                return method.Invoke(obj, null);
            }
        }

        public static object InvokeMethodWithArgs(this object obj, string methodName, object[] args, bool errorIfMethodDoesNotExist = true, bool accessNonPublic = false)
        {
            BindingFlags bindingFlags = accessNonPublic
                ? BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance
                : BindingFlags.Public | BindingFlags.Instance;

            var method = obj.GetType().GetMethod(methodName, bindingFlags);

            if (method == null && errorIfMethodDoesNotExist)
            {
                throw new Exception("Method " + methodName + " not found on object " + obj.GetType().FullName);
            }
            else
            {
                return method.Invoke(obj, args);
            }
        }

        public static string TypeName(this object obj)
        {
            return obj.GetType().Name;
        }
    }
}
