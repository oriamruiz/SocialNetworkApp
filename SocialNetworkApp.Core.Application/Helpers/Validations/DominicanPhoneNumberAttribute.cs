using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SocialNetworkApp.Core.Application.Helpers.Validations
{
    public class DominicanPhoneNumberAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value != null)
            {
                var phoneNumber = value.ToString();
                var regex = new Regex(@"^(1)?(809|829|849)\d{7}$");

                if (!regex.IsMatch(phoneNumber))
                {
                    return new ValidationResult("El número de teléfono debe ser un número válido de la República Dominicana.ejemplo: 18293439342");
                }
            }
            return ValidationResult.Success;
        }
    }
}
