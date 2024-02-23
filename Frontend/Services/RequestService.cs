

using System.Net.Http.Json;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace Frontend.Services
{
    public class RequestService : IRequestService
    {

        public string BaseAddres;
        private string? ErrorMessage;
        private readonly HttpClient _httpClient;

        public RequestService(HttpClient http)
        {
            BaseAddres = http.BaseAddress!.ToString() + "api/";
            _httpClient = http;


        }

        public async Task<string> GetAllAsync(string action)
        {
            var response = await _httpClient.GetAsync(BaseAddres + action);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }

        public async Task<string> GetAsync(int id, string action)
        {
            var url = BaseAddres + action + $"/{id}";
            var reponse = await _httpClient.GetAsync(url);
            reponse.EnsureSuccessStatusCode();
            return await reponse.Content.ReadAsStringAsync();
        }

        public async Task<bool> PostAsync(string jsonObject, string action)
        {
            try
            {
                var content = new StringContent(jsonObject, UnicodeEncoding.UTF8, "aplication/json");
                var postResult = await _httpClient.PostAsJsonAsync(BaseAddres + action, content);
                var postContent = await postResult.Content.ReadAsStringAsync();
                if (!postResult.IsSuccessStatusCode)
                {
                    return false;
                }

                return true;
            }
            catch (Exception ex)
            {

                throw new Exception($"error al agregar los datos:{ex.Message}", ex);
            }
        }


        public async Task<bool> PutAsync(int id, string jsonObject, string action)
        {
            var url = $"{BaseAddres}{action}/{id}";
            var content = new StringContent(jsonObject, UnicodeEncoding.UTF8, "aplication/json");
            var PuthResult = await _httpClient.PutAsync(url, content);
            var PutContent = await PuthResult.Content.ReadAsStringAsync();
            if (PuthResult.IsSuccessStatusCode)
            {
                return false;
            }

            return true;
        }


        public async Task<bool> DeleteAsync(int id, string action)
        {
            var url = $"{BaseAddres}{action}/{id}";
            var DeleteResult = await _httpClient.DeleteAsync(url);
            if (DeleteResult.IsSuccessStatusCode)
            {
                return false;
            }
            return true;
        }
    }
}
