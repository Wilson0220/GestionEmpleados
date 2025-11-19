using ApiEmpleadosPorDeptartamentos.Interfaces;
using ApiEmpleadosPorDeptartamentos.Models.DTOs;
using Microsoft.EntityFrameworkCore;

namespace ApiEmpleadosPorDeptartamentos.Services
{
    public class DepartamentoService : IDepartamentoService
    {
        private readonly IEmpresaDbContext _context;

        public DepartamentoService(IEmpresaDbContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<DepartamentoDto>> ObtenerDepartamentosAsync()
        {
            return await _context.Departamentos
                .Select(e => new DepartamentoDto
                {
                    Id = e.Id,
                    Nombre = e.Nombre
                })
                .ToListAsync();
        }
    }
}
