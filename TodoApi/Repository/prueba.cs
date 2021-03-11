﻿using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Todo.domain.soloopera;
using TodoApi.Models;
using Microsoft.AspNetCore.Mvc;
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
        /// <param name="id">Identificador de registro</param>
        /// <returns></returns>        
        Task<(bool Success, object Data)> GetIdf(long id);

        /// <summary>
        /// </summary>
        /// <param name="todoItem"></param>
        /// <returns></returns>
        Task<ActionResult<TodoItem>> PostAllAsync(TodoItem todoItem);

        Task<ActionResult<TodoItem>> DeleteAllAsync(long id);
        Task<(bool Success, object Data)> PutIdAsync(TodoItem todoItem);

    }
    public class PruebaRepository : IPrueba
    {
        private readonly ILogger<PruebaRepository> Logger;
        private readonly TodoContext _context;

        public PruebaRepository(ILogger<PruebaRepository> logger, TodoContext context)//constructor 
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

        public async Task<(bool Success, object Data)> GetIdf(long idt)
        {
            try
            {
                Logger.LogInformation("Se procede a consultar el id: {ID}", idt);
                TodoItem data = await _context.TodoItems.FindAsync(idt);
                return data == null
                    ? (true, data)
                    : (false, new Responsedet() { Error = false, Data = data });
            }
            catch (ArgumentException Ex) {
                Logger.LogError(Ex, "Se produjo un error en {metodo}", nameof(GetIdf));
                return (false, new Responsedet() { Error = false, Data = "El id es nulo"}); 
            }
            catch (Exception Ex)
            {
                Logger.LogCritical(Ex, "Se produjo un error");
                return (false, new Responsedet() { Error = false, Data = "Se produjo un error general." });
            }
        }

        public async Task<(bool Success, object Data)> PutIdAsync(TodoItem data)
        {
            try
            {
                if (data is null)
                {
                    Logger.LogError("Se produjo un error en {metodo}", nameof(PutIdAsync));
                    return (false, new Responsedet() { Error = false, Data = "El objeto enviado no es valido" }); 
                }
                else if (data.Id == 0)
                {
                    Logger.LogError("Se produjo un error de id=0");
                    return (false, new Responsedet() { Error = false, Data = "el id ingresado no es valido" });
                }
                else
                {
                    _context.Entry(data).State = EntityState.Modified;
                    await _context.SaveChangesAsync();
                    Logger.LogInformation("Se produjo una modificación");
                    return (true, data);
                }
            }
            catch (DbUpdateConcurrencyException ex)
            {
                Logger.LogError(ex, "Se produjo un error en {metodo}", nameof(PutIdAsync));
                return (false, new Responsedet() { Error = false, Data = "No podemos realizar la actualización" });
                
            }
            catch (Exception Ex)
            {
                Logger.LogCritical(Ex, "Se produjo un error");
                return (false, new Responsedet() { Error = false, Data = "Se produjo un error general." });
            }
        }

        public async Task<ActionResult<TodoItem>> PostAllAsync(TodoItem todoItem)
        {
            try
            {
                Logger.LogInformation("Se procede a ingresar un Usuario {todoItem}", todoItem);
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
            var todoItem = await _context.TodoItems.FindAsync(id);
            //var todoItem = new TodoItem();
            try
            {
                if (todoItem == null)
                {
                    return new NotFoundResult();
                }
                _context.Entry(todoItem).State = EntityState.Modified;
                //_context.TodoItems.Remove(todoItem.Name);
                await _context.SaveChangesAsync();

            }
            catch (Exception Ex)
            {
                Logger.LogError(Ex, "Se produjo un error en: {Ex}");
                throw;
            }
            return todoItem;
        }

    }
}
