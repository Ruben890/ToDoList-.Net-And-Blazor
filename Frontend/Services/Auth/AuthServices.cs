using Backend.Shared;
using Microsoft.JSInterop;
using System.Net.Http.Json;
using System.Net.Http.Headers;
using Newtonsoft.Json;
namespace Frontend.Services.Auth
{
    public class AuthServices : IAuthService
    {

        private readonly HttpClient _httpClient;
        private readonly IJSRuntime _jsRuntime;

        public AuthServices(HttpClient httpClient, IJSRuntime jsRuntime)
        {
            _httpClient = httpClient;
            _jsRuntime = jsRuntime;
        }


        public async Task<string> AddUser(UserDTO userDTO)
        {
            var response = await _httpClient.PostAsJsonAsync("/api/Auth/Register", userDTO);
            if (response.IsSuccessStatusCode)
            {
                string result = await response.Content.ReadAsStringAsync();
                return result;
            }

            // Manejar el caso en que la solicitud no fue exitosa
            // Por ejemplo, lanzar una excepción o devolver un valor predeterminado
            throw new HttpRequestException($"Failed to add user. Status code: {response.StatusCode}");




        }

        public async Task<UserDTO> GetMyUser()
        {
            // Importar el módulo JavaScript que contiene la función getCookies
            var module = await _jsRuntime.InvokeAsync<IJSObjectReference>("import", "./js/Cookies.js");
            // Invocar la función JavaScript GetCookie para obtener el valor de la cookie "AuthToken"
            var token = await module.InvokeAsync<string>("GetCookie", "AuthToken");



            if (string.IsNullOrEmpty(token))
            {
                throw new Exception("Token not found in cookies.");
            }

            // Configurar el encabezado de autorización con el token recibido
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            // Realizar la solicitud GET a la API para obtener los datos del usuario
            var response = await _httpClient.GetAsync("/api/Auth/myUser");

            // Verificar si la solicitud fue exitosa
            if (response.IsSuccessStatusCode)
            {
                // Leer el contenido de la respuesta como una cadena JSON
                string jsonContent = await response.Content.ReadAsStringAsync();

                // Deserializar la cadena JSON en un objeto UserDTO
                UserDTO user = JsonConvert.DeserializeObject<UserDTO>(jsonContent);
                return user;
            }

            // Manejar el caso en que la solicitud no fue exitosa
            // Por ejemplo, lanzar una excepción o devolver un valor predeterminado
            throw new HttpRequestException($"Failed to fetch user data. Status code: {response.StatusCode}");

        }

        public async Task<string> SingIn(SingInDTO singInDTO)
        {


            var response = await _httpClient.PostAsJsonAsync("/api/Auth/login", singInDTO);

            if (response.IsSuccessStatusCode)
            {
                // Leer el token de la respuesta
                string token = await response.Content.ReadAsStringAsync();

                // Importar el módulo JavaScript que contiene la función getCookies
                var module = await _jsRuntime.InvokeAsync<IJSObjectReference>("import", "./js/Cookies.js");

                // Guardar el token en una cookie utilizando JavaScript interop
                await module.InvokeVoidAsync("AddCookies", "AuthToken", token, 7);

                return "Se ha logeado corectamente";
            }


            throw new HttpRequestException($"Failed to sign in. Status code: {response.StatusCode}");


        }
    }

}


