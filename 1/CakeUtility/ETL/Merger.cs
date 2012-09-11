using System;
using DirectAgents.Common.Aspects;

namespace DirectAgents.Common
{
    public class Merger<TSource, TTarget, TContainer> : AbstractCommand, IMerger<TSource, TTarget, TContainer>
        where TSource : class
        where TTarget : class, new()
        where TContainer : IDisposable
    {
        public Merger(Source<TSource> source, EntityTarget<TTarget, TContainer> target)
        {
            this.Source = source;
            this.Target = target;
        }

        [LogMethodBoundary()]
        public override void Execute()
        {
            MergeUtility.Merge(this.Source, this.Target);
        }

        public Source<TSource> Source { get; set; }

        public EntityTarget<TTarget, TContainer> Target { get; set; }
    }
}
