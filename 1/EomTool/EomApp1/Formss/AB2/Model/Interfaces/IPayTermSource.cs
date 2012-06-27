using System;
namespace EomApp1.Formss.AB2.Model
{
    public interface IPayTermSource
    {
        System.Collections.Generic.IEnumerable<PayTerm> PayTerms { get; }
    }
}
