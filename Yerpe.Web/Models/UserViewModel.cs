using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Yerpe.Web.Models
{
    public class UserViewModel
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public int RoomId { get; set; }
        public string Password { get; set; }
    }
}
