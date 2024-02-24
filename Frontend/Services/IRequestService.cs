using Microsoft.JSInterop;

namespace Frontend.Services
{
    public interface IRequestService
    {
        Task CreateCookies(string name, string value, int daysToExpire);
        Task<string> GetCookie(string nameCookie);

        void SetAuthorizationToken(string authorizationToken);

        Task<bool> DeleteAsync(int id, string action);
        Task<string> GetAllAsync(string action);
        Task<string> GetAsync(int id, string action);
        Task<(bool isSuccess, string responseContent)> PostAsync(string jsonObject, string action);
        Task<bool> PutAsync(int id, string jsonObject, string action);
        
    }
}
