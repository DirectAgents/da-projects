using System;
using System.Collections.Generic;
using System.Data.Objects;
using System.Linq;
using Microsoft.Practices.Unity;

namespace DirectAgents.Common
{
    public static class MergeUtility
    {
        /// <summary>
        /// TODO: documentation
        /// </summary>
        /// <typeparam name="TItem">The type of the source items.</typeparam>
        /// <typeparam name="TContainer">The type of the destination container.</typeparam>
        /// <typeparam name="TTarget">The type of the destination items.</typeparam>
        /// <param name="sourceItemsCollection">A function returning an enumerable set of source items.</param>
        /// <param name="createDestinationContainer">A function returning a destination container.</param>
        /// <param name="sourceKey">The name of the property on each source item who's value uniquely identifies it in the whole set.</param>
        /// <param name="targetSetName">The name of the property on the target container who's value is the ObjectSet containing the target items.</param>
        /// <param name="targetKey">The name of the property on each target item who's value uniquely identifies it in the whole set.</param>
        /// <param name="save">A function called when the merge is completed to save the changes in the target container.</param>
        public static void Merge<TItem, TContainer, TTarget>(Func<IEnumerable<TItem>> sourceItemsCollection,
                                                             Func<TContainer> createDestinationContainer,
                                                             string sourceKey,
                                                             string targetSetName,
                                                             string targetKey,
                                                             Action<TContainer> save,
                                                             Func<TItem, bool> itemFilter = null,
                                                             bool deleteUnmatched = false)
            where TItem : class
            where TContainer : IDisposable
            where TTarget : class, new()
        {
            // Filter the source collection
            var filter = itemFilter ?? (c => true);

            Log<TItem, TContainer, TTarget>();

            // Create the destination context
            using (var destinationContainer = createDestinationContainer())
            {
                var targetObjectSet = destinationContainer.MemberValue<ObjectSet<TTarget>>(targetSetName);
                var existing = targetObjectSet.ToDictionary(c => c.MemberValue<object>(targetKey));
                var mapper = new Mapper<TItem, TTarget>();

                foreach (var item in sourceItemsCollection().Where(c => filter(c)))
                {
                    var id = item.MemberValue<object>(sourceKey);
                    TTarget entity;

                    if (existing.ContainsKey(id))
                    {
                        entity = existing[id];
                    }
                    else
                    {
                        entity = new TTarget();
                        targetObjectSet.AddObject(entity);
                    }

                    mapper.MapProperties(item, entity, false);
                }

                if (deleteUnmatched)
                {
                    // TODO: implement
                }

                save(destinationContainer);
            }
        }

        public static void Merge<TSource, TSourceContainer, TTarget, TTargetContainer>(EntitySource<TSource, TSourceContainer> source,
                                                                                       EntityTarget<TTarget, TTargetContainer> target)
            where TSource : class
            where TSourceContainer : IDisposable
            where TTarget : class, new()
            where TTargetContainer : IDisposable
        {
            using (var container = source.Container())
            {
                Merge<TSource, TTargetContainer, TTarget>(
                    () => container.MemberValue<ObjectSet<TSource>>(source.Set),
                    target.Container,
                    source.Key,
                    target.Set,
                    target.Key,
                    target.Save,
                    source.Filter);
            }
        }

        public static void Merge<TSource, TTarget, TContainer>(Source<TSource> source,
                                                               EntityTarget<TTarget, TContainer> target)
            where TSource : class
            where TContainer : IDisposable
            where TTarget : class, new()
        {
            Merge<TSource, TContainer, TTarget>(
                source.Items,
                target.Container,
                source.Key,
                target.Set,
                target.Key,
                target.Save);
        }

        public static void Merge<TSource, TTarget, TContainer>()
            where TSource : class
            where TTarget : class, new()
            where TContainer : IDisposable
        {
            Merge(
                Locator.Get<Source<TSource>>(),
                Locator.Get<EntityTarget<TTarget, TContainer>>());
        }

        private static void Log<TSource, TContainer, TTarget>()
            where TSource : class
            where TContainer : IDisposable
            where TTarget : class, new()
        {
            Console.WriteLine("-------------------------------");
            Console.WriteLine("MERGE");
            Console.WriteLine("       Source: " + typeof(TSource).FullName);
            Console.WriteLine("    Container: " + typeof(TContainer).FullName);
            Console.WriteLine("       Target: " + typeof(TTarget).FullName);
            Console.WriteLine("-------------------------------");
        }
    }
}
