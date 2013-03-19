using System.ServiceModel;

namespace LTWeb.ws
{
    [ServiceContract]
    public interface ILeadService
    {
        [OperationContract]
        string RefiLead(LeadPost leadPost, string password);
    }
}
