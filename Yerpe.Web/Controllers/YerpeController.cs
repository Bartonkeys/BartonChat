using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using Yerpe.Web.Models;
using Yerpe.Web.Extensions;

namespace Yerpe.Web.Controllers
{
    [Authorize]
    [RoutePrefix("api/yerpe")]
    public class YerpeController : BaseApiController
    {
        [Route("messages/{messagesToSkip?}")]
        public HttpResponseMessage Get(int messagesToSkip = 0)
        {
            var room = YerpeContext.AspNetUsers.Single(u => u.UserName == User.Identity.Name).Rooms.First();

            var model = new YerpeChatViewModel
            {
                Name = User.Identity.Name,
                IsAdmin = User.IsInRole("Admin"),
                Messages = room.Messages
                .Where(m => m.Room_Id == room.Id)
                .OrderByDescending(x => x.Id)
                .Skip(messagesToSkip)
                .Take(10)
                .Select(m => new MessageViewModel
                {
                    From = m.AspNetUser.UserName.GetNameFromEmail(),
                    Message = m.Text,
                    SentDate = m.DateCreated.ToShortTimeString()
                }).ToList(),
                UsersInRoom = room.AspNetUsers.Select(u => u.UserName.GetNameFromEmail()).ToList()
            };
            model.Messages.Reverse();
            return Request.CreateResponse(HttpStatusCode.OK, model);
        }

    }
}