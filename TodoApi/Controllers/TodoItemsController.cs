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
using Nest;

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
            _context = context;
            Prueba = prueba;
        }

        // GET: api/TodoItems
        [HttpGet]
        public async Task<ActionResult> GetTodoItems() => Ok(await Prueba.GetAllAsync());

        // GET: api/TodoItems/5
        [HttpGet("{id}")]
        public async Task<ActionResult> GetTodoItem(long id)
        {
            var (Success, Data) = await Prueba.GetIdf(id);

            if (Data is TodoItem todo)
            {
                todo.IsComplete = false;
            }

            return Success
                ? Ok(Data)
                : BadRequest(Data);

        }

        public class Responsedet
        {
            public bool Error { get; set; }
            public object Data { get; set; }
        }

        // PUT: api/TodoItems/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut]
        public async Task<ActionResult> PutTodoItem(TodoItem data) 
        {
            var (Success, Data) = await Prueba.PutIdAsync(data);

            if (Data is TodoItem todo)
            {
                todo.IsComplete = false;
            }

            return Success
                ? Ok(Data)
                : BadRequest(Data);
        }            

        // POST: api/TodoItems
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        // POST: api/TodoItems
        [HttpPost]
        //public async Task<ActionResult> PostTodoItem(TodoItem todoItem) => Ok(await Prueba.PostAllAsync(todoItem));
        public async Task<ActionResult> PostTodoItem(TodoItem todoItem)
        {
            var (Succes, Data) = await Prueba.PostAllAsync(todoItem);
            if(Data is TodoItem todo)
            {
                todo.IsComplete = false;
            }
            return Succes
                ? Ok(Data)
                : BadRequest(Data);
        }
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
