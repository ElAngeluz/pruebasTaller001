namespace Todo.Domain.Tareas
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class TodoItem
    {
        [Key]
        public long Id { get; set; }

        /// <summary>
        /// NOmbre del registro
        /// </summary>
        /// <example>Pedro</example>
        [Required]
        [StringLength(50)]
        [Column("Nombre")]
        
        public string Name { get; set; }
        public bool IsComplete { get; set; }
    }
}
