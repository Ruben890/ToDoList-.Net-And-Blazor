using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToDoList.Shared
{
    internal class TasksDTO
    {
        public string Title { get; set; } = null!;

        public string Description { get; set; } = null!;

        public bool? IsCompleted { get; set; }

        public int? UserId { get; set; }
    }
}
