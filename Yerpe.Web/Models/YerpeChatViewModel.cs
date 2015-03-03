using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yerpe.Web.Models
{
    public class YerpeChatViewModel
    {
        public string Name { get; set; }
        public RoomViewModel Room { get; set; }
        public List<MessageViewModel> Messages { get; set; }
        public List<string> UsersInRoom { get; set; }
        public bool IsAdmin { get; set; }
    }
}
