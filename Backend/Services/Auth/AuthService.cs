using Backend.Models;
using Backend.Services.Auth;
using Backend.Shared;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
namespace Backend.Services.Users
{
    public class AuthService:IAuthService
    {
        private readonly ToDoListContext _context;
        public AuthService(ToDoListContext toDoListContext)
        {
            _context = toDoListContext;
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


        public Task<UserDTO> GetMyUser(int userId)
        {
            throw new NotImplementedException();
        }

        public async Task<string> SingIn(SingInDTO singInDTO)
        {
            try
            {
                var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == singInDTO.Email);
                if (user != null)
                {
                    bool isPasswordCorrect = VerifyPassword(singInDTO.Password, user.Password);

                    if (!isPasswordCorrect)
                    {
                        return "La contraseña es incorrecta";
                    }


                }
                else
                {
                    return "El usuario no existe";
                }

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

                // Combinar la sal y el hash
                byte[] hashBytes = new byte[36];
                Array.Copy(salt, 0, hashBytes, 0, 16);
                Array.Copy(hash, 0, hashBytes, 16, 20);

                // Convertir a una cadena hexadecimal en lugar de base64
                string hashedPassword = BitConverter.ToString(hashBytes).Replace("-", "").ToLower();

                return hashedPassword;
            }
        }


        private bool VerifyPassword(string password, string hashedPassword)
        {
            // Extraer la sal y el hash de la contraseña almacenada
            byte[] hashBytes = Convert.FromBase64String(hashedPassword);
            byte[] salt = new byte[16];
            Array.Copy(hashBytes, 0, salt, 0, 16);

            // Derivar la clave de la contraseña utilizando PBKDF2
            using (var pbkdf2 = new Rfc2898DeriveBytes(password, salt, 10000))
            {
                byte[] hash = pbkdf2.GetBytes(20);

                // Comparar el hash calculado con el hash almacenado
                for (int i = 0; i < 20; i++)
                {
                    if (hashBytes[i + 16] != hash[i])
                    {
                        return false;
                    }
                }
                return true;
            }
        }

    }
}
