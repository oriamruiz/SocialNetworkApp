using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialNetworkApp.Core.Application.ViewModels.Users
{
    public class ForgotPasswordViewModel
    {

        [Required(ErrorMessage = "Debe ingresar el nombre de usuario")]
        [DataType(DataType.Text)]
        public string Username { get; set; }

        [Required(ErrorMessage = "Debe ingresar la contraseña")]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; }

        [Compare(nameof(NewPassword), ErrorMessage = "Las contraseñas deben coincidir")]
        [Required(ErrorMessage = "Debe repetir la contraseña")]
        [DataType(DataType.Password)]
        public string ConfirmNewPassword { get; set; }
    }
}
