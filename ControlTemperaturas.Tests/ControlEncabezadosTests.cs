using System.Net;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;
using ControlTemperaturas.API.Models;

namespace ControlTemperaturas.Tests
{
    public class ControlEncabezadosTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly WebApplicationFactory<Program> _factory;

        public ControlEncabezadosTests(WebApplicationFactory<Program> factory)
        {
            _factory = factory;
        }

        // =============================
        // GET: Lista
        // =============================
        [Fact]
        public async Task Get_Lista_DeberiaRetornarOk()
        {
            var client = _factory.CreateClient();

            var response = await client.GetAsync("/api/ControlEncabezados/Lista");

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        // =============================
        // POST: Guardar
        // =============================
        [Fact]
        public async Task Post_Guardar_DeberiaCrearRegistro()
        {
            var client = _factory.CreateClient();

            var nuevo = new ControlEncabezado
            {
                Destino = "Prueba Unitaria",
                FechaProduccion = DateOnly.FromDateTime(System.DateTime.Now),
                FechaDescongelacion = DateOnly.FromDateTime(System.DateTime.Now.AddDays(-1)),
                RealizadoPor = "Tester",
                RevisadoPor = "Supervisor"
            };

            var response = await client.PostAsJsonAsync("/api/ControlEncabezados/Guardar", nuevo);

            response.EnsureSuccessStatusCode();

            var result = await response.Content.ReadFromJsonAsync<dynamic>();
            Assert.Contains("Control guardado correctamente", result?.ToString());
        }

        // =============================
        // PUT: Editar
        // =============================
        [Fact]
        public async Task Put_Editar_DeberiaActualizarRegistro()
        {
            var client = _factory.CreateClient();

            // primero creamos un registro
            var nuevo = new ControlEncabezado
            {
                Destino = "Editar Antes",
                FechaProduccion = DateOnly.FromDateTime(System.DateTime.Now),
                FechaDescongelacion = DateOnly.FromDateTime(System.DateTime.Now),
                RealizadoPor = "Tester",
                RevisadoPor = "Supervisor"
            };

            var createResponse = await client.PostAsJsonAsync("/api/ControlEncabezados/Guardar", nuevo);
            var creado = await createResponse.Content.ReadFromJsonAsync<dynamic>();
            int idCreado = (int)creado?.id;

            // luego editamos
            var actualizado = new ControlEncabezado
            {
                IdControl = idCreado,
                Destino = "Editar Después",
                FechaProduccion = DateOnly.FromDateTime(System.DateTime.Now),
                FechaDescongelacion = DateOnly.FromDateTime(System.DateTime.Now),
                RealizadoPor = "Tester 2",
                RevisadoPor = "Supervisor 2"
            };

            var response = await client.PutAsJsonAsync("/api/ControlEncabezados/Editar", actualizado);

            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadAsStringAsync();

            Assert.Contains("Control actualizado", result);
        }

        // =============================
        // DELETE: Eliminar
        // =============================
        [Fact]
        public async Task Delete_Eliminar_DeberiaBorrarRegistro()
        {
            var client = _factory.CreateClient();

            // primero creamos un registro
            var nuevo = new ControlEncabezado
            {
                Destino = "Eliminar Prueba",
                FechaProduccion = DateOnly.FromDateTime(System.DateTime.Now),
                FechaDescongelacion = DateOnly.FromDateTime(System.DateTime.Now),
                RealizadoPor = "Tester",
                RevisadoPor = "Supervisor"
            };

            var createResponse = await client.PostAsJsonAsync("/api/ControlEncabezados/Guardar", nuevo);
            var creado = await createResponse.Content.ReadFromJsonAsync<dynamic>();
            int idCreado = (int)creado?.id;

            // lo eliminamos
            var response = await client.DeleteAsync($"/api/ControlEncabezados/Eliminar/{idCreado}");

            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadAsStringAsync();

            Assert.Contains("eliminado correctamente", result);
        }
    }
}
