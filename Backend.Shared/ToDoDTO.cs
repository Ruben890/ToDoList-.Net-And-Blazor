using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Backend.Shared
{
    public class ToDoDTO
    {

        public int TasksId { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido.") ]
        public string Title { get; set; } = null!;

        [Required(ErrorMessage = "El campo {0} es requerido.")]
        public string Description { get; set; } = null!;

        public bool IsCompleted { get; set; } = false;
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "El campo {0} es requerido.")]
        public int? UserId { get; set; }
        public DateTime? DateCreated { get; set; } = DateTime.Now;
    }
}
