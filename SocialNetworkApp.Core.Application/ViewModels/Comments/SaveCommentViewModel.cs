using SocialNetworkApp.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialNetworkApp.Core.Application.ViewModels.Comments
{
	public class SaveCommentViewModel
	{
		public int Id { get; set; }
		[Required(ErrorMessage = "Debe ingresar el contenido a comentar")]
		[DataType(DataType.Text)]
		public string Text { get; set; }
        public DateTime PostingDate { get; set; }
        public int UserId { get; set; }
        public User? User { get; set; }
        public int PostId { get; set; }
		public Post? Post { get; set; }
        
        public string RedirectTo { get; set; }
        

    }
}
