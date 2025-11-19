using ApiEmpleadosPorDeptartamentos.Interfaces;
using ApiEmpleadosPorDeptartamentos.Services;
using Microsoft.AspNetCore.Mvc;

namespace ApiEmpleadosPorDeptartamentos.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DepartamentoController : ControllerBase
    {
        private readonly IDepartamentoService _departamentoService;

        public DepartamentoController(IDepartamentoService departamentoService)
        {
            _departamentoService = departamentoService;
        }

        [HttpGet]
        public async Task<IActionResult> ObtenerDepartamentos()
        {
            var departamentos = await _departamentoService.ObtenerDepartamentosAsync();
            return Ok(departamentos);
        }
    }
}
