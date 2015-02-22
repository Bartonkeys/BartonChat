using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using Yerpe.Data;

namespace Yerpe.Web.Hubs
{
    public class YerpeHub : Hub
    {
        private readonly Yerpe_dbEntities _yerpeContext = new Yerpe_dbEntities();

        public void Send(string name, string message)
        {
            // Call the addNewMessageToPage method to update clients.
            _yerpeContext.Messages.Add(new Message
            {
                AspNetUser = _yerpeContext.AspNetUsers.Single(u => u.UserName == name),
                DateCreated = DateTime.Now,
                Room = _yerpeContext.Rooms.Single(r => r.Name == "Yerma"),
                Text = message
            });
            _yerpeContext.SaveChanges();

            Clients.All.addNewMessageToPage(name, message);
        }
    }
}