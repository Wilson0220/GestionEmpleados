using ApiEmpleadosPorDeptartamentos.Interfaces;
using ApiEmpleadosPorDeptartamentos.Models;
using ApiEmpleadosPorDeptartamentos.Models.Enums;
using Microsoft.EntityFrameworkCore;
using System.Runtime.InteropServices;

namespace ApiEmpleadosPorDeptartamentos.Services
{
    public class HelperService : IHelperService
    {
        private readonly IEmpresaDbContext _context;

        public HelperService(IEmpresaDbContext context)
        {
            _context = context;
        }

        public async Task<object> ObtenerResumenAsync()
        {
            var totalEmpleados = await _context.Empleados.CountAsync();
            var totalDepartamentos = await _context.Departamentos.CountAsync();
            var totalEmpleadosActivos = await _context.Empleados
                .Where(e => e.EstadoEmpleadoId == (int)EstadoEmpleadoEnum.Activo).CountAsync();

            return new
            {
                TotalEmpleados = totalEmpleados,
                TotalDepartamentos = totalDepartamentos,
                TotalEmpleadosActivos = totalEmpleadosActivos
            };
        }
    }
}
