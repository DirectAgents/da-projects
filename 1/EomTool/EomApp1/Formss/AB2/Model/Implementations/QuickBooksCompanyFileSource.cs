using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EomApp1.Formss.AB2.Model
{
    public class QuickBooksCompanyFileSource : EomApp1.Formss.AB2.Model.IQuickBooksCompanyFileSource
    {
        public IEnumerable<QuickBooksCompanyFile> QuickBooksCompanyFiles
        {
            get
            {
                yield return new QuickBooksCompanyFile
                {
                    Name = "US"
                };

                yield return new QuickBooksCompanyFile
                {
                    Name = "INTL"
                };
            }
        }
    }
}
