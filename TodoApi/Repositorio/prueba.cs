namespace TodoApi.Repositorio
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Logging;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Todo.Domain.Tareas;
    using TodoApi.Models;

    public interface IPrueba
    {
        /// <summary>
        /// funcion que consulta todas las tareas
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<TodoItem>> GetAllAsync();

        /// <summary>
        /// FUncion de prueba
        /// </summary>
        /// <param name="parametro">parametros de entrada</param>
        void metodo(string parametro);
        (bool Success, object Data) Metodo(string parametros);
    }

    public class Prueba : IPrueba
    {
        private readonly ILogger<Prueba> Logger;
        private readonly TodoContext _context;

        public Prueba(ILogger<Prueba> logger, TodoContext context)
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

                throw;
            }
            
        }

        public (bool Success, object Data) Metodo(string parametros)
        {
            try
            {



                if (string.IsNullOrEmpty(parametros))
                {

                }

                _ = parametros?.ToString();
                return (true, new SuccessAPIResponse());
            }
            catch (ArgumentNullException Ex)
            {
                Logger.LogError("Parametros es nulo");
                return (false, new ErrorAPIResponse());
            }
            catch (Exception Ex)
            {
                Logger.LogCritical(Ex, "Error inesperado");
                return (false, new ErrorAPIResponse());
            }
        }

        class ErrorAPIResponse //xxx
        {
            public string Mensaje { get; set; } //Tiempo de espera muy largo
            public string ErroHttp { get; set; } // 408
        }

        class SuccessAPIResponse //200
        {
            public dynamic Link { get; set; }
            public object Object { get; set; }
        }

        public void metodo(string parametro)
        {
            try
            {                
                Logger.LogInformation("Se realizo correctamente la operacion X");
                Logger.LogWarning("Se realizo correctamente la operacion X");
                Logger.LogError("Se realizo correctamente la operacion X");
            }
            catch (TimeoutException Ex)
            {
                Logger.LogCritical(Ex, "Error inesperado en metodo 1, cuando se hacia la operacion x {parametro}", parametro);
            }
        }

    }


}
