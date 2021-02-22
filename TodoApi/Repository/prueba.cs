using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Todo.domain.soloopera;
using TodoApi.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;



namespace TodoApi.Repository
{

    public interface IPrueba
    {
        /// <summary>
        /// Funcion que consulta todas las tareas 
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<TodoItem>> GetAllAsync();
        /// <summary>
        /// Funcion de prueba
        /// </summary>
        /// <param name="parametro">parametro de entrada</param>
        
        Task<ActionResult<TodoItem>> GetIdAsync(long id);
        Task<ActionResult<TodoItem>> PostAllAsync(TodoItem todoItem);

        Task<IActionResult> DeleteAllAsync(long id);
        Task<IActionResult> PutIdAsync(long id, TodoItem todoItem);
        (bool Success, object Data) Metodo(string parametro);
    }
    public class Prueba : IPrueba
    {
        private readonly ILogger<Prueba> Logger;
        private readonly TodoContext _context;

        public Prueba(ILogger<Prueba> logger, TodoContext context)//constructor 
        {
            Logger = logger;
            _context = context;
        }

        public async Task<IEnumerable<TodoItem>> GetAllAsync()
        {
            try
            {
                return await _context.TodoItems.ToListAsync();
            }
            catch (Exception Ex)
            {
                Logger.LogError("Se produjo un error en: {Ex}", Ex);
                throw;
            }
        }

        public async Task<ActionResult<TodoItem>> GetIdAsync(long id)
        {
            Logger.LogInformation("Se procede a consultar la tarea {ID}", id);
            var todoItem = await _context.TodoItems.FindAsync(id);

            if (todoItem == null)
            {
                var not = new NotFoundResult();
                return not;
            }

            return todoItem;
        }

        public async Task<IActionResult> PutIdAsync(long id, TodoItem todoItem)
        {
            if (id != todoItem.Id)
            {
                var bad = new BadRequestResult();
                return bad;
            }

            _context.Entry(todoItem).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (todoItem.Id == 0)//revisar
                {
                    var not = new NotFoundResult();
                    return not;
                }
                else
                {
                    throw;
                }
            }
            var noconten = new NoContentResult();
            return noconten;
        }
        public async Task<ActionResult<TodoItem>> PostAllAsync(TodoItem todoItem)
        {
            try
            {
                Logger.LogInformation("Se procede a ingresar un Usuario {todoItem}",todoItem);
                _context.TodoItems.Add(todoItem);
                await _context.SaveChangesAsync();

                return todoItem;
            }
            catch (Exception Ex)
            {
                Logger.LogError("Se produjo un error en: {Ex}", Ex);
                throw;
            }

        }

        public async Task<IActionResult> DeleteAllAsync(long id)
        {
            var todoItem = await _context.TodoItems.FindAsync(id);
            if (todoItem == null)
            {
                var not = new NotFoundResult();
                return not;
            }

            _context.TodoItems.Remove(todoItem);
            await _context.SaveChangesAsync();

            var noconten = new NoContentResult();
            return noconten;
        }

        public (bool Success, object Data) Metodo(string parametro)
        {
            try
            {
                return (true, null);
            }
            catch (TimeoutException Ex)
            {
                Logger.LogCritical(Ex, "Error inesperado en metodo 1, cuando se hacia la operación x{parametro}", parametro);
                return (false, null);
            }
        }
    }
}
