﻿using Microsoft.EntityFrameworkCore;
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
        /// <param name="id">Identificador de registro</param>
        /// <returns></returns>        
        Task<(bool Success, object Data)> GetIdAsync(long id);

        /// <summary>
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

        public async Task<(bool Success, object Data)> GetIdAsync(long id)
        { 
            try
            {
                Logger.LogInformation("Se procede a consultar el id: {ID}", id);
                TodoItem data = await _context.TodoItems.FindAsync(id);
                return data == null 
                    ? (false, null) 
                    : (true, new responsedet() { Error = false, Data = data });
            }
            catch (ArgumentException Ex){ return (false, null); }
            catch (Exception Ex)
            {
                Logger.LogError(Ex, "Se produjo un error en: {Ex}");
                return (false, null);
            }
        }

        public async Task<IActionResult> PutIdAsync(TodoItem todoItem)
        {
            try
            {
                _context.Entry(todoItem).State = EntityState.Modified;//PREGUNTAR
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                //return todoItem.Id == 0 ? new NotFoundResult(): Logger.LogInformation("Se produjo una modificación");
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
                Logger.LogError(Ex,"Se produjo un error en: {Ex}");
                throw;
            }            
            return todoItem; 
        }

    }
}
