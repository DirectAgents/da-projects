using System;
using System.Linq;
namespace DirectTrack
{
    public class NoCacheInterceptor : Spring.Http.Client.Interceptor.IClientHttpRequestBeforeInterceptor
    {
        public void BeforeExecute(Spring.Http.Client.Interceptor.IClientHttpRequestContext request)
        {
            request.Headers["Cache-Control"] = "no-cache";
        }
    }
}
