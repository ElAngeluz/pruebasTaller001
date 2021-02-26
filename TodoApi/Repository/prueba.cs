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
using TodoApi.Controllers;
using static TodoApi.Controllers.TodoItemsController;

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
        /// 
        /// </summary>
        /// <param name="id">hfghfghgdh</param>
        /// <returns></returns>
        Task<ActionResult<Tuple<bool, TodoItem>>> GetIdAsync(long id);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="todoItem"></param>
        /// <returns></returns>
        Task<ActionResult<TodoItem>> PostAllAsync(TodoItem todoItem);

        Task<ActionResult<TodoItem>> DeleteAllAsync(long id);
        Task<IActionResult> PutIdAsync(TodoItem todoItem);
    }
    public class Prueba : IPrueba
    {
        private readonly ILogger<Prueba> Logger;
        private readonly TodoContext _context;
        bool t = true;
        bool f = false;
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

        public async Task<ActionResult<Tuple<bool,TodoItem>>> GetIdAsync(long id)
        { 
            try
            {
                Logger.LogInformation("Se procede a consultar el id: {ID}", id);
                var data = await _context.TodoItems.FindAsync(id);
                var t1 = (saludo: t, destino: data);
                //var t2 = (saludo: t, destino: data);
                //Console.WriteLine("{0} {1}", t1.saludo, t1.destino);
                return data == null ? Tuple.Create(f, data) :Tuple.Create(t1.saludo, t1.destino);
            }
            catch (Exception Ex)
            {
                Logger.LogError(Ex, "Se produjo un error en: {Ex}");
                throw;
            }
        }

        public async Task<IActionResult> PutIdAsync(TodoItem todoItem)
        {
            try
            {
                _context.Entry(todoItem).State = EntityState.Modified;
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (todoItem.Id == 0)
                {
                    return new NotFoundResult();
                }
                else
                {
                    Logger.LogInformation("Se produjo una modificación");
                    throw;
                }
            }
            return new NoContentResult();
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
                Logger.LogError(Ex, "Se produjo un error en: {Ex}");
                throw;
            }

        }

        public async Task<ActionResult<TodoItem>> DeleteAllAsync(long id)
        {
            var _todoItem = await _context.TodoItems.FindAsync(id);
            var todoItem = new TodoItem();
            try
            {
                if (_todoItem == null)
                {
                    return new NotFoundResult();
                }
                _context.Entry(todoItem).State = EntityState.Modified;
                //_context.TodoItems.Remove(todoItem.Name);
                await _context.SaveChangesAsync();

            }
            catch (Exception Ex)
            {
                Logger.LogError(Ex,"Se produjo un error en: {Ex}");
                throw;
            }            
            return todoItem; 
        }

    }
}
