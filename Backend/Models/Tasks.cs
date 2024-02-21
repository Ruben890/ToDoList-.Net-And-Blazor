using System;
using System.Collections.Generic;

namespace Backend.Models;

public partial class Tasks
{
    public int TasksId { get; set; }

    public string Title { get; set; } = null!;

    public string Description { get; set; } = null!;

    public bool IsCompleted { get; set; }

    public DateTime? DateCreated { get; set; }

    public int? UserId { get; set; }

    public virtual User? User { get; set; }
}
