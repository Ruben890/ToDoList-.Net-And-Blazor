using Backend.Shared;
using System.Net.Http.Json;
using System.Security.Cryptography.X509Certificates;
using Microsoft.JSInterop;
using System.Text;
using Newtonsoft.Json.Linq;
using System.Net.Http.Headers;


namespace Frontend.Services
{
    public class RequestService : IRequestService
    {
      
        public string BaseAddres;
        private readonly HttpClient _httpClient;
        private readonly IJSRuntime _jsRuntime;

        public RequestService(HttpClient http, IJSRuntime jsRuntime)
        {
            BaseAddres = http.BaseAddress!.ToString() + "api/";
            _httpClient = http;
            _jsRuntime = jsRuntime;
        }

        public async Task<string> GetAllAsync(string action)
        {
            try
            {
                var response = await _httpClient.GetAsync(BaseAddres + action);
                response.EnsureSuccessStatusCode();
                return await response.Content.ReadAsStringAsync();
            }
            catch (Exception ex)
            {
                throw new Exception($"ha ocurrido un error al motrar los datos:{ex.Message}");
            }
        }

        public async Task<string> GetAsync(int id, string action)
        {
            try
            {
                var url = BaseAddres + action + $"/{id}";
                var reponse = await _httpClient.GetAsync(url);
                reponse.EnsureSuccessStatusCode();
                return await reponse.Content.ReadAsStringAsync();
            }
            catch (Exception ex)
            {
                throw new Exception($"ha ocurido un error al motrar el dato: {ex.Message}");
            }
        }


        public async Task<string> SearchToDoTitleAsync(string query, string action)
        {
            try
            {
                var url = $"{BaseAddres}{action}?title={query}";
                var response = await _httpClient.GetAsync(url);
                response.EnsureSuccessStatusCode();
                return await response.Content.ReadAsStringAsync();

            }
            catch (Exception ex)
            {
                throw new Exception($"Ha ocurrido un error al buscar un toDo por el titulo:{ex.Message}");
            }
        }

        public async Task<(bool isSuccess, string responseContent)> PostAsync(string jsonObject, string action)
        {
            try
            {
                var content = new StringContent(jsonObject, UnicodeEncoding.UTF8, "application/json");
                var postResult = await _httpClient.PostAsync(BaseAddres + action, content);
                var responseContent = await postResult.Content.ReadAsStringAsync();
                return (postResult.IsSuccessStatusCode, responseContent);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al agregar los datos: {ex.Message}");
            }
        }


        public async Task<bool> PutAsync(int id, string jsonObject, string action)
        {
            try
            {

                var url = $"{BaseAddres}{action}/{id}";
                var content = new StringContent(jsonObject, UnicodeEncoding.UTF8, "application/json");
                var PuthResult = await _httpClient.PutAsync(url, content);
                var PutContent = await PuthResult.Content.ReadAsStringAsync();
                if (PuthResult.IsSuccessStatusCode)
                {
                    return false;
                }

                return true;
            }
            catch (Exception ex)
            {
                throw new Exception($"ha ocurrido un error al actualizar la data:{ex.Message}");
            }
        }


        public async Task<bool> DeleteAsync(int id, string action)
        {
            try
            {

                var url = $"{BaseAddres}{action}/{id}";
                var DeleteResult = await _httpClient.DeleteAsync(url);
                if (DeleteResult.IsSuccessStatusCode)
                {
                    return false;
                }
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception($"Ha ocurrido un error al borra el dato{ex.Message}");
            }
        }

        public async Task CreateCookies(string name, string value, int daysToExpire)
        {
            try
            {
                var module = await _jsRuntime.InvokeAsync<IJSObjectReference>("import", "./js/Cookies.js");
                await module.InvokeVoidAsync("AddCookies", name, value, daysToExpire);
            }
            catch (Exception ex)
            {
                throw new Exception("error al agregar la cookie" + ex.Message);
            }
        }
        public async Task<string> GetCookie(string nameCookie)
        {
            try
            {
                var module = await _jsRuntime.InvokeAsync<IJSObjectReference>("import", "./js/Cookies.js");
                var data = await module.InvokeAsync<string>("GetCookie", nameCookie);

                if (string.IsNullOrWhiteSpace(data))
                {
                    return "No se ha encontrado la cookie solicitada";
                }

                return data;
            }
            catch (Exception ex)
            {
                throw new Exception("error al obtener la cookie" + ex.Message);
            }
        }


        public void SetAuthorizationToken(string authorizationToken)
        {
            if (!string.IsNullOrEmpty(authorizationToken))
            {
                if (_httpClient.DefaultRequestHeaders.Contains("Authorization"))
                {
                    _httpClient.DefaultRequestHeaders.Remove("Authorization");
                }
                _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {authorizationToken}");
            }
        }



    }
}
