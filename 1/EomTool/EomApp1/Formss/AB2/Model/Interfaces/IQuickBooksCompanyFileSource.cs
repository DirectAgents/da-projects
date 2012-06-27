using System;
namespace EomApp1.Formss.AB2.Model
{
    public interface IQuickBooksCompanyFileSource
    {
        System.Collections.Generic.IEnumerable<QuickBooksCompanyFile> QuickBooksCompanyFiles { get; }
    }
}
