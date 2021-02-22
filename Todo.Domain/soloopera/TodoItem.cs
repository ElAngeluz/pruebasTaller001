namespace Todo.domain.soloopera
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Threading.Tasks;
    public class TodoItem
    {
         public Func<int, int> suma = (a) => a * 2;
         public long Id { get; set; }
         [Required]
         public string Name { get; set; }
         public bool IsComplete { get; set; }

    }
}
