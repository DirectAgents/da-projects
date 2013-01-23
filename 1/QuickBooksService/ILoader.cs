using System;
using System.Xml.Linq;
namespace QuickBooksService
{
    public interface ILoader
    {
        void Load(XElement element);
    }
}
