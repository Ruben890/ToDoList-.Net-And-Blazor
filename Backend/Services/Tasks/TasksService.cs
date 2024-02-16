using Backend.Models;
using Backend.Shared;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
namespace Backend.Services.Tasks
{
    public class TasksService : ITacksService
    {
        private readonly ToDoListContext _context;
        public TasksService(ToDoListContext toDoListContext)
        {
            _context = toDoListContext;
        }

        public async Task AddTask(TaskDTO task)
        {
            try
            {
                _context.Add(task);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                // Si hay un error de actualización de la base de datos
                throw new Exception("Error al agregar la tarea a la base de datos.", ex);
            }
            catch (Exception ex)
            {
                // Para cualquier otro tipo de error
                throw new Exception("Error desconocido al agregar la tarea.", ex);
            }
        }

        public Task DeleteTask(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<TaskDTO> GetTask(int id)
        {
            try
            {
                var taskEntity = await _context.Tasks.FindAsync(id);

                if (taskEntity == null)
                {
                    throw new Exception($"No se encontró ninguna tarea con el ID {id}.");
                }

                return new TaskDTO
                {
                    UserId = taskEntity.UserId,
                    Title = taskEntity.Title,
                    Description = taskEntity.Description,
                    IsCompleted = taskEntity.IsCompleted
                };
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener la tarea: " + ex.Message, ex);
            }
        }


        public async Task<List<TaskDTO>> GetTasks()
        {
            try
            {
                var tasks = await _context.Tasks
                    .Select(t => new TaskDTO
                    {
                        UserId = t.UserId,
                        Title = t.Title,
                        Description = t.Description,
                        IsCompleted = t.IsCompleted
                    })
                    .ToListAsync();

                if (tasks.Count == 0)
                {
                    throw new Exception("No se encontraron tareas en la base de datos.");
                }

                return tasks;
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener tareas: " + ex.Message, ex);
            }
        }





        public Task UpdateTask(int id, TaskDTO task)
        {
            throw new NotImplementedException();
        }
    }
        
}
