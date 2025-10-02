using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Cors;
using ControlTemperaturas.API.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ControlTemperaturas.API.Controllers
{
    [EnableCors("ReglasCors")]
    [ApiController]
    [Route("api/[controller]")]
    public class ControlEncabezadosController : ControllerBase
    {
        private readonly ControlTemperaturasContext _dbcontext;

        public 
            
            ControlEncabezadosController(ControlTemperaturasContext context)
        {
            _dbcontext = context;
        }

        // =============================
        // GET: Obtener todos los encabezados con detalles
        // =============================
        [HttpGet]
        [Route("Lista")]
        public IActionResult Lista()
        {
            List<ControlEncabezado> lista = new();
            try
            {
                lista = _dbcontext.ControlEncabezados
                                   .Include(e => e.ControlDetalles)
                                   .ToList();

                return Ok(lista);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new { mensaje = ex.Message, response = lista });
            }
        }

        // =============================
        // GET: Buscar un control por ID
        // =============================
        [HttpGet]
        [Route("Buscar/{id}")]
        public IActionResult Buscar(int id)
        {
            try
            {
                var control = _dbcontext.ControlEncabezados
                                        .Include(e => e.ControlDetalles)
                                        .FirstOrDefault(e => e.IdControl == id);

                if (control == null)
                {
                    return NotFound(new { mensaje = "Control no encontrado" });
                }

                return Ok(control);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = ex.Message });
            }
        }

        // =============================
        // DELETE: Eliminar un control por ID
        // =============================
        [HttpDelete]
        [Route("Eliminar/{id:int}")]
        public IActionResult Eliminar(int id)
        {
            var control = _dbcontext.ControlEncabezados
                                    .Include(e => e.ControlDetalles)
                                    .FirstOrDefault(e => e.IdControl == id);

            if (control == null)
            {
                return BadRequest("Control no encontrado");
            }

            try
            {
                // Eliminar primero los detalles
                _dbcontext.ControlDetalles.RemoveRange(control.ControlDetalles);
                _dbcontext.ControlEncabezados.Remove(control);
                _dbcontext.SaveChanges();

                return Ok(new { mensaje = "Control eliminado correctamente" });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = ex.Message });
            }
        }

        // =============================
        // POST: Crear un nuevo control con detalles
        // =============================
        [HttpPost]
        [Route("Guardar")]
        public IActionResult Guardar([FromBody] ControlEncabezado control)
        {
            if (control == null)
                return BadRequest("El objeto enviado es nulo");

            try
            {
                _dbcontext.ControlEncabezados.Add(control);
                _dbcontext.SaveChanges();

                return Ok(new { mensaje = "Control guardado correctamente", id = control.IdControl });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = ex.Message });
            }
        }

        // =============================
        // PUT: Editar encabezado + detalles (upsert y borrado)
        // =============================

        [HttpPut]
        [Route("Editar")]
        public IActionResult Editar([FromBody] ControlEncabezado control)
        {
            if (control == null || control.IdControl <= 0)
                return BadRequest("Se requiere IdControl y datos válidos.");

            try
            {
                var existente = _dbcontext.ControlEncabezados
                    .Include(e => e.ControlDetalles)
                    .FirstOrDefault(e => e.IdControl == control.IdControl);

                if (existente == null)
                    return NotFound("Control no encontrado");

                // ========================
                // 1) Actualizar encabezado
                // ========================
                existente.Destino = control.Destino ?? existente.Destino;
                if (control.FechaDescongelacion != default) existente.FechaDescongelacion = control.FechaDescongelacion;
                if (control.FechaProduccion != default) existente.FechaProduccion = control.FechaProduccion;
                existente.RealizadoPor = control.RealizadoPor ?? existente.RealizadoPor;
                existente.RevisadoPor = control.RevisadoPor ?? existente.RevisadoPor;

                // ========================
                // 2) Actualizar/Insertar detalles
                // ========================
                var detallesEnviados = control.ControlDetalles ?? new List<ControlDetalle>();
                var indiceExistentes = existente.ControlDetalles.ToDictionary(d => d.IdDetalle);

                foreach (var d in detallesEnviados)
                {
                    if (d.IdDetalle > 0 && indiceExistentes.TryGetValue(d.IdDetalle, out var dBD))
                    {
                        // actualizar
                        dBD.NumeroCoche = d.NumeroCoche;
                        dBD.CodigoProducto = d.CodigoProducto;
                        dBD.HoraInicioDescongelado = d.HoraInicioDescongelado;
                        dBD.TemperaturaProducto = d.TemperaturaProducto;
                        dBD.HoraInicioConsumo = d.HoraInicioConsumo;
                        dBD.HoraFinConsumo = d.HoraFinConsumo;
                        dBD.Observaciones = d.Observaciones;
                    }
                    else
                    {
                        // insertar nuevo
                        d.IdDetalle = 0;
                        d.IdControl = existente.IdControl;
                        _dbcontext.ControlDetalles.Add(d);
                    }
                }

                // ========================
                // 3) Eliminar los que ya no vienen
                // ========================
                var idsEnviados = new HashSet<int>(detallesEnviados.Where(x => x.IdDetalle > 0).Select(x => x.IdDetalle));
                var paraEliminar = existente.ControlDetalles.Where(d => !idsEnviados.Contains(d.IdDetalle)).ToList();
                if (paraEliminar.Count > 0)
                    _dbcontext.ControlDetalles.RemoveRange(paraEliminar);

                // ========================
                // 4) Guardar cambios
                // ========================
                _dbcontext.SaveChanges();

                // ========================
                // 5) Logs para depuración
                // ========================
                foreach (var entry in _dbcontext.ChangeTracker.Entries<ControlDetalle>())
                {
                    Console.WriteLine($"{entry.State} => {System.Text.Json.JsonSerializer.Serialize(entry.Entity)}");
                }

                // ========================
                // 6) Retornar objeto actualizado
                // ========================
                var actualizado = _dbcontext.ControlEncabezados
                    .Include(e => e.ControlDetalles)
                    .FirstOrDefault(e => e.IdControl == control.IdControl);

                return Ok(new
                {
                    mensaje = "Control actualizado (encabezado y detalles).",
                    data = actualizado
                });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = ex.Message });
            }
        }




        // =============================
        // GET: Reportería por rango de fechas (y destino opcional)
        // /api/ControlEncabezados/Reporte?desde=2025-09-28&hasta=2025-10-03&destino=Planta%20Manta
        // =============================
        [HttpGet]
        [Route("Reporte")]
        public IActionResult Reporte([FromQuery] DateOnly desde, [FromQuery] DateOnly hasta, [FromQuery] string? destino = null)
        {
            try
            {
                var query = _dbcontext.VistaControlTemperaturas
                    .Where(v => v.FechaProduccion >= desde && v.FechaProduccion <= hasta);

                if (!string.IsNullOrWhiteSpace(destino))
                    query = query.Where(v => v.Destino.Contains(destino));

                var data = query
                    .OrderByDescending(v => v.FechaProduccion)
                    .ToList();

                return Ok(data);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = ex.Message });
            }
        }


    }
}

