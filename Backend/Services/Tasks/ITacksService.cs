using Backend.Shared;
namespace Backend.Services.Tasks
{
    public interface ITacksService
    {

        Task<List<TaskDTO>> GetTasks();
        Task<TaskDTO> GetTask(int id);
        Task AddTask(TaskDTO task);
        Task DeleteTask(int id);
        Task UpdateTask(int id, TaskDTO task);


        Task<List<TaskDTO>> SearchTask(string title);


        
       
    }
}
