using SocialNetworkApp.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialNetworkApp.Core.Application.ViewModels.Users
{
    public class UserViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string AccountImgUrl { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public bool State { get; set; } = false;
        public DateTime CreatedDate { get; set; }
        public ICollection<Friendship> Friends { get; set; } = new List<Friendship>();
        public ICollection<Post> Posts { get; set; } = new List<Post>();
        public string ActivationToken { get; set; }



    }
}
