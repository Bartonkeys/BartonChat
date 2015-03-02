using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Yerpe.Data;
using Yerpe.Web.Models;

namespace Yerpe.Web.Controllers
{
    [Authorize(Roles="Admin")]
    public class RoomController : BaseApiController
    {
        public HttpResponseMessage Get()
        {
            var rooms = YerpeContext.Rooms.Select(x => new RoomViewModel { Id = x.Id, Name = x.Name, 
                    Users = x.AspNetUsers.Select(u => new UserViewModel{ Id = u.Id, UserName = u.UserName }).ToList()})
                    .ToList();

            return Request.CreateResponse(HttpStatusCode.OK, rooms);
        }

        public HttpResponseMessage Post([FromBody] RoomViewModel roomModel)
        {
            var room = new Room 
            {  
                Name = roomModel.Name,
                AspNetUsers = new List<AspNetUser>(),
                Messages = new List<Message>()
            };

            YerpeContext.Rooms.Add(room);
            YerpeContext.SaveChanges();

            return Request.CreateResponse(HttpStatusCode.OK, room);
        }
    }
}
