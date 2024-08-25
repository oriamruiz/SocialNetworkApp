using SocialNetworkApp.Core.Application.ViewModels.Users;
using SocialNetworkApp.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialNetworkApp.Core.Application.ViewModels.Friendships
{
    public class FriendshipViewModel
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
        public int FriendId { get; set; }
        public User Friend { get; set; }
    }
}
