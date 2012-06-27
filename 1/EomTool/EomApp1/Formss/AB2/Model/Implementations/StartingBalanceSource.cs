using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EomApp1.Formss.AB2.Model
{
    public class StartingBalanceSource : IStartingBalanceSource
    {
        private readonly DirectAgentsEntities _model;
        private DirectAgentsEntities Model { get { return _model; } }

        public StartingBalanceSource(DirectAgentsEntities model)
        {
            _model = model;
        }

        public IEnumerable<StartingBalance> StartingBalances
        {
            get
            {
                foreach (var client in Model.Clients)
                {
                    client.StartingBalance = new StartingBalance {
                        Item = new Item {
                            Amount = new Amount {
                                Quantity = 0,
                                Unit = (Unit)client
                            },
                            DateSpan = Model.DateSpans.First(),
                            Client = client
                        }
                    };
                    yield return client.StartingBalance;
                }
            }
        }
    }
}
