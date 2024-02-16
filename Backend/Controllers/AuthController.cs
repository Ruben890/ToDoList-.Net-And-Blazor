using Backend.Services.Users;
using Backend.Shared;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
            return Ok("Se ha registrado exitosamente");
        
        }


        [HttpPost("login")]
        public async Task<ActionResult<string>> AuthLogin(SingInDTO singInDTO)
        {
            var data = singInDTO;
<<<<<<< HEAD
            var response = await _authService.SingIn(data);

            return Ok(response);
=======

            return Ok();
>>>>>>> 2ef309c2080be09bd8e85c52156528790734abed
        
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
