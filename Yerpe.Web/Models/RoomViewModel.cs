using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Yerpe.Web.Models
{
    public class RoomViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<UserViewModel> Users { get; set; }
    }
}