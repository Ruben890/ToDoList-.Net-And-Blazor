using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Shared
{
    internal class SingInDTO
    {
        [Required(ErrorMessage = "El campo {0} es requerido.")]
        public string Email { get; set; } = null!;

        [Required(ErrorMessage = "El campo {0} es requerido.")]
        public string Password { get; set; } = null!;


        public bool IsValidEmail()
        {
            if (string.IsNullOrWhiteSpace(Email))
                return false;

            // Verificar si el correo electrónico contiene un "@" y al menos un "." después del "@"
            int atIndex = Email.IndexOf('@');
            if (atIndex == -1 || atIndex == 0 || Email.IndexOf('.', atIndex) == -1)
                return false;

            return true;
        }

    }
}
