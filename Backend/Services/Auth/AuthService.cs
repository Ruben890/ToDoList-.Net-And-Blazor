using Backend.Models;
using Backend.Services.Auth;
using Backend.Shared;
using Microsoft.EntityFrameworkCore;
using BCrypt.Net;
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
                string hashedPassword = BCrypt.Net.BCrypt.HashPassword(userDTO.Password);

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
            catch(DbUpdateException ex) 
            {
                throw new Exception("Ha ocurrido un error al agregar el Usuario" + ex.Message, ex);
            }
            catch (Exception ex)
            {
                throw new Exception("ha ocurrido un error inesperado al agregar un nuevo Usuario" + ex.Message, ex);
            }
        }

        public Task<UserDTO> GetMyUser(int userId)
        {
            throw new NotImplementedException();
        }

        public Task<string> SingIn(SingInDTO singInDTO)
        {
            throw new NotImplementedException();
        }


        
    }
}
