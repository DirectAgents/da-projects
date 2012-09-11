using System;
using System.Collections.Generic;
using Microsoft.Practices.ServiceLocation;
using Microsoft.Practices.Unity;

namespace DirectAgents.Common
{
    public static class Locator
    {
        public static void Initialize(Microsoft.Practices.Unity.IUnityContainer unityContainer)
        {
            Current = new ServiceLocator(unityContainer);
        }

        public static T Get<T>()
        {
            return Current.GetInstance<T>();
        }

        public static T Get<T>(string key)
        {
            return Current.GetInstance<T>(key);
        }

        public static object Get(Type type)
        {
            return Current.GetInstance(type);
        }

        public static object Get(Type type, string key)
        {
            return Current.GetInstance(type, key);
        } 

        public static TAs Get<T, TAs>()
        {
            return Locator.Get<TAs>(typeof(T).Name);
        }

        private static IServiceLocator Current
        {
            get
            {
                return Microsoft.Practices.ServiceLocation.ServiceLocator.Current;
            }
            set
            {
                Microsoft.Practices.ServiceLocation.ServiceLocator.SetLocatorProvider(() =>
                {
                    return (Microsoft.Practices.ServiceLocation.IServiceLocator)value;
                });
            }
        }

        #region Private Inner Classes
        private class ServiceLocator : ServiceLocatorImplBase
        {
            private IUnityContainer container;

            public ServiceLocator(Microsoft.Practices.Unity.IUnityContainer container)
            {
                this.container = container;
            }

            protected override IEnumerable<object> DoGetAllInstances(Type serviceType)
            {
                var instances = this.container.ResolveAll(serviceType);
                return instances;
            }

            protected override object DoGetInstance(Type serviceType, string key)
            {
                var instance = this.container.Resolve(serviceType, key);
                return instance;
            }
        }
        #endregion
    }
}
