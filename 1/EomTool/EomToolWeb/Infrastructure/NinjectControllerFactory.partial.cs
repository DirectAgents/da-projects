using DirectAgents.Domain.Abstract;
using DirectAgents.Domain.Concrete;

namespace EomToolWeb.Infrastructure
{
    public partial class NinjectControllerFactory
    {
        // setup classes in the DirectAgents.Domain namespace
        public void SetupOther()
        {
            kernel.Bind<ICampaignRepository>().To<CampaignRepository>();
            kernel.Bind<IAdmin>().To<AdminImpl>();
            kernel.Bind<IMainRepository>().To<MainRepository>();
        }
    }
}

//public void SetupMocks()
//{
//var person1 = new Person { PersonId = 1, Name = "Person One" };
//var person2 = new Person { PersonId = 2, Name = "Person Two" };
//var person3 = new Person { PersonId = 3, Name = "Person Three" };
//var mock = new Mock<ICampaignRepository>();
//mock.Setup(m => m.Campaigns).Returns(new List<Campaign>
//{
//    new Campaign { Pid = 1, Name = "Campaign One", AccountManagers = new List<Person> { person1 }, MediaBuyers = new List<Person> { person2 }},
//    new Campaign { Pid = 2, Name = "Campaign Two", AccountManagers = new List<Person> { person1, person2 }, MediaBuyers = new List<Person> { person3 } },
//    new Campaign { Pid = 3, Name = "Campaign Three", AccountManagers = new List<Person> { person2 }, MediaBuyers = new List<Person> { person1 } },
//}.AsQueryable());
//kernel.Bind<ICampaignRepository>().ToConstant(mock.Object);
//}