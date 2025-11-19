using ApiEmpleadosPorDeptartamentos.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.Design;

namespace ApiEmpleadosPorDeptartamentos.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class HelperController : ControllerBase
    {
        private readonly IHelperService _helperService;

        public HelperController(IHelperService helperService)
        {
            _helperService = helperService;
        }

        [HttpGet("resumen")]
        public async Task<IActionResult> ObtenerResumen()
        {
            var resumen = await _helperService.ObtenerResumenAsync();
            return Ok(resumen);
        }
    }
}
