using ApiEmpleadosPorDeptartamentos.Models.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ApiEmpleadosPorDeptartamentos.Interfaces
{
    public interface IEmpleadoService
    {
        Task<IEnumerable<EmpleadoDto>> ObtenerEmpleadosAsync();
        Task<EmpleadoDto?> ObtenerEmpleadoPorIdAsync(int id);
        Task<IEnumerable<EmpleadosPorDepartamentoDto>> ObtenerEmpleadosPorDepartamentoAsync();
        Task<EmpleadoDto> CrearEmpleadoAsync(CrearEmpleadoDto dto);
        Task<bool> ActualizarEmpleadoAsync(ActualizarEmpleadoDto dto);
        Task<bool> EliminarEmpleadoAsync(int id);
    }
}
