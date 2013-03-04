using System;
using System.Web;
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

    public class ControllerBase : Controller
    {
        private readonly ModelStateDictionary modelStateDictionary;
        private readonly HttpRequestBase request;
        private readonly HttpSessionStateBase session;

        public ControllerBase()
        {
            session = null;
            request = null;
            modelStateDictionary = null;
        }

        public ControllerBase(HttpSessionStateBase ssb, HttpRequestBase rq, ModelStateDictionary msd)
        {
            session = ssb;
            request = rq;
            modelStateDictionary = msd;
        }

        protected HttpSessionStateBase GetSession()
        {
            return session ?? Session;
        }

        protected HttpRequestBase GetRequest()
        {
            return request ?? Request;
        }

        protected ModelStateDictionary GetModelState()
        {
            return modelStateDictionary ?? ModelState;
        }
    }
}