using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialNetworkApp.Core.Domain.Entities
{
    public class Reply
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public DateTime PostingDate { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
        public int CommentId { get; set; }
        public Comment Comment { get; set; }
    }
}
