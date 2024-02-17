using Backend.Shared;
using System.Threading.Tasks;
namespace Frontend.Services.Tasks
{
    public interface ITaskService
    {

        Task<List<TaskDTO>> GetTasks();
        Task<TaskDTO> GetTask(int id);
        Task<string> AddTask(TaskDTO task);
         Task<string> DeleteTask(int id);
         Task<string> UpdateTask(int id, TaskDTO task);
        Task<List<TaskDTO>> SearchTask(string title);
    }
}
