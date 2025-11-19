using ApiEmpleadosPorDeptartamentos.Interfaces;
using ApiEmpleadosPorDeptartamentos.Models.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace ApiEmpleadosPorDeptartamentos.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EmpleadoController : ControllerBase
    {
        private readonly IEmpleadoService _empleadoService;

        public EmpleadoController(IEmpleadoService empleadoService)
        {
            _empleadoService = empleadoService;
        }

        [HttpGet]
        public async Task<IActionResult> ObtenerEmpleados()
        {
            var empleados = await _empleadoService.ObtenerEmpleadosAsync();
            return Ok(empleados);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> ObtenerEmpleadoPorId(int id)
        {
            var empleado = await _empleadoService.ObtenerEmpleadoPorIdAsync(id);
            if (empleado == null)
                return NotFound(new { mensaje = "Empleado no encontrado" });

            return Ok(empleado);
        }

        [HttpGet("ObtenerEmpleadosPorDepartamento")]
        public async Task<IActionResult> ObtenerEmpleadosPorDepartamento()
        {
            try
            {
                var data = await _empleadoService.ObtenerEmpleadosPorDepartamentoAsync();
                return Ok(data);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { mensaje = "Error al obtener empleados por departamento", detalle = ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> CrearEmpleado([FromBody] CrearEmpleadoDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var empleadoCreado = await _empleadoService.CrearEmpleadoAsync(dto);
            return CreatedAtAction(nameof(ObtenerEmpleadoPorId), new { id = empleadoCreado.Id }, empleadoCreado);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> ActualizarEmpleado(int id, [FromBody] ActualizarEmpleadoDto dto)
        {
            if (id != dto.Id)
                return BadRequest(new { mensaje = "El ID de la URL no coincide con el del cuerpo" });

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var actualizado = await _empleadoService.ActualizarEmpleadoAsync(dto);
            if (!actualizado)
                return NotFound(new { mensaje = "Empleado no encontrado" });

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> EliminarEmpleado(int id)
        {
            var eliminado = await _empleadoService.EliminarEmpleadoAsync(id);
            if (!eliminado)
                return NotFound(new { mensaje = "Empleado no encontrado" });

            return NoContent();
        }
    }
}
