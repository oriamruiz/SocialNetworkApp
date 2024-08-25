using Microsoft.AspNetCore.Http;
using SocialNetworkApp.Core.Application.Helpers.Validations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialNetworkApp.Core.Application.ViewModels.Users
{
	public class UpdateUserViewModel
	{
		public int Id { get; set; }

		[Required(ErrorMessage = "Debe ingresar el nombre")]
		[DataType(DataType.Text)]
		public string Name { get; set; }

		[Required(ErrorMessage = "Debe ingresar el apellido")]
		[DataType(DataType.Text)]
		public string LastName { get; set; }

		[Required(ErrorMessage = "Debe ingresar un numero telefono")]
        [DominicanPhoneNumber(ErrorMessage = "El número de teléfono debe ser un número válido de la República Dominicana. ejemplo: 18293439342")]
        [DataType(DataType.Text)]
		public string PhoneNumber { get; set; }

		[Required(ErrorMessage = "Debe ingresar el email")]
		[DataType(DataType.Text)]
		public string Email { get; set; }

		[Required(ErrorMessage = "Debe ingresar el nombre de usuario")]
		[DataType(DataType.Text)]
		public string Username { get; set; }

		
		[DataType(DataType.Password)]
		public string? Password { get; set; }

		[Compare(nameof(Password), ErrorMessage = "Las contraseñas deben coincidir")]
		[DataType(DataType.Password)]
		public string? ConfirmPassword { get; set; }

		[DataType(DataType.Upload)]
		public IFormFile? ImageFile { get; set; }

		public string? AccountImgUrl { get; set; }

	}
}
