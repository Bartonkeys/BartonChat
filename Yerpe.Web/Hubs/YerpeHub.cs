using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using Yerpe.Data;
using Yerpe.Web.Models;
using Yerpe.Web.Extensions;

namespace Yerpe.Web.Hubs
{
    public class YerpeHub : Hub
    {
        private readonly Yerpe_dbEntities _yerpeContext = new Yerpe_dbEntities();

        public override System.Threading.Tasks.Task OnConnected()
        {
            var roomEntity = GetARoom();
            Groups.Add(Context.ConnectionId, roomEntity.Name);
            return base.OnConnected();
        }

        private Room GetARoom()
        {
            return _yerpeContext
                .AspNetUsers
                .Single(u => u.UserName == Context.User.Identity.Name)
                .Rooms.First();
        }

        public void Send(string name, string message)
        {
            var newMessage = new Message
            {
                AspNetUser = _yerpeContext.AspNetUsers.Single(u => u.UserName == name),
                DateCreated = DateTime.Now,
                Room = GetARoom(),
                Text = message
            };

            _yerpeContext.Messages.Add(newMessage);
            _yerpeContext.SaveChanges();

            Clients.Group(newMessage.Room.Name).addNewMessageToPage(new MessageViewModel 
            {
                From = newMessage.AspNetUser.UserName.GetNameFromEmail(),
                Message = message, 
                SentDate = DateTime.Now.ToShortTimeString()
            });
        }
    }
}