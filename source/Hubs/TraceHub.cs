using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;
using Microsoft.AspNet.SignalR;

namespace TraceMe.Hubs
{
    public class TraceHub : Hub
    {
        public void Dummy()
        {
            
        }

        public static void Trace(string hook, IPrincipal user, string message)
        {
            var hub = GlobalHost.ConnectionManager.GetHubContext<TraceHub>();

            hub.Clients.All.traceEntry(hook, user.Identity.IsAuthenticated ? user.Identity.Name : "Anonymous", message);
        }
    }
}