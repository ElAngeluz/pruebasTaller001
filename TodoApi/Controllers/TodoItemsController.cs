using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
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
        private readonly ILogger<TodoItemsController> Logger;

        public TodoItemsController(TodoContext context, IPrueba prueba, ILogger<TodoItemsController> logger)
        {
            _context = context;
            Prueba = prueba;
            Logger = logger;
        }

        // GET: api/TodoItems
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TodoItem>>> GetTodoItems() => Ok(await Prueba.GetAllAsync());

        // GET: api/TodoItems/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Tuple<bool, TodoItem>>> GetTodoItem(long id)
        {
            var (Success, Data) = await Prueba.GetIdAsync(id);

            return Success
                ? Ok(Data)
                : NotFound();

        }

         public class responsedet
        {
            public bool Error { get; set; }
            public object Data { get; set; }
        }

        // PUT: api/TodoItems/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut]
        public async Task<IActionResult> PutTodoItem(TodoItem todoItem) => Ok(await Prueba.PutIdAsync(todoItem));

        // POST: api/TodoItems
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        // POST: api/TodoItems
        [HttpPost]
        public async Task<ActionResult<TodoItem>> PostTodoItem(TodoItem todoItem) => (await Prueba.PostAllAsync(todoItem));

        // DELETE: api/TodoItems/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<TodoItem>> DeleteTodoItem(long id) => Ok(await Prueba.DeleteAllAsync(id));
        
        
        public class ErrorApiResponse
        {
            public string Mensaje { get; set; }
            public string ErrorHttp { get; set; }
        }
        public class SuccessApiResponse
        {
            public dynamic Link { get; set; }
            public object Object { get; set; }
        }
        

    }
}
