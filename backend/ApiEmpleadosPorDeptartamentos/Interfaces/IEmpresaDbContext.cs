using ApiEmpleadosPorDeptartamentos.Models;
using Microsoft.EntityFrameworkCore;

namespace ApiEmpleadosPorDeptartamentos.Interfaces
{
    public interface IEmpresaDbContext
    {
        DbSet<Empleado> Empleados { get; set; }
        DbSet<Departamento> Departamentos { get; set; }
        DbSet<EstadoEmpleado> EstadosEmpleado { get; set; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
