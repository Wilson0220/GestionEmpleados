using ApiEmpleadosPorDeptartamentos.Interfaces;
using ApiEmpleadosPorDeptartamentos.Models;
using ApiEmpleadosPorDeptartamentos.Models.DTOs;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiEmpleadosPorDeptartamentos.Services
{
    public class EmpleadoService : IEmpleadoService
    {
        private readonly IEmpresaDbContext _context;

        public EmpleadoService(IEmpresaDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<EmpleadoDto>> ObtenerEmpleadosAsync()
        {
            return await _context.Empleados
                .Include(e => e.Departamento)
                .Include(e => e.EstadoEmpleado)
                .Select(e => new EmpleadoDto
                {
                    Id = e.Id,
                    Nombre = e.Nombre,
                    Apellido = e.Apellido,
                    FechaIngreso = e.FechaIngreso,
                    Salario = e.Salario,
                    Departamento = e.Departamento.Nombre,
                    EstadoEmpleado = e.EstadoEmpleado.Descripcion
                })
                .ToListAsync();
        }

        public async Task<EmpleadoDto?> ObtenerEmpleadoPorIdAsync(int id)
        {
            var e = await _context.Empleados
                .Include(x => x.Departamento)
                .Include(x => x.EstadoEmpleado)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (e == null) return null;

            return new EmpleadoDto
            {
                Id = e.Id,
                Nombre = e.Nombre,
                Apellido = e.Apellido,
                FechaIngreso = e.FechaIngreso,
                Salario = e.Salario,
                DepartamentoId = e.Departamento.Id,
                Departamento =e.Departamento.Nombre,
                EstadoEmpleadoId =e.EstadoEmpleadoId,
                EstadoEmpleado = e.EstadoEmpleado.Descripcion
            };
        }

        public async Task<IEnumerable<EmpleadosPorDepartamentoDto>> ObtenerEmpleadosPorDepartamentoAsync()
        {
            return await _context.Empleados
                .Include(e => e.Departamento)
                .GroupBy(e => e.Departamento.Nombre)
                .Select(g => new EmpleadosPorDepartamentoDto
                {
                    Departamento = g.Key,
                    Cantidad = g.Count()
                })
                .ToListAsync();
        }


        public async Task<EmpleadoDto> CrearEmpleadoAsync(CrearEmpleadoDto dto)
        {
            var nuevo = new Empleado
            {
                Nombre = dto.Nombre,
                Apellido = dto.Apellido,
                FechaIngreso = dto.FechaIngreso,
                Salario = dto.Salario,
                DepartamentoId = dto.DepartamentoId,
                EstadoEmpleadoId = dto.EstadoEmpleadoId
            };

            _context.Empleados.Add(nuevo);
            await _context.SaveChangesAsync();

            return await ObtenerEmpleadoPorIdAsync(nuevo.Id) ?? new EmpleadoDto();
        }

        public async Task<bool> ActualizarEmpleadoAsync(ActualizarEmpleadoDto dto)
        {
            var e = await _context.Empleados.FindAsync(dto.Id);
            if (e == null) return false;

            e.Nombre = dto.Nombre;
            e.Apellido = dto.Apellido;
            e.Salario = dto.Salario;
            e.DepartamentoId = dto.DepartamentoId;
            e.EstadoEmpleadoId = dto.EstadoEmpleadoId;

            _context.Empleados.Update(e);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> EliminarEmpleadoAsync(int id)
        {
            var e = await _context.Empleados.FindAsync(id);
            if (e == null) return false;

            _context.Empleados.Remove(e);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
