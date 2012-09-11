using System;
namespace DirectAgents.Common
{
    public interface IMerger<TSource, TTarget, TContainer> : ICommand
        where TSource : class
        where TTarget : class, new()
        where TContainer : IDisposable
    {
        DirectAgents.Common.Source<TSource> Source { get; set; }
        DirectAgents.Common.EntityTarget<TTarget, TContainer> Target { get; set; }
    }
}
