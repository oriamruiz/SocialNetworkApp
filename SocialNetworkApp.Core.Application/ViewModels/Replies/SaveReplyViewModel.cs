using SocialNetworkApp.Core.Application.ViewModels.Comments;
using SocialNetworkApp.Core.Application.ViewModels.Users;
using SocialNetworkApp.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialNetworkApp.Core.Application.ViewModels.Replies
{
	public class SaveReplyViewModel
	{
		public int Id { get; set; }
		[Required(ErrorMessage = "Debe ingresar el contenido de la respuesta")]
		[DataType(DataType.Text)]
		public string Text { get; set; }
		public int CommentId { get; set; }
		public Comment? Comment { get; set; }
        public int UserId { get; set; }
        public User? User { get; set; }
        public string RedirectTo { get; set; }
        public DateTime PostingDate { get; set; }
    }
}
