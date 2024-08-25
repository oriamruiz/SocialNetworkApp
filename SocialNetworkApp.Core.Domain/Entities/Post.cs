using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialNetworkApp.Core.Domain.Entities
{
    public class Post
    {
        public int Id { get; set; }
        public string? Text { get; set; }
        public string? ImgUrl { get; set; }
        public string? VideoUrl { get; set; }
        public int UserId { get; set; }
        public User? User { get; set; }
        public DateTime PostingDate { get; set; }
        public DateTime? LastModified { get; set; }
        public ICollection<Comment>? Comments { get; set; } = new List<Comment>();

        public string? TypePost{ get; set; }




    }
}
