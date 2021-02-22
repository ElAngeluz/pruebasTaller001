
namespace TodoApi.Controllers
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Logging;
    using Todo.Domain.Tareas;
    using TodoApi.Models;
    using TodoApi.Repositorio;

    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public class TodoItemsController : ControllerBase
    {
        private readonly TodoContext _context;
        private readonly IPrueba Prueba;
        private readonly ILogger<TodoItemsController> Logger;

        public TodoItemsController(TodoContext context,
                                   IPrueba prueba,
                                   ILogger<TodoItemsController> logger)
        {
            _context = context;
            Prueba = prueba;
            Logger = logger;
        }

        /// <summary>
        /// retorna todos los items activos de la base de datos
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        
        public async Task<ActionResult<IEnumerable<TodoItem>>> GetTodoItems() => Ok(await Prueba.GetAllAsync());

              
        [HttpGet("{id}")]
        public async Task<ActionResult<TodoItem>> GetTodoItem(long id)
        {
            Logger.LogInformation("Se procede a consulktar la tarea {ID}", id);
            var todoItem = await _context.TodoItems.FindAsync(id);

            if (todoItem == null)
            {
                return NotFound();
            }

            (bool Success, object Data) = Prueba.Metodo("prueba");

            ActionResult valor = Success
                ? Ok(Data)
                : BadRequest(Data);

            return Success ? Ok(Data) : (ActionResult<TodoItem>)BadRequest(Data);

        }

        // PUT: api/TodoItems/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id">identificacdor del registro</param>
        /// <param name="todoItem">entidad a modificar</param>
        /// <returns></returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTodoItem(long id, TodoItem todoItem)
        {
            if (id != todoItem.Id)
            {
                return BadRequest();
            }

            _context.Entry(todoItem).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TodoItemExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/TodoItems
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        // POST: api/TodoItems
        [HttpPost]
        [ProducesResponseType(typeof(TodoItem), StatusCodes.Status200OK)]
        public async Task<ActionResult<TodoItem>> PostTodoItem(TodoItem todoItem)
        {
            _context.TodoItems.Add(todoItem);
            await _context.SaveChangesAsync();

            //return CreatedAtAction("GetTodoItem", new { id = todoItem.Id }, todoItem);
            return CreatedAtAction(nameof(GetTodoItem), new { id = todoItem.Id }, todoItem);
        }

        // DELETE: api/TodoItems/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTodoItem(long id)
        {
            var todoItem = await _context.TodoItems.FindAsync(id);
            if (todoItem == null)
            {
                return NotFound();
            }

            _context.TodoItems.Remove(todoItem);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool TodoItemExists(long id)
        {
            return _context.TodoItems.Any(e => e.Id == id);
        }
    }
}
