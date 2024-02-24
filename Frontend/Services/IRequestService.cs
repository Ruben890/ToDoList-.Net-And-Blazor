namespace Frontend.Services
{
    public interface IRequestService
    {
        public Task<string> GetAllAsync(string action);
        public Task<string> GetAsync(int id, string action);
        Task<bool> PostAsync(string jsonObject, string action);
        Task<bool> PutAsync(int id, string jsonobject, string action);
        Task<string> SearchToDoTitleAsync(string query, string action);
    }
}
