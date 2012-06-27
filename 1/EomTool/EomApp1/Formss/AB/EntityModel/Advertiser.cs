using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EomApp1.Formss.AB.EntityModel
{
    partial class Advertiser
    {
        public static explicit operator Data.Advertiser(Advertiser advertiser)
        {
            var result = new Data.Advertiser();
            result.Name = advertiser.name;
            return result;
        }
    }
}
