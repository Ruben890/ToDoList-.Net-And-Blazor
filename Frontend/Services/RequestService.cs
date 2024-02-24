using Backend.Shared;
using System.Net.Http.Json;
using System.Security.Cryptography.X509Certificates;
using Microsoft.JSInterop;
using System.Text;
using Microsoft.JSInterop.Implementation;


namespace Frontend.Services
{
    public class RequestService : IRequestService
    {

        public string BaseAddres;
        private readonly HttpClient _httpClient;
        private readonly JSRuntime _runtime;

        public RequestService(HttpClient http, JSRuntime jS)
        {
            BaseAddres = http.BaseAddress!.ToString() + "api/";
            _httpClient = http;
            _runtime = jS;


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

        public async Task<bool> PostAsync(string jsonObject, string action)
        {
            try
            {
                var content = new StringContent(jsonObject, UnicodeEncoding.UTF8, "application/json");
                var postResult = await _httpClient.PostAsync(BaseAddres + action, content);
                var postContent = await postResult.Content.ReadAsStringAsync();
                if (!postResult.IsSuccessStatusCode)
                {
                    return false;
                }

                return true;
            }
            catch (Exception ex)
            {

                throw new Exception($"error al agregar los datos:{ex.Message}");
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


        public async Task<bool> CreateCookies(string name, int value, int daysToExpire)
        {
            var model = await _runtime.InvokeAsync<JSObjectReference>("import", "./js/Cookies");
            var cookies = await _runtime.InvokeAsync<bool>("AddCookies", name, value, daysToExpire);

            if (!cookies)
            {
                return false;
            }

            return true;

        }

        public async Task<string> GetCookie(string nameCookie) 
        {
            var model = await _runtime.InvokeAsync<JSObjectReference>("import", "./js/Cookies");
            var token = await model.InvokeAsync<string>("GetCookie", nameCookie);

            if (string.IsNullOrWhiteSpace(token))
            {
                return "No se ha encontrado la cookies colisitada";
            }


            return token;
        } 
    }
}
