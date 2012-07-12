using System.Collections.Generic;
using System.Linq;
using CakeMarketing.WebServices.V1;

namespace CakeMarketing
{
    public class Cake
    {
        private Service.CakeWebService cakeWebService;

        private Cake()
        {
            this.cakeWebService = new Service.CakeWebService();
        }

        public static Cake Create()
        {
            return new Cake();
        }

        public List<currency> Currencies
        {
            get { return this.cakeWebService.Currencies().ToList(); }
        }
    }
}
