using Backend.Shared;
using Frontend.Services.Tasks;
using Microsoft.JSInterop;
using Microsoft.VisualBasic;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace Frontend.Services.Task
{
    public class TaskService : ITaskService
    {
        private readonly HttpClient _httpClient;
        private readonly IJSRuntime _jsRuntime;

        public TaskService(HttpClient httpClient, IJSRuntime jSRuntime)
        {
            _httpClient = httpClient;
            _jsRuntime = jSRuntime;

        }

        public async Task<string> AddTask(TaskDTO task)
        {
            if (!await EnsureValidToken())
            {
                return "User needs to log in again.";
            }


            var response = await _httpClient.PostAsJsonAsync("/api/Tasks/AddTask", task);
            if (response.IsSuccessStatusCode)
            {
                string result = await response.Content.ReadAsStringAsync();
                return result;
            }

            return $"Failed to add task. Status code: {response.StatusCode}";


        }


        public async Task<TaskDTO> GetTask(int id)
        {
            // Ensure that the user has a valid token
            if (!await EnsureValidToken())
            {
                throw new Exception("User needs to log in again.");
            }

            try
            {
                var response = await _httpClient.GetFromJsonAsync<TaskDTO>($"/api/Tasks/{id}");

                return response;
            }
            catch (HttpRequestException ex)
            {

                throw new Exception("Failed to retrieve task. Please check your internet connection and try again.", ex);
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while retrieving the task.", ex);
            }
        }


        public async Task<List<TaskDTO>> GetTasks()
        {
            if (!await EnsureValidToken())
            {
                throw new Exception("User needs to log in again.");
            }

            var response = await _httpClient.GetAsync("/api/Tasks");

            if (response.IsSuccessStatusCode)
            {
                string JsonContent = await response.Content.ReadAsStringAsync();
                var tasks = JsonConvert.DeserializeObject<List<TaskDTO>>(JsonContent);

                return tasks;

            }

            throw new HttpRequestException($"Failed to fetch tasks data. Status code: {response.StatusCode}");


        }

        public async Task<List<TaskDTO>> SearchTask(string title)
        {
            if (!await EnsureValidToken())
            {
                throw new Exception("User needs to log in again.");
            }

            if (!string.IsNullOrEmpty(title))
            {
                try
                {
                    var response = await _httpClient.GetFromJsonAsync<List<TaskDTO>>($"/api/Tasks/Search?title={title}");
                    return response;
                }
                catch (Exception ex)
                {
                    throw new Exception("An error occurred while searching for tasks.", ex);
                }
            }

            throw new ArgumentException("The title cannot be null or empty.");
        }


        public async Task<string> DeleteTask(int id)
        {

            if (!await EnsureValidToken())
            {
                throw new Exception("User needs to log in again.");
            }

            try
            {

                var response = await _httpClient.DeleteAsync($"/api/Tasks/{id}");

                if (response.IsSuccessStatusCode)
                {
                    return "Task deleted successfully.";
                }
                else
                {
                    return $"Failed to delete task. Status code: {response.StatusCode}";
                }
            }
            catch (HttpRequestException ex)
            {
                throw new Exception("Failed to delete task. Please check your internet connection and try again.", ex);
            }
            catch (Exception ex)
            {

                throw new Exception("An error occurred while deleting the task.", ex);
            }
        }

        public async Task<string> UpdateTask(int id, TaskDTO task)
        {

            if (!await EnsureValidToken())
            {
                throw new Exception("User needs to log in again.");
            }

            try
            {

                var response = await _httpClient.PutAsJsonAsync($"/api/Tasks/{id}", task);


                if (response.IsSuccessStatusCode)
                {
                    return "Task updated successfully.";
                }
                else
                {

                    return $"Failed to update task. Status code: {response.StatusCode}";
                }
            }
            catch (HttpRequestException ex)
            {

                throw new Exception("Failed to update task. Please check your internet connection and try again.", ex);
            }
            catch (Exception ex)
            {

                throw new Exception("An error occurred while updating the task.", ex);
            }
        }


        private async Task<bool> EnsureValidToken()
        {
            // Leer el token de las cookies utilizando JavaScript interop
            string token = await _jsRuntime.InvokeAsync<string>("document.getCookie", "AuthToken");
            if (string.IsNullOrEmpty(token))
            {
                return false;
            }

            // Configurar el encabezado de autorización con el token recibido
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            return true;
        }
    }
}
