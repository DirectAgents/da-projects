using Huggies.Web.Models;
using KimberlyClark.Services.Abstract;
using KimberlyClark.Services.Concrete;
using RestSharp;

namespace Huggies.Web.Services
{
    public interface IService
    {
        bool SendLead(ILead lead, out IProcessResult processResult);
        bool SendLead(ILead lead, out IRestResponse<ProcessResult> restResponse, out IConsumer consumer);
        void SaveLead(ILead lead, string[] validationErrors);
    }
}