using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Routing;
using Microsoft.Ajax.Utilities;
using Microsoft.AspNet.SignalR;
using Yerpe.Web.Hubs;
using Yerpe.Web.Models;

namespace Yerpe.Web.Controllers
{
    /// <summary>
    /// Handles word stuff
    /// </summary>
    [System.Web.Http.RoutePrefix("api/YerpeExternal")]
    public class YerpeExternalController : HubController<YerpeHub> //ApiControllerWithHub <YerpeHub> //HubController<YerpeHub>
    {
        
        // POST /api/YerpeExternal/SendMessage
        /// <summary>
        /// Post to SignalR Hub
        /// </summary>
        /// <returns></returns>
        [Route("SendMessage/{message}")]
        [System.Web.Http.HttpPost]
        public HttpResponseMessage SendMessage (string message)
        {

            // Notify the connected clients
            HubContext.Clients.All.Send(message, message);
            
            //Hub.Clients.All.Send(message, message);

            var context = GlobalHost.ConnectionManager.GetHubContext<YerpeHub>();
            context.Clients.All.Send(message, message);
            // or
            context.Clients.Group("hg-YerpeHub").Send(message, message);
            
            // Return the new item, inside a 201 response
            var response = Request.CreateResponse(HttpStatusCode.Created, message);
            return response;

        }
    }
}
