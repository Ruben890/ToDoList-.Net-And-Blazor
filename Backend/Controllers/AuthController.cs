using Backend.Services.Users;
using Backend.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Text.RegularExpressions;

namespace Backend.Controllers
{
    [Route("api/Auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly AuthService _authService;
        public AuthController(AuthService authService)
        {
            _authService = authService;
        
        }



        [HttpPost("Register")]
        public  async Task<ActionResult> AuthRegsiter(UserDTO userDTO) 
        {
            if (!IsValidEmail(userDTO.Email)) 
            {
                return BadRequest(new { error = "El email no es válido" });
            }

            await _authService.AddUser(userDTO);
            return Ok(new {message = "Se ha registrado exitosamente" });
        
        }


        [HttpPost("login")]
        public async Task<ActionResult<string>> AuthLogin(SingInDTO singInDTO)
        {
            var data = singInDTO;

            var response = await _authService.SingIn(data);

            if(response != null) {
                return Ok(new{token = response });
            }

            return BadRequest(new { error = "Las credenciales proporcionadas son incorrectas. Por favor, verifica tu correo electrónico y contraseña e intenta nuevamente." });

        }

        [HttpGet("myUser")]
        [Authorize]
        public async Task<ActionResult<UserDTO>> GetMyUser() 
        {
            var currentUser = HttpContext.User;
            var userDTO = await _authService.GetMyUser(currentUser);

            if (userDTO != null)
            {
                return Ok(userDTO);
            }

            return NoContent();
        }

        private bool IsValidEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return false;

            // Utilizar una expresión regular para validar el formato del correo electrónico
            string pattern = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";
            Regex regex = new Regex(pattern);

            return regex.IsMatch(email);
        }
    }


    

}
