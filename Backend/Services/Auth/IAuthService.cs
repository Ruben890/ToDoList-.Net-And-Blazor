using Backend.Shared;
using System.Security.Claims;
namespace Backend.Services.Auth
{
    public interface IAuthService
    {

        Task AddUser(UserDTO userDTO);
        Task<string> SingIn(SingInDTO singInDTO);

        Task<UserDTO> GetMyUser(ClaimsPrincipal user);
    }
}
