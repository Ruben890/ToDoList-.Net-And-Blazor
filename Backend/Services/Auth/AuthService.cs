using Backend.Models;
using Backend.Services.Auth;
using Backend.Services.JWT;
using Backend.Shared;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Security.Cryptography;
namespace Backend.Services.Users
{
    public class AuthService:IAuthService
    {
        private readonly ToDoListContext _context;
        private readonly JWTServices _jwtServices;
        public AuthService(ToDoListContext toDoListContext, JWTServices jWTServices)
        {
            _context = toDoListContext;
            _jwtServices = jWTServices;
        }

        public async Task AddUser(UserDTO userDTO)
        {
            try
            {
                string hashedPassword = PasswordHasher(userDTO.Password); 

                var userEntity = new User
                {
                    Name = userDTO.Name,
                    LastName = userDTO.LastName,
                    Email = userDTO.Email,
                    Password = hashedPassword,
                };

                _context.Add(userEntity);
                await _context.SaveChangesAsync();

            }
            catch (DbUpdateException ex)
            {
                throw new Exception("Ha ocurrido un error al agregar el Usuario" + ex.Message, ex);
            }
            catch (Exception ex)
            {
                throw new Exception("Ha ocurrido un error inesperado al agregar un nuevo Usuario" + ex.Message, ex);
            }
        }


        public async Task<UserDTO> GetMyUser(ClaimsPrincipal user)
        {
            try
            {
                var email = user.FindFirst(ClaimTypes.Email)?.Value;


                if (email != null)
                {
                    var userData = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);

                    if (userData != null)
                    {
                        var userDTO = new UserDTO
                        {
                            Name = userData.Name,
                            LastName = userData.LastName,
                            Email = userData.Email
                        };

                        return userDTO;
                    }
                }

                return null;
            }
            catch (Exception ex)
            {
                throw new Exception("Ha ocurrido un error al obtener el usuario: " + ex.Message, ex);
            }
        }


        public async Task<string> SingIn(SingInDTO singInDTO)
        {
            try
            {
                var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == singInDTO.Email);
                if (user != null)
                {
                    bool isPasswordCorrect = VerifyPassword(singInDTO.Password, user.Password);

                    if (isPasswordCorrect)
                    {
                        string jwtToken = _jwtServices.CreateJwt(singInDTO.Email);
                        return jwtToken;
                    }
                }
            
                return null;
            }
            catch (Exception ex)
            {
                throw new Exception("Error interno del servidor: " + ex.Message, ex);
            }
        }


        private string PasswordHasher(string password)
        {
            // Generar una sal aleatoria
            byte[] salt;
            using (var rng = RandomNumberGenerator.Create())
            {
                salt = new byte[16];
                rng.GetBytes(salt);
            }

            // Derivar la clave de la contraseña utilizando PBKDF2
            using (var pbkdf2 = new Rfc2898DeriveBytes(password, salt, 10000))
            {
                byte[] hash = pbkdf2.GetBytes(20);

                // Combinar la sal y el hash en una sola cadena
                byte[] combinedBytes = new byte[salt.Length + hash.Length];
                Array.Copy(salt, 0, combinedBytes, 0, salt.Length);
                Array.Copy(hash, 0, combinedBytes, salt.Length, hash.Length);

                // Convertir la cadena combinada a base64
                string hashedPassword = Convert.ToBase64String(combinedBytes);

                return hashedPassword;
            }
        }

        private bool VerifyPassword(string password, string hashedPassword)
        {
            // Convertir la cadena base64 de vuelta a bytes
            byte[] combinedBytes = Convert.FromBase64String(hashedPassword);

            // Extraer la sal y el hash de la cadena combinada
            byte[] salt = new byte[16];
            byte[] hash = new byte[combinedBytes.Length - salt.Length];
            Array.Copy(combinedBytes, 0, salt, 0, salt.Length);
            Array.Copy(combinedBytes, salt.Length, hash, 0, hash.Length);

            // Derivar la clave de la contraseña utilizando PBKDF2
            using (var pbkdf2 = new Rfc2898DeriveBytes(password, salt, 10000))
            {
                byte[] computedHash = pbkdf2.GetBytes(20);

                // Comparar los hashes calculados
                for (int i = 0; i < hash.Length; i++)
                {
                    if (hash[i] != computedHash[i])
                    {
                        return false;
                    }
                }
                return true;
            }
        }



    }
}
