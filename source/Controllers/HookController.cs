using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Results;
using Microsoft.AspNet.SignalR;
using TraceMe.Hubs;

namespace TraceMe.Controllers
{
    public class HookController : ApiController
    {
        [HttpPost]
        public async Task<OkResult> Tell()
        {
            string payload = await Request.Content.ReadAsStringAsync();

            TraceHub.Trace(Request.RequestUri.AbsolutePath, User, payload);

            return Ok();
        }
    }
}
