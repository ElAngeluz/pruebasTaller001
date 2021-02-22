namespace TodoApi.Models
{
    using Microsoft.EntityFrameworkCore;
    using Todo.Domain.Tareas;

    public class TodoContext : DbContext
    {
        public TodoContext(DbContextOptions<TodoContext> options)
            : base(options)
        {
            
        }

        public DbSet<TodoItem> TodoItems { get; set; }
    }
}
