using Backend.Shared;
using System.Security.Claims;

namespace Frontend.Services.Auth
{
    public interface IAuthService
    {

        Task<string> AddUser(UserDTO userDTO);
        Task<string> SingIn(SingInDTO singInDTO);

        Task<UserDTO> GetMyUser();
    }
}
