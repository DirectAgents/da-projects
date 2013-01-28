using EomTool.Domain.Entities;
using System;

namespace EomApp1.Screens.PubRep1.MVP.PublisherDetails
{
    public interface IView<TItem, TKey>
    {
        void Fill(TItem[] items);
        event MessageEvent<TKey> SelectionChanged;
        event MessageEvent Add;
    }

    public interface INotes : IView<PubNote, string>
    {
    }

    public interface IAttachments : IView<PubAttachment, string>
    {
        event MessageEvent<int> OpenAttachment;
    }

    // Event Delegates
    //
    public delegate void MessageEvent<TKey>(TKey selectedValue);
    public delegate bool MessageEvent();

    // Event Delegate Extensions
    //
    public static class Extensions
    {
        public static void Notify(this EventHandler e, object o)
        {
            if (e != null)
                e(o, EventArgs.Empty);
        }

        public static void Notify<T>(this MessageEvent<T> e, T o)
        {
            if (e != null)
                e(o);
        }

        public static bool Notify(this MessageEvent e)
        {
            return (e != null ? e() : false);
        }
    }
}
