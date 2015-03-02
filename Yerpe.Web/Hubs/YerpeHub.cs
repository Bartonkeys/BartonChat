using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using Yerpe.Data;
using Yerpe.Web.Models;

namespace Yerpe.Web.Hubs
{
    public class YerpeHub : Hub
    {
        private readonly Yerpe_dbEntities _yerpeContext = new Yerpe_dbEntities();

        public void Send(string name, string message)
        {
            var newMessage = new Message
            {
                AspNetUser = _yerpeContext.AspNetUsers.Single(u => u.UserName == name),
                DateCreated = DateTime.Now,
                Room = _yerpeContext.AspNetUsers.Single(u => u.UserName == name).Rooms.First(),
                Text = message
            };

            _yerpeContext.Messages.Add(newMessage);
            _yerpeContext.SaveChanges();

            Clients.All.addNewMessageToPage(new MessageViewModel 
            { 
                From = newMessage.AspNetUser.UserName.Split('@')[0],
                Message = message, 
                SentDate = "Just now" 
            });
        }
    }
}