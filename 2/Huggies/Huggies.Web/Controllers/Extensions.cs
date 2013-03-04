using System.Linq;
using System.Web.Mvc;

namespace Huggies.Web.Controllers
{
    public static class Extensions
    {
        public static string[] ToStringArray(this ModelState modelState)
        {
            return modelState == null
                       ? new string[] {}
                       : modelState.Errors.Select(c => c.ErrorMessage).ToArray();
        }
    }
}