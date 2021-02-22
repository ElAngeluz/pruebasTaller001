using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TodoApi.Models;
using Todo.domain.soloopera;
using TodoApi.Repository;
using Microsoft.Extensions.Logging;

namespace TodoApi.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public class TodoItemsController : ControllerBase
    {
        private readonly TodoContext _context;
        private readonly IPrueba Prueba;

        public TodoItemsController(TodoContext context, IPrueba prueba)
        {
            //var todo = new TodoItem();
            //Console.WriteLine(todo.suma(23));
            _context = context;
            Prueba = prueba ;
        }

        // GET: api/TodoItems
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TodoItem>>> GetTodoItems() => Ok(await Prueba.GetAllAsync());

        // GET: api/TodoItems/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TodoItem>> GetTodoItem(long id) => Ok(await Prueba.GetIdAsync(id));

        // PUT: api/TodoItems/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTodoItem(long id, TodoItem todoItem) => Ok(await Prueba.PutIdAsync(id, todoItem));

        // POST: api/TodoItems
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        // POST: api/TodoItems
        [HttpPost]
        public async Task<ActionResult<TodoItem>> PostTodoItem(TodoItem todoItem) => Ok(await Prueba.PostAllAsync(todoItem));

        // DELETE: api/TodoItems/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTodoItem(long id) => Ok(await Prueba.DeleteAllAsync(id));
        

        private bool TodoItemExists(long id)
        {

            (bool Success, object Data) = Prueba.Metodo("sa");

            //if (Success)
            //{
            //    return Ok(Data);
            //}
            //else
            //{
            //    return BadRequest(Data);
            //}

            return _context.TodoItems.Any(e => e.Id == id);
        }
    }
}
