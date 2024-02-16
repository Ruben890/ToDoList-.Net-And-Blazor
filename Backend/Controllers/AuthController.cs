using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers
{
    [Route("api/Auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {








        private bool IsValidEmail(string Email)
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
