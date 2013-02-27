using Huggies.Web.Models;
using KimberlyClark.Services.Abstract;

namespace Huggies.Web.Services
{
    public interface IService
    {
        bool SendLead(ILead lead, out IProcessResult processResult);
        void SaveLead(ILead lead, string[] validationErrors);
    }
}