using ApiEmpleadosPorDeptartamentos.Models.DTOs;

namespace ApiEmpleadosPorDeptartamentos.Interfaces
{
    public interface IDepartamentoService
    {
        Task<IEnumerable<DepartamentoDto>> ObtenerDepartamentosAsync();
    }
}
