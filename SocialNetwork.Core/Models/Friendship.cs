using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialNetwork.Core.Models
{
    class Friendship
    {
        public int ProfileId { get; set; }
        public int FriendId { get; set; }
        public Profile Profile { get; set; }
        public Friend Friend { get; set; }
    }
}
