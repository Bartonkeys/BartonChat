using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;
using Yerpe.Web.Models;

namespace Yerpe.Web.Controllers
{
    [Authorize]
    public class YerpeController : BaseApiController
    {
        public HttpResponseMessage Get()
        {
            var model = new YerpeChatViewModel
            {
                Name = User.Identity.Name,
                Messages = YerpeContext.Rooms
                .Single(r => r.Name == "Yerma")
                .Messages.Select(m => new MessageViewModel
                {
                    From = m.AspNetUser.UserName,
                    Message = m.Text,
                    SentDate = m.DateCreated.ToLongDateString()
                }).Reverse().ToList()
            };

            return Request.CreateResponse(HttpStatusCode.OK, model);
        }

    }
}