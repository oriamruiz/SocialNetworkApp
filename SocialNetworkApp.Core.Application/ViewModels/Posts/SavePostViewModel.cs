using Microsoft.AspNetCore.Http;
using SocialNetworkApp.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialNetworkApp.Core.Application.ViewModels.Posts
{
	public class SavePostViewModel
	{
		public int Id { get; set; }
		[Required(ErrorMessage = "Debe ingresar el contenido a postear")]
		[DataType(DataType.Text)]
		public string? Text { get; set; }
		[DataType(DataType.Text)]
		public string? ImgUrl { get; set; }
        [Required(ErrorMessage = "Debe ingresar el url del video")]
        [DataType(DataType.Text)]
		public string? VideoUrl { get; set; }
		[DataType(DataType.Upload)]
		public IFormFile? ImageFile { get; set; }
		public string? TypePost { get; set; }
        public int UserId { get; set; }
        public User? User { get; set; }
        public DateTime PostingDate { get; set; }
        public DateTime LastModified { get; set; }

    }
}
