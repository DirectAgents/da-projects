using System;
using System.Web.Mvc;

namespace Huggies.Web.Controllers
{
    public class ControllerBase<T> : Controller where T : class, IDisposable
    {
        private readonly Lazy<T> lazyContext = new Lazy<T>();

        protected T Context
        {
            get { return lazyContext.Value; }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && lazyContext.IsValueCreated)
                this.Context.Dispose();

            base.Dispose(disposing);
        }
    }
}