using Backend.Shared;
namespace Backend.Services.Tasks
{
    public interface ITacksService
    {

        Task<List<ToDoDTO>> GetTasks();
        Task<ToDoDTO> GetTask(int id);
        Task AddTask(ToDoDTO task);
        Task DeleteTask(int id);
        Task UpdateTask(int id, ToDoDTO task);


        Task<List<ToDoDTO>> SearchTask(string title);

        
       
    }
}
