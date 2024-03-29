﻿using Backend.Models;
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

        public async Task AddTask(ToDoDTO taskDTO)
        {
            try
            {
                var task = new Models.ToDo
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

        public async Task<ToDoDTO> GetTask(int id)
        {
            try
            {
                var taskEntity = await _context.Tasks.FindAsync(id);

                if (taskEntity == null)
                {
                    throw new Exception($"No se encontró ninguna tarea con el ID {id}.");
                }

                return new ToDoDTO
                {
                    TasksId = taskEntity.TasksId,
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


        public async Task<List<ToDoDTO>> GetTasks()
        {
            try
            {
                var tasks = await _context.Tasks
                    .Select(t => new ToDoDTO
                    {
                        TasksId = t.TasksId,
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


        public async Task<List<ToDoDTO>> SearchTask(string title)

        {
            try
            {
                var results = await _context.Tasks

                    .Where(t => t.Title.Contains(title))
                    .Select(t => new ToDoDTO
                    {
                        // Mapea los campos necesarios de Task a TaskDTO
                        TasksId = t.TasksId,
                        UserId = t.UserId,
                        Title = t.Title,
                        Description = t.Description,
                        IsCompleted = t.IsCompleted,
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


        public async Task UpdateTask(int id, ToDoDTO updatedTask)
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
