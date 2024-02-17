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

        public async Task AddTask(TaskDTO taskDTO)
        {
            try
            {
                var task = new Models.Tasks
                {
                    Title = taskDTO.Title,
                    Description = taskDTO.Description,
                    IsCompleted = taskDTO.IsCompleted,
                    UserId = taskDTO.UserId,
                };
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

        public async Task DeleteTask(int id)
        {
            try
            {
                var taskEntity = await _context.Tasks.FindAsync(id);

                if (taskEntity == null)
                {
                    throw new Exception($"No se encontró ninguna tarea con el ID {id}.");
                }

                _context.Tasks.Remove(taskEntity);
                await _context.SaveChangesAsync();

            }
            catch (DbUpdateException ex)
            {
                throw new Exception("Error al borrar la tarea de la db", ex);
            }
            catch (Exception ex)
            {
               
                throw new Exception("Error desconocido al borrar la tarea.", ex);
            }
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
                    IsCompleted = taskEntity.IsCompleted,
                    DateCreated = taskEntity.DateCreated
                    
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
                        IsCompleted = t.IsCompleted,
                        DateCreated = t.DateCreated
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

<<<<<<< HEAD
        public async Task<List<TaskDTO>> SearchTask(string title)
=======
        public async Task<List<TaskDTO>> SearchTask(string title, string description)
>>>>>>> c490b9e29e49f31c2da983f13c5905bb9f4f20e3
        {
            try
            {
                var results = await _context.Tasks
<<<<<<< HEAD
                    .Where(t => t.Title.Contains(title))
=======
                    .Where(t => t.Title.Contains(title) || t.Description.Contains(description))
>>>>>>> c490b9e29e49f31c2da983f13c5905bb9f4f20e3
                    .Select(t => new TaskDTO
                    {
                        // Mapea los campos necesarios de Task a TaskDTO
                        Title = t.Title,
                        Description = t.Description,
                        IsCompleted = t.IsCompleted,
                        UserId = t.UserId,
                        DateCreated = t.DateCreated
                    })
                    .ToListAsync();

                return results;

            }
            catch (Exception ex)
            {
                throw new Exception("Ha ocurrido un error inesperado al buscar las tareas: " + ex.Message, ex);
            }
        }


        public async Task UpdateTask(int id, TaskDTO updatedTask)
        {
            try
            {
                var taskEntity =  await _context.Tasks.FindAsync(id);

                if (taskEntity == null)
                {
                    throw new Exception($"No se encontró ninguna tarea con el ID {id}.");
                }

              
                taskEntity.UserId = updatedTask.UserId;
                taskEntity.Title = updatedTask.Title;
                taskEntity.Description = updatedTask.Description;
                taskEntity.IsCompleted = updatedTask.IsCompleted;

                await _context.SaveChangesAsync();


            }
            catch (DbUpdateException ex)
            {

                throw new Exception("Error al actualizar la tarea en la base de datos.", ex);
            }


            catch (Exception ex) {
                throw new Exception("Error al Actulizar la tarea: " + ex.Message, ex);
            }
        }
    }
        
}
