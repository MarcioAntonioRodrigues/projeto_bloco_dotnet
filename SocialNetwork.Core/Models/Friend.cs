using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialNetwork.Core.Models
{
    public class Friend
    {
        public int FriendId { get; set; }
        public int ProfileId { get; set; }
        public Profile Profile { get; set; }
    }
}
