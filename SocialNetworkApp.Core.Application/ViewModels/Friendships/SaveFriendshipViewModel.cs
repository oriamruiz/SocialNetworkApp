using SocialNetworkApp.Core.Application.ViewModels.Users;
using SocialNetworkApp.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialNetworkApp.Core.Application.ViewModels.Friendships
{
    public class SaveFriendshipViewModel
    {
        public int Id { get; set; }
        public string FriendUsername{ get; set; }
        public int FriendId { get; set; }
        public User Friend { get; set; }
    }
}
