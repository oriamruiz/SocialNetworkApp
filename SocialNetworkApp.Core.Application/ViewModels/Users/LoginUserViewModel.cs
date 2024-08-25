using SocialNetworkApp.Core.Domain.Entities;

using System.ComponentModel.DataAnnotations;


namespace SocialNetworkApp.Core.Application.ViewModels.Users
{
    public class LoginUserViewModel
    {
        [Required(ErrorMessage = "Debe ingresar el nombre de usuario")]
        [DataType(DataType.Text)]
        public string Username { get; set; }

        [Required(ErrorMessage = "Debe ingresar la contraseña")]
        [DataType(DataType.Password)]
        public virtual string Password { get; set; }


    }
}
